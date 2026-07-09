using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace ModbusScadaDemo
{
    /// <summary>
    /// Modbus 驱动：轮询、读写寄存器/线圈、数据校验、事件发布
    /// </summary>
    public class ModbusDriver : IDisposable
    {
        private readonly Logger _logger = LogConfig.Logger;
        private readonly SerialPortManager _serialMgr;
        private System.Threading.Timer _pollingTimer;
        private int _isPolling;
        private int _pollCounter;
        private float _lastValidTemp = float.NaN;
        private bool _isDisposed;

        /// <summary>温度数据更新事件（后台线程触发）</summary>
        public event Action<TemperatureEventArgs> TemperatureUpdated;

        /// <summary>
        /// 通信故障事件（后台线程触发）
        /// </summary>
        public event Action<string> ConnectionLost;

        public ModbusDriver(SerialPortManager serialMgr)
        {
            _serialMgr = serialMgr ?? throw new ArgumentNullException(nameof(serialMgr));

            _pollingTimer = new System.Threading.Timer(
                PollingCallback, null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>启动轮询</summary>
        public void StartPolling()
        {
            _pollingTimer?.Change(
                ScadaConstants.PollingIntervalMs,
                ScadaConstants.PollingIntervalMs);
        }

        /// <summary>停止轮询</summary>
        public void StopPolling()
        {
            _pollingTimer?.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// 轮询回调（后台线程）
        /// </summary>
        private void PollingCallback(object state)
        {
            // 防止重入
            if (Interlocked.CompareExchange(ref _isPolling, 1, 0) != 0)
                return;

            try
            {
                if (!_serialMgr.IsConnected || _isDisposed)
                    return;

                lock (_serialMgr.SyncLock)
                {
                    var master = _serialMgr.Master;
                    if (master == null)
                    {
                        HandleFailure("Modbus Master 未就绪");
                        return;
                    }

                    try
                    {
                        // 读取保持寄存器：地址0开始共2个寄存器
                        ushort[] registers = master.ReadHoldingRegisters(
                            ScadaConstants.SlaveAddress,
                            ScadaConstants.TempRegisterAddr, 2);

                        // 寄存器0: 温度值(×100)
                        float rawTemp = registers[0] / 100f;

                        // 异常值检测：范围校验 + 跳变检测
                        bool rangeOk = (rawTemp >= ScadaConstants.TempMinValid &&
                                        rawTemp <= ScadaConstants.TempMaxValid);
                        float delta = float.IsNaN(_lastValidTemp) ? 0f :
                            Math.Abs(rawTemp - _lastValidTemp);
                        bool jumpOk = float.IsNaN(_lastValidTemp) ||
                            delta <= ScadaConstants.TempJumpThreshold;

                        float? validTemp = null;
                        bool sensorFault = false;

                        if (rangeOk && jumpOk)
                        {
                            validTemp = rawTemp;
                            _lastValidTemp = rawTemp;
                            sensorFault = false;
                        }
                        else
                        {
                            sensorFault = true;
                            if (!rangeOk)
                                _logger.Warn("温度传感器异常值: {0}℃（有效范围 {1}~{2}℃），已丢弃",
                                    rawTemp, ScadaConstants.TempMinValid, ScadaConstants.TempMaxValid);
                            if (!jumpOk)
                                _logger.Warn("温度传感器跳变异常: {0}℃ → {1}℃（Δ={2}℃ > 阈值{3}℃），已丢弃",
                                    _lastValidTemp, rawTemp, delta, ScadaConstants.TempJumpThreshold);
                        }

                        // 寄存器1: 继电器状态
                        bool relayOn = registers[1] == 1;

                        // 发布温度事件
                        TemperatureUpdated?.Invoke(new TemperatureEventArgs
                        {
                            Temperature = validTemp,
                            SensorFault = sensorFault,
                            RelayOn = relayOn
                        });

                        // 轮询计数
                        _pollCounter++;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Modbus 轮询读取失败");
                        HandleFailure(ex.Message);
                    }
                }
            }
            finally
            {
                Interlocked.Exchange(ref _isPolling, 0);
            }
        }

        /// <summary>
        /// 获取当前轮询计数（供外部判断是否触发存储）
        /// </summary>
        public int GetPollCounter() => _pollCounter;

        /// <summary>
        /// 重置轮询计数（存储后由外部调用）
        /// </summary>
        public void ResetPollCounter() => _pollCounter = 0;

        /// <summary>
        /// 写入单个线圈（异步）
        /// </summary>
        public async Task WriteCoilAsync(bool value)
        {
            await Task.Run(() =>
            {
                lock (_serialMgr.SyncLock)
                {
                    var master = _serialMgr.Master;
                    if (master == null)
                        throw new InvalidOperationException("Modbus Master 未就绪");

                    master.WriteSingleCoil(ScadaConstants.SlaveAddress,
                        ScadaConstants.CoilAddr, value);
                }
            });
        }

        /// <summary>
        /// 处理通信故障
        /// </summary>
        private void HandleFailure(string reason)
        {
            StopPolling();
            _serialMgr.HandleConnectionLost(reason);
            ConnectionLost?.Invoke(reason);
        }

        public void Dispose()
        {
            _isDisposed = true;
            _pollingTimer?.Change(Timeout.Infinite, Timeout.Infinite);
            _pollingTimer?.Dispose();
            _pollingTimer = null;
        }
    }
}
