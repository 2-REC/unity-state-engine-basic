# Unity State Engine Basic

Simple State Management Engine for Unity 6.3+


## Setup

### 1. Create the `GlobalManager` prefab

Attach on the same GameObject:
- `GlobalManager`
- `GlobalStateManager`
- `GlobalSessionManager`
- `GlobalDataManager`

TODO:
+add XML file(s)... (states, (values))

### 2. Create the `GameManager` prefab

Attach on the same GameObject:
- `GameManager`
- `GameStateManager`
- `GameSessionManager`
- `GameDataManager`

TODO:
+add XML files... (states, values, levels)

### 3. Create the `ManagerHost` object or prefab

Attach:
- `ManagerHost`

Assign:
- `GlobalManager` prefab
- `GameManager` prefab

### 4. Ensure a host exists before first manager access

Choose one:
- place `ManagerHost` in bootstrap scene, or
- place `ManagerHostBootstrap` in scenes started from the editor

> **NOTE:** Change the script execution order of `ManagerHostBootstrap` to make sure it executes before other scripts.

### 5. Usage

- `GlobalManager.Instance`
- `GameManager.Instance`
- `GlobalManager.Instance.State.LeaveToScene("MainMenu")`
- `GameManager.Instance.State.LeaveToScene("WorldMap")`
- `GameManager.Instance.State.QuitApplicationFromState()`
- `ManagerHost.Instance.DestroyGameManager()`
- `ManagerHost.Instance.DestroyGlobalManager()`
- `ManagerHost.Instance.DestroyAllManagers()`
