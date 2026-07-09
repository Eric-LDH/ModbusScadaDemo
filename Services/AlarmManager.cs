using System;
using NLog;

namespace ModbusScadaDemo
{
    /// <summary>
    /// 报警管理器：死区防抖逻辑
    /// </summary>
    public class AlarmManager
    {
        private readonly Logger _logger = LogConfig.Logger;
        private int _alarmCount;
        private bool _alarmActive;
        private float _alarmThreshold = 50f;

        /// <summary>报警状态变更事件</summary>
        public event Action<AlarmEventArgs> AlarmStateChanged;

        /// <summary>当前报警阈值（℃）</summary>
        public float Threshold
        {
            get => _alarmThreshold;
            set
            {
                _alarmThreshold = value;
                _logger.Info("报警阈值已修改: {0}℃", _alarmThreshold);
            }
        }

        /// <summary>当前报警是否激活</summary>
        public bool IsAlarmActive => _alarmActive;

        /// <summary>
        /// 死区防抖温度报警判断
        /// 连续3次超阈值触发报警，连续3次低于阈值解除报警
        /// </summary>
        public void Check(float temp)
        {
            bool overThreshold = temp > _alarmThreshold;

            if (overThreshold)
                _alarmCount = Math.Min(_alarmCount + 1, ScadaConstants.AlarmConsecutiveCount + 1);
            else
                _alarmCount = Math.Max(_alarmCount - 1, -(ScadaConstants.AlarmConsecutiveCount + 1));

            // 触发报警
            if (!_alarmActive && _alarmCount >= ScadaConstants.AlarmConsecutiveCount)
            {
                _alarmActive = true;
                _alarmCount = 0;
                _logger.Warn("温度报警触发! 当前温度: {0}℃, 阈值: {1}℃", temp, _alarmThreshold);
                FireAlarmChanged(true, temp);
            }
            // 解除报警
            else if (_alarmActive && _alarmCount <= -ScadaConstants.AlarmConsecutiveCount)
            {
                _alarmActive = false;
                _alarmCount = 0;
                _logger.Info("温度报警解除。当前温度: {0}℃, 阈值: {1}℃", temp, _alarmThreshold);
                FireAlarmChanged(false, temp);
            }
        }

        private void FireAlarmChanged(bool active, float currentTemp)
        {
            AlarmStateChanged?.Invoke(new AlarmEventArgs
            {
                Active = active,
                CurrentTemp = currentTemp,
                Threshold = _alarmThreshold
            });
        }
    }
}
