using System;
using ScottPlot;
using ScottPlot.Plottables;

namespace ModbusScada
{
    /// <summary>
    /// 曲线管理器：ScottPlot 初始化、数据更新、阈值线管理
    /// </summary>
    public class PlotManager
    {
        private Scatter _scatterPlot;
        private HorizontalLine _thresholdLine;
        private int _dataCount;

        /// <summary>X轴数据（0~299）</summary>
        public readonly double[] XData = new double[ScadaConstants.MaxDataPoints];

        /// <summary>Y轴数据（温度值）</summary>
        public readonly double[] YData = new double[ScadaConstants.MaxDataPoints];

        /// <summary>
        /// 初始化曲线（在窗体显示后调用）
        /// </summary>
        public void Initialize(ScottPlot.WinForms.FormsPlot plotControl)
        {
            // 初始化数据数组
            for (int i = 0; i < ScadaConstants.MaxDataPoints; i++)
            {
                XData[i] = i;
                YData[i] = 0;
            }

            // 添加散点图
            _scatterPlot = plotControl.Plot.Add.Scatter(XData, YData);
            _scatterPlot.Color = new ScottPlot.Color(41, 128, 185);
            _scatterPlot.LineWidth = 2;
            _scatterPlot.MarkerSize = 0;

            // 添加报警阈值虚线
            _thresholdLine = plotControl.Plot.Add.HorizontalLine(0);
            _thresholdLine.Color = new ScottPlot.Color(231, 76, 60);
            _thresholdLine.LineWidth = 2;

            // 设置坐标轴文本
            plotControl.Plot.Title("温度实时曲线 (当前: ---℃)");
            plotControl.Plot.YLabel("温度 (℃)");
            plotControl.Plot.XLabel("采样点");

            // 设置中文字体
            const string chineseFont = "Microsoft YaHei UI";
            plotControl.Plot.Font.Set(chineseFont);
            plotControl.Plot.Axes.Title.Label.FontName = chineseFont;
            plotControl.Plot.Axes.Bottom.Label.FontName = chineseFont;
            plotControl.Plot.Axes.Left.Label.FontName = chineseFont;
            plotControl.Plot.Axes.Bottom.TickLabelStyle.FontName = chineseFont;
            plotControl.Plot.Axes.Left.TickLabelStyle.FontName = chineseFont;

            plotControl.Plot.Axes.AutoScale();
            plotControl.Refresh();
        }

        /// <summary>
        /// 更新曲线数据（左移追加新值）
        /// </summary>
        public void UpdateData(float temp, float alarmThreshold, ScottPlot.WinForms.FormsPlot plotControl)
        {
            // 数据左移
            Array.Copy(YData, 1, YData, 0, ScadaConstants.MaxDataPoints - 1);
            YData[ScadaConstants.MaxDataPoints - 1] = temp;

            if (_dataCount < ScadaConstants.MaxDataPoints)
                _dataCount++;

            // 更新标题
            plotControl.Plot.Title("温度实时曲线 (当前: " + temp.ToString("0.0") + "℃)");

            // 更新阈值线位置
            _thresholdLine.Y = alarmThreshold;

            // 自动调整Y轴范围
            int startIdx = ScadaConstants.MaxDataPoints - _dataCount;
            double yMin = YData[startIdx];
            double yMax = YData[startIdx];
            for (int i = startIdx + 1; i < ScadaConstants.MaxDataPoints; i++)
            {
                if (YData[i] < yMin) yMin = YData[i];
                if (YData[i] > yMax) yMax = YData[i];
            }

            if (Math.Abs(yMin - yMax) < 0.01)
            {
                yMin -= 5;
                yMax += 5;
            }
            else
            {
                yMin -= 5;
                yMax += 5;
            }

            plotControl.Plot.Axes.SetLimits(0, ScadaConstants.MaxDataPoints - 1, yMin, yMax);
            plotControl.Refresh();
        }

        /// <summary>
        /// 更新阈值线位置（阈值变化时调用）
        /// </summary>
        public void UpdateThresholdLine(float threshold, ScottPlot.WinForms.FormsPlot plotControl)
        {
            if (plotControl != null && plotControl.IsHandleCreated)
            {
                _thresholdLine.Y = threshold;
                plotControl.Refresh();
            }
        }

        /// <summary>
        /// 安全刷新曲线控件
        /// </summary>
        public void RefreshPlot(ScottPlot.WinForms.FormsPlot plotControl)
        {
            if (plotControl != null && plotControl.IsHandleCreated && !plotControl.IsDisposed)
            {
                plotControl.Refresh();
            }
        }
    }
}
