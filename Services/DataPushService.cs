using System;
using System.Configuration;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using NLog;

namespace ModbusScada
{
    /// <summary>
    /// 数据推送服务：订阅 ModbusDriver/AlarmManager/SerialPortManager 事件，
    /// 通过 HMAC-SHA256 签名将实时数据 HTTP POST 到 Web 后端
    /// </summary>
    public class DataPushService : IDisposable
    {
        private readonly Logger _logger = LogConfig.Logger;
        private readonly HttpClient _httpClient;
        private readonly string _webApiUrl;
        private readonly string _clientId;
        private readonly byte[] _secretKey;

        private volatile bool _lastAlarmActive;
        private float _lastAlarmTemp;
        private volatile bool _lastConnected;
        private string _lastPortName = string.Empty;

        private bool _isDisposed;

        public DataPushService(ModbusDriver modbusDriver, AlarmManager alarmMgr, SerialPortManager serialMgr)
        {
            _webApiUrl = ConfigurationManager.AppSettings["WebApiUrl"] ?? "http://localhost:5000";
            _clientId = ConfigurationManager.AppSettings["ClientId"] ?? "scada-push-client";
            var secretKeyStr = ConfigurationManager.AppSettings["SecretKey"] ?? "default-secret-key-change-me!!";
            _secretKey = Encoding.UTF8.GetBytes(secretKeyStr);

            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };

            // 订阅事件
            modbusDriver.TemperatureUpdated += OnTemperatureUpdated;
            alarmMgr.AlarmStateChanged += OnAlarmChanged;
            serialMgr.ConnectionChanged += OnConnectionChanged;

            _logger.Info("DataPushService 已初始化, 推送目标: {0}, ClientId: {1}", _webApiUrl, _clientId);
        }

        #region 事件处理

        private void OnAlarmChanged(AlarmEventArgs e)
        {
            _lastAlarmActive = e.Active;
            _lastAlarmTemp = e.CurrentTemp;
        }

        private void OnConnectionChanged(ConnectionEventArgs e)
        {
            _lastConnected = e.Connected;
            _lastPortName = e.PortName ?? string.Empty;
        }

        private void OnTemperatureUpdated(TemperatureEventArgs e)
        {
            if (_isDisposed) return;

            var payload = new
            {
                temperature = e.Temperature,
                sensorFault = e.SensorFault,
                relayOn = e.RelayOn,
                alarmActive = _lastAlarmActive,
                alarmCurrentTemp = _lastAlarmTemp,
                alarmThreshold = 50.0,
                connectionConnected = _lastConnected,
                portName = _lastPortName,
                timestamp = DateTime.UtcNow.ToString("o")
            };

            SendDataAsync(payload);
        }

        #endregion

        #region HMAC 签名与 HTTP 发送

        private async void SendDataAsync(object payload)
        {
            try
            {
                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // 获取 Unix 时间戳（秒）
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

                // 签名串 = 请求体JSON + "\n" + 时间戳
                var signStr = json + "\n" + timestamp;
                string signature;
                using (var hmac = new HMACSHA256(_secretKey))
                {
                    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signStr));
                    signature = Convert.ToBase64String(hash);
                }

                // 附加 HMAC 签名请求头
                var request = new HttpRequestMessage(HttpMethod.Post, _webApiUrl + "/api/datareceiver/push")
                {
                    Content = content
                };
                request.Headers.Add("X-Client-Id", _clientId);
                request.Headers.Add("X-Timestamp", timestamp);
                request.Headers.Add("X-Signature", signature);

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.Warn("数据推送失败: {0} {1}", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                _logger.Warn("数据推送异常: {0}", ex.Message);
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            _httpClient?.Dispose();
            _logger.Info("DataPushService 已释放");
        }

        #endregion
    }
}
