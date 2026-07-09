# 严重 Bug 修复摘要

**日期**: 2026-05-28  
**依据**: `comprehensive-review-modbus-scada-2026-05-28.md` 审查报告  
**文件**: `Form1.cs`

---

## 修复清单（4 项 P0 阻塞项）

### 1. 🟠 报警计数器重置 Bug（双成员独立检出）

| 项目 | 内容 |
|------|------|
| 位置 | `CheckAlarm()` 行 764 / 772 |
| 问题 | 触发报警时 `_alarmCount = AlarmConsecutiveCount (3)`；解除时 `_alarmCount = -AlarmConsecutiveCount (-3)`。导致解除后需 6 次连续超阈值才能重新触发（预期 3 次），死区意外扩大一倍 |
| 修复 | 两处均改为 `_alarmCount = 0` |

```csharp
// 修复前 (Line 711)
_alarmCount = AlarmConsecutiveCount;  // = 3
// 修复后
_alarmCount = 0;  // 修复: 重置为0而非+3，避免死区意外扩大一倍

// 修复前 (Line 719)
_alarmCount = -AlarmConsecutiveCount; // = -3
// 修复后
_alarmCount = 0;  // 修复: 重置为0而非-3，避免解除后需6次连续超阈值才能重新触发
```

### 2. 🟠 `_isConnected` 数据竞争

| 项目 | 内容 |
|------|------|
| 位置 | `Form1.cs` 行 36 |
| 问题 | 在 `lock` 内写入，在 `lock` 外读取（`PollingCallback` 行 458），无 `volatile`。弱内存序平台可能读到过期值 |
| 修复 | `private bool _isConnected` → `private volatile bool _isConnected` |

### 3. 🟠 温度传感器完全缺失异常值检测

| 项目 | 内容 |
|------|------|
| 位置 | `PollingCallback()` 行 482-505 |
| 问题 | 直接接受 `registers[0] / 100f`，无范围校验也无跳变检测。0℃ 漏报警，999℃ 误报警 |
| 修复 | 添加三层防护： |

- **范围校验**: `-40℃ ≤ rawTemp ≤ 125℃`（一般工业温度传感器量程）
- **跳变检测**: `|rawTemp - lastValid| ≤ 30℃`（单次温差 >30℃ 视为异常）
- **故障标记**: 异常时保留上次有效值，在 UI 显示"⚠传感器异常"（橙色文字），写 WARN 日志后 `return`（数据不参与报警判断和存储）

```csharp
// 新增字段
private float _lastValidTemp = float.NaN;  // 上一次有效温度值
private bool _sensorFault;                 // 传感器故障标志

// 新增常量
private const float TempMinValid = -40f;
private const float TempMaxValid = 125f;
private const float TempJumpThreshold = 30f;
```

### 4. 🔴 Modbus RTU 无安全防护（操作确认 + 审计日志）

| 项目 | 内容 |
|------|------|
| 位置 | `BtnFanOn_Click()` / `BtnFanOff_Click()` |
| 问题 | 任何可访问串口的进程均可读取温度、控制风扇。工业控制系统典型攻击面 |
| 修复 | 短期方案：操作前弹确认对话框 + 审计日志 |

```csharp
// 修复后 - 开风扇
var result = MessageBox.Show(
    "确认手动开启风扇？\n此操作将直接控制 Modbus 从站继电器。",
    "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
if (result != DialogResult.Yes) return;
_logger.Warn("审计: 操作员确认手动开风扇");
```

---

## 变更统计

| 变更类型 | 数量 |
|---------|------|
| 新增字段 | 2 (`_lastValidTemp`, `_sensorFault`) |
| 新增常量 | 3 (`TempMinValid`, `TempMaxValid`, `TempJumpThreshold`) |
| 修改行 | 6 行核心逻辑 + UI 显示逻辑 |
| 新增日志点 | 4 处 (2 审计 + 2 传感器异常) |

## 未修复项（后续工作）

以下 P0 项被标记但需架构层面重构，不在本次修复范围：

- **提取 AlarmManager 并编写单元测试** — 需先完成分层重构
- **配置外部化**（App.config）— P1 项
- **分层重构 Form1**（上帝类拆分）— P1，2-3 周工作量
- **SQLite WAL 模式 + 重连泄漏** — P1
- **锁策略重构**（锁内 I/O 移到锁外）— P1
