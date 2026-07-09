using System;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using Modbus.Device;
using NLog;

namespace ModbusScadaDemo
{
    /// <summary>
    /// 串口管理器：扫描、连接、断开、自动重连
    /// </summary>
    public class SerialPortManager : IDisposable
    {
        private readonly Logger _logger = LogConfig.Logger;
        private readonly object _lockObj = new object();
        private readonly System.Windows.Forms.Timer _reconnectTimer;
        private SerialPort _serialPort;
        private IModbusSerialMaster _master;
        private volatile bool _isConnected;
        private volatile bool _isClosing;

        /// <summary>串口名称</summary>
        public string PortName { get; private set; }

        /// <summary>是否已连接</summary>
        public bool IsConnected => _isConnected;

        /// <summary>Modbus Master 实例（供 ModbusDriver 使用）</summary>
        public IModbusSerialMaster Master => _master;

        /// <summary>通信锁（保护串口临界区操作）</summary>
        public object SyncLock => _lockObj;

        /// <summary>是否启用自动重连</summary>
        public bool AutoReconnect
        {
            get => _reconnectTimer.Enabled;
            set
            {
                if (value && !_isConnected && !_isClosing)
                    _reconnectTimer.Start();
                else
                    _reconnectTimer.Stop();
            }
        }

        /// <summary>连接状态变更事件</summary>
        public event Action<ConnectionEventArgs> ConnectionChanged;

        public SerialPortManager()
        {
            _reconnectTimer = new System.Windows.Forms.Timer { Interval = ScadaConstants.ReconnectIntervalMs };
            _reconnectTimer.Tick += ReconnectTimer_Tick;
        }

        /// <summary>
        /// 扫描可用串口列表
        /// </summary>
        public string[] ScanPorts()
        {
            try
            {
                return SerialPort.GetPortNames();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "扫描串口失败");
                return new string[0];
            }
        }

        /// <summary>
        /// 连接指定串口并创建 Modbus Master
        /// </summary>
        /// <returns>null 表示成功，否则返回错误消息</returns>
        public string Connect(string portName)
        {
            lock (_lockObj)
            {
                try
                {
                    // 先清理旧连接
                    CleanupInternal();

                    PortName = portName;

                    _serialPort = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One)
                    {
                        ReadTimeout = 1000,
                        WriteTimeout = 1000
                    };
                    _serialPort.Open();

                    _master = ModbusSerialMaster.CreateRtu(_serialPort);

                    _isConnected = true;
                    _logger.Info("串口已连接: {0} 9600-8-N-1", portName);

                    // 停止重连定时器
                    _reconnectTimer.Stop();

                    FireConnectionChanged(true);
                    return null;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "串口连接失败");
                    CleanupInternal();
                    return ex.Message;
                }
            }
        }

        /// <summary>
        /// 断开串口连接
        /// </summary>
        public void Disconnect()
        {
            try
            {
                _reconnectTimer.Stop();

                lock (_lockObj)
                {
                    CleanupInternal();
                }

                _isConnected = false;
                _logger.Info("串口已断开");

                FireConnectionChanged(false);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "断开串口异常");
            }
        }

        /// <summary>
        /// 处理通信故障：断开并可选启动重连
        /// </summary>
        public void HandleConnectionLost(string reason)
        {
            _logger.Warn("通信故障，进入自动恢复流程: {0}", reason);

            lock (_lockObj)
            {
                CleanupInternal();
            }

            _isConnected = false;
            FireConnectionChanged(false);
        }

        /// <summary>
        /// 设置关闭标志（防止 Form 关闭时重连）
        /// </summary>
        public void SetClosing()
        {
            _isClosing = true;
            _reconnectTimer.Stop();
        }

        /// <summary>
        /// 自动重连定时器回调
        /// </summary>
        private void ReconnectTimer_Tick(object sender, EventArgs e)
        {
            if (_isConnected || _isClosing) return;

            _logger.Info("正在尝试自动重连...");
            string error = Connect(PortName);

            if (_isConnected)
            {
                _reconnectTimer.Stop();
            }
        }

        /// <summary>
        /// 触发连接状态变更事件
        /// </summary>
        private void FireConnectionChanged(bool connected)
        {
            ConnectionChanged?.Invoke(new ConnectionEventArgs
            {
                Connected = connected,
                PortName = PortName
            });
        }

        /// <summary>
        /// 清理串口和 Modbus 资源（需在锁内调用）
        /// </summary>
        private void CleanupInternal()
        {
            try
            {
                _master?.Dispose();
                _master = null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "释放 Modbus Master 异常");
            }

            try
            {
                if (_serialPort != null)
                {
                    if (_serialPort.IsOpen)
                        _serialPort.Close();
                    _serialPort.Dispose();
                }
                _serialPort = null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "关闭串口异常");
            }
        }

        public void Dispose()
        {
            _reconnectTimer?.Stop();
            _reconnectTimer?.Dispose();

            lock (_lockObj)
            {
                CleanupInternal();
            }
        }
    }
}
