# ModbusScada — Modbus RTU 温度监控系统 v1.0

基于 .NET Framework 4.7.1 + WinForms 的桌面 SCADA 应用程序，通过 Modbus RTU 串行通信协议连接下位机（PLC/温度采集模块），实现实时温度监控、报警和继电器控制。

---

## 功能概览

| 模块 | 说明 |
|------|------|
| **串口连接管理** | 自动扫描 COM 口，连接/断开 Modbus 从站，支持自动重连（3 秒间隔） |
| **Modbus RTU 轮询** | 每秒读取保持寄存器（地址 0: 温度×100，地址 1: 继电器状态），含异常值校验 |
| **实时温度显示** | 大字数值显示，传感器异常时标黄警告，通信中断显示"通信异常" |
| **温度实时曲线** | ScottPlot 5 驱动，保留最近 300 个采样点（5 分钟窗口），自动缩放 Y 轴 |
| **温度报警** | 可配置阈值（0~200℃），死区防抖逻辑（连续 3 次确认触发/解除） |
| **手动风扇控制** | 通过 Modbus 写线圈控制继电器，含操作确认弹窗和审计日志 |
| **数据持久化** | SQLite 本地存储温度、继电器、报警状态（每 5 次轮询写一次） |
| **Web 数据推送** | HMAC-SHA256 签名，HTTP POST 实时数据到 Web 后端 |
| **结构化日志** | NLog 文件输出，按日归档保留 7 天 |

---

## 技术栈

| 类别 | 技术 | 版本 |
|------|------|------|
| 运行时 | .NET Framework | 4.7.1 |
| UI 框架 | WinForms | — |
| Modbus 协议 | NModbus4 | 2.1.0 |
| 图表 | ScottPlot + SkiaSharp | 5.1.58 / 3.119.0 |
| 数据库 | SQLite (System.Data.SQLite) | 1.0.119.0 |
| 日志 | NLog | 5.3.4 |
| JSON | Newtonsoft.Json | 13.0.4 |

---

## 项目结构

```
ModbusScada/
├── Form1.cs                  # 主窗体（UI + 服务编排）
├── Form1.Designer.cs         # 窗体设计器代码
├── Program.cs                # 应用入口
├── App.config                # 应用配置（Web推送密钥等）
├── build_and_run.bat         # 一键构建启动脚本
├── Models/
│   └── ScadaData.cs          # 全局常量 + 事件参数类
├── Services/
│   ├── SerialPortManager.cs  # 串口扫描/连接/自动重连
│   ├── ModbusDriver.cs       # Modbus 轮询/读写/校验
│   ├── AlarmManager.cs       # 死区防抖报警逻辑
│   ├── DataLogger.cs         # SQLite 数据持久化
│   ├── PlotManager.cs        # 实时曲线管理
│   ├── DataPushService.cs    # Web 数据推送（HMAC签名）
│   └── LogConfig.cs          # NLog 代码配置
├── document/                 # 工程文档
│   ├── architecture/         # 架构设计文档
│   ├── development/          # 开发指南
│   ├── user/                 # 用户手册
│   ├── api/                  # API 接口文档
│   └── deployment/           # 部署运维文档
└── deliverables/             # 交付物（审查报告等）
```

---

## 快速开始

### 环境要求

- Windows 7+ / Windows Server 2012+
- .NET Framework 4.7.1
- Visual Studio 2019/2022（开发）或仅运行时（部署）
- RS-232/RS-485 串口 + Modbus RTU 从站设备

### 构建与运行

**方式一：Visual Studio**

1. 打开 `ModbusScada.sln`
2. 还原 NuGet 包（右键解决方案 → 还原 NuGet 包）
3. 按 F5 启动调试

**方式二：命令行**

```cmd
build_and_run.bat
```

### 使用流程

1. 启动程序，在"串口设置"面板点击"刷新"扫描 COM 口
2. 选择目标串口，点击"连接"
3. 连接成功后自动开始 1 秒/次轮询，温度数值和曲线实时更新
4. 在"报警设置"中调整报警阈值（默认 50℃）
5. 需要时点击"开风扇"/"关风扇"手动控制继电器
6. 勾选"自动重连"可在通信中断后自动恢复

---

## 配置说明

编辑 `App.config` 中的 `appSettings`：

| 配置项 | 说明 | 默认值 |
|--------|------|--------|
| `WebApiUrl` | Web 后端地址 | `http://127.0.0.1:5000` |
| `ClientId` | HMAC 签名客户端 ID | `scada-push-client` |
| `SecretKey` | HMAC-SHA256 签名密钥 | 需自定义 |

修改 `Models/ScadaData.cs` 中的常量可调整：

| 常量 | 说明 | 默认值 |
|------|------|--------|
| `SlaveAddress` | Modbus 从站地址 | 1 |
| `PollingIntervalMs` | 轮询间隔（毫秒） | 1000 |
| `ReconnectIntervalMs` | 重连间隔（毫秒） | 3000 |
| `AlarmConsecutiveCount` | 报警确认次数 | 3 |
| `MaxDataPoints` | 曲线数据点数 | 300 |

---

## 文档索引

| 文档 | 路径 | 说明 |
|------|------|------|
| 架构设计文档 | `document/architecture/架构设计文档.md` | 系统架构、组件职责、线程模型 |
| 开发指南 | `document/development/开发指南.md` | 环境搭建、调试、测试策略 |
| 用户手册 | `document/user/用户手册.md` | 界面说明、操作流程、故障排查 |
| API 接口文档 | `document/api/接口文档.md` | Web 推送接口、签名算法 |
| 部署运维文档 | `document/deployment/部署运维文档.md` | 安装部署、配置调优、日常维护 |

---

## 已知限制

- 仅支持单 Modbus RTU 从站（从站地址硬编码为 1）
- 串口参数固定为 9600-8-N-1，不支持用户配置
- Web 推送使用 fire-and-forget 模式，推送失败仅记录日志
- 无单元测试覆盖
- 不支持历史数据查询（SQLite 仅写入，无查询 UI）

---

## 许可证

内部项目，未指定开源许可证。
