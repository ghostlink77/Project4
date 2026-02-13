# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Unity 6 (6000.3.3f1) 2D tower-defense/survivor hybrid game. Korean development team — comments and commit messages are in Korean. The project uses URP, Addressables, and the new Input System.

## Build & Development

- **Engine:** Unity 6000.3.3f1 — open via Unity Hub
- **Rendering:** Universal Render Pipeline (URP 17.3.0)
- **Key Packages:** Addressables 2.7.6, Input System 1.17.0, 2D Animation/Sprite/Tilemap
- **No CLI build scripts or CI/CD** — all building is done through the Unity Editor
- **Git workflow:** Feature branches merge into `develop` via PRs

## Architecture

### Asset Folder Convention

Numbered prefix directories under `Assets/`: `01.Scenes`, `02.Prefabs`, `03.Data`, `04.Scripts`, `05.Sprites`, `06.Animations`, `07.Font`, `08.Audio`.

### Script Organization (`Assets/04.Scripts/`)

| Directory | Purpose |
|-----------|---------|
| `_Common/` | `SingletonBehaviour<T>`, `IDamageable`, `SceneLoader`, `DataTableManager` |
| `_ScriptableObjects/` | SO definitions: `PlayerDefaultData`, `WeaponStatData`, `TurretData` |
| `Player/` | Manager + Controllers pattern (see below) |
| `Enemy/` | `EnemySpawnManager` → `EnemySpawnPoint[]` → `EnemySpawner` (pooled) |
| `Weapon/` | `WeaponManager` → `WeaponStatController` + `WeaponShootController` |
| `Turret/` | `TurretPlacer`, `AttackTurret` |
| `Item_Grounded/` | `ItemGroundedBase` (abstract) → `ExpObject`, `Scrap` |
| `UI/` | `UIManager` (Singleton) + `BaseUI` base class with fade transitions |
| `Agit/` | Base/home structure (`IDamageable`) |

### Core Patterns

**Singleton:** `SingletonBehaviour<T>` with `DontDestroyOnLoad` — used by `SceneLoader`, `DataTableManager`, `UIManager`, `AudioManager`, `InGameManager`, `EnemySpawner`, `EnemySpawnManager`.

**Manager-Controller:** `PlayerManager` (Singleton) aggregates `PlayerMoveController`, `PlayerStatController`, `PlayerItemController`, `PlayerAnimationController`, `PlayerEventController`, `PlayerSoundController`.

**Event-Driven:** `PlayerEventController` exposes `Action` events (`Death`, `Revive`, `Hurt`, `Move`, `Stop`). Other controllers subscribe to these rather than polling state.

**Object Pooling:** `ObjectPool<GameObject>` used for enemies (`EnemySpawner`), projectiles (`WeaponShootController`), and loot items (`ExpObjectSpawner`, `ScrapSpawner`).

**Interfaces:** `IDamageable` (damage intake), `IItemGrounded` (collectible items), `IGameData` (data loading).

### Data Pipeline

- **CSV tables** in `Assets/Resources/DataTable/` loaded by `DataTableManager` (implements `IGameData`)
- **ScriptableObjects** in `Assets/03.Data/` for player/weapon/turret base stats
- **Addressable Assets** for enemies: loaded via `Addressables.LoadAssetAsync<GameObject[]>("EnemyPrefabs")`

### Scene Flow

`Title` → `Lobby` → `InGame`. Loaded via `SceneLoader.LoadScene(ESceneType)`. Test scenes live in `Assets/01.Scenes/TestScenes/`. `SceneLoader` appends `_` suffix to scene names when loading test variants.

### Audio

Dual system: `AudioManager` (global BGM/SFX singleton) and `SoundManager` (player-specific SFX). Audio types: `BGM` (looped) and `SFX` (OneShot).

### UI

`UIManager` loads UI prefabs from `Resources/UI/{TypeName}`. `BaseUI` subclasses support `PlayableDirector`-driven fade in/out. Per-scene managers: `TitleManager`, `LobbyManager`, `InGameManager`.

## Conventions

- `SerializeField` for inspector-exposed fields; private backing fields with property accessors
- `#region` blocks for code organization within files
- Event trigger methods named `CallEventName()`
- Animator parameter hashes cached as `static readonly int`
- Coroutines for delayed/timed actions
- Damage formula: `damage * 100 / (100 + defense)`
