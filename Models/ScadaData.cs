using System;

namespace ModbusScada
{
    /// <summary>
    /// 上位机全局常量定义
    /// </summary>
    public static class ScadaConstants
    {
        // 日志
        public const string LogFileName = "scada.log";
        public const string DbFileName = "scada_data.db";

        // Modbus 参数
        public const byte SlaveAddress = 1;
        public const ushort TempRegisterAddr = 0;
        public const ushort RelayRegisterAddr = 1;
        public const ushort CoilAddr = 0;

        // 报警参数
        public const int AlarmConsecutiveCount = 3;
        public const int DbSaveInterval = 5;

        // 温度传感器校验参数
        public const float TempMinValid = -40f;
        public const float TempMaxValid = 125f;
        public const float TempJumpThreshold = 30f;

        // 曲线参数
        public const int MaxDataPoints = 300;

        // 定时器参数
        public const int PollingIntervalMs = 1000;
        public const int ReconnectIntervalMs = 3000;
    }

    /// <summary>
    /// 温度读数事件参数
    /// </summary>
    public class TemperatureEventArgs : EventArgs
    {
        /// <summary>温度值（℃），异常时为 null</summary>
        public float? Temperature { get; set; }

        /// <summary>传感器是否故障</summary>
        public bool SensorFault { get; set; }

        /// <summary>继电器状态</summary>
        public bool RelayOn { get; set; }
    }

    /// <summary>
    /// 报警状态变更事件参数
    /// </summary>
    public class AlarmEventArgs : EventArgs
    {
        /// <summary>报警是否激活</summary>
        public bool Active { get; set; }

        /// <summary>当前温度（℃）</summary>
        public float CurrentTemp { get; set; }

        /// <summary>报警阈值（℃）</summary>
        public float Threshold { get; set; }
    }

    /// <summary>
    /// 连接状态变更事件参数
    /// </summary>
    public class ConnectionEventArgs : EventArgs
    {
        /// <summary>是否已连接</summary>
        public bool Connected { get; set; }

        /// <summary>串口名称</summary>
        public string PortName { get; set; }
    }
}
