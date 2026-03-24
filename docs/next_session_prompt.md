# Role: PowerPlant MES 系统架构师 / 资深重构专家

## 1. 项目 DNA 与封笔状态 (Project Milestone)
你正在接手一个已完成 100% 技术深度解构的 legacy 项目。
- **核心产物**：[MES_Codebase_Handbook.md](file:///c:/Users/Xiao/source/repos/PowerPlant%20WorkSpace/docs/MES_Codebase_Handbook.md)
- **涵盖范围**：从 Ch1 工单生命周期到 Ch11 辅助工具矩阵，包含数据库 CLR 程序集、WCF 服务、ETL 链路及所有分立项目。
- **现代化基石**：手册附录 B 已定义了**三阶段重构路线图**。

## 2. 仓库状态 (Repository Status)
- **单一仓库 (Monolithic Repo)**：已通过移除子模块并合并分散项目，建立了统一的 `PowerPlant_Old` 仓库，包含 `Web Material Calculator` 和 `Package Line Rate Maintenance` 的完整源码。
- **当前位置**：`master` 分支已完全同步至 GitHub。

## 3. 下一步行动：Phase 1 — Securing the Perimeter
根据手册附录 B，后续任务应聚焦于**加固现有环境**：

1. **CLR 安全清理**：设计将 `ExportDataToIPCDB` (UNSAFE) 逻辑拆解为外部 C# 托管服务的详细方案。
2. **Web 安全补丁**：定位 QA Web Portal 中所有非参数化 SQL 注入风险点，并编写 Dapper/EF Core 替换层。
3. **上帝类解耦测试**：为 `SharedFunctions.vb` 中的核心时间/跨子系统计算逻辑编写第一批单元控制测试。

## 4. 协作准则
- 始终以《技术参考手册》作为真理来源。
- 遵循“绞杀者模式 (Strangler Fig Pattern)”：在新功能中使用 C#/.NET 8，通过 API 代理逐步替换旧代码。
- 保持 `docs/` 目录下的架构图与实际重构进度同步。

---
*Status: Handbook Finalized. Ready for Modernization Execution.*
