using System;
using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace ModbusScada
{
    /// <summary>
    /// NLog 日志配置（代码配置，无需 nlog.config 文件）
    /// </summary>
    public static class LogConfig
    {
        private static Logger _logger;

        /// <summary>获取日志器实例</summary>
        public static Logger Logger => _logger;

        /// <summary>
        /// 初始化 NLog：文件输出，每天归档，保留7天
        /// </summary>
        public static void Initialize()
        {
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget("logfile")
            {
                FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ScadaConstants.LogFileName),
                Layout = "${longdate} | ${level:uppercase=true} | ${message}",
                ArchiveEvery = FileArchivePeriod.Day,
                MaxArchiveFiles = 7,
                ArchiveFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scada.{#}.log"),
                ArchiveNumbering = ArchiveNumberingMode.Date
            };

            config.AddRule(LogLevel.Info, LogLevel.Fatal, fileTarget);
            LogManager.Configuration = config;

            _logger = LogManager.GetCurrentClassLogger();
            _logger.Info("NLog 初始化完成");
        }

        /// <summary>关闭 NLog</summary>
        public static void Shutdown()
        {
            LogManager.Shutdown();
        }
    }
}
