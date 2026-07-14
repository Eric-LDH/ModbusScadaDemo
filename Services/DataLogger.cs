using System;
using System.Data.SQLite;
using System.IO;
using NLog;

namespace ModbusScada
{
    /// <summary>
    /// 数据存储管理器：SQLite 初始化、参数化写入、自动重连
    /// </summary>
    public class DataLogger : IDisposable
    {
        private readonly Logger _logger = LogConfig.Logger;
        private SQLiteConnection _sqliteConn;

        /// <summary>
        /// 初始化 SQLite 数据库，创建温度日志表
        /// </summary>
        public void Initialize()
        {
            try
            {
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ScadaConstants.DbFileName);
                _sqliteConn = new SQLiteConnection("Data Source=" + dbPath);
                _sqliteConn.Open();

                string createSql = @"CREATE TABLE IF NOT EXISTS temperature_log (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
                    temperature REAL,
                    relay_state INTEGER,
                    alarm_state INTEGER)";

                using (var cmd = new SQLiteCommand(createSql, _sqliteConn))
                {
                    cmd.ExecuteNonQuery();
                }

                _logger.Info("数据库初始化完成: {0}", dbPath);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "数据库初始化失败");
            }
        }

        /// <summary>
        /// 保存温度记录（参数化查询，支持自动重连）
        /// </summary>
        public void SaveRecord(float temp, int relayState, int alarmState)
        {
            try
            {
                // 断线自动重连
                if (_sqliteConn == null || _sqliteConn.State != System.Data.ConnectionState.Open)
                {
                    try
                    {
                        string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ScadaConstants.DbFileName);
                        _sqliteConn = new SQLiteConnection("Data Source=" + dbPath);
                        _sqliteConn.Open();
                        _logger.Info("数据库已重连: {0}", dbPath);
                    }
                    catch (Exception dbEx)
                    {
                        _logger.Error(dbEx, "数据库重连失败，跳过本次写入");
                        return;
                    }
                }

                string insertSql = @"INSERT INTO temperature_log (temperature, relay_state, alarm_state)
                                     VALUES (@temp, @relay, @alarm)";

                using (var cmd = new SQLiteCommand(insertSql, _sqliteConn))
                {
                    cmd.Parameters.AddWithValue("@temp", temp);
                    cmd.Parameters.AddWithValue("@relay", relayState);
                    cmd.Parameters.AddWithValue("@alarm", alarmState);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "数据库写入失败");
            }
        }

        public void Dispose()
        {
            try
            {
                _sqliteConn?.Close();
                _sqliteConn?.Dispose();
                _sqliteConn = null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "关闭数据库异常");
            }
        }
    }
}
