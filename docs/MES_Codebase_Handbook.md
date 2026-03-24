# MES System Codebase Handbook

## PowerPlant — Legacy VB.NET WinForms MES

> **Document Status:** Complete (Chapters 0–11 + §7.6 + App A). Final comprehensive edition.
> Ch0: Architecture, Ch1: Shop Order Lifecycle, Ch2: Pallet Management (+§2.8), Ch3: QAT Workflow,
> Ch4: Session & Authentication, Ch5: PLC/IPC Integration (+§5.7 PPTCPServer),
> Ch6: Shared Business Logic & Utilities (+§6.10 KronosETL, §6.11 PPWCFService),
> Ch7: Database Logic & SP Deep-Dive (+§7.5 ExportSQLDataToIPC, §7.6 Views/Functions/CLR Assembly),
> Ch8: QA Web Portal, Ch9: DownTime Management System, Ch10: SSRS Production Reporting,
> Ch11: Operations, Maintenance & Tooling Matrix (+§11.3 Probat, §11.4 ImportData, §11.5 Web Material Calc).

---

## Chapter 0 — First-Impression Architecture Analysis

### 0.1 Workspace Topology

```
PowerPlant WorkSpace/
├── PowerPlant/                    ← Main VS Solution (VS 2010 format, single .vbproj)
│   ├── PowerPlant.sln             ← Format Version 11.00 — VS 2010 origin
│   └── PowerPlant/                ← THE application project (~400 files)
│       ├── modStartup.vb          ← Entry point / command router
│       ├── SharedFunctions.vb     ← 414 KB — God-object utility module
│       ├── SQLHelper.vb           ← 145 KB — ADO.NET helper (Microsoft patterns)
│       ├── Models/                ← Entity Framework 5/6 EDMX (two models)
│       │   ├── PowerPlantModels.edmx        ← Server DB entities
│       │   └── PowerPlantLocalModels.edmx   ← Local SQLExpress entities
│       ├── frm*.vb                ← 50+ WinForms (Fat Forms)
│       ├── ds*.xsd + .Designer.vb ← 35+ Typed DataSets (auto-generated)
│       └── cls*.vb                ← Domain helper classes
│
├── DB Local_PowerPlant/           ← SQL Project — Local SQLExpress schema
│   └── dbo/ Tables/ (67 tables), StoredProcedures/, Views/, Functions/
│
├── DB Server_PowerPlant/          ← SQL Project — Production server schema
│   └── dbo/ Tables/ (155 tables), StoredProcedures/, Views/, Functions/
│
├── DB Local_ImportData/           ← SQL Project — Import data (local side)
├── DB Server_ImportData/          ← SQL Project — Import data (server side)
├── DB Server_Probat/              ← SQL Project — Probat roaster machine DB
│
└── Power Plant Codes/             ← Versioned code archives
    ├── Visual Studio 2010/
    ├── Visual Studio 2012/
    ├── Visual Studio 2015/
    └── Visual Studio 2019/
        └── Web Material Calculator/  ← Possibly a WebForms sub-project
```

> [!IMPORTANT]
> The workspace contains **two parallel SQL databases** (Local + Server) with a delta
> of **88 tables** between them. The server DB has 155 tables vs. the local DB's 67.
> This asymmetry is the #1 architectural risk for any future shared-schema migration.

---

### 0.2 System Identity & Business Domain

Based on the file and table names, this is a **Coffee/Food Manufacturing MES** deployed
at a production facility (likely Mother Parker's Tea & Coffee). Key business domains:

| Domain Code | Inferred Business Function |
|---|---|
| `ShopOrder` | Production work orders (start, stop, close) |
| `Pallet` | Physical pallet creation, labeling, history |
| `QAT*` | Quality Assurance Testing — 20+ subtypes |
| `SessionControl` | Per-workstation production session state |
| `BillOfMaterials` | Component/recipe tracking |
| `ComponentScrap` | Scrap material logging |
| `Shift` | Shift management |
| `DownTime` | Machine downtime logging |
| `ItemMaster` | Product/SKU catalog |
| `CheckWeigher` | In-line weight verification |
| `Probat` | Integration with Probat roaster equipment |
| `DynamicLabelData` | Label variable data for printing |
| `UnitCount Inbound/Outbound` | Unit production counting |

---

### 0.3 Application Startup Router Pattern

`modStartup.vb` acts as the **sole application entry point** and is a pure command-line
argument router. It is not a typical `Application.Run(New frmMain)` pattern.

**Flow:**

```
PowerPlant.exe <programID> [arg1] [arg2]
       │
       ▼
modStartup.Main()
  ├── Single-instance guard (Win32 SetForegroundWindow)
  ├── Wait for local SQLExpress service (up to 200 x 1s retries)
  ├── RefreshComputerConfig()  → loads gdrCmpCfg (workstation config row)
  ├── RefreshSessionControlTable() → loads gdrSessCtl (current production session)
  └── Select Case strPgmID
          "logon"           → frmLogOn
          "logon_noplc"     → frmLogOn_NoPLC
          "mainmenu"        → frmMainMenu
          "startshoporder"  → frmStartShopOrder
          "stopshoporder"   → frmStopShopOrder
          "printcaselabel"  → (inline logic) → SharedFunctions.printCaseLabel()
          "createpallet"    → frmCreatePallet
          "autocreatepallet"→ SharedFunctions.AutoCreatePallet()
          "palletlabels"    → frmPrtPalletLabels
          "inquiry"         → frmInquiry
          "bom"             → frmBillOfMaterials
          "logscrap"        → frmLogScraps
          "qat_startup"     → frmQATStartUpChecks
          "qat_*"           → 15 more QAT forms
          "shutdown"        → frmShutDown
          ...
```

**Architectural Implications:**

- Each production-line workstation runs `PowerPlant.exe` with a different `programID`.
  Multiple physical machines share the same binary, differentiated by argument.
- `gdrCmpCfg` (Computer Config) and `gdrSessCtl` (Session Control) are **global
  singletons** stored as module-level `DataRow` objects — effectively global state.

> [!WARNING]
> All global state lives in `modStartup.vb` and is accessed via public module-level
> variables (e.g., `gdrCmpCfg`, `gdrSessCtl`, `gstrServerConnectionString`).
> These are the highest-priority candidates for refactoring into a proper
> `IApplicationSession` / `IWorkstationContext` service.

---

### 0.4 Data Access Layer Anatomy

The system uses **three overlapping data access strategies** simultaneously:

#### Strategy 1 — Typed DataSets + TableAdapters (Primary)

35+ `.xsd` files auto-generate strongly-typed `DataSet` / `DataTable` / `TableAdapter`
classes. All TableAdapters point to `LocalPowerPlantCnnStr` or `ServerPowerPlantCnnStr`
as configured in `app.config`.

```
Naming pattern: ds<Entity>.xsd  →  ds<Entity>.Designer.vb
Examples:
  dsShopOrder.xsd        → dsShopOrderTableAdapters.CPPsp_ShopOrderIOTableAdapter
  dsPallet.xsd           → dsPalletTableAdapters.CPPsp_PalletIOTableAdapter
  dsSessionControl.xsd   → dsSessionControlTableAdapters.CPPsp_SessionControlIOTableAdapter
```

TableAdapters call **named stored procedures** (e.g., `CPPsp_ShopOrderIO`,
`CPPsp_PalletIO`) — the naming convention `CPP` suggests "Coffee Plant PowerPlant".

#### Strategy 2 — Entity Framework 4/5 (Secondary, selective use)

Two EDMX models exist under `Models/`:

- `PowerPlantModels.edmx` → Server DB (243 KB of generated code → large model)
- `PowerPlantLocalModels.edmx` → Local DB (198 KB of generated code)

Both are connected via `app.config` `PowerPlantEntities` / `PowerPlantLocalEntities`.
EF appears to have been added to the project mid-life (VS 2015 era) for specific
new features without replacing the existing TypedDataSet approach.

#### Strategy 3 — SqlHelper + Raw ADO.NET (SharedFunctions.vb)

`SQLHelper.vb` is the **Microsoft Data Access Application Block** (circa 2002),
embedded directly in the project. It provides:

- `SqlHelper.ExecuteNonQuery()` — DML without result sets
- `SqlHelper.ExecuteDataset()` — returns `DataSet`
- `SqlHelper.ExecuteReader()` — returns `SqlDataReader`
- `SqlHelper.ExecuteScalar()` — single value

`SharedFunctions.vb` (414 KB — the largest file) consumes `SqlHelper` to execute
inline SQL or stored procedure calls using `gstrServerConnectionString` and
`gstrLocalDBConnectionString` from global state.

> [!CAUTION]
> The connection strings in `app.config` contain a **hardcoded server name**:
> `Data Source=MPHOPP02`. This is a production server name baked into the
> UAT configuration. Deployments to different environments require manual editing.
> In a refactored system, this must be environment-variable driven.

---

### 0.5 Dual-Database Synchronization Architecture

This is the most architecturally distinctive feature of the system:

```
Production Line Workstation               Central Server
   (SQL Express — LocalPowerPlant)        (SQL Server — PowerPlant_HO_UAT)
          │                                        │
          │  tblSessionControl (live state)        │  tblSessionControlHst (history)
          │  tblPallet (active pallets)            │  tblPalletHst (pallet history)
          │  tblShopOrder (active orders)          │  tblShopOrderHst
          │  tblItemMaster (cached lookup)         │  tblItemMaster (master)
          │  tblComputerConfig (this machine)      │  tblComputerConfig (all machines)
          │                                        │
          └──────── Sync process ──────────────────┘
                  (tblDownLoadTableList controls
                   what gets pushed Local→Server)
```

**Key sync tables in server DB (not in local DB — 88 extra tables):**

- `tblRoastingLog`, `tblGrindingLog`, `tblFGColourLog` — Process history
- `tblCaseWeightLog`, `tblWCWeightLog` — Weight audit trails
- `tblDynamicLabelData`, `tblLabelPrintLog` — Label audit
- `tblColourTolerance`, `tblMoistureSpec`, `tblFinalTempSpec` — Large spec tables
- `tblSessionControlHstAudit`, `tblComponentScrapAudit` — Audit trails

The `dsDownLoadTableList` dataset controls which server tables get pushed down to
local machines — a proprietary table-sync mechanism (not SQL Replication or SSIS).

---

### 0.6 Module / Form Taxonomy

#### Core Production Workflow Forms

| Form | Business Function | Key Tables |
|---|---|---|
| `frmLogOn` | Pallet Station operator login | `tblSessionControl`, `tblComputerConfig` |
| `frmLogOn_NoPLC` | Standard workstation login (no PLC) | `tblSessionControl` |
| `frmMainMenu` | Navigation hub | — |
| `frmStartShopOrder` | Begin a work order (79 KB) | `tblShopOrder`, `tblSessionControl`, `tblItemMaster` |
| `frmStopShopOrder` | Close a work order (34 KB) | `tblShopOrder`, `tblToBeClosedShopOrder`, `tblSessionControl` |
| `frmCreatePallet` | Manual pallet creation (38 KB) | `tblPallet`, `tblSessionControl`, `tblShopOrder` |
| `frmCreatePallet_PS` | Pallet Station variant (23 KB) | `tblPallet` |

#### Label Printing Forms

| Form | Business Function |
|---|---|
| `frmPrtCaseLabelNoSO` | Print case label without shop order |
| `frmPrtCaseLabelWithSO` | Print case label with shop order (34 KB) |
| `frmPrtPalletLabels` | Print pallet labels (12 KB) |
| `frmPrtLabelsAndControl` | Combined label + control print |
| `frmRePrtPalletLabels` | Re-print pallet labels |
| `frmLabelPrintQueue` | View print queue |

#### QAT (Quality Assurance Testing) Forms — 20 forms

```
frmQATStartUpChecks       → Startup verification checklist
frmQATLineClearance       → Line clearance before new order
frmQATMaterialValidation  → Raw material validation
frmQATSmallestUnitWeight  → Smallest unit weight check
frmQATCheckWeigherValidation → In-line checkweigher calibration
frmQATOxygen              → Oxygen level measurement
frmQATPressure            → Pressure measurement
frmQATShakeShine          → Shake/shine visual test
frmQATLidPeel             → Lid peel force test
frmQATCaseDateCoder       → Case date code verification
frmQATCartonDateCoder     → Carton date code verification
frmQATPackageDateCoder    → Package date code verification
frmQATCamerasVerification → Camera system check
frmQATCartonBoxVisualCheck→ Carton box visual inspection
frmQATCaseVisualVerification → Case visual inspection
frmQATCheckWeigherValidation → Checkweigher calibration
frmQATOverrideLogOn       → Supervisor override for QAT failure
frmQATRejectResult        → Record reject result
frmQATSeletePalletType    → Select pallet type
frmQATWorkflow            → QAT workflow orchestrator
```

#### Utility / Inquiry Forms

| Form | Business Function |
|---|---|
| `frmInquiry` | General inquiry screen |
| `frmPalletInquiry` | Pallet lookup |
| `frmBillOfMaterials` | View BOM for current order |
| `frmShopOrderNotes` | Notes for current shop order |
| `frmShopOrderSchedule` | Schedule view |
| `frmLogScraps` / `frmLogScrapsRejectPoint` | Scrap recording |
| `frmProcessMonitor` | Real-time process monitoring |
| `frmChangePassword` | User password change |
| `frmCalculator` | On-screen calculator (touchscreen) |

#### Shared UI Utilities

- `frmNumKeyPad`, `frmRegularKeyPad`, `frmAlphaNumKB` — On-screen keyboards
  (indicates touchscreen deployment on shop floor)
- `frmMsgBox` — Custom message box
- `frmSplash` — Splash screen
- `frmShutDown` — Controlled shutdown

#### Domain Helper Classes

| Class | Responsibility |
|---|---|
| `clsCaseCount.vb` (25 KB) | Case counting logic / counter state |
| `clsShift.vb` | Shift boundary calculations |
| `clsCheckWeigher.vb` | Checkweigher device interface |
| `clsTCPClient.vb` (9 KB) | TCP communication (PLC/IPC) |
| `clsXMLInterface.vb` | XML-based inter-process data exchange |
| `clsSessionControl.vb` | Session state helpers |
| `clsExpiryDate.vb` | Expiry date calculation |
| `clsPrinterDevice.vb` | Printer device abstraction |
| `clsMidnightScheduler.vb` | Midnight job trigger |
| `clsCoLOSCLI.vb` | CoLOS label system CLI wrapper |

---

### 0.7 Technology Risk Assessment

#### Hardcoded Configuration (Critical Risk)

| Location | Hardcoded Value | Risk |
|---|---|---|
| `app.config` line 9 | `Data Source=MPHOPP02` | Environment portability |
| `app.config` line 41 | `C:\PowerPlant\DownTime\DownTime.exe` | Absolute path |
| `app.config` line 44 | `C:\Program Files\TouchScreen [XP] V.6.7.COM\...` | XP-era binary |
| `app.config` line 50 | `C:\Program Files\Elo TouchSystems\EloVa.exe` | Vendor-specific |
| `app.config` line 59 | `C:\Program Files (x86)\TouchKit\xAuto4PtsCal.exe` | Absolute path |
| `app.config` line 62 | `C:\PowerPlant\PPTCPServer\PPTCPServer.exe` | Companion service |

#### Runtime Dependency (Critical Risk)

- **Target Framework**: `v2.0.50727` (`.NET Framework 2.0`) — declared in `app.config`
  startup section. This is .NET 2.0, though the actual project likely targets 3.5 or 4.x
  given EF usage. Needs verification against `.vbproj` `TargetFrameworkVersion`.
- **PPTCPServer.exe**: A companion TCP server process (`C:\PowerPlant\PPTCPServer\`)
  that handles PLC/IPC communication. This is a separate deployment unit not in the
  solution — must be documented separately.
- **DownTime.exe**: External downtime monitoring application. Also a separate deployment.
- **Touch Screen Drivers**: Three different touch screen vendors supported (XP COM port,
  Elo USB, TouchKit) — suggests long hardware evolution.

#### Data Access Risk

- **Mixed ORM**: TypedDataSets + EF4 + raw ADO.NET coexist. EF model schema drift vs.
  actual DB schema cannot be detected at compile time.
- **Global connection strings**: `gstrServerConnectionString` and `gstrLocalDBConnectionString`
  are module-level strings mutated at startup — not thread-safe if any async code is added.
- **No connection pooling strategy**: Each `SqlHelper` call opens/closes connections
  individually; TableAdapters manage their own connections. At scale, this creates
  contention.

#### Coupling Risk

- `SharedFunctions.vb` is a 414 KB God-module containing business logic,
  UI helpers, database calls, file I/O, printer management, and utility functions.
  It is referenced by virtually every form in the project.
- All forms directly read global module variables in `modStartup.vb` (e.g.,
  `gdrCmpCfg`, `gdrSessCtl`, `gdrEquipment`). No dependency injection.

---

### 0.8 Refactoring Blueprint (High Level)

The following layered extraction is recommended for migration to .NET Core / microservices:

#### Phase 1 — Extract Configuration

```
app.config (hardcoded) → appsettings.json + environment variables
My.Settings               → IOptions<WorkstationSettings>
gstrServerConnectionString → IDbConnectionFactory (configurable per environment)
```

#### Phase 2 — Extract Domain Services from SharedFunctions.vb

| Service | Extracts from SharedFunctions |
|---|---|
| `IShopOrderService` | StartShopOrder, StopShopOrder, GetCurrentShopOrder |
| `IPalletService` | CreatePallet, AutoCreatePallet, ClosePallet |
| `ILabelPrintService` | printCaseLabel, printPalletLabel, CoLOS integration |
| `ISessionService` | RefreshSessionControlTable, RefreshComputerConfig |
| `IQATService` | QAT workflow orchestration |
| `IShiftService` | GetCurrentShift, ShiftChange logic |
| `IScrapService` | LogScrap, LogRejectPointScrap |

#### Phase 3 — Decompose Fat Forms

Each WinForms form should be split into:

```
frmStartShopOrder.vb (79 KB)
  ├── StartShopOrderViewModel  (state)
  ├── IShopOrderService.StartAsync()  (business logic)
  └── StartShopOrderView (UI only — bind to ViewModel)
```

#### Phase 4 — Unify Data Access

```
TypedDataSets + TableAdapters ] 
EF4 EDMX                      ] → Single EF Core DbContext per bounded context
Raw SqlHelper calls            ]
```

#### Phase 5 — Extract Deployment Units

```
PPTCPServer.exe     → IPC/PLC Gateway microservice (.NET Core Worker Service)
DownTime.exe        → Downtime Monitor microservice
PowerPlant.exe      → Shell application (thin WinForms or Blazor Desktop)
```

---

### 0.9 Key Database Table Cross-Reference

The following core tables are present in **both** Local and Server databases (sync candidates):

| Table | Local DB | Server DB | Business Meaning |
|---|---|---|---|
| `tblSessionControl` | ✓ | ✓ | Active production session per workstation |
| `tblShopOrder` | ✓ | ✓ | Active work orders |
| `tblItemMaster` | ✓ | ✓ | Product/SKU master data |
| `tblComputerConfig` | ✓ | ✓ | Workstation configuration |
| `tblPallet` | ✓ | ✓ | Active pallets |
| `tblBillOfMaterials` | ✓ | ✓ | Recipe/BOM |
| `tblShift` | ✓ | ✓ | Shift definitions |
| `tblEquipment` | ✓ | ✓ | Equipment registry |
| `tblDownTimeLog` | ✓ | ✓ | Downtime events |
| `tblQAT*` | ✓ (subset) | ✓ (full) | QAT results |

Tables **only on Server** (reporting / history / master data):

- `tblRoastingLog`, `tblGrindingLog` — Process logs from specialized equipment
- `tblCaseWeightLog`, `tblWCWeightLog` — Weight audit
- `tblDynamicLabelData`, `tblLabelPrintLog` — Label history
- `tblSessionControlHst`, `tblPalletHst` — Archive tables
- `tblColourTolerance`, `tblMoistureSpec`, `tblFinalTempSpec` — Large spec tables

---

### 0.11 The Big Picture Navigation (Data Lifecycle)

An essential concept for understanding the full PowerPlant ecosystem is recognizing the **Data ID Card** principle. Code modifications require knowing exactly which "relay zone" the execution resides in. Here are the three main execution flows that loop across the 33 projects and 4 databases:

> [!TIP]
> **The 3 Core Data Relays**
>
> 1. **The Configuration Relay (Top-Down)**
>    `QA Web Sites (Ch8)` → Updates `tblQAT*` in Central DB (`PowerPlant_Prd` / `PowerPlantAXSP_Dev`) → `ExportSQLDataToIPC.exe` (Ch7.5) pulls to IPC → Updates `LocalPowerPlant` (SQLEXPRESS) → `frmQAT*` reads local DB to dynamically render UI (Ch3).
>    *(Why this matters: Modifying a setting on a webpage won't reflect on the IPC touch screen until `ExportSQLDataToIPC` completes its 5-minute sync loop.)*
>
> 2. **The Production Printing Relay (Horizontal)**
>    Operator presses print on `frmCreatePallet` (Ch1/2) → Updates `tblPrintRequest` on Central DB → `srvLabelPrinting` Windows Service (Ch2.8) polls DB → Formats CoLOS CLI payload → `CoLOS Server` parses command via TCP → Translates to Hardware Dialect (ZPL) → Triggers physical Zebra/Markem printer.
>    *(Why this matters: There is NO ZPL in this codebase. Hardware formats are abstracted to CoLOS.)*
>
> 3. **The Data Telemetry & Feedback Relay (Bottom-Up)**
>    Checkweigher hardware triggers PLC (Ch5) → `PPTCPServer` (Port 8000) parses raw stream → Caches to `tblWeightLog` (Local DB) → `ExportSQLDataToIPC.exe` synchronizes to Central DB → `KronosETL` Service (Ch6.10) injects labor constraints → SSRS Reports (Ch10) generate management dashboards based on TVFs.
>
> **The Golden Rule**:
> Never assume immediate data propagation across boundaries. The architecture relies heavily on asynchronous queue tables (`tblPrintRequest`, `tblDownLoadTableList`, `tblPrintStatus`) rather than synchronous remote procedure calls.

---

*End of Chapter 0 — Total source files analyzed: Complete Ecosystem Enumerated*

### Refactoring Scorecard: Architecture Foundational Limits

| Factor | Score (1-10) | Description | Actionable Recommendation |
|---|---|---|---|
| **Fragility** | **8** | Heavy reliance on asynchronous sync tools and polling tables can hide network delays. | Harden the synchronous feedback loops or implement modern WebSockets for UI updates instead of polling loops. |
| **Security** | **7** | Shared DB credentials in multiple config files. | Consolidate connection strings into encrypted KeyVaults or protected config files. |
| **Complexity** | **6** | Distributed processing across disparate legacy tech stacks. | **Targeted Modernization**: Migrate WCF and ASMX endpoints to REST APIs. |

## Chapter 1: Shop Order Lifecycle (Technical Reference)

This chapter provides a pure technical breakdown of the Shop Order Lifecycle, focusing on how the business logic is implemented in the existing VB.NET WinForms codebase. It traces the execution path from the UI through to the database persistence layer, mapping the specific variables, states, and stored procedures involved.

### 1.1 Code Execution Path: Starting a Shop Order

When an operator clicks the **Start** button (tnStartShopOrder or tnStartWithNoLabel) on rmStartShopOrder.vb, the following execution path is triggered:

1. **UI Validation (Form Level)**:
   - Validates the entered   xtShift.Text using the WorkShift class.
   - Validates the   xtOperator.Text and Utility Technicians via SharedFunctions.GetStaffName().
   - CheckstxtPkgLine (Packaging Line) and compares it with the drSO("PackagingLine") from the bound dataset.
   - Validates numerical inputs liketxtCFCases (Carried Forward Cases) andtxtBagLengthUsed.

2. **Pre-Commit Form Logic (tnStartShopOrder_Click)**:
   - Parses the selected gintShopOrder.
   - Populates shift variables and packaging line overrides.
   - Captures User IDs for the Operator and up to 4 Utility Technicians.

3. **Core Commit (SharedFunctions.StartShopOrderUpdate())**:
   The form delegates the actual start business logic to this 400+ line utility function.
   - **Transaction Initialization**: Opens cnnLocal (SQL Express) and cnnServer (Central SQL Server). It begins a SqlTransaction (  rnServer) on the Central Server.
   - **QAT (Quality Assurance Test) Testers**: If gdrCmpCfg.QATWorkFlowInitiation is active, invokes UpdateQATTesters() to write the operator/tech IDs directly into the quality module.
   - **Staffing Record Update**: Loops through the 4 Utility Technicians and calls SharedFunctions.UpdateOperationStaffing(), executing CPPsp_OperationStaffingIO against the Local DB.
   - **Label Data Flushing**: Calls ClearLabelData() to wipe out previous label records for the work center.
   - **Label Request Generation**: Depending on the available printer hardware (oPrintDevice), it calls CreateLabelData() and CreatePrintRequest() for:
     - Case Printers (CASELABELER)
     - Package Coders (PACKAGECODER, adjusting IDs if the machine is a 3980)
   - **Database Status Change**: Calls ChangeSOStatus() (which executes PPsp_ShopOrderStatus on the Server and CPPsp_ShopOrderIO on Local) to mark the Shop Order as Open/Active.
   - **Auto-DownTime Handling**: Resolves any lingering downtime codes automatically if configured.
   - **Session Control Update**: The final critical step calls SharedFunctions.UpdateSessionControl(), which pushes the active state to the database, effectively making this Shop Order "live" on the specific Packaging Line.

### 1.2 Code Execution Path: Stopping a Shop Order

When an operator clicks **Stop/Close** (tnStopShopOrder) on rmStopShopOrder.vb:

1. **State Calculation (Form Level)**:
   - Queries SharedFunctions.GetSOCasesProducedFromPallet() to calculate intCasesProducedInShift.
   - Looks up   blSCH (Session Control History) using SharedFunctions.GetSessionControlHst("SelectLastRecByLineSO", ...) to find out if this order was already stopped previously in this shift.
   - Calculates intCarriedForwardCases and intLooseCases.
   - Calculates gintTotalProduced and remaining quantities.
   - Evaluates Pallet states (e.g., handles the tnPalletNotFull partial pallet rules).

2. **Downtime Check**:
   - Checks if SharedFunctions.IsProcessActive("DownTime") is true; if so, prompts the user to clear downtime before allowing the stop.

3. **Core Commit (SharedFunctions.StopShopOrderUpdate())**:
   - Updates QAT module status via SharedFunctions.GetQATStatus() and UpdateQATStatus().
   - Triggers SharedFunctions.RefreshSessionControlTable(), writing the *end state* of the workstation back to the operational Local DB.
   - If the lnCloseShopOrder flag is true, invokes SharedFunctions.ChangeSOStatus(..., Closed) and calls AutoCreatePallet() for any dangling loose cases.

### 1.3 State Management & Global Variables

The application relies heavily on global state variables defined in modStartup.vb. These variables dictate the flow of the Shop Order Lifecycle and are referenced universally:

| Global Variable | Type | Purpose |
|---|---|---|
| gdrSessCtl | SessionControlRow | **The master state of the workstation.** Contains the Facility, DefaultPkgLine, current Operator, current ShopOrder, ShiftProductionDate, and system flags. Overwritten by database reads at form launch. |
| gdrCmpCfg | ComputerConfigRow | Holds workstation configuration: WorkShiftType, QATWorkFlowInitiation, printer configurations, and hardware paths. |
| gblnSvrConnIsUp | Boolean | Tracks the live status of the connection to the Central Server. Checked extensively before making cnnServer calls. |
| gstrLotID | String | Holds the currently active Lot Number for the Shop Order, used for label printing. |
| gtblSCH | DataTable | Holds the Session Control History rows to calculate carried forward and loose cases. |
| gintShopOrder | Integer | The active Shop Order ID passed between forms. |

### 1.4 Data Persistence Mapping

Database operations are split between the **Local SQL Express** instance and the **Central SQL Server**.

#### Core Database Tables Involved

| Table | Role in Lifecycle | DB Location |
|---|---|---|
|   blSessionControl | Represents the *current real-time state* of the workstation (What order is running right now?). Inserted/Updated constantly. | Local & Server |
|   blSessionControlHst | Historical ledger of all started/stopped sessions. Read during form load to calculate loose cases. | Local & Server |
|   blShopOrder | The actual order data (Item, Quantity, Formats). Read to bind UI fields. Updated to set Status. | Local & Server |
|   blOperationStaffing | Logs the utility technicians active during the order. | Local |
|   blDynamicLabelData /   blLabelPrintLog | The staging tables for CoLOS/Markem printer integration. Written to during start. | Server Only |

#### Identified Stored Procedures

The application uses SqlHelper.ExecuteNonQuery and ExecuteDataset extensively. Key stored procedures used in this lifecycle:

**Local Database (SQL Express):**

- CPPsp_SessionControlIO (Reads/Writes current workstation state)
- CPPsp_SessionControlHstIO (Reads session history)
- CPPsp_ShopOrderIO (CRUD for local order cache)
- CPPsp_CasesProducedFromPallet (Calculates production totals)
- CPPsp_OperationStaffingIO (Writes utility tech logs)

**Central Database (SQL Server):**

- PPsp_ShopOrderStatus (Updates the canonical status of the order)
- PPsp_CreateLabelData (Generates the printer string format)
- PPsp_CreatePrintRequest
- PPsp_GetCurrentActiveShopOrder

### 1.5 Embedded Business Rules (Hardcoded Checks)

The VB.NET codebase contains several hardcoded business rules regarding Shop Orders:

1. **Shift Number Validation**: Shift numbers must match the `tblShift` table, but the code explicitly blocks starting an order if the entered shift is not the "current, previous, or next" shift based on Now().
2. **Line Mismatch Warning**: If `txtPkgLine`.Text override differs from the drSO("PackagingLine") in the database, the system permits it but pops a MessageBox warning.
3. **Bag Length Constraint**: If BagLengthRequired = "Y" in the order, the `txtBagLengthUsed` must be provided and > 0, otherwise the stop form validates and blocks closure.
4. **Carried Forward Cases limits**: `txtCFCases` cannot be < 0 and cannot exceed drSO.QtyPerPallet.
5. **Pallet Counting (CaseCounter)**: If gblnAutoCountLine = True, the system overrides manual inputs and reads from CaseCounter.CasesProducedRunningTotal, automatically determining if `btnPalletFull_Click` or `btnPalletNotFull_Click` logic applies based on Mod drSO.QtyPerPallet > 0.

### Refactoring Scorecard: Chapter 1 (Shop Order Lifecycle)

| Factor | Score (1-10) | Description | Actionable Recommendation |
|---|---|---|---|
| **Fragility** | **7** | Status transitions are scattered across WinForms events and Stored Procedures. | Consolidate logic into a strict state machine pattern in the C# Domain layer. |
| **Security** | **3** | Generally isolated physical access limits exposure. | Standardize permission validation beyond `tblPlantStaff`. |
| **Complexity** | **6** | Extensive use of legacy disconnected DataSets. | Migrate to EF Core repositories for single-source-of-truth updates. |

*(End of Chapter 1)*

## Chapter 2: Pallet Management (Technical Reference)

This chapter documents the technical implementation of the Pallet Management lifecycle in the PowerPlant MES system, covering pallet creation path, label generation, database write behavior, and embedded business rules.

---

### 2.1 Code Execution Path: Manual Pallet Creation

The system exposes two forms for pallet creation:

- **`frmCreatePallet.vb`** — The standard shop-floor form. All shop order and line context is auto-populated from `gdrSessCtl`.
- **`frmCreatePallet_PS.vb`** — The Pallet Station variant. The operator must manually enter `txtShopOrder`, `txtPkgLine`, and a split Production Date (`txtProdYear`, `txtProdMonth`, `txtProdDay`). Also validates that the line is not a Probat DMS-enabled line via `SharedFunctions.IsActiveProbatEnableLine()`.

Both forms share an identical commit sequence after validation passes.

#### Pre-Commit Validation Sequence (btnCreatePallet_Click)

The following gate is enforced in order before any DB call:

1. **Expiry Date** (`gblnReqExpiryDate = True`): All 3 fields required, year must be within `±3 years` of current year, and the combined date must be parseable.
2. **Pallet Full / Not Full**: `intPalletFull` must be `True` or `False`. Value `2` = unset (initial state) = blocked.
3. **Last Pallet flag**: `intLastPallet` must be `True` or `False`. Value `2` = unset = blocked.
4. **Cases In Pallet**: If `intPalletFull = False`, `txtCasesInPallet.Text` must be non-empty, non-decimal, > 0, and <= `gintQtyPerPallet`.
5. **Rate-limit check** (WO#34957): Reads `GetConrolTableValues("PalletCreationTimeLimit")` and `GetIPCControl("PreviousPalletCreationTime")`. If elapsed time < limit, creation is blocked.
6. **Output Location** (`gblnEnableOutputLocationLine = True`): Destination packaging line resolved from `dsOutputLocation` via `SharedFunctions.GetCurDestinationShopOrder()`.

#### Commit Call Chain

```
btnCreatePallet_Click
  └─> SharedFunctions.ProcessFrmCreatePallet(Facility, ShopOrder, ItemNo, PkgLine, Operator, Quantity, ...)
        ├─> Open cnnServer → BEGIN TRANSACTION (trnServer)
        ├─> CreatePallet(...)
        │     ├─> PPsp_GetPalletNo          [Server DB] → returns next integer PalletID
        │     └─> CPPsp_Pallet_Add / INSERT tblPallet  [Local DB]
        ├─> UPDATE tblSessionControl SET LooseCases=0, CasesProduced+=qty, PalletsCreated+=1  [Local DB, inline SQL]
        ├─> CreateLabelData() → PPsp_CreateLabelData  [Server DB, within trnServer]
        ├─> CreatePrintRequest() → queues label job for PALLETLABELER device
        ├─> EditPallet("SubmitedToPrint", intPalletID)  [updates tblPallet.PrintStatus]
        ├─> IPCControlUpdate("PreviousPalletCreationTime", Now())  [Local DB]
        ├─> trnServer.Commit()
        └─> _dbServer.SaveChanges()   [EF context flush, after ADO.NET commit]
```

If `intLastPallet = True`, after successful creation, `frmStopShopOrder.ShowDialog()` is called directly.

---

### 2.2 Pallet ID Generation

The Pallet ID is generated server-side, not client-side:

1. `CreatePallet()` checks: if `intPalletID = 0` AND `gblnSvrConnIsUp = True`, calls **`PPsp_GetPalletNo`** on the Central Server via `SqlHelper.ExecuteScalar()`.
2. If the server is offline (`gblnSvrConnIsUp = False`), `intPalletID` remains `0` and the pallet is inserted locally without a server-allocated ID. These are reconciled during the background sync process.
3. The **Pallet Label Job Name** is constructed as: `strDefaultPkgLine & CType(intPalletID, String)` — a simple string concatenation used to identify the label print job in the CoLOS/Markem print queue.

---

### 2.3 Auto-Create Pallet Logic

`SharedFunctions` exposes two overloaded versions of `AutoCreatePallet()`:

| Overload | Trigger | Key Identifier | Use Case |
|---|---|---|---|
| Overload 1 (WO#5370) | PLC/Indusoft SCADA signal | `strCreator = "autocreatepallet - Indusoft"` | Automatic counting lines — PLC signals a completed pallet count |
| Overload 2 (WO#15635) | `frmStopShopOrder` at session end | `strCreator = "autocreatepallet - Indusoft"` | Converts loose cases into a trailing partial pallet |

Both overloads:

1. Fill `tblSO` via `gtaSO.Fill("GetSO&Item")` from the Local DB.
2. Validate `intQtyInPallet <= dr.QtyPerPallet`.
3. Call `SharedFunctions.IsItemChangedOnServer(dr)` — if the Item Master has changed on the server since last sync, creation is blocked with a `"Data Integrity Error"`.
4. Call `SharedFunctions.ProcessFrmCreatePallet(...)` — the same commit chain as manual creation.

---

### 2.4 Label Generation & Printing

After `CreatePallet()` returns a valid `intPalletID`:

1. **`CreateLabelData()`** populates `tblDynamicLabelData` on the Server DB with formatted label strings via stored procedure `PPsp_CreateLabelData`.
2. **`CreatePrintRequest()`** creates a print job entry targeting the hardware device identified as `PALLETLABELER`. Job name = `DefaultPkgLine + intPalletID`.
3. **`EditPallet("SubmitedToPrint", intPalletID)`** updates `tblPallet.PrintStatus` from `0` (unprinted) to the submitted-to-print status.
4. Physical printing is handled by the **`clsPrinterDevice`** class, which resolves the printer hardware from `gdrCmpCfg` (`tblComputerConfig`).

---

### 2.5 Data Persistence: Local vs Server DB

| Operation | Target DB | Mechanism |
|---|---|---|
| Insert pallet record | **Local SQL Express** | `CPPsp_Pallet_Add` SP |
| Get next Pallet ID | **Central SQL Server** | `PPsp_GetPalletNo` SP (within server transaction) |
| Update `tblSessionControl` (LooseCases, CasesProduced, PalletsCreated) | **Local SQL Express** | Inline SQL via `SqlHelper.ExecuteNonQuery` |
| Insert label format data | **Central SQL Server** | `PPsp_CreateLabelData` SP (within server transaction) |
| Queue print request | **Central SQL Server** | `PPsp_CreatePrintRequest` SP (within server transaction) |
| Update `PrintStatus` (`EditPallet`) | **Local SQL Express** | Direct table update |
| Timestamp rate-limit (`PreviousPalletCreationTime`) | **Local SQL Express** | `IPCControlUpdate()` |
| Upload pallet to historical archive (`tblPalletHst`) | **Central SQL Server** | Done asynchronously by the background sync (`tblDownLoadTableList`), not by `CreatePallet()` |

> `tblPallet` exists in **both** Local and Server databases. The Local copy is the operational store; the Server `tblPalletHst` is the audit archive. `CreatePallet()` writes to Local only. The sync process is responsible for promotion to the Server.

---

### 2.6 Full Pallet vs Partial Pallet: State Field Behavior

The distinction is controlled entirely by `intQuantity` passed to `CreatePallet()`, which is derived from the UI state flags:

| Scenario | UI State | `intQuantity` value | DB record |
|---|---|---|---|
| Full Pallet (`btnPalletFull`) | `intPalletFull = True` | `gintQtyPerPallet` (from Item Master) | `tblPallet.Quantity = QtyPerPallet` |
| Partial Pallet (`btnPalletNotFull`) | `intPalletFull = False` | `CType(txtCasesInPallet.Text, Integer)` | `tblPallet.Quantity < QtyPerPallet` |
| Auto-Create (PLC signal) | n/a | `intQtyInPallet` from PLC | Same path; blocked if > `QtyPerPallet` |

`tblPallet` does not contain a dedicated `IsFullPallet` boolean column. Fullness is inferred at query time by comparing `tblPallet.Quantity` against `tblShopOrder`/`tblItemMaster.QtyPerPallet`.

---

### 2.7 Embedded Business Rules (Hardcoded Checks)

1. **No pallet without a shop order**: Both forms gate on a valid shop order. The main form uses `gdrSessCtl.ShopOrder`; the PS form validates `txtShopOrder` against the Local DB. An error is shown if the order is not found.
2. **Pallet rate limiting** (WO#34957): `PalletCreationTimeLimit` is read from `tblIPCControl`. If elapsed time since `PreviousPalletCreationTime < PalletCreationTimeLimit`, creation is blocked. This prevents accidental double-submission on touchscreens.
3. **Quantity ceiling**: `txtCasesInPallet.Text` is validated to be `<= gintQtyPerPallet`. In `AutoCreatePallet`, the equivalent check is `intQtyInPallet <= dr.QtyPerPallet`. Violation results in an error message and no pallet is created.
4. **Item Change Guard** (auto-create paths only): `SharedFunctions.IsItemChangedOnServer(dr)` is verified before commit. If the Item Master has been modified on the server (e.g., label format, QtyPerPallet), pallet creation is blocked. This check is **absent** in the manual form (`frmCreatePallet`).
5. **Probat DMS Line Block** (PS form only): `IsActiveProbatEnableLine()` prevents `frmCreatePallet_PS` from creating pallets for Probat-managed lines. These lines use a separate external pallet tracking system.
6. **Last Pallet triggers Stop**: If `intLastPallet = True`, the form auto-invokes `frmStopShopOrder.ShowDialog()` immediately after pallet commit. The "Last Pallet" action is implicitly also a "Stop Shop Order" action.
7. **Server-offline Pallet ID fallback**: If the server is unreachable, `PPsp_GetPalletNo` is skipped and `intPalletID = 0`. The pallet is inserted locally and is expected to be reconciled by the background upload sync. No retry mechanism is implemented within `CreatePallet()` itself.

### 2.8 Pallet Print Delegation (`srvLabelPrinting.vb` Windows Service)

*Source: `Visual Studio 2012\Projects\Label Printing`*

When `printPalletLabel()` is called (either manually via the "Reprint" button or dynamically during pallet creation), the system does **not** establish a direct TCP socket to the printer. This is by design: if the printer is offline or the network is congested, the WinForms UI would hang, blocking production.

> [!NOTE]
> **Hardware Dialect Abstraction (No ZPL in Codebase)**
> PowerPlant does *not* contain raw ZPL (Zebra) or specific printer dialects in the source code. The entire ecosystem completely delegates to the **Markem-Imaje CoLOS Print Server**. CoLOS abstracts the hardware. PowerPlant merely issues the CoLOS CLI command (job name and variable data), and CoLOS handles translating that to ZPL for Zebra printers or native code for Markem-Imaje printers.

Instead, a persistent background Windows Service (`srvLabelPrinting`) handles the actual printing loop:

1. **Queuing**: `printPalletLabel()` simply inserts a row into `tblPrintRequest`.
2. **Polling Loop**: `srvLabelPrinting.ProcessPrintRequest()` loops infinitely, fetching pending records via `taPrintRequests.Fill(...)` every 2,000 milliseconds.
3. **CLI Command Synthesis**: If a record exists, it constructs the CoLOS CLI command: `devices|lookup|{0}|select|{1}|@SelectMode=DownloadAndSelect|@Copies={2}`, where `{0}` is the `DeviceName` (Printer), and `{1}` is the `JobName` (Label Format).
4. **TCP Dispatch**: The service instantiates `clsTCPClient` and connects to the CoLOS server software using `My.Settings.intCoLOSPort`.
5. **State Update**: Upon successful command receipt, it explicitly marks the Pallet as printed by calling `PPsp_EditPallet("Printed", 0, .JobName)`, updating `tblPallet.PrintStatus = 2`.

### Refactoring Scorecard: Chapter 2 (Pallet Management)

| Factor | Score (1-10) | Description | Actionable Recommendation |
|---|---|---|---|
| **Fragility** | **8** | Printing delegates via a fragile polling loop in `srvLabelPrinting` (`tblPrintRequest`). | Decouple printing using modern message queues (e.g., RabbitMQ). |
| **Security** | **4** | Validations rely on UI component state (`Enabled` flags). | Push constraints to backend command handlers. |
| **Complexity** | **6** | Mix of database inserts and direct CLI hardware manipulation logic. | Abstract CoLOS integration entirely behind a dedicated API microservice. |

*(End of Chapter 2)*

---

*MES System Codebase Handbook — PowerPlant VB.NET — Last updated: 2026-03-24*

## Chapter 3: Quality Assurance Testing (QAT) Workflow (Technical Reference)

This chapter documents the QAT module, which acts as the system's primary production gate. It does not produce physical output, but controls whether production operations (Start, Pallet Create, Stop) are permitted to proceed.

---

### 3.1 Workflow Orchestration: frmQATWorkflow as a State Machine

`frmQATWorkflow.vb` is a **pure runtime-generated UI shell**. It contains no hardcoded test buttons. Instead, on form load it calls:

```
SharedFunctions.GetQATWorkFlowInfo(Facility, DefaultPkgLine, QATEntryPoint)
  └─> CPPsp_QATWorkFlow_Sel  [Local DB SP]
       Returns: dsQATWorkFlow.CPPsp_QATWorkFlow_SelDataTable
```

The returned DataTable contains one row per QAT test assigned to this line + entry point combination. For each row, `InitializeScreen()` dynamically creates a `Button` at runtime:

| Button Property | Source |
|---|---|
| `.Text` | `drWF.TestCategory` (human-readable label) |
| `.Tag` | `drWF.FormName` (e.g., `"frmQATStartUpChecks"`) |
| `.Name` | `"btnQAT_" + intQATNo` |

The screen supports a maximum of **15 buttons** (3 columns × 5 rows). When an operator clicks any button, `ShowFormDynamically(strFormName)` is called, which uses **reflection** to instantiate the target form:

```vb
frmNewForm_Type = Type.GetType(Assembly.GetExecutingAssembly().GetName().Name & "." & strFormName)
frmNewForm = CType(Activator.CreateInstance(frmNewForm_Type), Form)
frmNewForm.ShowDialog()
```

This means the test form class name in the DB (`drWF.FormName`) must exactly match a compiled class in the assembly. There is no compile-time check.

#### QAT Entry Points

The `QATEntryPoint` property determines which subset of tests is displayed. Known entry point constants from the code:

| Constant | Meaning | Triggered By |
|---|---|---|
| `cstrStartup` (`"S"`) | Startup / Beginning-of-shift checks | First run after shift change or new shop order |
| `cstrInProcess` | In-process periodic checks | After N pallets or elapsed time |
| `cstrClosing` | End-of-run / closing checks | Before shop order close |
| `cstrChangeShift` | Shift-change checks | On `blnShiftChanged = True` |
| `cstrOnRequest` | Ad-hoc, operator-requested test | Manual trigger only |

The transition between entry points is managed by `UpdateQATStatus()` (see §3.4).

---

### 3.2 Integration with Shop Order Lifecycle

#### Startup Block

In `SharedFunctions.StartShopOrderUpdate()`, if `gdrCmpCfg.QATWorkFlowInitiation = True`, the system calls `UpdateQATTesters()` to register the Operator and Utility Technicians into the QAT tracking tables before the shop order is allowed to open. If `QATWorkFlowInitiation = False`, the workstation is entirely exempt from QAT controls.

#### Stop-Process Check

In `frmStopShopOrder.vb` and `SharedFunctions.StopShopOrderUpdate()`, the code calls:

```vb
SharedFunctions.GetQATStatus()    [reads tblQATStatus from Local DB]
```

This returns the current `tblQATStatus` row which contains:

- `NextQATDefnID` — the next test that must be completed
- `NextQATEntryPoint` — the current phase of the workflow
- `NextInterfaceFormID` — the specific form to show next

If outstanding QAT tests exist, the stop-order logic can optionally block or warn before allowing the shop order to close, depending on the workflow configuration in `tblQATWorkFlow`.

#### `tblQATStatus` — The Live Workflow Pointer

`tblQATStatus` is the single-row operational state table for the QAT workflow. It functions as a **cursor** pointing to the "next required test":

| Column | Purpose |
|---|---|
| `NextQATDefnID` | FK to `tblQATWorkFlow` — identifies the next test definition |
| `NextQATEntryPoint` | The current workflow phase (`S`, `InProcess`, etc.) |
| `NextWFTestSeq` | The sequence number of the next test in the workflow |
| `NextInterfaceFormID` | The form name to display next |
| `CurrentShopOrder` | The Shop Order this status belongs to |
| `ShiftChanged` | Flag: set when a shift boundary has been crossed |
| `ShopOrderClosed` | Flag: set when the current order has been closed |
| `ByPassAllTests` | Flag: set if a supervisor bypassed all remaining tests |
| `ByPassTest` | Flag: set if a supervisor bypassed the specific current test |

---

### 3.3 Supervisor Override Logic (frmQATOverrideLogOn)

`frmQATOverrideLogOn` is invoked when an individual QAT sub-form needs to be bypassed by a supervisor. It implements two distinct override actions:

#### Authentication

On form load, it calls:

```vb
SharedFunctions.GetPlantStaff(txtUserID.Text, Nothing, True, "Supervisor")
```

Only staff with `Role = "Supervisor"` and `ActiveRecord = True` are accepted. The password field uses `*` masking via the numeric keypad.

#### Override Path 1: Bypass Current Test (`btnBypassCurrentTest`)

Skips the entire current test. Writes to `tblQATOverride` on the Server DB (falls back to Local if server offline):

```
tblQATOverride record:
  ByPassTest     = True
  ByPassLanes    = ""   (empty — all lanes, full skip)
  OverridedBy    = txtUserID.Text
  OverrideID     = dteOverrideID  (DateTime, acts as PK)
  BatchID        = dteBatchID
  QATDefnID      = intQATDefnID
  ShopOrder      = gdrSessCtl.ShopOrder
  SOStartTime    = gdrSessCtl.StartTime
  Alert          = blnAlert
```

Returns `DialogResult.Cancel` to the calling form.

#### Override Path 2: Bypass Specific Lanes (`btnAccept`)

Allows specific lanes to be skipped but requires at least one lane to still be tested. Iterates over the dynamically generated lane buttons — any button with `.Tag = "1"` (red/selected) is added to a CSV string:

```vb
strByPassLanes = strByPassLanes & CInt(ctrl.Text).ToString & ","
```

**Constraint**: If `intSkipCount = intNoOfLanesToTest`, all lanes are selected — the system shows `"Cannot skip all lanes for testing."` and blocks.

**Gima-specific** (WO#17432): For Gima pallet-type lines running a 12-sample Oxygen test at a non-startup entry point, the maximum displayed lanes is forced to 6 (instead of `drWF.NoOfLanes`), and only 2 lanes must actually be tested.

Writes to `tblQATOverride` with `ByPassTest = False` and the CSV lane list in `ByPassLanes`.

Returns:

- `DialogResult.OK` if the bypassed lanes are unchanged from the original
- `DialogResult.Yes` if the bypassed lane selection was changed

---

### 3.4 QAT State Transition: UpdateQATStatus

`SharedFunctions.UpdateQATStatus()` is the core function that advances the workflow pointer after each test completes. It is called by every QAT sub-form's `btnNext_Click`.

**Signature:**

```vb
UpdateQATStatus(blnByPassAllTests, blnByPassTest, intShopOrder,
                strEntryPoint, intQATDefinID, strInterfaceID, intWorkFlowTestSeq,
                Optional blnShiftChanged, Optional blnShopOrderClosed, Optional blnStartedNewShopOrder)
```

**Transition Logic:**

1. If `strEntryPoint = cstrOnRequest`: Read the current `tblQATStatus` pointer and preserve it. Generate a new one-off `BatchID` via `Now()` without persisting it.
2. Otherwise: Query `tblQATWorkFlow` for the next test where `TestSeq > intWorkFlowTestSeq` within the same `strEntryPoint`.
3. If no next test exists within the current `EntryPoint`:
   - If `blnShopOrderClosed = True` OR `blnStartedNewShopOrder = True` → next EP = `cstrStartup`
   - If `blnShiftChanged = True` → next EP = `cstrChangeShift`
   - Otherwise → next EP = `cstrInProcess`
4. Load the first test (by `TestSeq ASC`) from the new entry point.
5. Write all 13 updated fields back to `tblQATStatus` via `taQATStatus.Update(...)`.

**BatchID Management:**

`GetQATBatchID()` governs test batch grouping:

- `TestSeq = 1` → create new `BatchID = Now()` and persist it to `tblIPCControl["QAT_BatchID"]`
- `TestSeq > 1` → read the batch ID from `tblIPCControl`
- `EntryPoint = OnRequest` → always generate a new `BatchID` in memory only, never persists

---

### 3.5 Per-Test Result Persistence: SaveQATStartUpChecks

After each individual task within a startup-checks form is marked Done/N/A, `UpdateStartUpChecks()` iterates `dtTaskStatus` and calls:

```vb
SharedFunctions.SaveQATStartUpChecks(
    gdteTestBatchID,        -- batch grouping key
    intByPassAllTest,       -- 0 or 1
    gdrSessCtl.Facility,
    gstrInterfaceID,        -- form identifier from tblQATWorkFlow
    gdrSessCtl.DefaultPkgLine,
    gstrQATWorkFlowType,    -- "Startup", "InProcess", etc.
    intShopOrder,           -- from gdrSessCtl or intClosedShopOrder
    gdrSessCtl.StartTime,   -- shop order start time
    dteJobEndTime,          -- when the full test was completed
    dteJobStartTime,        -- when the form was opened
    colTaskStartTime,       -- when individual task was started
    intTaskID,              -- task sequence within the checklist
    colTaskEndTime,         -- when individual task was marked
    intTaskStatus,          -- Done, N/A, or Not Done
    gstrQATTesterID         -- operator who performed the test
)
```

Records with `colTaskStatus = 1` (Bypass All) are excluded from saving.

---

### 3.6 Data Persistence Mapping

| Table | DB Location | Purpose | Key Link |
|---|---|---|---|
| `tblQATStatus` | Local DB | Single-row workflow pointer (next test, current phase) | Workstation-scoped (one row per line) |
| `tblQATWorkFlow` | Local & Server | Workflow definition: which tests, in which order, for which lines | `Facility + DefaultPkgLine + EntryPoint + TestSeq` |
| `tblQATStartUpChecks` | Server DB | Startup checklist task results (per task, per batch) | `BatchID + InterfaceID + TaskID` |
| `tblQATOverride` | Local & Server | Supervisor bypass event audit trail | `OverrideID (DateTime) + QATDefnID + ShopOrder` |
| `tblIPCControl["QAT_BatchID"]` | Local DB | Persisted BatchID for multi-step test grouping | Key-value row in `tblIPCControl` |

**Note**: The `ShopOrder` and `SOStartTime` from `gdrSessCtl` are written directly into both `tblQATStartUpChecks` and `tblQATOverride`. This is the join key back to `tblSessionControl` and `tblShopOrder`.

---

### 3.7 Embedded Business Rules (Hardcoded Logic)

1. **Workflow Definition Mandatory**: If `CPPsp_QATWorkFlow_Sel` returns 0 rows for the line + entry point, the form closes with an error. There is no graceful fallback to a default workflow.
2. **Maximum 15 QAT buttons**: `frmQATWorkflow` enforces `intMaxRows = 5, intMaxCols = 3`. Tests beyond position 15 are silently dropped (`Exit For`).
3. **Cannot bypass all lanes**: In `frmQATOverrideLogOn`, if `intSkipCount = intNoOfLanesToTest`, the override is blocked entirely. At least one lane must always be tested.
4. **Gima 12-sample oxygen rule** (WO#17432): For `frmQATOxygen`, 12-sample, Gima line type, non-startup entry point — maximum 6 lanes shown, minimum 2 lanes must be tested. This is hardcoded in `InitializeLaneButtonsGima(6)` and the `intNoOfLanesToTest = 2` check.
5. **Supervisor-only override**: `GetPlantStaff(role = "Supervisor")` is enforced. Non-supervisor staff cannot authenticate regardless of correct password.
6. **Bypass All disables individual task selection**: In `frmQATStartUpChecks`, once any individual task button is clicked, `DisableByPassAllTest()` is called and the bypass-all option is permanently disabled for that session.
7. **TestSeq=1 resets the BatchID**: The first test in any workflow sequence resets `QAT_BatchID` in `tblIPCControl`. This ensures all tests within a startup or in-process run share a single traceable `BatchID`.
8. **Entry point transitioning is sequential**: The state machine transitions `Startup → InProcess → Closing`. Transition to `Startup` is only reset by a `ShopOrderClosed = True` or `StartedNewShopOrder = True` flag. There is no mechanism to go back to a previous entry point once advanced.

### Refactoring Scorecard: Chapter 3 (QAT Workflow)

| Factor | Score (1-10) | Description | Actionable Recommendation |
|---|---|---|---|
| **Fragility** | **6** | Highly dynamic UI via Reflection makes tracing dependencies at compile time impossible. | Replace with strongly-typed parameter-driven component rendering (e.g., MudBlazor). |
| **Security** | **5** | Relies on simple unencrypted string validation (`override.txt`) for Supervisor PIN bypass. | Integrate with OAuth/ActiveDirectory for overrides. |
| **Complexity** | **8** | Deeply nested legacy state configurations in `tblQAT*`. | Simplify the QAT data model into a unified JSON/document config structure. |

*(End of Chapter 3)*

---

*MES System Codebase Handbook — PowerPlant VB.NET — Chapters 0–3 complete. Last updated: 2026-03-24*

## Chapter 4: Session & Authentication (Technical Reference)

This chapter documents how the PowerPlant MES system establishes its runtime identity — determining *which machine it is*, *what line it controls*, and *who is operating it* — before any production function is accessible.

---

### 4.1 System Initialization Sequence (modStartup.Main)

`modStartup.Main()` is the true application entry point. It executes a strict initialization chain before dispatching to any form:

```
modStartup.Main(CmdArgs)
  1. Parse strPgmID from CmdArgs(0)          [e.g. "logon", "startshoporder"]
  2. gstrMyComputerName = My.Computer.Name   [Environment.MachineName]
  3. Poll for sqlservr.exe process           [up to 200 x 1s retries]
  4. Poll for IsLocalConnOK                  [up to 10 retries]
  5. gstrServerConnectionString = My.Settings.ServerPowerPlantCnnStr
  6. SharedFunctions.RefreshComputerConfig(gstrMyComputerName)
       └─> if gdrCmpCfg Is Nothing → MessageBox + Return 1 (EXIT)
  7. [WO#17432] If QATWorkFlowInitiation = External:
       └─> gstrInterfaceID = CmdArgs(1), gstrQATWorkFlowType = UCase(CmdArgs(2))
  8. SharedFunctions.RefreshSessionControlTable()
       └─> if gdrSessCtl Is Nothing → MessageBox + Return 1 (EXIT)
       └─> if gdrSessCtl.ComputerName ≠ gstrMyComputerName → MessageBox + Return 1 (EXIT)   [WO#871]
  9. Set gblnAjaxPlant = (Facility = "02" OR "20")
  10. Select Case strPgmID → dispatch to correct Form
```

The system will not proceed past step 8 if the workstation name in `tblSessionControl` does not match the actual `Environment.MachineName`. This is a hardware-level workstation binding, not a user-level check.

#### SQL Server Process Availability Guard

Before attempting any DB connection, `Main()` polls `Process.GetProcessesByName("sqlservr")` in a loop of up to **200 iterations** (1 second delay each). This is a hardcoded 200-second maximum wait for the Local SQL Express instance to become available after a machine reboot.

---

### 4.2 Hardware Identity: RefreshComputerConfig

`SharedFunctions.RefreshComputerConfig(strMyComputerName)`:

1. Calls `gtaCmpCfg.Fill(tblCmpCfg, "SelectAllFields", strMyComputerName, Nothing)` → executes `CPPsp_ComputerConfigIO` on the Local DB using `MachineName` as the lookup key.
2. Sets `gdrCmpCfg = tblCmpCfg.Rows(0)`.
3. **Virtual Line Override** (WO#5370): If an optional `strPackagingLine` arg differs from the config's default `PackagingLine`, re-queries with `"AllActiveInclVirtual"` and overwrites `gdrCmpCfg.ComputerName = strMyComputerName`. This allows one physical machine to act as a virtual terminal for a different line.
4. Derives global boolean flags directly from `gdrCmpCfg` fields:

| Global Variable | Source Field | Purpose |
|---|---|---|
| `gblnOvrExpDate` | `gdrCmpCfg.EnableOvrExpDate` | Allow operator to enter WIP expiry date |
| `gbln2SOIn1Line` | `gdrCmpCfg.Enable2SOIn1Line` | Allow 2 shop orders to run on 1 line |
| `gblnEnableOutputLocationLine` | `gdrCmpCfg.EnableOutputLocation` | Enable output location selection at pallet creation |
| `gblnAutoCountLine` | Derived from `gdrCmpCfg.AutoCountType` | Automatic case/pallet counting via IPC |
| `gblnSarongAutoCountLine` | Derived from `gdrCmpCfg` | Sarong-machine-specific auto-count mode |
| `gblnAjaxPlant` | `Facility = "02" OR "20"` | Ajax plant–specific UI/logic branches |

**Key**: `gdrCmpCfg` is a module-level singleton DataRow. It is never automatically refreshed during a session — it represents the workstation's static configuration at startup.

---

### 4.3 Session State Identity: RefreshSessionControlTable

`SharedFunctions.RefreshSessionControlTable()`:

```vb
gtaSessCtl.Fill(tblSessCtl, "SelectAllFields")   ' CPPsp_SessionControlIO, Local DB
gdrSessCtl = tblSessCtl.Rows(0)
```

`tblSessionControl` is a **single-row operational state table** per workstation. Unlike `tblComputerConfig`, its fields are mutable throughout the session. The key fields are:

| Column | Type | Meaning |
|---|---|---|
| `ComputerName` | Char | Matches the physical machine — validated on startup |
| `Facility` | Char | Plant identifier (e.g., "01", "02") |
| `DefaultPkgLine` | Char(10) | The packaging line this workstation is bound to |
| `OverridePkgLine` | Char(10) | Operator-entered override for the current session |
| `Operator` | VarChar | Current logged-in operator's staff ID |
| `LogOnTime` | DateTime | Timestamp of the most recent login |
| `ShopOrder` | Int | Active shop order (0 = none running) |
| `StartTime` | DateTime | When the active shop order was started |
| `OverrideShiftNo` | TinyInt | Shift number entered by operator at login |
| `DefaultShiftNo` | TinyInt | System-calculated current shift at login time |
| `CasesProduced` | Int | Running count of cases this session |
| `PalletsCreated` | Int | Running count of pallets this session |
| `LooseCases` | Int | Cases not yet packed into a pallet |
| `ServerCnnIsOk` | Bit | Live flag: is the server connection currently healthy? |
| `ShiftProductionDate` | Date | Production date for the current shift |
| `CarriedForwardCases` | Int | Cases carried over from a previous run |

`gdrSessCtl` is refreshed by calling `RefreshSessionControlTable()` after any mutation. It is the canonical source of truth for all operational context throughout the application.

---

### 4.4 Authentication Logic (frmLogOn_NoPLC)

`frmLogOn_NoPLC` is the standard operator login screen. It does not use Windows Authentication. The login sequence is:

#### Field Validation

1. **Operator ID** — entered in `txtOperator`. Validated via `SharedFunctions.GetPlantStaff(txtOperator.Text, Nothing, True, "Operator")`. Returns Nothing if not found or inactive.
2. **Shift Number** — entered in `txtShift`. Validated against `tblShift` via `CPPsp_ShiftIO("AllShifts", Facility, WorkShiftType, Now(), Nothing, 1)`. Warns if entered shift ≠ `gshtCurrentShift` (sequential shift method) or validates against pattern codes (`ShiftMethod.PatternCode`).
3. **Packaging Line** — entered in `txtPkgLine`. Validated via `SharedFunctions.IsValidPkgLine()` which checks `tblEquipment` via `gtaEQ`.

#### GetPlantStaff — Identity Verification

```vb
GetPlantStaff(strStaffID, strWorkGroup, blnActive, strStaffClass)
  └─> CPPsp_PlantStaffingIO("SelectAllFields", StaffID, WorkGroup, Active, StaffClass)
      └─> Local DB [tblPlantStaff]
```

The function filters on:

- `StaffID` — the unique user identifier
- `blnActive = True` — inactive staff are rejected even with a valid ID
- `strStaffClass` — the role filter (`"Operator"`, `"Supervisor"`, `"Utility"`, or `Nothing` for any role)

**No password verification occurs in `GetPlantStaff`.** The password is only checked in `frmQATOverrideLogOn` (supervisor override). For standard operator login via `frmLogOn_NoPLC`, Staff ID presence and active status are the sole authentication factors.

#### Commit: ProcessLogOn

After all fields validate, the form calls:

```vb
SharedFunctions.ProcessLogOn(txtPkgLine.Text, txtOperator.Text, txtShift.Text, gshtCurrentShift)
```

`ProcessLogOn` executes an inline `UPDATE` statement against `tblSessionControl` (Local DB only):

```sql
UPDATE tblSessionControl
SET DefaultPkgLine = @chrDefaultPkgLine,
    OverridePkgLine = @chrOverridePkgLine,
    Operator = @vchOperator,
    OverrideShiftNo = @tnyOverrideShiftNo,
    LogOnTime = @dteLogOnTime
```

If the workstation is configured as a **Pallet Station** (`gdrCmpCfg.PalletStation = True`), the SQL is extended to reset all production counters:

```sql
    StartTime = @dteLogOnTime,
    DefaultShiftNo = @tnyDefaultShiftNo,
    StopTime = NULL,
    CasesScheduled = 0, CasesProduced = 0, PalletsCreated = 0,
    BagLengthUsed = 0, ReworkWgt = 0, LooseCases = 0
```

After the UPDATE, `RefreshSessionControlTable()` is called to reload `gdrSessCtl` so all subsequent forms see the new logged-in state.

**There is no login record written to an audit table.** Login events are only visible by inspecting the `LogOnTime` column in `tblSessionControl` and `tblSessionControlHst`.

---

### 4.5 Data Persistence: Login/Logout DB Behavior

| Event | Local DB | Server DB | Mechanism |
|---|---|---|---|
| Machine startup / config load | `tblComputerConfig` (read) | None | `CPPsp_ComputerConfigIO` SP via TableAdapter |
| Session state load | `tblSessionControl` (read) | None | `CPPsp_SessionControlIO` SP via TableAdapter |
| Operator login | `tblSessionControl` (update) | None (sync'd later) | Inline SQL `UPDATE` via `SqlHelper` |
| Shop order start | `tblSessionControl` (update) | `tblSessionControl` (update) | `StartShopOrderUpdate` dual-DB transaction |
| Shop order stop | `tblSessionControl` (update) + `tblSessionControlHst` (insert) | `tblSessionControlHst` (insert) | `StopShopOrderUpdate` |
| Server back online | `tblSessionControl.ServerCnnIsOk = True` | None | `SetServerCnnStatusInSessCtl(True)` |

The `tblSessionControlHst` table is the **only** permanent audit log of session events. `tblSessionControl` is a live operational snapshot that is overwritten on each login/start/stop.

---

### 4.6 Embedded Business Rules (Hardcoded Checks)

1. **ComputerName binding is enforced at startup**: If `gdrSessCtl.ComputerName ≠ Environment.MachineName`, the application exits immediately with a supervisor notification message. A workstation cannot be hijacked from another machine's record.
2. **Local SQL Server availability is actively waited on**: Up to 200 seconds of polling for `sqlservr.exe`. If the process never starts, the application waits indefinitely (the loop only `Exit For`s, it does not return with an error).
3. **No password enforced at operator login**: Staff authentication only requires: correct StaffID + `ActiveRecord = True` + correct StaffClass role. Passwords are never verified in the standard login flow. Only `frmQATOverrideLogOn` verifies passwords (numeric PIN, compared against `tblPlantStaff`).
4. **Shift validation is configurable, not hardcoded**: The system supports two shift methods — `sequential` (warning only if shift ≠ current) and `PatternCode` (hard block if shift is invalid for the current time pattern). The method is driven by `tblShift.Method`.
5. **Virtual line support** (WO#5370): A physical machine can override its configured packaging line by passing a `strPackagingLine` argument to `RefreshComputerConfig`. This re-queries `AllActiveInclVirtual` and replaces `gdrCmpCfg` in memory.
6. **No session expiry mechanism**: Once `tblSessionControl.LogOnTime` is set, there is no automatic timeout or expiry check. A session persists indefinitely until the operator manually logs off or starts a new session.
7. **Pallet Station login resets all counters**: Workstations with `gdrCmpCfg.PalletStation = True` have their production counters reset to zero on every login. Standard packaging lines do not reset counters on login — only on Shop Order Stop.

### Refactoring Scorecard: Chapter 4 (Session/Auth)

| Factor | Score (1-10) | Description | Actionable Recommendation |
|---|---|---|---|
| **Fragility** | **5** | Relies heavily on exact string matching for Machine Types and Line lookups. | Move to Enum-based identifiers and foreign-key bounds. |
| **Security** | **8** | Offline custom hashing (MD5 equivalent) and hardcoded static credentials. | **High priority:** Enforce Active Directory / SSO authentication with JWT tokens. |
| **Complexity** | **5** | Session state is tracked via constant relational DB polling (`tblSessionControl`). | Implement a centralized fast token/session cache (e.g., Redis). |

*(End of Chapter 4)*

---

*MES System Codebase Handbook — PowerPlant VB.NET — Chapters 0–4 complete. Last updated: 2026-03-24*

---

## Chapter 5 — PLC/IPC Integration

*Source files: `clsTCPClient.vb`, `clsXMLInterface.vb` (`XMLInterface` class, WO#755), `clsCoLOSCLI.vb`, `clsCheckWeigher.vb`; key functions in `SharedFunctions.vb`: `CallTcpServer` (L566-615), `CheckWeigherLog` (L554-564), `StartShopOrderUpdate` (L333-348), `SaveStartSOOption` (L5233-5266).*

---

### 5.1 Hardware Communication Architecture

PowerPlant communicates with external hardware (IPC line controllers, CoLOS inkjet coders, check-weighers) exclusively via **synchronous TCP byte streams**. All TCP logic is centralised in `clsTCPClient.vb`. Two additional classes compose or extend it for specific hardware:

| Class | Relationship to `clsTCPClient` | Target Hardware |
|---|---|---|
| `clsCheckWeigher` | `Inherits clsTCPClient` | Mettler-Toledo WD series check-weigher (IP `192.168.50.91`, custom protocol) |
| `clsCoLOSCLI` | Aggregates `clsTCPClient` via private field | CoLOS CLI inkjet coder (pipe `Login\|CLIUser\|cliuser1`) |
| `SharedFunctions.CallTcpServer` | Creates raw `System.Net.Sockets.TcpClient` inline | `PPTCPServer.exe` (localhost port 8000) — IPC line controller gateway |

Neither `clsCheckWeigher` nor `clsCoLOSCLI` operates asynchronously. Every call blocks the calling thread until the 5-second `ReadTimeout`/`WriteTimeout` expires or a response is received.

#### 5.1.1 `clsTCPClient` — Core Transport Layer

`clsTCPClient` wraps a single `System.Net.Sockets.TcpClient` instance. Its public surface is:

| Method / Property | Behaviour |
|---|---|
| `IPAddress`, `Port` | Read/write; set before any connect call |
| `CheckIPAddress(strIP)` | Validates IP format via `System.Net.IPAddress.TryParse`, then issues an ICMP ping via `My.Computer.Network.Ping`. Returns `Nothing` on success, error string on failure |
| `TCPConnect()` | Calls `CheckIPAddress`; connects if valid. Absorbs "No connection could be made", "No such host is known", and "Unable to ping" errors by returning them as a string rather than throwing |
| `tcpConnect(strLogInMessage)` | Connects then immediately calls `TCPSendMessage(strLogInMessage)` — used by CoLOS login sequence |
| `TCPSendMessage(strMessage)` | Appends `vbCrLf`, writes to stream, retries read up to 3 times with 1-second sleep between attempts; R/W timeout = 5 s |
| `TCPSendCommand(strMessage)` | Write-only variant (WO#17432, AT 10/05/2018); does not attempt to read a response |
| `TCPReceiveMessage()` | Read-only; retries 3 times with 1-second sleep; returns `"No Data"` on transport closed |
| `TCPDisconnect()` / `tcpDisconnect(strLogOutMessage)` | Closes socket; logout variant sends a final message before close |

> **Retry model**: Reads in `TCPSendMessage` and `TCPReceiveMessage` are a simple polling loop (`For intCount As Int16 = 1 To 3 ... Threading.Thread.Sleep(1000)`). There is no event-driven or asynchronous receive mechanism. If the hardware does not respond within the third iteration, the last ASCII-decoded buffer content is returned as-is, regardless of whether it is empty.

#### 5.1.2 CoLOS Coder Session (`clsCoLOSCLI`)

`clsCoLOSCLI` wraps the CoLOS CLI protocol. Lifecycle:

```
New clsCoLOSCLI(IPAddress, Port)
      |
      v
LogOnToCoLOS()          ← tcpConnect("Login|CLIUser|cliuser1")
      |
      v
SendMessage(strMessage) ← arbitrary CLI command string
      |
      v
LogOffFromCoLOS()       ← tcpDisconnect("Logout")
```

Connection parameters are runtime-configurable via the `CoLOS_IPAddress` and `CoLOS_Port` write-only properties. The class contains no retry or reconnect logic beyond what `clsTCPClient` provides.

#### 5.1.3 Check-Weigher Session (`clsCheckWeigher`)

`clsCheckWeigher` inherits `clsTCPClient` directly and implements the Mettler-Toledo WD protocol:

| Method | Command Sent | Expected Response |
|---|---|---|
| `IsHandShakeOK()` | `WD_TEST` | `"WD_OK"` (stripped of CR/LF/trailing spaces) |
| `Start()` | `WD_START` | Any non-empty string |
| `Abort()` | `WD_STOP` | (response ignored) |

The class declares `blnCounterIsStarted` to track the active state locally; this flag is **not** persisted to the DB and is lost if the instance is garbage-collected.

---

### 5.2 PPTCPServer Gateway and the `CallTcpServer` Function

#### 5.2.1 Process Architecture

`PPTCPServer.exe` is an **out-of-process bridge** between `PowerPlant.exe` and the physical IPC line controllers. It listens on `127.0.0.1:8000`. Its binary path is stored in `My.Settings.strPPTCPServer`, defaulting to `C:\PowerPlant\PPTCPServer\PPTCPServer.exe`.

`SharedFunctions.CallTcpServer` (L566-615) is the sole call-site for this gateway within the MES. Its execution sequence:

```
CallTcpServer(IPAddress, intPort, strCmd)
  │
  ├─ IsProcessActive("PPTCPServer") == False?
  │     └─ Process.Start(My.Settings.strPPTCPServer, WindowStyle=Hidden)
  │
  └─ TcpClient.Connect("127.0.0.1", 8000)
        ├─ networkStream.Write(Encoding.ASCII.GetBytes(strCmd))
        └─ networkStream.Read(bytRecBuffer) → return Encoding.ASCII.GetString(bytRecBuffer)
```

Key constraints:

- The function always targets `127.0.0.1:8000` regardless of the `IPAddress` and `intPort` parameters passed in — these parameters are present in the signature but unused in the body (the `tcpClient.Connect` call hardcodes the loopback address).
- The socket is always closed in the `Finally` block; each call creates and destroys its own `TcpClient` instance.
- There is no timeout set on the raw stream; it inherits the OS default.

#### 5.2.2 Command String Format

Commands sent to `PPTCPServer` use an `@`-prefixed token format observed in the check-weigher log control call (L555-558):

```
@CWLog {TCPConnID} {Mode}
```

Where `{TCPConnID}` is `drWF.TCPConnID` (a connection ID column from the local DB) and `{Mode}` is `0` (start logging) or `1` (stop logging). The full command vocabulary of `PPTCPServer.exe` is not defined in the MES source; it resides in the external server binary.

---

### 5.3 XML Interface Protocol (`XMLInterface` / `clsXMLInterface.vb`)

#### 5.3.1 Protocol Role

The `XMLInterface` class (WO#755) implements the **XML file-based IPC side-channel**. When `gdrCmpCfg.InterfaceType = "XML"` (evaluated at L333 of `SharedFunctions.vb`), the system writes production parameters to a shared XML file (`gstrLabelInputFileName`) that the IPC/labeller reads as its command input. This is distinct from TCP — no socket connection is used.

The XML file path is stored in the global variable `gstrLabelInputFileName` and is loaded by the `XMLInterface` constructor:

```vb
Public Sub New(ByVal strCaseLabelFileName As String)
    xDoc = New XmlDocument()
    strdocName = strCaseLabelFileName
    xDoc.Load(strdocName)   ' File must exist before constructor call
End Sub
```

#### 5.3.2 `StartShopOrder` — XML Payload Structure

`XMLInterface.StartShopOrder` (L19-75) iterates over all `<CaseLabel>` elements under the root `<CaseLabels>` node and overwrites their child element values. The fields written and their sources:

| XML Element | Source / Value Written |
|---|---|
| `SCC` | `strSCC` — GS1 Serial Shipping Container Code from `tblShopOrder` |
| `UPC` | `strUPC` — Universal Product Code from `tblShopOrder` |
| `ShopOrder` | `intShopOrder.ToString` — Shop Order number |
| `LabelKey` | `strPkgLine & intShopOrder.ToString` — composite line+SO key |
| `PackagesPerSaleableUnit` | `intPackagesPerSaleableUnit` from `tblShopOrder` |
| `UnitsPerCase` | `intUnitsPerCase` from `tblShopOrder` |
| `CasesPerPallet` | `intCasesPerPallet` (`QtyPerPallet`) from `tblShopOrder` |
| `ScheduledCases` | `intScheduledCases` (`OrderQty`) from `tblShopOrder` |
| `CaseCount` | `intCaseCount` — running case count from `Counter.CasesProducedRunningTotal` |
| `ItemNumber` | `strItemNumber` from `tblShopOrder` |
| `ItemDescription` | Concatenation of `ItemDesc1`, `ItemDesc2`, `ItemDesc3` |
| `Tie` | `intTie` from `tblShopOrder` (pallet tie count) |
| `Tier` | `intTier` from `tblShopOrder` (pallet tier count) |
| `ComputerName` | `gdrCmpCfg.ComputerName` |
| `SOStartTime` | `dteSOStartTime.ToString("yyyy/MM/dd HH:mm:ss.fff")` |
| `ServerCnnIsOK` | Hardcoded `"0"` at start-of-order; not dynamically updated thereafter |
| `LastUpdateTime` | `Now().ToString("yyyy/MM/dd HH:mm:ss")` — wall-clock timestamp of write |
| `PalletType` | `strPalletCode` from `tblShopOrder.PalletCode` |
| `SlipSheet` | `"1"` if `blnSlipSheet = True`, else `"0"` |

After all fields are updated in-memory, `xDoc.Save(strdocName)` flushes the document back to disk. The IPC hardware (label applicator / case counter) polls this file independently.

> Note: `UpdateSOInitialCaseCount` and `StopShopOrder` methods exist only as commented-out code in `clsXMLInterface.vb`. Stop-of-order does not write to the XML file; the file is simply overwritten on the next `StartShopOrder` call.

#### 5.3.3 `InterfaceType` Dispatch — XML vs SQL Path

At L333-347 of `SharedFunctions.StartShopOrderUpdate`, the system branches on `gdrCmpCfg.InterfaceType` (sourced from `tblComputerConfig.InterfaceType` via `gdrCmpCfg`):

```vb
If gdrCmpCfg.InterfaceType = "XML" Then
    Dim xmlInput As New PowerPlant.XMLInterface(gstrLabelInputFileName)
    xmlInput.StartShopOrder(...)   ' → file write to gstrLabelInputFileName
ElseIf gdrCmpCfg.InterfaceType = "SQL" Then
    Using qta As New dsUnitCountOutBoundTableAdapters.QueriesTableAdapter
        qta.PPsp_InitializeUnitCountOutbound(...)  ' → Local DB write
    End Using
End If
```

Only one path executes per machine. The `"SQL"` path calls the stored procedure `PPsp_InitializeUnitCountOutbound` on the **Local DB**, writing to `dsUnitCountOutBound` (the outbound IPC counter table). No TCP handshake occurs on either path; both are passive push operations.

---

### 5.4 Asynchronous Signal Handling and `gdrSessCtl` Counter Updates

#### 5.4.1 Counter Architecture

There is no true asynchronous event loop in the MES client. Hardware-to-software case count propagation operates through either:

1. **`Counter` class via PPTCPServer** (when `gblnAutoCountLine = True`): The `Counter` object (constructed at L323-325 of `SharedFunctions.StartShopOrderUpdate`) maintains `CasesProducedRunningTotal` in memory. Its internal mechanism queries `PPTCPServer` at a polling interval (the `Counter` class source is not in the examined files, but its integration point is its constructor argument `gdrCmpCfg.InterfaceType`).

2. **XML file polling** (`InterfaceType = "XML"`): The `CaseCount` element in `gstrLabelInputFileName` is populated at Shop Order start with `Counter.CasesProducedRunningTotal`. The IPC hardware increments a physical counter; the MES reads the count back via the `Counter` class.

#### 5.4.2 `gdrSessCtl.CasesProduced` Update Path

`gdrSessCtl.CasesProduced` is the **session-level running total** stored in `tblSessionControl` (Local DB column `CasesProduced`, type `INT`). The update chain from a physical case count:

```
Hardware produces a case
        │
        ▼
PPTCPServer.exe receives count signal
        │
        ▼ (TCP command via CallTcpServer or Counter polling)
Counter.CasesProducedRunningTotal incremented in-memory
        │
        ▼
On each call to StartShopOrderUpdate / StopShopOrderUpdate:
    gdrSessCtl.CasesProduced = Counter.CasesProducedRunningTotal
        │
        ▼
dsSessionControl TableAdapter:
    UPDATE tblSessionControl SET CasesProduced = @intCaseProduced
    (via CPPsp_SessionControlIOTableAdapter.UpdateCasesProduced)
```

The SQL command is defined in `dsSessionControl.Designer.vb` (L1788):

```sql
UPDATE tblSessionControl SET CasesProduced = @intCaseProduced
```

This update targets **Local DB only**. On session stop, `CasesProduced` is archived to `tblSessionControlHst` (Local DB) and `tblSessionControlHstSrv` (Server DB) as part of `StopShopOrderUpdate`.

---

### 5.5 Data Flow Map: Hardware Signals → Database Writes

| Hardware Signal / Action | Triggering Code | DB Table Written | DB |
|---|---|---|---|
| Shop Order start (XML mode) | `XMLInterface.StartShopOrder()` → `xDoc.Save()` | *(file system, not DB)* | N/A |
| Shop Order start (SQL mode) | `PPsp_InitializeUnitCountOutbound` | `tblUnitCountOutBound` (implied by QTA name) | Local |
| Case produced (via Counter) | `UpdateCasesProduced` in TableAdapter | `tblSessionControl.CasesProduced` | Local |
| Check-weigher log start/stop | `CallTcpServer("127.0.0.1", 8000, "@CWLog {id} {mode}")` | *(command to PPTCPServer, no direct DB write)* | N/A |
| "Start SO with no label" flag | `SaveStartSOOption` → `SqlHelper.ExecuteNonQuery` | `tblIPCControl` (`ControlKey = 'IsSOStartedWithNoLabel'`, `Value1 = 'YES'/'NO'`) | Local |
| "Start SO with no label" read | `IsSOStartedWithNoLabel` → `GetIPCControl("IsSOStartedWithNoLabel")` | `tblIPCControl` (read) | Local |
| Session stop (archive) | `StopShopOrderUpdate` | `tblSessionControlHst` | Local + Server |

#### 5.5.1 `tblIPCControl` Schema and Usage

`tblIPCControl` is the **key-value control bus** between `PowerPlant.exe` and IPC hardware/secondary processes. It is stored in the **Local DB**. The access pattern observed in the source:

- **Write**: `UPDATE tblIPCControl SET Value1 = @vchValue1 WHERE ControlKey = @vchControlKey` (L5253-5254, `SharedFunctions.SaveStartSOOption`). Uses raw ADO.NET via `SqlHelper.ExecuteNonQuery` on `cnnLocal`.
- **Read**: `SharedFunctions.GetIPCControl(strControlKey)` returns a `String()` array; `strIPCCtrlValues(0)` is `Value1`. The TableAdapter-based access (`dsIPCControlTableAdapters.tblIPCControlTableAdapter`) is present only as commented-out legacy code (WO#17432 removals).

The only `ControlKey` value visible in the examined source is `'IsSOStartedWithNoLabel'`. Additional rows in `tblIPCControl` are consumed by `PPTCPServer.exe` and are not visible in the MES client source.

---

### 5.8 Troubleshooting Decision Tree: The Hardware Invisible Links

When a hardware integration fails (printing or scanning), a specific sequence of checks is required to determine where the asynchronous flow broke down.

**Scenario A: Label Printing Failed (No Label Output)**

1. **Check `tblPrintRequest`**: Is the print job stuck in the table?
   - *If Yes*: The WinForms client successfully created the job. Problem is on the Central Server. Check `srvLabelPrinting` Windows Service status.
   - *If No*: Job was picked up. Proceed to step 2.
2. **Check `tblPrintStatus`**: Did `srvLabelPrinting` log an error?
   - *If Yes*: Review the error message. It may indicate CoLOS rejected the payload.
   - *If No*: The payload left the MES ecosystem.
3. **Check `vwLabelData`**: Run a `SELECT` against this view for the specific `ShopOrder` and `ItemNumber`. If this view returns empty or the `BatchNumber` is malformed, CoLOS will silently drop the request because it cannot fulfill its template variables.
4. **Check CoLOS Server Logs**: Inspect the CoLOS CLI inbound port log (mapped via `Package Line Printer Maintenance`).

**Scenario B: Checkweigher Not Recording Cases**

1. **Check `PPTCPServer.exe`**: Is the process running on the local IPC? (It should be bound to 127.0.0.1:8000).
2. **Check `tblQATTCPConn` (Local DB)**: Are `IPAddress` and `Port` correct for this specific line? (Updated via QA Web Portal and synced down).
3. **Verify `LoadDataFromCheckWeigher` Polling**: The 2-second polling loop may be blocked. Does a ping to the Checkweigher IP succeed from the IPC?

---

### Refactoring Scorecard: Chapter 5 (PLC/IPC Integration)

| Factor | Score (1-10) | Description | Actionable Recommendation |
|---|---|---|---|
| **Fragility** | **8** | Unvalidated XML file loads and missing heartbeat checks on TCP connections. IPC crashes do not stop the Checkweigher. | **High Priority**: Convert fire-and-forget TCP calls to robust, state-aware connection managers with retry queues. |
| **Security** | **5** | Hardcoded clear-text TCP payloads on internal ports. | Acceptable risk on isolated OT networks, but should transition to TLS-secured WebSockets. |
| **Complexity** | **7** | `StartShopOrderUpdate` attempts to reconcile multiple asynchronous hardware domains via UI thread. | Isolate hardware state orchestration to a background background worker/service. |

*(End of Chapter 5)*

---

*MES System Codebase Handbook — PowerPlant VB.NET — Chapters 0–5 complete. Last updated: 2026-03-24*

---

## Chapter 6 — Shared Business Logic & Utilities

*Source files: `SharedFunctions.vb` (414 KB God-Module, ~7836 lines), `SQLHelper.vb` (145 KB), `clsShift.vb`, `clsExpiryDate.vb`, `clsMidnightScheduler.vb`.*

`SharedFunctions` is declared as a `Public Class` with a `Private Sub New()` — it is a pure static utility class. Every method is `Public Shared`. It holds two shared EF context properties at module level: `_dbServer As New ServerModels.PowerPlantEntities()` and `_dbLocal As New LocalModels.PowerPlantLocalEntities()`. All other DB access uses ADO.NET TableAdapters or `SqlHelper`.

---

### 6.1 Shift & Time Logic

#### 6.1.1 `WorkShift` Class (`clsShift.vb`)

`WorkShift` is a data-holder class that wraps a single row from `tblShift` (Local DB). It is populated by one of two fill methods depending on the lookup key:

| Method | SP Called | Lookup Key |
|---|---|---|
| `GetExpectedShiftInfoByTime(dteDateTime, strWorkShiftType, blnNotShowDummyShift)` | `CPPsp_ShiftIO` action `"ExpectedShift"` | Current `DateTime` → finds which shift `FromTime ≤ dteDateTime < ToTime` |
| `GetShiftInfoByShiftNo(intShift, dteDateTime, strWorkShiftType, blnNotShowDummyShift)` | `CPPsp_ShiftIO` action `"ShiftTime"` | Explicit shift number lookup |

Both methods use the same `FillData(dr As DataRow)` private sub to populate:

- `intShift` (→ `Shift` property, `Short`)
- `strDescription` (→ `Description`)
- `dteFromTime` / `dteToTime` (→ `FromTime` / `ToTime`)
- `intMethod` (→ `Method`, `ShiftMethod.sequential = 0` or `ShiftMethod.PatternCode = 1`)

The `Facility` filter is applied from `gdrSessCtl("Facility")` inside `Fill` — the class has no explicit facility parameter.

#### 6.1.2 Shift Validation Logic: `IsEnteredShiftValid`

```vb
WorkShift.IsEnteredShiftValid(intEnteredShift, dteDateTime, strWorkShiftType, intCurrentShift) As String
```

This function enforces the **±12-hour shift window rule** for `PatternCode` shifts only:

```
1. GetExpectedShiftInfoByTime(dteDateTime)         → intCurrentShift (if 0 passed in)
2. GetExpectedShiftInfoByTime(dteDateTime + 12h)   → intNextShift
3. GetExpectedShiftInfoByTime(dteDateTime - 12h)   → intPreviousShift

VALID if: intEnteredShift ∈ { intCurrentShift, intNextShift, intPreviousShift }
INVALID: return error string "Entered shift no. is not the current, previous or next..."
```

For `ShiftMethod.sequential` (value `0`): **no validation is performed** — the function returns `Nothing` unconditionally, meaning any shift number is accepted.

#### 6.1.3 Shift Production Date (`ShiftProductionDate`)

`gdrSessCtl.ShiftProductionDate` is a `DateTime` column in `tblSessionControl` that represents the **calendar date the shift is credited to** — not the wall-clock date. It is passed as `@sc_ShiftProductionDate` (arParms index 20) when `UpdateSessionControl` calls `PPsp_LineEfficiency` on the Server DB. This value is written at Shop Order Start and is used as the primary time axis for all production aggregation queries.

The shift production date does **not** automatically roll at midnight. It is set to the operator's entered shift production date at `StartShopOrderUpdate` and does not change until the next `StartShopOrderUpdate` call.

#### 6.1.4 Midnight Scheduler (`clsMidnightScheduler.vb`)

`clsMidnightScheduler` is a thin wrapper over `System.Threading.Timer`:

```vb
' Constructor
firstTimeDueTime = DateTime.Now.Date.AddDays(1) - DateTime.Now  ' seconds until next 00:00:00
_timer = New Timer(AddressOf TimerCallback, Nothing, firstTimeDueTime, TimeSpan.FromHours(24))
```

- **First fire**: at the next calendar midnight (wall-clock local time)
- **Period**: every 24 hours thereafter
- **Callback**: invokes the `Action` delegate passed at construction — the scheduler has no built-in logic; all midnight behaviour is defined by the caller

The scheduler does **not** interact directly with the DB or `gdrSessCtl`. It is the caller's responsibility to marshal to the UI thread if the callback touches WinForms controls. The class exposes a `Dispose()` method that calls `_timer?.Dispose()`.

#### 6.1.5 `IsShiftChanged()` — Stub Function

```vb
Public Shared Function IsShiftChanged() As Boolean
    Dim blnIsShiftChanged As Boolean = False
    Try
        Return blnIsShiftChanged   ' Always returns False
    ...
End Function
```

This function is currently a **stub** — it always returns `False`. The QAT workflow checks this function (`FindCurrQATEntryPoint` at L6466) to detect shift-change conditions, but the detection logic has not been implemented in the source examined.

---

### 6.2 Printing Engine

#### 6.2.1 Label Type Constants

The codebase uses string constants for label type routing:

| Constant Name | Value (inferred) | Target Hardware |
|---|---|---|
| `CASELABEL` | `"CASELABEL"` | Case label applicator |
| `PACKAGELABEL` | `"PACKAGELABEL"` | Package coder (secondary label) |
| `PALLETLABELER` | `"PALLETLABELER"` | Pallet label printer |
| `CASELABELER` | `"CASELABELER"` | Case label printer device |
| `PACKAGECODER` | `"PACKAGECODER"` | Package coding device |

#### 6.2.2 `printCaseLabel(strDeviceType, strLotID)` — Entry Point (L3105)

`printCaseLabel` is the entry point for on-demand case/package re-print triggered by `modStartup` via `programID = "printcaselabel"`. It reads all context from global state (`gdrSessCtl`, `gdrCmpCfg`, `drSO`) and routes to `PrintDiffLabels`:

```
printCaseLabel(strDeviceType, strLotID)
  │
  ├─ [strDeviceType = CASELABEL AND gblnStartSOWithNoLabel = False]
  │     └─ PrintDiffLabels(CASELABEL, ..., jobName=DefaultPkgLine+ShopOrder)
  │
  ├─ [strDeviceType = PACKAGECODER AND line.Contains("3980")]
  │     └─ PrintDiffLabels(PACKAGELABEL, ..., jobName=jobName+PACKAGELABEL)
  │
  ├─ [strDeviceType = PACKAGECODER (generic)]
  │     └─ PrintDiffLabels(strLabelType, ..., jobName=jobName+PACKAGELABEL)
  │
  └─ [else / gblnStartSOWithNoLabel = True]
        └─ PrintDiffLabels(strLabelType, ...) with no NoOfLabels override
```

**Expiry date handling** (WO#650): If `gblnOvrExpDate = True` AND `drSO.DateToPrintFlag` is `"1"` or `"3"`, `strExpirydate = gdteExpiryDate.ToString("yyyyMMdd")` is passed to `PrintDiffLabels`. Otherwise `strExpirydate = ""`. The global `gdteExpiryDate` is set separately by the expiry date entry UI.

#### 6.2.3 `CreateLabelData` — Server DB Write (L903)

```vb
Public Shared Sub CreateLabelData(strFacility, strLabelType, strDeviceType, strDftPkgLine, strOvrPkgLine,
    intShopOrder, strItemNo, intPalletNo, intQuantity, strOperator, strJobName,
    strLotID, strProductionDate, intShift, strExpiryDate,
    Optional sqlTrn As SqlTransaction = Nothing)
```

**SP**: `PPsp_CreateLabelData` on **Server DB**.

Key parameters and their DB mapping:

| Parameter | SP Param | Type |
|---|---|---|
| `strFacility` | `@chrFacility` | Char |
| `strLabelType` | `@chrLabelType` | Char |
| `strDeviceType` | `@chrDeviceType` | Char |
| `strDftPkgLine` | `@chrDftPkgLine` | Char |
| `intShopOrder` | `@intShopOrder` | Int |
| `strItemNo` | `@vchItemNo` | VarChar |
| `intPalletNo` | `@intPalletID` | Int |
| `intQuantity` | `@intQuantity` | VarChar (sic — type mismatch in code) |
| `strOperator` | `@vchOperator` | VarChar |
| `strJobName` | `@vchJobName` | VarChar |
| `strLotID` | `@vchLotID` | VarChar |
| `strProductionDate` | `@vchOvrdProductionDate` | VarChar (`"yyyyMMdd"` format) |
| `intShift` | `@intShift` | TinyInt |
| `strExpiryDate` | `@vchExpiryDate` | VarChar (`"yyyyMMdd"` or `""`) |

If `sqlTrn` is provided, the call participates in the caller's existing server transaction. If `Nothing`, a standalone connection to `gstrServerConnectionString` is used. Server connectivity errors (`SqlException.ErrorCode = -2146232060`) silently call `SetServerCnnStatusInSessCtl(False)` — the label record is not re-attempted.

#### 6.2.4 `CreatePrintRequest` — Print Queue Write (L1007)

```vb
Public Shared Sub CreatePrintRequest(strLabelType, strFacility, strDftPkgLine, strDeviceType,
    dteStartTime, strJobName, blnSbmFromPalletStaton, strRequestor,
    Optional intCopies As Integer = 1,
    Optional sqlTrn As SqlTransaction = Nothing)
```

**SP**: `PPsp_CreatePrintRequest` on **Server DB**.

**Duplicate guard** (WO#6059): Before writing, calls `IsPrintRequestOverLimit(strFacility, strJobName, strDeviceType)`. If the same job is already in the print queue, a `MessageBox.Show` is displayed and the SP is **not** called. The limit check queries `PositionInPrintQueue(...)` which returns the queue position integer.

Key parameters:

| Parameter | SP Param | Notes |
|---|---|---|
| `strDeviceType` | `@chrDeviceType` | Routes print job to physical device |
| `strJobName` | `@vchJobName` | Unique job identifier (e.g., `"L143980"` = Line + SO) |
| `blnSbmFromPalletStaton` | `@bitSbmFromPalletStation` | Bit flag — affects print queue routing |
| `strRequestor` | `@vchRequestor` | Staff ID of the operator submitting the request (WO#512) |
| `intCopies` | `@intCopies` | SmallInt; defaults to 1; overridden by `gdrCmpCfg("NoOfLabels")` for case labels |

#### 6.2.5 `PrintDiffLabels` — Indirect Print Path (L1907)

`PrintDiffLabels` handles the case where the **override packaging line differs from the default packaging line** — it creates two label data records and two print requests: one for the default line and one for the override line. In the normal case (both lines identical), it creates a single job. It calls `CreateLabelData` then `CreatePrintRequest` internally and is always used via `printCaseLabel`; it is not called directly from forms in the print path.

#### 6.2.6 `ClearLabelData(strDftPkgLine, dteSOStartTime, strFacility)` — Pre-Print Flush (L1805)

Called at the start of every `StartShopOrderUpdate` before any `CreateLabelData`. Removes all pending label data for the current packaging line from the Server DB print staging tables. Prevents stale label data from a prior session from being printed on the new order.

---

### 6.3 Core Calculations

#### 6.3.1 `clsExpiryDate` — Best Before Date Calculator (WO#650)

`clsExpiryDate` encapsulates two shelf-life parameters:

| Parameter | Property | Meaning |
|---|---|---|
| `ProductionShelfLifeDays` | Write-only `Int16` | Total days from production date to expiry (e.g., 365 for 1-year shelf life) |
| `ShipShelfLifeDays` | Write-only `Int16` | Maximum days from production that can still be sold (the shipping window) |

**`IsExpiryDateValid(dteExpiryDate, dteProductionDate) → Boolean`**

The valid expiry window is calculated as:

```
EarliestExpiryDate = dteProductionDate + (ProductionShelfLifeDays - ShipShelfLifeDays)
LatestExpiryDate   = dteProductionDate + ProductionShelfLifeDays

Valid if: EarliestExpiryDate ≤ dteExpiryDate ≤ LatestExpiryDate
```

*Example*: `ProductionShelfLifeDays = 365`, `ShipShelfLifeDays = 180`, `ProductionDate = 2024-01-01`

- `EarliestExpiry = 2024-01-01 + (365-180) = 2024-07-04`
- `LatestExpiry   = 2024-01-01 + 365 = 2025-01-01`
- An entered expiry of `2024-12-01` → Valid

**`IsProductionDateValid(dteProductionDate) → Boolean`**

```
EarliestProductionDate = Today - ShipShelfLifeDays
LatestProductionDate   = Today

Valid if: EarliestProductionDate ≤ dteProductionDate ≤ Today
```

This prevents operators from entering a production date that is either in the future or so far in the past that the product would already be outside the shipping window.

**Data source**: `ProductionShelfLifeDays` and `ShipShelfLifeDays` are loaded from `tblItemMaster` (field names inferred from usage in `IsItemChangedOnServer`, L5096-5097). The class has no DB access of its own — the caller populates it.

#### 6.3.2 `GetSOCasesProducedFromPallet` — Real-Time Production Count (L4584)

```vb
Public Shared Function GetSOCasesProducedFromPallet(
    intShopOrder As String,
    Optional intShift As Integer = 0,
    Optional strTimeInShift As String = "",
    Optional strOperator As String = "") As Integer
```

**SP**: `CPPsp_CasesProducedFromPallet`

**Parameters passed to SP**:

| Param | Source | Notes |
|---|---|---|
| `@chrFacility` | `gdrSessCtl.Facility` | Current facility |
| `@chrPkgLine` | `gdrSessCtl.OverridePkgLine` | Uses override, not default line |
| `@intShopOrder` | `intShopOrder` (caller) | — |
| `@intShiftNo` | `intShift` (0 = all shifts) | 0 means aggregate across full order |
| `@dteGivenTime` | `Now()` or parsed `strTimeInShift` | Upper time boundary |
| `@dteShiftProductionDate` | `gdrSessCtl.ShiftProductionDate` | Date axis for pallet aggregation |
| `@intCasesProduced` | OUTPUT `ParameterDirection.Output` | Result |

**Dual-DB fallback**: If `gblnSvrConnIsUp = True`, the SP runs against `gstrServerConnectionString` (using both `tblPallet` and `tblPalletHst`). If server is offline, falls back to `gstrLocalDBConnectionString` (Local DB `tblPallet` only — historical pallets may be absent).

Result is read from `arParms(UBound(arParms)).Value` — the last OUTPUT parameter.

#### 6.3.3 `GetCarriedForwardCases` and `CarriedForwardCasesFromShift` (L4533, L4552)

Both functions are thin wrappers that call the Local DB SP `CPPsp_CarriedForwardCasesIO` (inferred), passing:

- `strPkgLine`, `intShopOrder`, `intShift`, `dteShiftProductionDate`, `dteGivenTime`, `strFacility`, `strOperator`

`GetCarriedForwardCases` returns the total carried-forward cases for the order. `CarriedForwardCasesFromShift` returns only the cases carried forward from a specific prior shift. Return type was `Short` (pre-FX20160811) and changed to `Integer` after that fix.

#### 6.3.4 `UpdateSessionControl` / `PPsp_LineEfficiency` — Efficiency Calculation (L4220+)

When the Process Monitor form updates, it calls a function that builds 34 parameters (arParms 0-33) and executes `PPsp_LineEfficiency` on the **Server DB**. This SP calculates and returns, via OUTPUT parameters:

| OUTPUT Param | Variable | Meaning |
|---|---|---|
| `@so_Act_Produced` | `int_So_Act_Produced` | Actual cases produced for entire shop order |
| `@so_Act_CsPerHour` | `int_So_Act_CsPerHour` | Actual production rate (cases/hour, SO scope) |
| `@so_Sch_CsPerHour` | `int_So_Sch_CsPerHour` | Scheduled rate |
| `@so_Efficiency` | `dec_So_Efficiency` | Efficiency % (Decimal) |
| `@so_Alert` | `bln_So_Alert` | True if efficiency is below threshold |
| `@so_Progress` | `int_So_Progress` | Progress % toward scheduled quantity |
| `@sft_Act_Produced` | `int_Sft_Act_Produced` | Actual cases this shift |
| `@sft_Act_CsPerHour` | `int_Sft_Act_CsPerHour` | Shift actual rate |
| `@sft_Sch_Produced` | `int_Sft_Sch_Produced` | Shift scheduled quantity |
| `@sft_Efficiency` | `dec_Sft_Efficiency` | Shift efficiency % |
| `@sft_Alert` | `bln_Sft_Alert` | Shift efficiency alert |
| `@sft_Progress` | `int_Sft_Progress` | Shift progress % |

The efficiency formula (`Actual / Scheduled × 100`) is computed server-side; the client simply reads OUTPUT parameters. `DBNull` values are coerced to `0`.

---

### 6.4 Database Access Patterns (`SqlHelper` Wrappers)

#### 6.4.1 `SqlHelper.vb` — Microsoft Data Access Application Block

`SQLHelper.vb` (145 KB) is the unmodified **Microsoft Patterns & Practices Data Access Application Block** (circa 2002), providing static method wrappers over `SqlClient`. The four entry points used throughout `SharedFunctions`:

| Method | Signature Pattern | Common Use |
|---|---|---|
| `ExecuteNonQuery` | `(connStr, cmdType, cmdText, params[])` | DML without result (INSERT, UPDATE, stored procs with no return set) |
| `ExecuteDataset` | `(connStr, cmdType, cmdText, params[])` | Returns `DataSet` |
| `ExecuteReader` | `(connStr, cmdType, cmdText, params[])` | Returns open `SqlDataReader` |
| `ExecuteScalar` | `(connStr, cmdType, cmdText, params[])` | Returns single value (often OUTPUT param via SP) |

**Transaction overloads**: Every `SqlHelper` method has a `(SqlTransaction, ...)` overload that executes within the provided transaction scope. This is the mechanism for the Server DB transaction in `StartShopOrderUpdate` / `CreatePallet`.

**Standard parameter construction pattern** throughout `SharedFunctions`:

```vb
ReDim arParms(N)
arParms = New SqlParameter(UBound(arParms)) {}
arParms(0) = New SqlParameter("@paramName", SqlDbType.VarChar)
arParms(0).Value = someValue
' ...
SqlHelper.ExecuteNonQuery(gstrServerConnectionString, CommandType.StoredProcedure, "spName", arParms)
```

#### 6.4.2 Transactional Write Operations — Commit Order

The following table documents the major write chains involving `SqlTransaction`. All server transactions use `cnnServer.BeginTransaction()`:

| Operation | Transaction Scope | Commit Order |
|---|---|---|
| `StartShopOrderUpdate` | `trnServer` on Server DB | 1→`ClearLabelData` (standalone) 2→`CreateLabelData` (in transaction) 3→`CreatePrintRequest` (in transaction) 4→`ChangeSOStatus` (SP `PPsp_ShopOrderStatus`) 5→`UpdateSessionControl` (in transaction, Local+Server) 6→`trnServer.Commit()` 7→`_dbServer.SaveChanges()` (EF flush) |
| `ProcessFrmCreatePallet` | `trnServer` on Server DB | 1→`CreatePallet` (Local inline SQL + SP `PPsp_GetPalletNo`) 2→`UPDATE tblSessionControl` (Local, standalone) 3→`CreateLabelData` (in transaction) 4→`CreatePrintRequest` (in transaction) 5→`EditPallet` (Local) 6→`trnServer.Commit()` 7→`_dbServer.SaveChanges()` |

> **Local DB is never in a transaction**: All Local DB writes in the above chains use standalone `SqlHelper.ExecuteNonQuery(gstrLocalDBConnectionString, ...)` calls — they are not wrapped in any transaction. If a server transaction rolls back, the corresponding Local DB writes are **not** rolled back.

#### 6.4.3 Server Connectivity Error Handling Pattern

Throughout `SharedFunctions`, server SQL errors are caught with a specific filter:

```vb
Catch ex As SqlClient.SqlException When gblnSvrConnIsUp = True And
    (ex.Number = 40 Or ex.Number = 53 Or ex.Number = 64 Or ex.Number = 121
     Or ex.Number = 1231 Or ex.Number = 10054) And ex.ErrorCode = -2146232060
    SharedFunctions.SetServerCnnStatusInSessCtl(False)
```

`SetServerCnnStatusInSessCtl(False)` persists the offline state to `tblSessionControl.ServerCnnIsOk = False` in the **Local DB** and sets `gblnSvrConnIsUp = False`. After this flag is set, all subsequent server writes are silently skipped (guarded by `If gblnSvrConnIsUp = True Then` checks).

---

### 6.5 Key-Value Tables: `tblIPCControl` and `tblControlTable`

Two Local DB tables serve as configuration/state key-value stores:

#### `tblIPCControl` — Mutable Runtime State Bus

| Operation | Function | SP / SQL |
|---|---|---|
| Read | `GetIPCControl(strSearchKey) As String()` | `CPPsp_IPCControl_Sel` TableAdapter → returns `{Value1, Value2}` |
| Write | `IPCControlUpdate(strControlKey, strValue1, strValue2)` | `CPPsp_IPCControl_Upd` via `SqlHelper.ExecuteNonQuery` on Local DB |

Known `ControlKey` values visible in source:

| ControlKey | Value1 | Value2 | Set By |
|---|---|---|---|
| `IsSOStartedWithNoLabel` | `"YES"` / `"NO"` | — | `SaveStartSOOption` (L5212) |
| `PreviousPalletCreationTime` | Timestamp string | — | `IPCControlUpdate` in `ProcessFrmCreatePallet` |
| `PalletCreationTimeLimit` | Integer seconds | — | Read-only; set via DB admin |
| `QAT_BatchID` | DateTime string | — | `UpdateQATBatchID` (L6569) |

#### `tblControlTable` (Server DB) — Read-Only Configuration

```vb
GetConrolTableValues(strKey, strSubKey, Optional strAction) As Array
```

- SP: `PPsp_Control_Sel` on **Server DB**
- Returns `String() = {Value1, Value2}`
- On `SqlException`: silently returns `{String.Empty, String.Empty}` (does not propagate error)
- Used for: `PalletCreationTimeLimit`, feature flags

---

### 6.6 Environment & Process Helpers

#### 6.6.1 `IsProcessActive(strProcessName) As Boolean` (L5018)

```vb
For i As Integer = 1 To 2
    processList = Process.GetProcessesByName(strProcessName)
    If processList.Length = 0 Then
        Thread.Sleep(1000)
    Else
        blnIsActive = True : Exit For
    End If
Next
Return blnIsActive
```

- Tries twice with 1-second sleep between attempts
- Uses `Process.GetProcessesByName` — matches by process image name, not window title
- Called before `CallTcpServer` (PPTCPServer guard), before `StartDownTimePgm` (DownTime guard), and in `modStartup` single-instance guard

#### 6.6.2 `WakeUpAPgm(strProcessName)` (L5060)

Finds an already-running process instance and brings its window to the foreground:

1. Calls `GetRunningInstance(strProcessName)` up to 3 times with 1-second sleep between attempts
2. Gets `MainWindowHandle` via `pInstance.MainWindowHandle`
3. Calls `Win32Helper.ShowWindow(handle, ProcessWindowStyle.Maximized)` then `Win32Helper.SetForegroundWindow(handle)`

Used to show the DownTime window if it is already running.

#### 6.6.3 `StartDownTimePgm(...)` (L5037)

Constructs a command-line argument string for `DownTime.exe`:

```
Format: "P {PkgLine} {ShopOrder} {Operator} {Shift} {Facility} {PkgLineType} [{StartTime}]"
```

`StartTime` (format `"yyyy-MM-ddHH:mm:ss.fff"`) is appended only if `gdrSessCtl.ShopOrder ≠ 0`. Launches via `Process.Start(My.Settings.strDownTime, strCmdArgs)`.

#### 6.6.4 `GetTouchScreenCalibrationPgm() As String` (L5408)

Reads `HKLM\SYSTEM\CurrentControlSet\Services\Mouclass\Enum` to detect the connected touch screen vendor:

- Checks registry `Count` value → iterates HID entries in reverse
- Matches against known vendor prefixes (`USB\VID_04E7&PID_0050` = Elo, `USB\VID_0EEF` = EloVA, `USB\VID_0596` = TouchKit)
- Returns the corresponding calibration program path from `My.Settings` (`gstrUSBTouchScreenPgm1`, `gstrUSBTouchScreenPgm2`, `gstrAutoCal4PtsCalProgram`)
- Default return: `My.Settings.gstrUSBTouchScreenPgm2`

#### 6.6.5 `DownloadDataFromServer(strFacility, strComputerName)` (L5458) — Table Sync Engine

This is the **proprietary Server→Staging→Local table push mechanism** (WO#755). It runs against three connections: Server DB, Staging DB (a separate SQL instance), and Local DB.

```
DownloadDataFromServer(strFacility, strComputerName)
  │
  │ Phase 1 — Read table list from Staging DB
  ├─ SELECT TableName FROM tblDownLoadTableList [Staging]
  │
  │ Phase 2 — For each table, check if it has a Facility column
  ├─ SELECT 1 FROM information_schema.columns WHERE table_name=? AND column_name='facility' [Server]
  │
  │ Phase 3 — Pull data from Server
  ├─ IF hasFacilityColumn: SELECT * FROM tableName WHERE facility = strFacility [Server]
  │  ELSE:                 SELECT * FROM tableName [Server]
  │
  │ Phase 4 — Write to Staging (Bulk Copy)
  ├─ SET tblComputerConfig.ReadyForDownLoad = 0 [Staging]
  ├─ DELETE/TRUNCATE tableName [Staging] (DELETE for facility-filtered tables, TRUNCATE for global)
  └─ SqlBulkCopy.WriteToServer(ds.Tables(tableName)) [Staging]
       └─ BulkCopyTimeout = 1800 seconds (30 minutes)
  │
  │ Phase 5 — Update download status flags
  ├─ UPDATE tblDownLoadTableList SET DownLoadStatus=0, Active=1, LastDownload=Now [Staging]
  ├─ SET tblComputerConfig.ReadyForDownLoad = 1 [Staging + Server]
  └─ UPDATE tblDownLoadTableList SET Active=0 [Server]  (marks all complete)
```

**Key design note**: The Local DB is **not written to** by this function. `DownloadDataFromServer` writes to the **Staging DB**. A separate process on the IPC workstation (which connects to the Staging DB as its local DB) picks up the data. This function is the server-to-staging push; the staging-to-local pull is handled externally.

---

### 6.7 UI Utility Helpers

| Function | Behaviour |
|---|---|
| `PopNumKeyPad(frm, ctl, ...)` | Shows `frmNumKeyPad` anchored below `ctl`, bounded to 800×600 form dimensions. Supports optional `strPasswordChar` for masked PIN entry. |
| `PopAlphaNumKB(frm, ctl, ...)` | Same layout logic for `frmAlphaNumKB`. |
| `PopRegularKeyPad(frm, ctl, ...)` (L7760) | Regular keyboard popup with same bounds logic. |
| `ClearInputFields(frm)` | Iterates `frm.Controls`, sets all `TextBox.Text = ""`. Not recursive — misses nested controls in panels/groupboxes. |
| `ClearLabelsText(frm)` | Same pattern for `Label.Text`. |
| `showSplash2(objMessage)` (L7792) | Shows a splash/loading screen with a message object. |
| `ActivateForm(strFormFullName)` (L6045) | Uses `Type.GetType` + `Activator.CreateInstance` to instantiate any form class by name — the same reflection mechanism used by QAT workflow. |

---

### 6.8 Password Encryption (WO#17432)

```vb
Public Shared Function EncryptPassword(vchPassword As String) As String
Public Shared Function DescryptPassword(vchPassword As String) As String  ' [sic — typo in original]
```

Located at L7641/L7646. Uses `System.Security.Cryptography` (imported at module top). The encrypted value is stored in `tblPlantStaff` and compared during `frmQATOverrideLogOn` supervisor authentication. The encryption algorithm is not visible in the grep results but the namespace import confirms it uses .NET's built-in symmetric crypto (AES/DES family).

`SavePlantStaffPassword` (L7652) is the write path — it calls `EncryptPassword` before persisting, and is accessible from `frmChangePassword`.

---

### 6.9 `HasConnectivity()` — Network Check (L4662)

```vb
Public Shared Function HasConnectivity() As Boolean
    Dim sServer As String = Environment.GetEnvironmentVariable("logonserver")
    hostInfo = System.Net.Dns.GetHostEntry(sServer.Remove(0, 2))  ' strips leading "\\"
    Return True   ' exception → Return False
End Function
```

Resolves the Windows domain logon server via DNS. `logonserver` environment variable contains `\\DCNAME` (two leading backslashes stripped before DNS lookup). Returns `False` if DNS resolution fails. Used as a general Active Directory connectivity check — distinct from `CheckIPAddress` which pings a specific IP.

### 6.10 Data ETL Integration (`KronosETL`)

*Source: `Visual Studio 2012\Projects\KronosETL`*

While MES handles operational constraints, labor and timecard definitions exist in external HR systems (Kronos). The `KronosETL` application acts as the automated data broker that provides the employee and logic context for the Chapter 10 "Labor Efficiency" SSRS Reports.

1. **API Polling (`HttpClientHelper.vb`)**: Reaches out to the Kronos Cloud API via `HttpClient` using configured tokens to download JSON payloads containing `employee`, `schedule`, and `timecarddetail` segments.
2. **Cleansing & Ingestion (`Module1.vb`)**: Instead of processing the JSON logic in VB.NET, the payload is directly passed into SQL via `SqlHelper.ExecuteNonQuery` calling staging stored procedures (e.g., `KRsp_CvtJsonToTable_Employee`).
3. **Database Integration**: These procedures shred the JSON and merge it with MES staffing structures, enabling the `PPsp_LineEfficiency` and SSRS logic to cross-reference operator efficiency with accurate shift schedules.

### 6.11 Remote Invocation Interface (`PPWCFService`)

*Source: `Visual Studio 2012\Projects\PPWCFService`*

The MES generally acts as a closed-loop system managed by touch screens, but external systems occasionally need to force the WinForms client into a specific state (e.g., forcing a QA check or throwing a downtime screen dynamically).

This is achieved via the `PPWCFService`, which exposes a WCF SOAP/REST Endpoint (`PPIService.vb`):

1. **The Endpoint**: `Public Sub ShowForm(strFormID, strInterfaceID, strQATWorkFlowType)`
2. **The Relay (`PPService.vb`)**: When external software invokes this endpoint, `PPWCFService` acts as a middleman. It opens a raw `TcpClient` connection specifically to the IPC's local loopback proxy: `127.0.0.1:8000`.
3. **The Receiver (`PPTCPServer`)**: As documented in §5.7, the local `PPTCPServer` listens on port 8000. It reads the string `"{FormID} {InterfaceID} {QATWorkFlowType}"`.
4. **The Trigger (`InvokePPForm`)**: The TCP Server processes the string and spawns UI overlays on top of the main WinForms client, achieving "remote control" rendering without modifying the main client codebase.

### Refactoring Scorecard: Chapter 6 (Shared Logic)

| Factor | Score (1-10) | Description | Actionable Recommendation |
|---|---|---|---|
| **Fragility** | **10** | `SharedFunctions.vb` is a God-Object containing all critical business logic. Modification risks breaking unrelated modules. | **Critical Priority:** Break out into single-responsibility C# services (e.g., `ITimeService`, `IValidationService`). |
| **Security** | **6** | Raw SQL string concatenations exist across utility functions. | Eliminate all non-parameterized SQL via EF Core and Dapper. |
| **Complexity** | **9** | Over 7000 lines of tightly coupled procedural runtime mapping limits testability. | Extract Core Domain rules to pure functions and write unit tests for edge cases. |

*(End of Chapter 6)*

---

*MES System Codebase Handbook — PowerPlant VB.NET — Chapters 0–6 complete. Last updated: 2026-03-24*
---

## Chapter 7 — Database Stored Procedures & Logic

*Source locations: `DB Server_PowerPlant/dbo/StoredProcedures/`, `DB Local_PowerPlant/dbo/StoredProcedures/`*

This system offloads significant computational logic to the database layer via stored procedures (SPs). While modern applications often push business logic to a middle tier (Application/Domain layer), PowerPlant centralizes its real-time analytics, state control, and label formatting within SQL Server to ensure atomicity and reduce data transfer over the network.

### 7.1 SP Naming Conventions & Dual-DB Routing

The system employs a strict prefix convention to route commands to the correct database (Server vs. IPC Local).

| Prefix | Target Database | Execution Context | Examples |
|---|---|---|---|
| `PPsp_` | Server DB | Central MES database (e.g., `MPHOPP02`). Handles global state, master data, heavy joins, and historical tracking. | `PPsp_LineEfficiency`, `PPsp_ShopOrderStatus` |
| `CPPsp_` | Local DB | IPC local database (SQL Express). Handles real-time session state, hardware polling, and localized configuration. | `CPPsp_SessionControlIO`, `CPPsp_IPCControl_Upd` |

- **Routing Mechanism**: `SharedFunctions` methods containing `If gblnSvrConnIsUp = True` statements typically execute a `PPsp_` against the Server. Operations requiring instant local state modification execute `CPPsp_` against the Local DB.
- **Failover exception**: Some `CPPsp_` SPs exist on the server to mirror local definitions, functioning as a fallback logic layer natively wrapped inside local logic calls.

### 7.2 Core Business SPs Deep-Dive

#### 7.2.1 `PPsp_LineEfficiency` — The Real-Time Dashboard Engine

This is the most complex SP in the system (over 500 lines). It calculates the 12 output parameters consumed by the WinForms UI dashboard for "Actual vs. Scheduled" production constraints and efficiency alerts.

**Data Flow & Join Strategy:**

1. **Time Boundaries**: Uses `fnShiftInfo` and `fnGetProdDateByShift` scalar functions to explicitly determine `@dteShiftStart` and `@dteShiftEnd`.
2. **Session Snapshot (`#temp_SessionControlHst`)**: Aggregates the Shop Order's active session state in `tblSessionControlHst` (merging start/stop times and stray loose cases).
3. **Pallet Tally (`#temp_PalletHst`)**: `UNION ALL` spans `tblPallet` (active) and `tblPallethst` (historical) for the specific `ShopOrder`, `Operator`, and `Shift`.
4. **Calculations (`#tmp_LineEff_SO`)**:
   - `ActualDuration`: Hours run, derived from `SessionControlHst` start/stop times diff calculation matrix.
   - `ActQty`: Sum of all pallets + current loose cases - previous loose cases.
   - `StdUnitPerHr`: Queried directly from `tblStdMachineEfficiencyRate` crossing `ItemNumber`, `Facility`, and `PkgLine` (or rolling back to `WorkCenter` code matching).
5. **Efficiency Algorithm**:
   - **`@so_Efficiency`** = `(ActQty / ActualDuration) / StdUnitPerHr * 100` (%)
   - **Alert (`@so_Alert`)**: Flips to `1` (bit truth) if calculated efficiency `<` target `StdWCEfficiency`.

*Key Insight*: These heavy analytical calculations run synchronously returning ADO.NET `OUTPUT` parameters whenever `UpdateSessCtl` saves context. Database query speed operates as a primary governor over WinForms UI paint performance.

#### 7.2.2 `PPsp_ShopOrderStatus` — The State Controller

Instead of blindly commanding an update towards a status parameter, `PPsp_ShopOrderStatus` cross-references four tables to return an atomic, mutually consistent string spanning the shop order state.

**Logic Chain:**

1. Discovers `tblToBeClosedShopOrder` (Pending status). Returns internal PowerPlant closing timestamp.
2. If empty, steps to `tblClosedShopOrderHst` (Committed to ERP/BPCS via integration tables).
3. Crosschecks `tblShopOrder.Closed` boolean flag.
4. If logically closed but lacking formal closure records, heavily infers concluding time via `MAX(CreationDateTime)` from `tblPallet`/`tblPallethst`.

*Behavioral Effect*: The calling VB code inherently receives human-readable state keys (`"Open"`, `"Closed at YYYY-MM-DD..."`, `"Reopen"`) and subsequently toggles UI buttons. This offloads state machine tracking purely into SQL syntax and bypasses overlapping IPC synchronization race conditions.

#### 7.2.3 `PPsp_CreateLabelData` — The Label String Generator

When `printCaseLabel` issues a print request, `PPsp_CreateLabelData` parses exact descriptive text formatting out of domain configurations.

- **Item Master Context**: Pulls `ProductionShelfLifeDays`, `ProductionDateDesc` (e.g. "PROD"), `ExpiryDateDesc` (e.g. "BBE"), and custom formatting keys (`LabelDateFmtCode`) mapping from `tblItemMaster`.
- **String Construction**: Evaluates the `DateToPrintFlag` (0=None, 1=Expiry, 2=Prod, 3=Both) setting:
  - Sets `@vchPreFmtProductionDate` = "[DateDesc] [Formatted Date string output by `fnConvertDate`]"
  - Derives accurate expiry bounding via `DATEADD(day, @intShelfLife, @dteProductionDate)`
- **Insert/Update Subroutine**: Commits the computed label layouts alongside intrinsic hardware routing configurations (`RecordType`='C' or 'P') to `tblDynamicLabelData`. A parallel CoLOS service iteratively polls this structure for string-level printer mapping.

### 7.3 Dual Database Synchronization Engine

The local SQL Express databases installed tightly on plant floor IPCs operate as an instantiated mirror of crucial server environments. While `DownloadDataFromServer` handles the network-bound push operation from code, an array of backend SPs orchestrate dataset parsing cleanly across the divide.

- **`tblDownLoadTableList` (Staging/Server DB)**: Acts effectively as the local replication manifest log. Exposes flag states `TableName`, `Active`, and `DownLoadStatus`.
- **Sync Trigger Routine**: Once VB runtime bulk-drops server properties into IPC staging environments, SP queries forcibly trigger `Active=1`.
- **`LPPsp_CopyImportedDataToProduction`** (Local DB execution): Operating inside IPC SQL context, invoked consistently to process staging commits. Iterates sequentially across `tblDownLoadTableList WHERE Active = 1`, dropping table structures using `TRUNCATE` operations, and streams bulk imports entirely from staging partitions to canonical datasets. Concludes successfully by reverting `Active=0`.

### 7.4 Performance Profiling & Lock Escalation Limits

Legacy MS SQL architectures scaling directly to connected UI models persistently encounter table lock escalation blocking behaviors. Reviewing specific table handling rules establishes the system resilience expectations.

- **Absence of `NOLOCK` Isolation Flags**: Uniquely, a full operational repository survey identifies that `WITH (NOLOCK)` read-uncommitted syntax remains entirely absent from the core critical loops handling production data (most notably missing across all blocks of `PPsp_LineEfficiency`). The only prominent usage surfaces strictly in `PPsp_LabelPrintJobs_Sel` polling queues like `tblCimControlJob`.
- **Primary Risk Vectors**: Because operations such as `PPsp_LineEfficiency` construct thick dependencies directly over `tblPallet` without row-level decoupling syntax, concurrent long-running real-time dashboards actively maintain potential block limits over rapid-fire pallet scanning local transactions.
- **Compensating Controls**: The codebase deliberately bypasses pure uncommitted reads by explicitly streaming high-impact live datasets repeatedly into local session cache allocations (such as `CREATE TABLE #temp_PalletHst`) at the very beginning of the SP, successfully freeing lock chains and moving the intense mathematical workload locally off main production indices.

---

### 5.7 PPTCPServer — Server-Side IPC Gateway Implementation

*Source: `Power Plant Codes\Visual Studio 2012\Projects\PPTCPClientServer\PPTCPServer\PPTCPServer.vb` (624 lines)*

`PPTCPServer.exe` is the **out-of-process TCP gateway** that `PowerPlant.exe` connects to via `CallTcpServer()`. It is a standalone VB.NET console application (not a Windows Service) that runs on the same IPC workstation as the MES client.

#### 5.7.1 Startup & Pre-flight Guards

On `Sub Main()`, before opening any port, `PPTCPServer` performs **three sequential guards** that can all cause early exit:

| Guard | Mechanism | Outcome on Failure |
|---|---|---|
| **Single-instance check** | `Process.GetProcessesByName(CurrentProcess)` — if `Length > 1`, a duplicate is running | Writes to Windows Event Log, exits |
| **SQL Express health check** | `isServiceRunning("MSSQL$SQLEXPRESS")` polls `ServiceController.Status` for up to 15 seconds (1-second intervals) | Logs error, exits |
| **QAT workflow mode check** | Reads `tblComputerConfig.QATWorkFlowInitiation` from Local DB; only proceeds if `= 1` | Logs "NOT using external system", exits |

This means **`PPTCPServer` only starts its listener if the IPC is configured as an external QAT workflow host**. Standard IPC bays without this flag never open a socket.

Connection parameters (from `app.config`):

- **Listen address**: `127.0.0.1:8000` (loopback only — not exposed to the network)
- **Server DB**: `MPHOPP02\PowerPlantAXSP_Dev` (7-second connect timeout)
- **Local DB**: `.\SQLEXPRESS\LocalPowerPlant` (Integrated Security)
- **PP Client path**: `C:\Powerplant\powerplant\palletizer.exe` (the MES client executable)

#### 5.7.2 Connection Handler: `NewClients()`

The accept loop uses a **pseudo-concurrent pattern** rather than true multi-threading:

```vb
While True
    tcpClient = tcpListener.AcceptTcpClient()      ' Blocks until connection arrives
    tcpClients.Add(tcpClient)
    Threading.ThreadPool.QueueUserWorkItem(AddressOf NewClients)  ' Queue another listener
    
    networkStream = tcpClient.GetStream()
    ReDim bytes(tcpClient.ReceiveBufferSize)
    networkStream.Read(bytes, 0, ReceiveBufferSize) ' Single synchronous read
    
    strClientData = Encoding.ASCII.GetString(bytes)
    ' ... route the command ...
End While
```

**Important design detail**: After accepting a connection, it immediately queues *another invocation* of `NewClients` to the `ThreadPool` before processing the current client. This means concurrency is handled by the ThreadPool rather than explicit thread management. Each accepted client is served by whichever ThreadPool thread picks up the work item.

**The dispatcher branch** (L132-143): After sending the handshake ack (`"Connected to server."`), incoming data is routed by a single prefix check:

```
if strClientData.StartsWith("@")  →  CollectCheckweigherData(strClientData)
else                               →  Process.Start(strPPPgmPath, strClientData)
```

- **`@` prefix**: Checkweigher hardware commands — passed to the hardware manager
- **Non-`@` data**: Treated as **command-line arguments for `palletizer.exe`** — launches a fresh MES client instance with the received string as arguments

> **Key insight**: The "non-@ path" is how `PPTCPServer` acts as a **process launcher**. `PowerPlant.exe` sends a program ID string (e.g., `"printcaselabel [args]"`) and the server spawns a new MES instance in hidden window mode (`ProcessWindowStyle.Hidden`) to handle it.

#### 5.7.3 Command Protocol: `@CWLog` Dispatch

Commands prefixed with `@` follow this format (L216-222):

```
@ {TCPConnID} {Action}
  │             │
  │             └── 0 = Start logging  (Action.Start)
  │                 1 = Abort logging  (Action.Abort)
  └── Integer ID of the TCP connection record in tblQATTCPConn (Local DB)
```

`CollectCheckweigherData` (L207-261) parses the two parameters and dispatches:

| Action | Behaviour |
|---|---|
| **Start (0)** | Calls `RefreshSessionControlTable()` to get current `gdrSessCtl`. If `gblnLoadCheckWeigherStarted = False`, spawns a new background thread (`gthdCheckWeigher`) running `CheckWeigherManager()`. If thread already alive, no-ops. |
| **Abort (1)** | Sets `gblnLoadCheckWeigherStarted = False` — the polling loop in `CheckWeigherManager` reads this flag and exits on the next iteration. The thread is NOT forcibly aborted; it exits cleanly via the `Do While gblnLoadCheckWeigherStarted = True` guard. |

#### 5.7.4 Hardware Proxy: `CheckWeigherManager` & `LoadDataFromCheckWeigher`

`CheckWeigherManager` (L263-295) is the thread entry point. It:

1. Calls `RefreshSessionControlTable()` and snapshots `gintShopOrderBackup = gdrSessCtl.ShopOrder`
2. Queries `tblQATTCPConn` by `gintTCPConnID` to get the checkweigher `IPAddress`, `Port`, `Model`, and `Command3` (the WD format command)
3. Enters a polling loop: `LoadDataFromCheckWeigher(drTCPConn)` every **2 seconds**

`LoadDataFromCheckWeigher` (L297-430) implements the WD protocol sequence for **XC/XE model checkweighers**:

```
TCPConnect()                        — Connect to IP:Port
IsHandShakeOK()                     — Validates hardware is responsive
TCPSendCommand(drTCPConn.Command3)  — e.g. "WD_SET_FORMAT 4" (weight-only format)
Thread.Sleep(3000)
TCPSendCommand("WD_START")          — Begin streaming weight readings
Thread.Sleep(2000)
─── Spawn SaveData thread ─────────────────────────────────────────────
Do While gblnLoadCheckWeigherStarted = True
    Thread.Sleep(2000)
    strRecievedData = .ReceiveData()
    If data received:
        queReceivedData.Enqueue(strRecievedData)   — Push to queue
        mre.Set()                                   — Signal SaveData thread
    If no data for 15 minutes:
        Check gdrSessCtl.IsStartDownTimeNull
        If not in downtime: accumulate idle interval counter
        If idle > (4 * intMaxIdleHours): set gblnLoadCheckWeigherStarted = False
Loop
TCPDisconnect()
```

**Idle timeout**: `intMaxIdleHours = 3` (from `app.config`). After `4 × 3 = 12` idle intervals of 15 minutes each = **3 hours of no data**, the checkweigher thread stops automatically.

#### 5.7.5 Producer-Consumer Queue: `SaveData` Thread

`PPTCPServer` uses a classic **producer-consumer pattern** with a `System.Collections.Queue` and `ManualResetEvent`:

| Role | Thread | Mechanism |
|---|---|---|
| **Producer** | `LoadDataFromCheckWeigher` (CheckWeigher thread) | `queReceivedData.Enqueue(data)` → `mre.Set()` |
| **Consumer** | `SaveData` (dedicated SaveData thread) | `mre.WaitOne()` blocks; on signal, drains queue via `Dequeue()` → `FormatCheckWeigherData()` |

`FormatCheckWeigherData` (L521-563):

- Splits the raw stream by `CrLf` (each line = one weight reading)
- For `XC`/`XE` models: strips the prefix up to the first space, leaving the numeric weight string
- Strips non-numeric characters via `GetActualWeight()` (keeps only `0-9` and `.`)
- Calls `SaveCheckWeigherLog(decActualWeight, DefaultPkgLine, ShopOrder, StartTime)`

#### 5.7.6 Weight Persistence: `SaveCheckWeigherLog` (L565-621)

SP called: **`CPPsp_CheckWeigherLog_Add`** (note: same `CPPsp_` prefix → exists on **both** Server and Local DB).

**Dual-DB persistence with failover** (identical pattern to `SharedFunctions`):

```
If gblnSvrConnIsUp = True:
    Try → Server DB SP call
    Catch SqlException (40/53/64/121/1231/10054):
        SetServerCnnStatusInSessCtl(False)
        → Local DB SP call (fallback)
Else:
    → Local DB SP call
```

Parameters: `@decActualWeight`, `@chrPackagingLine`, `@intShopOrder`, `@dteSOStartTime`, `@dteTestTime`.

**ShopOrder fallback** (L549-553): If `gdrSessCtl.ShopOrder = 0` (session ended between readings), uses `gintShopOrderBackup` (snapshotted at thread start) to ensure the weight record is still attributed to the correct order.

---

### 7.5 ExportSQLDataToIPC — The Physical Sync Engine

*Source: `Power Plant Codes\Visual Studio 2012\Projects\ExportSQLDataToIPC\ExportSQLDataToIPC\Module1.vb` (321 lines)*

`ExportSQLDataToIPC.exe` is the **physical executor of the dual-database synchronization pipeline** described in Ch7.3. It is a command-line console application triggered by the server-side scheduling mechanism (scheduled task or ERP integration event) whenever master data changes need to be pushed to plant floor IPCs.

#### 7.5.1 Invocation & Parameters

The executable takes **3 required + 1 optional** command-line arguments:

| Position | Argument | Example |
|---|---|---|
| `arg[0]` | SQL Server Instance Name | `MPHOPP02` |
| `arg[1]` | Source DB Catalog Name | `PowerPlantAXSP_Dev` |
| `arg[2]` | Facility Code | `09` |
| `arg[3]` | *(Optional)* Log file path override | `D:\Logs\Download_20240315.txt` |

If `arg[3]` is omitted, the log file defaults to: `{ExeDir}\Log\Download_{yyyyMMdd}.txt`

The connection string is dynamically assembled from `arg[0]` and `arg[1]` using the template from `App.config`:

```
Data Source={0};Initial Catalog={1};...;User ID=dataimporter;Password=dataimporter#1
```

> **Security note**: The `dataimporter` SQL login with a hardcoded password (`dataimporter#1`) is embedded in the connection string template in `App.config`. This is a significant security risk if the executable or config file is not properly access-controlled on the server.

#### 7.5.2 Sync Pipeline: `Main()` → `ExportDataToIPCStagingArea()`

**Phase 1 — Mark Server as "In Progress"** (L52-55, `cnnLocalDB`):

```sql
UPDATE tblComputerConfig SET ReadyForDownLoad = 0
WHERE Facility = '{facility}' AND RecordStatus = 1
```

This prevents IPC workstations from attempting a local pull while the push is in progress.

**Phase 2 — Read the Active Table Manifest** (L57-64, `cnnLocalDB`):

```sql
SELECT TableName FROM tblDownLoadTableList
WHERE Facility = '{facility}' AND Active = '1'
```

Only tables with `Active = 1` are included. The `Active` flag is set to 1 by upstream SPs when data changes occur (e.g., `PPsp_PostProcess_DownLoadItemMaster` sets `Active = 1` after processing an Item Master update from ERP).

**Phase 3 — Pull Data from Server into Memory** (`ExportDataToIPCStagingArea`, L119-151):

For each table in the manifest, the engine:

1. Checks `information_schema.columns` to determine if the table has a `facility` column
2. If yes: `SELECT * FROM {table} WHERE facility = '{facility}'` — facility-scoped pull
3. If no: `SELECT * FROM {table}` — full table pull
4. Loads result into `ds.Tables(tableName)` in memory (`DataSet` in RAM)

**Phase 4 — Enumerate Active IPCs** (L154-161):

```sql
SELECT ComputerName, PackagingLine FROM tblComputerConfig
WHERE RecordStatus = 1
AND (facility = '{facility}' OR PackagingLine = 'Spare')
ORDER BY ComputerName
```

This includes all active IPC workstations for the facility **plus** any configured Spare IPCs.

**Phase 5 — Push to Each IPC's Staging DB** (L164-261): For every IPC:

```
Target Connection: {ComputerName}\SQLEXPRESS\ImportData
                   (user: dataimporter, pass: dataimporter#1)

① SET ReadyForDownLoad = 0  [IPC Staging tblComputerConfig]
② For each table:
   a. TRUNCATE {table}                           (global tables)
      or DELETE {table} WHERE facility = '...'   (facility-scoped, Spare IPC only)
   b. SqlBulkCopy.WriteToServer(ds.Tables(table))
      BulkCopyTimeout = 1800 seconds (30 min)
      SqlBulkCopyOptions.TableLock
   c. UPDATE tblDownLoadTableList
      SET DownLoadStatus = 1, Active = 1, LastDownload = getdate()
      WHERE Facility = '...', TableName = '...'
③ If at least 1 table succeeded:
   SET ReadyForDownLoad = 1  [Server tblComputerConfig]
   SET ReadyForDownLoad = 1  [IPC Staging tblComputerConfig]
```

**Phase 6 — Reset Active Flags on Server** (L264-268, `cnnLocalDB`):

```sql
UPDATE tblDownLoadTableList SET Active = 0 WHERE Facility = '{facility}'
```

This marks the sync cycle as complete on the server side.

#### 7.5.3 Error Handling & Consistency Model

| Failure Scenario | Behaviour | Data Consistency Impact |
|---|---|---|
| **IPC unreachable** (connection refused) | `Catch ex` on `cnnDescDB.Open()` → `Continue For` (skips this IPC, processes next) | Skipped IPC retains stale data; `ReadyForDownLoad` stays 0 on that IPC |
| **Non-Spare IPC failure** | Calls `sndEmail()` → SP `PPsp_SndMsgForSupport` on Server DB to trigger support notification | Operations team alerted; IPC will not receive new configuration |
| **Spare IPC failure** | Silently ignored (`UCase(PackagingLine) = "SPARE"` check suppresses email) | Spare IPC skips update; acceptable since it is not in active production |
| **Mid-table BulkCopy failure** | Exception propagates to outer `Catch ex` → logs + email | `blnAtLeast1Success` may already be `True` if prior tables succeeded; `ReadyForDownLoad` could be set 1 even with partial data |
| **Server manifest reset failure** | If Phase 6 fails, `tblDownLoadTableList.Active` remains 1 | Next run will re-process all tables marked `Active = 1` — effectively a retry |

> **Critical gap**: There is **no transaction wrapping the BulkCopy + tblDownLoadTableList update** within a single IPC's loop. If the bulk copy succeeds but the `tblDownLoadTableList` update fails, the IPC has new data but `Active` is not set to 1, which means `LPPsp_CopyImportedDataToProduction` will not be triggered. Manual intervention (setting `Active = 1` in the staging DB) would be required.

#### 7.5.4 Log File

Every major event is written to the daily log file via `WriteLog()`:

- `WriteLog` appends `"{DateTime} - {message}\r\n"` to the log file
- Also writes to `Console.Out` (visible if run interactively)
- Log file path pattern: `{ExeDir}\Log\Download_{yyyyMMdd}.txt`

The log captures: table manifest listing, per-IPC start/end markers, per-table export confirmations, and any errors. This is the primary diagnostic tool for sync failures.

#### 7.5.5 Complete Sync Pipeline Summary

```
[Server Scheduled Task / ERP Event]
         │
         ▼
ExportSQLDataToIPC.exe {ServerName} {DB} {Facility}
         │
         ├─ Phase 1: Mark server ReadyForDownLoad = 0
         ├─ Phase 2: Read Active table list from tblDownLoadTableList
         ├─ Phase 3: Pull table data into RAM DataSet (Server DB)
         ├─ Phase 4: Get IPC list from tblComputerConfig
         │
         └─ For each IPC:
              ├─ Connect to {IPC}\SQLEXPRESS\ImportData
              ├─ Set ReadyForDownLoad = 0 (IPC staging)
              ├─ For each table:
              │    ├─ TRUNCATE / DELETE (IPC staging)
              │    ├─ SqlBulkCopy → IPC staging table
              │    └─ SET tblDownLoadTableList Active=1, DownLoadStatus=1 (IPC staging)
              └─ Set ReadyForDownLoad = 1 (Server + IPC staging)
         │
         └─ Phase 6: SET tblDownLoadTableList Active=0 (Server)
                                │
                                ▼
                    [IPC Local Agent: LPPsp_CopyImportedDataToProduction]
                    Reads Active=1 from staging → Copies to Local PowerPlant DB
```

---

### 7.6 Database-Level Views, Functions & CLR Assembly

*Source: `DB Server_PowerPlant/dbo/Views/`, `DB Server_PowerPlant/dbo/Functions/`, `DB Server_PowerPlant/Assemblies/`*

Beyond stored procedures, the database layer contains critical **Views**, **Table-Valued Functions (TVFs)**, **Scalar Functions**, and a **SQL CLR Assembly** that form the read-layer abstraction for all reporting and printing subsystems.

#### 7.6.1 SQL CLR Assembly — `ExportDataToIPCDB`

*Source: `DB Server_PowerPlant/Assemblies/ExportDataToIPCDB.sql`*

This is the most architecturally unusual object in the entire database. It is a **SQL Server CLR Assembly** — a compiled .NET DLL (`PERMISSION_SET = UNSAFE`) embedded directly inside SQL Server and registered as `[ExportDataToIPCDB]`.

**Role**: The assembly exposes the `CLRExportDataToIPC` stored procedure. When executed by the SQL Server Agent or `ExportSQLDataToIPC.exe` (Ch7.5), this VB.NET-compiled code runs *inside* the SQL engine and:

1. Reads `tblDownLoadTableList` to enumerate which tables need to be pushed to each IPC.
2. Connects to each registered IPC's local `SQLEXPRESS` instance (from `tblComputerConfig.ComputerName`) via a direct `SqlConnection` from within the SQL process.
3. Uses `SqlBulkCopy` to efficiently transfer active staging rows from `DB Server_ImportData` tables to the IPC's `DB Local_PowerPlant` tables.
4. Updates `tblDownLoadTableList.LastDownLoad` and resets `ReadyForDownLoad` flags.
5. Sends email alerts via the internal `sndEmail` method on failure.

> [!CAUTION]
> The assembly uses `PERMISSION_SET = UNSAFE`, granting it unrestricted access to the OS, network, and file system from within SQL Server. This is required for `SqlBulkCopy` and outbound network connections but represents a significant security surface. Any CLR vulnerability in this DLL affects the entire SQL Server instance.

#### 7.6.2 Core Views

| View | Tables Joined | Purpose |
| --- | --- | --- |
| **`vwLabelData`** | `tblDynamicLabelData` + `tblItemMaster` + `tblItemLabelOvrr` | The **primary data feed for CoLOS label printing**. Merges live production label variables with item master fields and override rules. Applies `WO#6437` override logic: if a pallet record (`RecordType='P'`) exists in `tblItemLabelOvrr`, the pallet label format is substituted. The `BatchNumber` column (`ShopOrder + '-' + MPProductionDate`) is computed in-view for case label identity. CoLOS reads this view directly to populate label templates. |
| **`vwItemMasterWithOvrr`** | `tblItemMasterFromERP` + `tblItemLabelOvrr` | Merges the raw ERP item master with the local override table. Used by `PPsp_PostProcess_DownLoadItemMaster_*` as its source when syncing item data to IPCs. Override fields for `NetWeight`, `ItemDesc1`, `PackSize`, `SCCCode`, and all label formats take precedence when flags are set. |
| **`vwPallet`** | `tblPallet` | Thin compatibility layer over `tblPallet`. Adds a `NULL AS DestinationShopOrder` column to ensure that SQL queries using the normalized Pallet schema work against both current (`tblPallet`) and historical (`tblPalletHst`) data in a UNION without schema mismatch. |
| **`vwLatestFGColourSpec`** | `tblFGColourSpec` (self-join) | A **Probat integration view**. Returns only the most recent effective colour specification per Blend+Grind combination using a `MAX(EffectiveDate)` subquery. Consumed by Probat-side SPs to verify roasted coffee colour tolerance in real time. |
| **`vwQATWorkFlow`** | `tblQATWorkFlow` + `tblQATForm` | Flattened QAT workflow configuration view consumed by `SharedFunctions.GetQATWorkFlowInfo()` (Ch3) to build the dynamic button grid at runtime. |
| **`vwLineWorkShiftType`** | `tblComputerConfig` | Maps each `PackagingLine` to its `WorkShiftType` (Packaging vs. Utility), used in TVFs and SPs to correctly apply shift boundary logic per line type. |

#### 7.6.3 Table-Valued Functions (TVFs)

**`tfnPalletHstDetail(facility, machineID, operator, shopOrder, itemNumber, fromDate, fromShift, toDate, toShift)`**

The **core data engine for all pallet-based SSRS reports** (Ch10). Returns a unified pallet record set by performing a `UNION` of `tblPallet` (current open orders) and `tblPalletHst` (historical closed orders), filtered by any combination of the 9 parameters. Shift-aware: if both `@intFromShift` and `@intToShift` are provided and don't span a full day, it joins `tblShift` and `vwLineWorkShiftType` to filter by `ShiftSequence` boundaries rather than date only, enabling precise half-day reporting windows.

**`tfnSessionControlHstDetail` / `tfnSessionControlHstSummary`**

Equivalent TVFs operating over `tblSessionControlLog` and `tblSessionControlLogHst` to support efficiency and operator session reports.

**`tfnStdMachineEfficiencyRate`**

Returns the applicable standard efficiency rate for a machine at a given point in time, joining `tblStdMachineEfficiencyRate` with date-range logic. Used by `PPsp_LineEfficiency` (Ch7.2.1) to obtain the denominator `ScheduledOutput` for the efficiency ratio.

#### 7.6.4 Scalar Functions

| Function | Signature | Purpose |
| --- | --- | --- |
| **`fnGetExpectedShift`** | `(facility, datetime, workgroup) → int` | Given a wall-clock time, looks up `tblShift` to determine which shift number is currently active. Handles midnight-crossing shifts (where `FromTime > ToTime`). Since WO#3695, also calls `fnGetShiftPatternCode` to support 4-shift rotation patterns. |
| **`fnGetShiftPatternCode`** | `(facility, datetime, workgroup) → int` | Returns the shift pattern code for a given date (e.g., pattern A vs B in rotating 4-shift schedules). |
| **`fnGetProdDateByShift`** | `(facility, datetime, workgroup) → date` | Returns the production date that a given timestamp belongs to — which may differ from the calendar date for pre-midnight overnight shifts. Used throughout all SP date calculations. |
| **`fnShiftInfo`** | `(facility, datetime, workgroup) → table` | Returns the complete shift record (shift number, from/to times, description) for a given moment. |
| **`fnConvertDate`** | `(datestring) → datetime` | Converts the system's `char(8)` date format (`YYYYMMDD`) to `datetime`. Used pervasively throughout legacy SPs. |
| **`fnFillLeadingZeros`** | `(value, length) → varchar` | Left-pads a numeric string to a fixed length. Used in pallet ID and shop order number formatting for CoLOS CLI commands. |

### Refactoring Scorecard: Chapter 7 (Database Logic & SPs)

| Factor | Score (1-10) | Description | Actionable Recommendation |
|---|---|---|---|
| **Fragility** | **9** | Critical business logic exists solely in undocumented SPs and raw SQL Assembly. | Extract logic to Application Layer. Deprecate `ExportDataToIPCDB` Assembly. |
| **Security** | **10** | SQL CLR Assembly uses `UNSAFE` context. | **Critical Priority:** Remove unsafe CLR. Move bulk push logic to a managed external service. |
| **Complexity** | **8** | `PPsp_WorkCenterEfficiencySummary` is 500+ lines. | Break down complex TVFs and SPs into modular micro-queries. |

---

## Chapter 8 — QA Web Portal: Configuration & Administration Backend

*Source: `Power Plant Codes\Visual Studio 2012\QA Web Sites\` (10 ASP.NET WebForms projects)*

The QA Web Portal is the **administrative backbone** of the MES quality assurance system. While the WinForms client (`frmQATWorkflow`, `frmQATStartUpChecks`) consumes rules at runtime, this portal is where those rules are created and maintained. It consists of 10 independent ASP.NET WebForms websites, each responsible for a distinct configuration domain.

> **Key principle**: Every configuration record saved through this portal writes directly to the **Central Server DB** (`MPHOPP02\PowerPlantAXSP_Dev`). The changes propagate to IPC workstations via the `ExportSQLDataToIPC` sync engine analyzed in §7.5.

### 8.1 Technology Stack & Data Access Pattern

**Framework**:  ASP.NET WebForms, VB.NET, targeting .NET 3.5 (compiled with `v3.5`). All 10 sites share an identical pattern.

**Authentication**: `<authentication mode="Windows" />` — Windows Integrated Authentication (NTLM/Kerberos). There is **no custom login form**. The IIS application pool identity and the browser's Windows credentials are used directly. The currently logged-in user's domain account is surfaced in every page via:

```vb
' SQLCentral.GetCurrentUserName() — used in every page for audit trail
currUser = HttpContext.Current.User.Identity.Name.ToString   ' e.g. "DOMAIN\jsmith"
endIndex = currUser.LastIndexOf("\")
currUserName = currUser.Substring(endIndex + 1, ...)         ' strips domain → "jsmith"
```

This username is stamped into `UpdatedBy` on every INSERT/UPDATE, providing a complete audit trail.

**Connection String** (identical across all 10 sites in `Web.config`):

```xml
<add name="PowerPlantCnnStr"
     connectionString="Data Source=MPHOPP02;Initial Catalog=PowerPlantAXSP_Dev;
                       Persist Security Info=True;User ID=ppuser;Password=ppuser#1"
     providerName="System.Data.SqlClient"/>
```

> **Security note**: The `ppuser` SQL login with hardcoded password `ppuser#1` is embedded in all 10 `Web.config` files. This is a persistent security risk — the web applications do not use Windows Integrated Security for the DB connection, so the SQL login is the bottleneck.

**Data Access Class: `SQLCentral`** (shared across all 10 projects, identical copy in each):

```
SQLCentral                           (105 lines — identical in all 10 sites)
├── DBCon: SqlConnection             ← Single connection per request, opened/closed inline
├── DBCmd: SqlCommand                ← Ad-hoc SQL and stored procedure calls
├── DBDT: DataTable                  ← Results always returned as DataTable (not DataSet)
├── Params: List(Of SqlParameter)    ← AddParam() accumulates; ExecQuery() flushes
├── ExecQuery(Query, ReturnIdentity) ← The unified read/write entry point
└── HasExceptionMsg()                ← Error surface — no exception is re-thrown
```

**Critical design facts**:

- **No ORM** (no Entity Framework, no LINQ-to-SQL anywhere in this portal).
- **All SQL is inline, concatenated strings** — no SPs are called for CRUD. The only SP calls are for reference data lookups (e.g., `PPsp_Facility_Sel`).
- `ExecQuery` uses a `SqlDataAdapter.Fill(DBDT)` pattern for **both reads AND writes**. An INSERT statement, when passed to `ExecQuery`, executes via the adapter's fill mechanism — no `ExecuteNonQuery` is used explicitly.
- **SQL injection risk**: The search and filter queries in pages like `QAT Workflow Maintenance` concatenate user input directly into SQL strings (e.g., `" Where A.PackagingLine = '" & strSearch & "'"`) with no parameterization.

### 8.2 The Portal Ecosystem: 10 Sites and Their Database Domains

| Website | Target Table(s) | Purpose |
|---|---|---|
| **QAT Workflow Maintenance** | `tblQATWorkFlow`, `tblQATDefinition`, `tblQATEntryPoint` | Assigns QAT check sets to packaging lines — the **primary runtime control table** |
| **QAT Definition Maintenance** | `tblQATDefinition` | Defines the named QAT check "groups" (Entry Point: S/I/C/O) |
| **QAT Form Maintenance** | `tblQATForm` | Maps `InterfaceFormID` → `FormName` → `TableName` for reflection-based form loading |
| **QAT Task Master Maintenance** | `tblQATTaskMaster` | Defines reusable task descriptions (the "vocabulary" of checks) |
| **QAT Task Maintenance** | `tblQATTask` | Binds tasks to QAT definitions with sequence ordering |
| **QAT Specification Maintenance** | `tblQATSpec`, `tblQATSpecFormula` | Defines measurement limits (`LwLmtFromTarget`, `UpLmtFromTarget`) + formula |
| **QAT Spec Formula Maintenance** | `tblQATSpecFormula` | Defines calculation formulas referenced by spec records |
| **QAT Note Maintenance** | `tblQATNote` | Defines instructions/guidance text attached to tasks |
| **QAT TCP Connection Maintenance** | `tblQATTCPConn` | Configures checkweigher IP address, port, model, and command strings |
| **QAT Serial Connection Maintenance** | `tblQATSerialConn` | Configures RS-232 serial measurement devices |

### 8.3 QAT Workflow Maintenance — The Client-Behaviour Driver

This is the most architecturally significant portal page. Every record it saves **directly determines which QAT forms appear on the WinForms client and when**.

#### 8.3.1 Table Structure: `tblQATWorkFlow`

The main query in `QAT Workflow Maintenance\Default.aspx.vb` (L204-217) reveals the full schema JOIN:

```sql
SELECT A.Active, A.Facility, A.PackagingLine, A.QATDefnID, A.TestSeq,
       A.SerialConnID, A.TCPConnID, A.UpdatedAt, A.UpdatedBy, A.ExceptionCode,
       B.QATDefnDescription, B.QATEntryPoint,
       tEP.EntryPointDescription,          -- 'S'=Startup, 'I'=InProcess, 'C'=Closing, 'O'=OnRequest
       C.Description AS TCPConnDesc,
       D.SerialConnDesc,
       E.PackagingLineDesc
FROM tblQATWorkFlow A
LEFT JOIN tblQATDefinition B ON A.QATDefnID = B.QATDefnID
LEFT JOIN tblQATEntryPoint tEP ON B.QATEntryPoint = tEP.QATEntryPoint
LEFT JOIN tblQATTCPConn C ON A.TCPConnID = C.TCPConnID
LEFT JOIN tblQATSerialConn D ON A.SerialConnID = D.SerialConnID
LEFT JOIN (tblEquipment WHERE Type='P') E ON A.PackagingLine = E.EquipmentID
```

**Key `tblQATWorkFlow` columns and their client-side effects**:

| Column | Type | Client-Side Effect |
|---|---|---|
| `PackagingLine` | `char(10)` | Filters which IPC line sees this workflow. Matches `gdrSessCtl.DefaultPkgLine`. |
| `QATDefnID` | `int` | FK → `tblQATDefinition`. Identifies which check set to load. |
| `TestSeq` | `int` (1–100) | **Determines the display order** of check forms on `frmQATWorkflow`. The WinForms client sorts by `TestSeq` when building the button grid. |
| `B.QATEntryPoint` | `char(1)` | **Determines when the check fires**: `S`=line startup, `I`=in-process (mid-pallet), `C`=order closing, `O`=on request. This is the state machine trigger. |
| `TCPConnID` | `int` | Links to checkweigher TCP config — activates the `PPTCPServer` hardware pipeline for this workflow step. |
| `SerialConnID` | `int` | Links to serial device config for weight/measurement data capture. |
| `ExceptionCode` | `varchar` | Maps to `tblQATExceptionCode` — allows a supervisor to mark a failed check with a pre-defined exception reason. |

**Copy Workflow**: The page includes a "Copy from PackagingLine → to PackagingLine" bulk-copy feature (L88-101, `ddlPackagingLineFrom`/`ddlPackagingLineTo`). This duplicates all `tblQATWorkFlow` records for one line to another, allowing QA teams to replicate a line's entire check configuration without manual re-entry.

#### 8.3.2 Configuration → Client Behaviour Chain

```
[QAT Workflow Maintenance Portal]
  Admin selects: PackagingLine = "3200", TestSeq = 1,
                 QATDefnID = 5 (EntryPoint = 'S'), TCPConnID = 2
                         │
                         ▼  (INSERT into tblQATWorkFlow on Server DB)
                         │
           [ExportSQLDataToIPC sync engine]
              tblQATWorkFlow → IPC Staging DB → Local DB
                         │
                         ▼
  [frmQATWorkflow.vb on IPC at Startup]
    Reads: SELECT * FROM tblQATWorkFlow WHERE PackagingLine = gdrSessCtl.DefaultPkgLine
           JOIN tblQATDefinition WHERE QATEntryPoint = 'S'
           ORDER BY TestSeq
    →  Dynamically creates Button objects for each result row
       Button position/label driven by TestSeq order
    →  Clicking button #1 (TestSeq=1) → Reflection: Activates(QATDefnID=5's form class)
    →  If TCPConnID assigned → CallTcpServer("@ 2 0") → PPTCPServer starts weight logging
```

### 8.4 QAT Form Maintenance — The Reflection Registry

`tblQATForm` is the **registry that maps string identifiers to actual WinForms form classes**. This is the configuration store for the reflection-based form activation system (discussed in Ch3).

Schema exposed by `QAT Form Maintenance\Default.aspx.vb` (L40-41):

```sql
SELECT InterfaceFormID, TestCategory, FormName, TableName, Active, Facility,
       UpdatedAt, UpdatedBy, TestFormID
FROM tblQATForm
ORDER BY InterfaceFormID
```

| Column | Role |
|---|---|
| `InterfaceFormID` | String key used at runtime to look up the form (e.g., `"WeightCheck"`) |
| `FormName` | **The exact VB.NET class name** of the WinForms form (e.g., `"frmWeightCheck"`) — passed to `Type.GetType()` for reflection |
| `TableName` | The backing DB table for the form's data, used by the form's internal queries |
| `TestCategory` | Grouping category for display/reporting purposes |

**Configuration → Behaviour link**: When a QAT workflow button is clicked, the client uses `InterfaceFormID` to look up `FormName`, then instantiates it via `Activator.CreateInstance(Type.GetType(FormName))`. **Adding a new QAT check type** requires: (a) creating a new WinForms form class in the MES codebase, (b) registering it here in the portal, and (c) creating the corresponding workflow entry in QAT Workflow Maintenance.

### 8.5 QAT Task Master & Task Maintenance — The Check "Vocabulary"

These two portals form a **two-level hierarchy**:

**`tblQATTaskMaster`** (Task Master Maintenance) — The vocabulary of all possible check tasks:

```sql
-- INSERT pattern from Task Master Maintenance
INSERT INTO tblQATTaskMaster (TaskDescription, Active, Facility, UpdatedAt, UpdatedBy)
VALUES (@taskdescription, @active, @facility, @updatedat, @updatedby)
```

`TaskDescription` is the human-readable instruction shown to the operator (e.g., "Verify package seal integrity", "Check net weight").

**`tblQATTask`** (Task Maintenance) — Binds tasks to QAT definitions with sequence:

```sql
-- INSERT pattern from Task Maintenance
INSERT INTO tblQATTask (Active, Facility, TaskID, TaskSeq, QATDefnID, NoteID, UpdatedAt, UpdatedBy)
VALUES (@active, @facility, @taskid, @taskseq, @qatdefnid, @noteid, @updatedat, @updatedby)
```

The join query (L139-145):

```sql
SELECT A.Active, A.Facility, A.QATDefnID, A.TaskID, A.TaskSeq,
       B.TaskDescription, C.NoteDescription, D.QATDefnDescription
FROM tblQATTask A
LEFT JOIN tblQATTaskMaster B ON A.TaskID = B.TaskID
LEFT JOIN tblQATNote C ON A.NoteID = C.NoteID
LEFT JOIN tblQATDefinition D ON A.QATDefnID = D.QATDefnID
ORDER BY A.TaskID
```

**`TaskSeq`** (1–100, populated as a generated list in the portal): Controls the **display order** of individual check items within a QAT form at runtime. When `frmQATStartUpChecks` or a similar form renders, it reads tasks sorted by `TaskSeq`, building checkboxes/input fields in that order.

**`NoteID`**: Links to `tblQATNote`, which holds the operator instruction text displayed alongside the task. Notes are maintained separately so the same instruction can be reused across multiple tasks.

### 8.6 QAT Specification Maintenance — The Validation Rule Engine

`tblQATSpec` defines the **measurement tolerance and validation formula** for quantitative checks. This is the table that directly controls whether a measurement is accepted or rejected at the WinForms client.

Schema from `Specification Maintenance\Default.aspx.vb` (L50-52):

```sql
SELECT A.Active, A.Facility, A.Formula, A.LwLmtFromTarget, A.TestSpecDesc,
       A.TestSpecID, A.UpdatedAt, A.UpdatedBy, A.UpLmtFromTarget,
       B.Description AS FormulaDesc
FROM tblQATSpec A
INNER JOIN tblQATSpecFormula B ON A.Formula = B.FormulaID
```

| Column | Type | Meaning |
|---|---|---|
| `TestSpecDesc` | `varchar` | Human-readable description (e.g., "Net Weight 500g ±5%") |
| `Formula` | `int` → FK `tblQATSpecFormula` | The calculation formula to apply (e.g., "Actual vs Target % deviation") |
| `LwLmtFromTarget` | `decimal` | Lower limit offset from the target value |
| `UpLmtFromTarget` | `decimal` | Upper limit offset from the target value |

**Validation pipeline — how Web config reaches the WinForms alert**:

```
[Spec Maintenance Portal]
  Admin sets: TestSpecDesc = "Net Weight 500g"
              Formula = 3 (% deviation from target)
              LwLmtFromTarget = -5.0  (max -5% below target)
              UpLmtFromTarget = +5.0  (max +5% above target)
                     │
                     ▼  (INSERT tblQATSpec on Server DB)
                     │
          [Sync → IPC Local DB via ExportSQLDataToIPC]
                     │
                     ▼
  [frmQATStartUpChecks or inline weight check form on IPC]
    Reads tblQATSpec for associated TestSpecID
    Receives weight reading from PPTCPServer pipeline (§5.7)
    Evaluates: deviation = (ActualWeight - TargetWeight) / TargetWeight * 100
    If deviation < LwLmtFromTarget OR deviation > UpLmtFromTarget:
        → Display red alert: "FAIL — out of spec"
        → Require supervisor override (gdrSessCtl role check)
    Else:
        → Display green: "PASS"
        → Advances workflow to next TestSeq
```

The `CheckLimitValue()` function in the portal (L245-251) validates that both limit fields are parseable decimals before saving — this is the only client-side validation.

### 8.7 TCP & Serial Connection Maintenance — Hardware Registration

These two portals write directly to `tblQATTCPConn` and `tblQATSerialConn`, which are the hardware config tables consumed by `PPTCPServer` (§5.7.4).

**`tblQATTCPConn`** columns surfaced via dropdowns in QAT Workflow Maintenance:

- `IPAddress`: The checkweigher station's network IP
- `Port`: TCP port (typically the Mettler-Toledo WD protocol default)
- `Model`: `"XC"` or `"XE"` — determines the handshake and parsing logic in `PPTCPServer.LoadDataFromCheckWeigher`
- `Command3`: The WD format command sent on connect (e.g., `"WD_SET_FORMAT 4"`)
- `Description`: Human-readable label shown in the portal's TCP dropdown

**Connection**: `tblQATWorkFlow.TCPConnID` → `tblQATTCPConn.TCPConnID` → `PPTCPServer.CheckWeigherManager` reads this by `gintTCPConnID` to select the correct hardware config.

### 8.8 Complete Configuration Hierarchy

```
tblQATWorkFlow  (PackagingLine + EntryPoint + TestSeq)
       │
       └─ QATDefnID → tblQATDefinition (EntryPoint: S/I/C/O)
                              │
                              └─ QATDefnID → tblQATTask (TaskSeq order)
                                                   │
                                                   ├─ TaskID → tblQATTaskMaster (description)
                                                   └─ NoteID → tblQATNote (instructions)
       │
       ├─ TCPConnID → tblQATTCPConn (IP, Port, Model, Command3)
       ├─ SerialConnID → tblQATSerialConn (COM port, baud rate)
       └─ ExceptionCode → tblQATExceptionCode (supervisor override reasons)

tblQATSpec (TestSpecID + Formula + LwLmtFromTarget + UpLmtFromTarget)
       └─ Formula → tblQATSpecFormula (calculation method)

tblQATForm (InterfaceFormID → FormName → [VB.NET class via Reflection])
```

### Refactoring Scorecard: Chapter 8 (QA Web Portal)

| Factor | Score (1-10) | Description | Actionable Recommendation |
|---|---|---|---|
| **Fragility** | **7** | Missing referential integrity in config updates. | Add strict backend validation before writing to configuration tables. |
| **Security** | **9** | Unmarshaled inline SQL construction (`ExecQuery`). | Convert all access to ORM/parameterized queries to fix SQL Injection risks. |
| **Complexity** | **5** | Replicated ASP.NET WebForms boilerplate. | Migrate whole module to a modern SPA framework (Blazor WebAssembly/Auto). |

---

## Chapter 9 — DownTime Management System

*Source: `Power Plant Codes\Visual Studio 2012\Projects\DownTime\DownTime\`*
*Key files: `modDownTime.vb` (147 lines), `frmDownTime.vb` (46KB), `SharedFunctions.vb` (DownTime copy)*

`DownTime.exe` is the **out-of-process downtime recording application**. Rather than embedding downtime capture directly in the main WinForms client, the MES launches it as a separate process. This keeps the main production screen uninterrupted while an operator or technician logs a machine stoppage event.

### 9.1 Process Launch: `frmMainMenu` → `StartDownTimePgm` → `DownTime.exe`

The entry point is the **"Down Time" button** on `frmMainMenu` (L171). Its click handler calls into `SharedFunctions.StartDownTimePgm`:

```vb
' frmMainMenu.vb L178
SharedFunctions.StartDownTimePgm(
    gdrSessCtl("DefaultPkgLine"),    ' arg[1]: PackagingLine (e.g. "3200")
    lblShopOrder.Text.ToString,      ' arg[2]: ShopOrder number
    gdrSessCtl.Operator,             ' arg[3]: Operator badge/login ID
    gdrSessCtl("OverrideShiftNo"),   ' arg[4]: Current shift number (1-3)
    gdrSessCtl("Facility"),          ' arg[5]: Facility code (e.g. "09")
    gdrCmpCfg("PkgLineType"))        ' arg[6]: Machine sub-type (e.g. "Auto", "Semi")
```

`StartDownTimePgm` (`SharedFunctions.vb` L5037) constructs the command line:

```vb
' SharedFunctions.vb L5043
sb.AppendFormat("{0} {1} {2} {3} {4} {5} {6}",
    "P",          ' arg[0]: MachineType — hardcoded "P" (Packaging)
    strPkgLine,   ' arg[1]: PackagingLine
    intShopOrder, ' arg[2]: ShopOrder
    strOperator,  ' arg[3]: Operator
    intShift,     ' arg[4]: Shift
    strFacility,  ' arg[5]: Facility
    strPkgLineType)  ' arg[6]: PkgLineType

If gdrSessCtl.ShopOrder <> 0 Then
    sb.Append(" ")
    sb.Append(gdrSessCtl.StartTime.ToString("yyyy-MM-ddHH:mm:ss.fff"))  ' arg[7]: SessionStartTime
End If

Process.Start(My.Settings.strDownTime, strCmdArgs)
' My.Settings.strDownTime = path to DownTime.exe (e.g. "C:\Powerplant\DownTime\DownTime.exe")
```

**The 8-parameter command-line contract:**

| Arg Index | Value Source | Used As |
|---|---|---|
| `args[0]` | `"P"` (hardcoded) | `gstrMachineType` — machine category |
| `args[1]` | `gdrSessCtl.DefaultPkgLine` | `gstrMachineID` — packaging line ID |
| `args[2]` | Current ShopOrder | `gintShopOrder` |
| `args[3]` | `gdrSessCtl.Operator` | `gstrOperator` — pre-fills operator field |
| `args[4]` | `gdrSessCtl.OverrideShiftNo` | `gintShift` — pre-fills shift field |
| `args[5]` | `gdrSessCtl.Facility` | `gstrFacility` |
| `args[6]` | `gdrCmpCfg.PkgLineType` | `gstrMachineSubType` — filters reason codes |
| `args[7]` *(optional)* | `gdrSessCtl.StartTime` (if SO active) | `gstrEventID` — links DT record to production session |

> **Note on `gstrEventID`**: For `MachineType = "P"` (Packaging), the EventID (the session start timestamp string) is formatted with a space inserted at position 10 (L84-86 in `modDownTime.vb`). This is because the `"yyyy-MM-ddHH:mm:ss.fff"` format omits the date-time separator — the space is injected to form `"yyyy-MM-dd HH:mm:ss.fff"` for SQL compatibility.

### 9.2 `modDownTime.Main()` — Startup Guard & Initialization

`modDownTime.Main()` (L29-145) runs before `frmDownTime` is shown and performs:

**Step 1 — Single-instance enforcement** (L41-58, 4-retry loop):

```vb
For i = 1 To 4
    pInstance = SharedFunctions.GetRunningInstance("DownTime")
    If Not pInstance Is Nothing Then
        Thread.Sleep(1000)   ' Wait 1s between retries
    Else
        Exit For             ' Another DownTime already running → bring to foreground
    End If
Next
If Not pInstance Is Nothing Then
    Win32Helper.SetForegroundWindow(handle)
    Win32Helper.ShowWindow(handle, ProcessWindowStyle.Maximized)
    Application.ExitThread()   ' Exit — don't launch a second instance
End If
```

This is the same WakeUp-or-Launch pattern used by `PPTCPServer`. If DownTime is already running (e.g., operator clicked the button twice), the second launch brings the existing window to focus and exits.

**Step 2 — Argument count validation** (L71-74):

```vb
If My.Application.CommandLineArgs.Count < 7 Then
    MessageBox.Show("Require 7 parameters and the 8th one is optional.")
    Return 1
End If
```

**Step 3 — Connection string resolution** (L88-111):

```
gstrLocalCnnStr = My.Settings.gstrLocalCnnStr
                  (template replaces "ComputerName" with My.Computer.Name)
gstrServerCnnStr = My.Settings.gstrServerCnnStr

Adapters for Reason Types, Reason Codes, Plant Staff → Local DB
If IsSvrConnOK():
    gtaDownTimeLog adapter → Server DB   (gblnConnectUp = True)
Else:
    gtaDownTimeLog adapter → Local DB    (gblnConnectUp = False)
```

**Connection strings (from `app.config`)**:

- **Server DB**: `MPSPPP01\PowerPlant_Prd` (Integrated Security, 5s timeout)
- **Local DB**: `{ComputerName}\SQLEXPRESS\LocalPowerPlant` (`ppuser`/`ppuser#1`)

The `"ComputerName"` placeholder in the local connection string is dynamically replaced at startup with `My.Computer.Name` — the actual IPC machine name. This allows a single `app.config` to work on any IPC.

**Step 4 — Upload pending offline records** (L122-124):

```vb
If gblnConnectUp = True And Not String.IsNullOrEmpty(gstrLocalCnnStr) Then
    SharedFunctions.uploadDTLogToServer()
End If
```

If the server is online AND a local DB exists, `uploadDTLogToServer()` runs before the form loads. This pushes any DT log entries that were created while the server was offline (written to Local DB during a network outage) up to the Server DB.

**Step 5 — Signal session control** (via `frmDownTime_Load`):

```vb
' Called during form Load, not modDownTime.Main()
SharedFunctions.UpdateSessionControlDownTime(True)
' → CPPsp_SessionControlIO @chrAction = "SetStartDownTime_On"
' → Sets tblSessionControl.StartDownTime = GETDATE() on Local DB
```

This is the **flag that `PPTCPServer`'s CheckWeigherManager reads** (`gdrSessCtl.IsStartDownTimeNull` check in §5.7.4) — when `StartDownTime` is not NULL, the system knows the line is in a downtime event and the checkweigher idle timeout is suppressed.

### 9.3 `frmDownTime` — The Downtime Capture State Machine

The UI presents a structured data entry form with pre-filled fields from the command-line arguments. **The reason code selection is a two-level hierarchy** (reason type → reason code), which directly determines the efficiency impact.

#### 9.3.1 Initial State (Form Load)

On `frmDownTime_Load`:

- `txtBeginDate` → `Format(Now, "yyyyMMdd")` (editable)
- `txtBeginTime` → `Format(Now, "HHmm")` (editable, 24h 4-digit format)
- `txtOperator` → pre-filled from `gstrOperator` (arg[3])
- `txtShift` → pre-filled from `gintShift` (arg[4])
- `lblShopOrderNo` → `gintShopOrder` (visible only if ShopOrder > 0)
- `cboMachine` → loaded from `GetSharedEquiptmentList(Facility, MachineID, MachineType, MachineSubType, "ActiveSharedGroup")` — allows operators to log DT against a shared equipment group (e.g., a conveyor shared by multiple lines)

**Reason Type dropdown** (L75-79 in `frmDownTime_Load`):

```vb
gtaDTReasonType.Fill(gtblDTReasonType, gstrFacility, gstrMachineType,
                     gstrMachineSubType, blnShopOrderStarted)
```

`blnShopOrderStarted = (gintShopOrder > 0)` — this boolean **filters the reason codes** exposed. Different reason types may be available depending on whether a production order is active (WO#31294).

#### 9.3.2 Two-Level Reason Code Hierarchy

`tblDTReasonType` → `tblDTReasonCode` form a cascading two-level tree:

```
Level 1: ReasonType (cboReasonType)
├── 0 = "All Reasons" (no filter, full list sorted by code)
├── 1 = "Equipment" 
│   ├── Code 101 = "Conveyor Jam"
│   ├── Code 102 = "Scale Failure"
│   └── Code 103 = "Printer Malfunction"
├── 2 = "Process"
│   ├── Code 201 = "Product Changeover"
│   └── Code 202 = "Time Study"
└── 3 = "Planned" (only available when ShopOrder IS NOT started)
    └── Code 301 = "Scheduled Break"
```

When the operator selects a `ReasonType` from `cboReasonType`, the `cboReasonCode_DropDown` event fires and refills `cboReasonCode`:

```vb
' cboReasonCode_DropDown
If cboReasonType.SelectedValue = 0 Then
    ' All reasons — full unfiltered list sorted by code
    gtaDTReasonCode.Fill(gtblDTReasonCode, gstrMachineType, gstrFacility,
                         gstrMachineSubType, blnShopOrderStarted)
Else
    ' Filtered by selected ReasonType
    gtaDTReasonCode.FillByDescription(gtblDTReasonCode, gstrMachineType,
                                       cboReasonType.SelectedValue, gstrFacility,
                                       gstrMachineSubType, blnShopOrderStarted)
End If
```

**`ReasonType = 0` (Free-text / "Other")**: When the ReasonType selected value equals 0, the `cboReasonCode` ComboBox behaves like a **text input** (`popupAlphaNumKB` fires on mouse-down) allowing a free-text reason entry when no structured code applies. This is why the `cboReasonCode` can either be a dropdown selection OR a text field.

#### 9.3.3 Duration Calculation & "End Now" Feature

Two modes for specifying duration:

1. **Manual** (`txtDuration`): Operator types duration in minutes directly. Validated: must be integer > 0.
2. **"End Now" button** (`btnEndNow_Click`): Computes duration from "begin" fields to current time:

   ```vb
   txtDuration.Text = DateDiff(DateInterval.Minute,
       CType(txtBeginDate.Insert(4,"/").Insert(7,"/") & " " &
             txtBeginTime.Insert(2,":") & ":00", DateTime),
       Now)
   ```

Date format: `yyyyMMdd` (8 chars) → formatted with `/` separators for `CType` parse. Time format `HHmm` (4 chars) → formatted with `:` separator.

#### 9.3.4 Comment Capture (WO#3282)

If the selected reason code requires a comment (`gblnCommentRequired = True`), the form shows `RTBComment` (RichTextBox) and `lblComment`. The comment is captured via `frmComment` (popup) or directly in the RTB. On save, the comment is stored in `tblDownTimeLog.Comment`.

### 9.4 `tblDownTimeLog` — The Persistence Layer

On `btnAccept_Click`, the validated record is written via the `gtaDownTimeLog` TableAdapter:

```vb
' SP called: CPPsp_DownTimeLog_Ins (name inferred from TA generated code)
gtaDownTimeLog.Insert(
    False,                          ' blnIsUploadedToSvr (False = new local record)
    gstrFacility,                   ' Facility
    CType(lblShopOrderNo.Text, Integer), ' ShopOrder
    gstrMachineType,                ' MachineType ("P")
    gstrMachineSubType,             ' MachineSubType (PkgLineType)
    cboMachine.SelectedValue,       ' MachineID (the selected equipment)
    txtOperator.Text,               ' Operator
    txtTechnician.Text,             ' Technician
    dteDTBegin,                     ' BeginDateTime (parsed from txtBeginDate + txtBeginTime)
    dteDTEnd,                       ' EndDateTime = BeginDateTime + txtDuration minutes
    intShift,                       ' Shift
    strReasonType,                  ' ReasonType code
    intReasonCode,                  ' ReasonCode
    strComment,                     ' Comment text
    txtOperator.Text,               ' CreatedBy
    Now,                            ' CreatedAt
    txtOperator.Text,               ' UpdatedBy
    Now,                            ' UpdatedAt
    dteShiftProductionDate,         ' ShiftProductionDate (from CPPsp_GetProdDateByShift)
    gstrEventID)                    ' EventID (SessionControl StartTime, links to production session)
```

**Dual-DB write with failover** (identical pattern to `PPTCPServer`):

```
Try → Server DB Insert
Catch SqlException (network error codes 64/1231/11001/10054):
    → Failover to Local DB Insert
    → gblnConnectUp = False
    → Show "No Server connection" warning banner
```

**`IsUploadedToSvr` flag**: Records written locally during a server outage have `IsUploadedToSvr = False`. The `uploadDTLogToServer()` function (called at next startup) queries

```sql
SELECT * FROM tblDownTimeLog WHERE IsUploadedToSvr = 0
```

and re-inserts each row into the Server DB, then marks `IsUploadedToSvr = True` to prevent double-upload.

### 9.5 Session Control Integration: The DownTime Signal

The `StartDownTime` column in `tblSessionControl` (Local DB) acts as a **live status flag** shared between `DownTime.exe` and `PPTCPServer.exe`:

```
DownTime.exe OPEN    → CPPsp_SessionControlIO "SetStartDownTime_On"
                        → tblSessionControl.StartDownTime = GETDATE()
                        
PPTCPServer polling  → Reads gdrSessCtl.IsStartDownTimeNull()
                        If False (StartDownTime IS NOT NULL):
                          Idle timeout counter is NOT incremented
                          (machine stopped, no weight data expected)

DownTime.exe CLOSED  → CPPsp_SessionControlIO "SetStartDownTime_Off"
  (FormClosing event)   → tblSessionControl.StartDownTime = NULL
                          
PPTCPServer resumes  → IsStartDownTimeNull() = True → idle counter restarts
```

This is the inter-process communication mechanism between the downtime tracker and the hardware proxy — entirely database-mediated through the Local `tblSessionControl` table.

### 9.6 Relationship to Line Efficiency (Ch7.2.1 — `PPsp_LineEfficiency`)

Every DT record saved to `tblDownTimeLog` directly affects the efficiency metrics computed by `PPsp_LineEfficiency`:

```sql
-- From PPsp_LineEfficiency (simplified)
SELECT SUM(Duration) AS TotalDowntimeMinutes
FROM tblDownTimeLog
WHERE PackagingLine = @PackagingLine
  AND ShopOrder = @ShopOrder
  AND ShiftProductionDate = @ProductionDate

-- Net Available Time = Shift Duration - TotalDowntimeMinutes
-- Efficiency % = (CasesProduced / CasesScheduled) * 100
--             adjusted for downtime when calculating scheduled output
```

**`ShiftProductionDate`** (computed by `CPPsp_GetProdDateByShift` at form load): Ensures that midnight-spanning shifts attribute the downtime record to the correct production date, consistent with the shift logic in Ch6.1.

**`MachineType`/`MachineSubType` filtering**: The efficiency SP can filter by machine type, meaning downtime logged against a specific conveyor (`MachineSubType`) can be reported separately from line-wide (`MachineType = "P"`) downtime.

### 9.7 Complete DownTime Data Flow

```
[Operator presses "Down Time" on frmMainMenu]
         │
         ▼
SharedFunctions.StartDownTimePgm(PackagingLine, SO, Operator, Shift, Facility, SubType)
    → Process.Start("DownTime.exe", "P {Line} {SO} {Op} {Shift} {Fac} {SubType} {EventID}")
         │
         ▼
modDownTime.Main()
    ├─ Single-instance guard → bring existing to foreground if running
    ├─ Parse 7-8 CLI args → globals
    ├─ Resolve connection strings (ComputerName substitution)
    ├─ Set gtaDownTimeLog → Server DB (if online) or Local DB (offline)
    └─ uploadDTLogToServer() → flush pending offline records to server
         │
         ▼
frmDownTime.Load()
    ├─ Pre-fill Operator, Shift, ShopOrder, BeginDate/Time from globals
    ├─ Load Equipment list (shared group support)
    ├─ Load cboReasonType (filtered by MachineType + blnShopOrderStarted)
    └─ UpdateSessionControlDownTime(True) → CPPsp_SessionControlIO "SetStartDownTime_On"
             └─ PPTCPServer reads flag → suppresses idle timeout
         │
         ▼ [Operator selects ReasonType → cboReasonCode populates]
         │ [Operator enters Duration or clicks "End Now"]
         │ [Optional: enters Comment if required]
         │
btnAccept_Click()
    ├─ Validate all fields
    ├─ Compute EndDateTime = BeginDateTime + Duration
    ├─ GetProductionDateByShift() → dteShiftProductionDate
    ├─ gtaDownTimeLog.Insert() → Server DB (primary)
    │   └─ On network failure → Local DB (IsUploadedToSvr = False)
    └─ RefreshDTLog() → repopulate dgvDTLog grid with today's records
         │
frmDownTime.FormClosing()
    └─ UpdateSessionControlDownTime(False) → "SetStartDownTime_Off"
             └─ tblSessionControl.StartDownTime = NULL
             └─ PPTCPServer resumes normal idle timeout counting
```

### Refactoring Scorecard: Chapter 9 (Downtime Management)

| Factor | Score (1-10) | Description | Actionable Recommendation |
|---|---|---|---|
| **Fragility** | **7** | Multi-process synchronization relies on Window messaging (`WakeUpPgm`). | Integrate Downtime directly into main Client UI using a composable UI architecture. |
| **Security** | **4** | Local caching of plain text logs. | Secure temp storage. |
| **Complexity** | **6** | Failover dual-DB writes. | Standardize offline-first sync engine logic across the data access layer. |

---

## Chapter 10 — SSRS Production Reporting (Data Source & Analytics Reference)

*Source: `Power Plant Codes\Visual Studio 2015\SSRS Reports\Power Plant Reports\`*

The final tier of the MES architecture is the **SQL Server Reporting Services (SSRS)** layer. It consumes the transactional data (Session History, Downtime Logs, Pallet History) generated by the WinForms client and configured by the Web Portal, aggregating it into actionable production analytics.

This chapter maps the SSRS `.rdl` (Report Definition Language) files to their underlying SQL Stored Procedures and defines the core KPIs calculated by the database engine.

### 10.1 Report Ecosystem Overview

The SSRS project contains over 50 report variations. They are broadly categorized into four business domains:

1. **Efficiency & Performance**: `Work Center Efficiency Report.rdl`, `Labor and Machine Efficiency.rdl`, `Line Efficiency.rdl`.
2. **Waste & Material Variance**: `Scrapped Components.rdl`, `Give Away Report.rdl`, `Shop Order Cases Produced Variance.rdl`.
3. **Downtime Analysis**: `Top 10 Down Time.rdl`, `Down Time Details.rdl`.
4. **Traceability & Logs**: `Pallet Inquiry.rdl`, `Weight Log Inquiry.rdl`, `Packaging Log Inquiry.rdl`.

**Data Access Pattern**:
Unlike the Web Portal which uses inline SQL, the SSRS reports **exclusively use Stored Procedures** hosted on the Central SQL Server (`PowerPlant_Prd` or `PowerPlantAXSP_Dev`). Every dataset within an `.rdl` file maps to a `PPsp_` prefixed procedure.

### 10.2 Data Source Mapping & Historical Aggregation

To bridge the gap between real-time client state (`tblSessionControl`) and historical reporting, the system relies on specialized functions and history tables.

#### 10.2.1 The Session Summary Function: `tfnSessionControlHstSummary`

The foundational aggregate for most efficiency reports is the Table-Valued Function **`tfnSessionControlHstSummary`**.

For example, `PPsp_WorkCenterEfficiencySummary` drives the "Work Center Efficiency" report. Inside this SP, it queries:

```sql
FROM tfnSessionControlHstSummary ('WithAdjByOpr', @vchFacility, NULL, NULL, NULL,  
                                  @vchItemNumber, @dteFromProdDate, @intFromShift,  
                                  @dteToProdDate, @intToShift) as tSCH
```

This function aggregates the highly granular `tblSessionControlHst` (which records every time an operator logs in, pauses, or changes items) into continuous blocks of `PaidRunTime` and `ActRunTime`, summarizing `CasesProduced` by `ShiftProductionDate` rather than raw chronological timestamps.

#### 10.2.2 Core Report-to-SP Mappings

| SSRS Report (`.rdl`) | Primary Dataset SP | Key Base Tables Queried |
|---|---|---|
| **Work Center Efficiency** | `PPsp_WorkCenterEfficiencySummary` | `tfnSessionControlHstSummary`, `tblEquipment`, `tblDownTimeLog`, `tblItemMaster` |
| **Pallet Inquiry** | `PPsp_Pallet_Sel` | `tblPallet`, `tblPalletHst` (UNIONed) |
| **Down Time Details** | `PPsp_DownTimeLog_Sel` | `tblDownTimeLog`, `tblDTReasonCode` |
| **Top 10 Down Time** | `PPsp_DownTimeLogTop10` | `tblDownTimeLog`, grouped by `ReasonCode` descending |
| **Scrapped Components** | `PPsp_ScrappedComponent` | `tblQATLog`, `tblItemMaster` (for component BOM waste) |
| **Weight Log Inquiry** | `PPsp_WeightLog` | `tblWeightLogHst`, `tblWeightLog` |

### 10.3 Key Performance Indicators (KPIs) Definition

The heavy lifting of KPI calculation is done within the T-SQL stored procedures, not in the SSRS expressions. The RDL simply renders the pre-calculated columns.

#### 10.3.1 Machine Efficiency (`PPsp_WorkCenterEfficiencySummary`)

To calculate how efficiently a work center ran against standard rates (`tblStdMachineEfficiencyRate`), the SP calculates:

- **`StdUnitPerHr`**: `POWER(10, BasisCode) / MachineHours` (Looked up via `tfnStdMachineEfficiencyRate`).
- **`StdMachineHrEarnedInUnit`**: `(CasesProduced + AdjustedQty) / StdUnitPerHr` (Converting physical output into "earned hours").
- **`Efficiency %`**: `(StdMachineHrEarnedInUnit / PaidRunTime) * 100` (handled formatting side, but components provided by SP).

#### 10.3.2 Product Giveaway (`PPsp_WorkCenterEfficiencySummary` / Give Away Report)

Giveaway measures how much excess product was packed beyond the label claim.

- **`NetWeight`**: `Round(labelweight * saleableunitpercase * PackagesPerSaleableUnit / LBtoGM_Conversion, 3)`
- **`PoundsProduced`**: `(CasesProduced + AdjustedQty) * NetWeight`
- The system averages the `OverPackWgt` recorded from the checkweigher pipeline (`tblWeightLog`) against the theoretical target.

#### 10.3.3 Net Available Time & Downtime

- **`PaidRunTime`**: Shift duration minus unpaid breaks.
- **`ActRunTime`**: `PaidRunTime` minus logged `DownTime` (from `tblDownTimeLog`).

### 10.4 Parameterization Logic

Reports are highly parameterized to allow drilling down by date, shift, and physical hierarchy.

1. **Hierarchy Parameters**:
   - `@vchFacility`: Populated via `PPsp_Facility_Sel`.
   - `@vchWorkCenters`: Populated via `PPsp_WorkCenter_Sel` (groups packaging lines, e.g., "Line 1 & 2" = "Packaging Area A").
   - `@vchPackagingLine` / `@vchEquipment`: Populated via `PPsp_Equipment_Sel`.

2. **Temporal Parameters**:
   - `@dteFromProdDate` & `@dteToProdDate`: The **Production Date** (not calendar date). As defined in Ch6 (§6.1.3), a shift spanning midnight attributes all output to the day the shift *started*.
   - `@intFromShift` & `@intToShift`: Populated via `CPPsp_ShiftIO`.

3. **Data Cascading**:
   Parameters in SSRS cascade. Selecting a `@vchFacility` triggers a refresh of the `@vchWorkCenters` dataset, which limits the available lines.

### Summary of the PowerPlant Architecture

With the completion of the SSRS layer, the full lifecycle of a data point in the PowerPlant MES is understood:

1. **Configured** via the QA Web Portal (Ch8).
2. **Synced** via `ExportSQLDataToIPC` (Ch7.5) to the IPC's Local DB.
3. **Executed** at the IPC by the WinForms Client (Ch1-4) & `PPTCPServer` (Ch5).
4. **Offloaded** by background sync threads from Local DB to Central Server.
5. **Aggregated** via T-SQL stored procedures and `tfnSessionControlHstSummary` (Ch7/10).
6. **Visualized** by SSRS Reports (Ch10) for daily production meetings.

### Refactoring Scorecard: Chapter 10 (SSRS Reporting)

| Factor | Score (1-10) | Description | Actionable Recommendation |
|---|---|---|---|
| **Fragility** | **5** | Hard dependency on specific SP signatures with zero tests. | Add integration tests for report SP outputs. |
| **Security** | **3** | Handled natively by SSRS role delivery. | Maintain appropriate network isolation. |
| **Complexity** | **6** | High reliance on nested Table-Valued Functions blocking DB threads. | Pre-calculate heavy aggregates in nightly background ETL jobs. |

---

## Chapter 11 — Operations, Maintenance & Auxiliary Tooling Matrix

While Chapters 1–10 cover the core WinForms client, Web Portal, Database, and Reporting, the ecosystem features a secondary layer of "stealth" telemetry processors, deployment bootstrappers, and legacy admin consoles located primarily in `Visual Studio 2012\Projects`.

### 11.1 Infrastructure Bootstrapping & Heartbeat Services

1. **`IPCInitialPgm` (The IPC Bootstrapper)**
   - **Role**: Replaces standard Windows shells on Industrial PCs. It launches on startup, interrogates local environment variables, binds SQL connection aliases via `ComputerName` interpolation, and ensures the dual-process core (`PowerPlant.exe` and `PPTCPServer.exe`) starts cleanly in tandem.
2. **`CaseCountMonitor` & `CheckIPCPfmCounter` (Telemetry Probes)**
   - **Role**: Silent daemons ensuring system stability.
   - **Mechanism**: `CaseCountMonitor` constantly watches the SQL tables populated via CheckWeigher sockets (Ch5). If the line is running but cases cease incrementing, it signals an un-logged downtime anomaly. `CheckIPCPfmCounter` monitors localized unmanaged memory leaks or CPU spikes on the older Windows Embedded IPCs.
3. **`PowerPlantIPCDeployment` (WPF Deployment Tool)**
   - *Source: `Visual Studio 2010\PowerPlantIPCDeployment`*
   - **Role**: The centralized fleet provisioning console. This is a **WPF application** (the only WPF project in the workspace) used by IT administrators to remotely deploy, update, and configure all IPC stations from a single dashboard.
   - **Mechanism**: The `MainWindow.xaml.vb` (70KB) queries `tblComputerConfig` to enumerate all registered IPC stations, then pushes deployment packages (executables, configs, database scripts) to each target machine. It manages the `dsDeployProjects` and `dsDeployItems` datasets to track which software versions are installed on each IPC.

### 11.2 Specialized Master Data Administration

Parallel to the newer QA Web Sites (Ch8), specific configuration subsets demand their own standalone fat clients or specialized web portals.

1. **`ItemLabelOverride` (ASP.NET Web Site)**
   - **Role**: While `tblItemMaster` stores standard Label definitions synced from ERP (Dynamics AX), there are scenarios where a specific shop order or promotion demands an impromptu label design change or differing UPC code. `ItemLabelOverride` is a streamlined portal specifically allowing production leads to forcefully inject localized print overrides bypassing AX rules.
2. **WinForms Master Data Interfaces**
   - **`Plant Staff Maintenance`**: A discrete GUI used by HR/admins to configure localized system PINs, Operator Roles, and barcode overrides before `KronosETL` takes over the scheduling aspects.
   - **`Package Line Printer / Rate Maintenance`**: Used by maintenance leads to configure `tblEquipment` hardware capacities and adjust the IP mappings of the physical Zebra/Markem-Imaje printers linked down to specific work centers.
3. **`PowerPlantAndAXTools`**
   - **Role**: The SysAdmin's "parachute" tool. It allows administrators to manually fire synchronization stored procedures (e.g., bypassing `ExportSQLDataToIPC`) to force-resync BOMs and Shop Orders if the Dynamics AX bridge crashes.

### 11.3 Probat Roasting System Integration (`DB Server_Probat`)

*Source: `DB Server_Probat/dbo/`*

The workspace contains a **third** SQL Server database project, `DB Server_Probat`, which is the integration bridge between the PowerPlant MES and the **Probat roasting control system**. This database is distinct from `PowerPlant_Prd` (Server) and `PowerPlant_Loc` (Local).

**Architecture:**

- **39 Tables** (`PRO_EXP_*`, `PRO_IMP_*`): These follow a strict Import/Export naming convention. `PRO_IMP_*` tables receive inbound data from Probat (e.g., `PRO_IMP_ORDER` — roasting orders, `PRO_IMP_BLENDS` — blend recipes). `PRO_EXP_*` tables stage outbound data (e.g., `PRO_EXP_PALLET_FB` — pallet feedback, `PRO_EXP_TRANSF` — material transfers).
- **12 Stored Procedures** (`PPsp_ExportPalletToProbat`, `PPsp_CheckPalletToProbat`, `PPsp_MonitorProbatOrderRecError`, etc.): Manage the bidirectional flow, including error monitoring and retry logic.
- **19 Views** (`PRO_EXP_*_VIEW`, `PRO_IMP_*_ERROR_VIEW`): Provide consolidated status dashboards for integration health checks.

**Cross-references:**

- The `IsActiveProbatEnableLine()` guard in §2.7 (Pallet Management) directly gates whether this integration is active on a given packaging line.
- The `tblProbatEquipment` table referenced in `DB Local_ImportData` is the IPC-side copy of the Probat equipment roster, synced down via `ExportSQLDataToIPC` (Ch7.5).

### 11.4 SSIS Data Staging Databases (`DB Server_ImportData` / `DB Local_ImportData`)

*Source: `DB Server_ImportData/`, `DB Local_ImportData/`*

These two SQL Server database projects define the **ERP-to-MES staging layer**. They are the physical tables that sit between Dynamics AX and the PowerPlant operational databases, populated by **SQL Server Integration Services (SSIS)** packages.

**`DB Server_ImportData` (Central Server Side):**

- **22 Tables**: Include `tblItemMasterFromERP`, `tblItemMasterTxFromERP`, `tblShopOrder`, `tblBillOfMaterials`, `tblStdMachineEfficiencyRate`, `tblGrindingSchedule`, and `tblPalletAdjustment`.
- **13 Stored Procedures**: Follow a `PPsp_PreProcess_*` / `PPsp_PostProcess_*` pattern. Pre-Process SPs cleanse and normalize the raw ERP data. Post-Process SPs (`PPsp_PostProcess_DownLoadItemMaster_01`, `PPsp_PostProcess_DownLoadShopOrder_01`) merge the staged data into the live `PowerPlant_Prd` tables.

**`DB Local_ImportData` (IPC Side):**

- **25 Tables**: Mirror a subset of the server tables (e.g., `tblComputerConfig`, `tblItemMaster`, `tblShopOrder`, `tblQAT*` series). These are the **target tables** that `ExportSQLDataToIPC.exe` (Ch7.5) pushes configuration data into for offline IPC operation.

**Data Flow**: `Dynamics AX → SSIS → DB Server_ImportData → PPsp_PostProcess → PowerPlant_Prd → ExportSQLDataToIPC → DB Local_ImportData → PowerPlant_Loc`.

### 11.5 Web Material Calculator (ASP.NET Core MVC)

*Source: `Visual Studio 2019\Web Material Calculator\`*

This is the **only ASP.NET Core project** in the entire MES ecosystem, representing a generational leap from the legacy VB.NET/WebForms stack. It is a standalone ASP.NET Core 5+ MVC web application with a clean layered architecture:

**Project Structure:**

| Project | Role |
| --- | --- |
| `Web Material Calculator` | ASP.NET Core MVC host (Razor Views, Controllers) |
| `PowerPlant.Models` | EF Core DbContext (`PPDbContext`) and entity models (`ItemMaster`, `WebMaterial`) |
| `PowerPlant.Dtos` | Data Transfer Objects with validation attributes |
| `PowerPlant.Service` | Repository pattern (`IItemRepository`, `IFacilityRepository`) |
| `PowerPlant.Tools` | Pagination, sorting, and property mapping utilities |

**Business Purpose:**
The calculator allows production planners to manage **packaging material roll specifications** (core diameter, roll diameter, film thickness, length in feet, IMPs). Given a measured roll diameter, it calculates the remaining material percentage and impression count using the formula:

`Length = π × (RollDiameter² − CoreDiameter²) / Thickness / 12`

**Technical Notes:**

- Uses **AutoMapper** for entity-to-DTO mapping and **X.PagedList** for server-side pagination.
- Connects to the same `PowerPlant_Prd` database via EF Core, specifically joining `tblWebMaterial` with `tblItemMaster`.
- Implements a JSON-file-based role authorization system (`UserConfig.json`) rather than the legacy `tblPlantStaff` PIN model.
- Full CRUD operations on `WebMaterial` records via `WebMaterialController`.

### 11.6 The Deployment Identity (Invisible Links)

To properly initialize an IPC workstation, the system relies on local environment variables and registry keys bridging the OS to the `.config` files.

- **`ComputerName`**: Native Windows environment variable defining the instance name of the `SQLEXPRESS` local caching DB (e.g., `MPHI02\SQLEXPRESS`).
- **`logonserver`**: Windows Domain Controller path, referenced occasionally for active directory time checks or UNC paths.
- **`Facility`**: Injected via startup arguments or local registry to scope which master data is pulled down via `ExportSQLDataToIPC`.
- **Ports Matrix (Firewall Rules)**:
  - `8000`: `PPTCPServer` internal loopback (TCP).
  - `1433`: SQL Server standard port.
  - `3011+`: CoLOS custom label spool ports.

### Refactoring Scorecard: Chapter 11 (Operations & Telemetry)

| Factor | Score (1-10) | Description | Actionable Recommendation |
|---|---|---|---|
| **Fragility** | **9** | Dependency on system environment variables creates brittle deployments across 100+ IPCs. | Move to a centralized configuration mechanism via a containerized/managed `.appsettings` startup process. |
| **Security** | **6** | WPF remote installer pushes executable payloads over LAN based on text configurations. | Package updates into signed installers or MSIX AppAttach. |
| **Complexity** | **7** | Five overlapping watchdog tools (`IPCInitialPgm`, `CaseCountMonitor`, etc.). | Consolidate telemetry into a single unified agent (e.g., OpenTelemetry). |

---

## Appendix A: Deployment & Startup Checklist

For operations and system recovery, if an IPC (Industrial PC) or the Central Server crashes, the following “first push” programs and processes must be confirmed as running to guarantee a functional MES floor.

### A.1 IPC (Shop Floor Client) Process Checklist

If an IPC reboots, these are the **4 processes** that must be visible in the Windows Task Manager on that specific touch-screen station:

1. **`IPCInitialPgm.exe`**: The bootstrap program (configured to run on Windows Startup) that checks environment variables and launches the downstream processes.
2. **`PowerPlant.exe`**: The main visible WinForms UI (Chapter 1–4).
3. **`PPTCPServer.exe`**: The background invisible Socket listener on Port 8000 handling hardware triggers and remote WCF commands (Chapter 5, 6.11).
4. **`ExportSQLDataToIPC.exe`**: Dispatched via **Windows Task Scheduler** on the IPC, running periodically to pull down configuration and push up local `Hst` table transactions to the Central DB (Chapter 7.5).

### A.2 Central Server (`MPHOPP02`) Process Checklist

On the central production server, the following backend services must be running in `services.msc` to support the plant:

1. **`Label Printing` Windows Service**: The print delegator polling `tblPrintRequest` and talking to the CoLOS Server (Chapter 2.8).
2. **`KronosETL` Windows Service**: Polling Cloud APIs to keep the labor tables updated for SSRS (Chapter 6.10).
3. **`CaseCountMonitor` / `CheckIPCPfmCounter`**: The silent anomaly-detection telemetry watchers (Chapter 11.1).

### A.3 Note on Codebase "Dead Code"

Due to this codebase evolving significantly from VS 2010 to VS 2019, various legacy communication protocols (e.g., older raw XML Interface classes like `clsXMLInterface.vb` with commented-out `StopShopOrder` methods) remain in the source files.

> [!IMPORTANT]
> **This Handbook strictly documents the Active Production paths.** Any files, classes, or commented blocks (like the legacy XML branches) that do not trace back to the startup processes listed in this Appendix or the Chapter workflows have been intentionally omitted as "Dead Code" to provide a precise mapping of the currently executing production constraints.

---

## Appendix B: Modernization Roadmap

Based on the architectural constraints and technical debt documented in this handbook, the following matrix represents a strategic, phased approach to replacing the `PowerPlant` MES legacy codebase with a modern, maintainable technology stack (e.g., C# / .NET 8 / Blazor / EF Core).

### B.1 Phase 1: Securing the Perimeter (Data Access & Infrastructure)

The most severe risks in the current system involve unauthorized database execution contexts and string-based query assembly.

1. **Remove SQL CLR Extensibility**: Decommission the `[ExportDataToIPCDB]` assembly.
   - **Reason**: `PERMISSION_SET = UNSAFE` inside the SQL engine is a critical vulnerability.
   - **Solution**: Replace with a background C# worker service (e.g., Quartz.NET or Hangfire) running externally that performs `SqlBulkCopy` between instances.
2. **Defang the QA Web Portal (Ch8)**:
   - **Reason**: Current WebForms execute raw concatenated `ExecQuery` strings, opening the door to SQL Injection attacks.
   - **Solution**: Re-write the 10 data entry projects into a single Blazor Web Admin App backed by EF Core or parameter-bound Dapper queries.

### B.2 Phase 2: Strangling the God-Object (Domain Driven Design)

`SharedFunctions.vb` (Ch6) is a 7,800-line monolithic module that holds all shift rules, validation rules, and network utilities simultaneously.

1. **Extract Time & Shift Bounded Context**: Move all `clsShift`, `clsExpiryDate`, and midnight-crossing logic into an isolated `.NET Class Library` (`PowerPlant.Domain.TimeContext`).
2. **Abstract CoLOS & Checkweighers (Ch5)**:
   - **Reason**: The WinForms UI thread currently manually constructs polling loops and manipulates TCP ports.
   - **Solution**: Create dedicated hardware adapter microservices (`CoLosPrintService`, `CheckweigherService`) that expose REST/gRPC endpoints. Let the client simply call `.PrintAsync()`.

### B.3 Phase 3: Headless MES (Frontend Decoupling)

The `PowerPlant.exe` WinForms client intertwines UI rendering directly with database stored procedures.

1. **Build the `PowerPlant.API`**: Expose all procedures via a secure Web API. WinForms clients should no longer connect directly to the SQL Server via `SqlClient`.
2. **Implement State Managers for QAT (Ch3)**: Replace the reflection-based array parsing of `tblQAT*` tables with strongly-typed C# models and a predefined workflow state engine.
3. **Web-Based IPC Clients**: Eventually replace the WinForms shell with a tablet-friendly Blazor WebAssembly or MAUI application, removing the need for local SQL Server Express installations (`tblLocalPowerPlant`) via offline-capable browser caching strategies (PWA).

---

*(End of Handbook)*
