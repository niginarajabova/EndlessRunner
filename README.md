# Candy Rush — Endless Runner

A candy-themed 3-lane endless runner built with **Unity 6** (6000.4.6f1) using the Universal Render Pipeline.

![Unity](https://img.shields.io/badge/Unity-6000.4.6f1-blue)
![Platform](https://img.shields.io/badge/Platform-PC-green)
![License](https://img.shields.io/badge/License-Educational-yellow)

---

## About

The player runs endlessly through a colorful candy world, collecting sweets for points while dodging obstacles like tigers and fences. A flying donut companion follows the player, adding personality to the gameplay.

---

## Gameplay

- **3-Lane System** — move left/right between lanes to collect items and avoid obstacles
- **Jumping** — leap over ground-level hazards
- **Collectibles** — gather cookies, lollipops, macarons, marshmallows, and marmelades for points
- **Obstacles** — tigers and fences end the game on contact
- **Syrup Traps** — slow the player down and deduct points
- **Speed Acceleration** — the game gets faster over time
- **Donut Companion** — a flying donut character follows along ahead of the player

---

## Controls

| Key | Action |
|-----|--------|
| `Space` | Start game / Jump |
| `A` or `←` | Move left |
| `D` or `→` | Move right |

---

## Project Structure

```
Assets/
├── Scripts/
│   ├── PlayerMovement.cs      — Main player controller (movement, jump, collisions, game state)
│   ├── Segement.cs            — Procedural level generation (road segments, item/obstacle spawning)
│   ├── ScoreManager.cs        — Singleton score system with UI updates
│   ├── ItemData.cs            — Collectible item logic (score value, effects)
│   ├── CakeCollectible.cs     — Floating animation for cake items
│   ├── CakeVisual.cs          — Rotation animation for cookies
│   ├── FloatingText.cs        — "+N" floating score text feedback
│   ├── PopUpObstacle.cs       — Jelly bounce-in animation for obstacles
│   ├── ThiefRunner.cs         — Donut companion flight logic
│   └── ThiefLogic.cs          — Companion hover/tilt animation and particles
├── Prefab/
│   ├── CakeCollectible        — Collectible cake
│   ├── CookiePivot            — Rotating cookie with animator
│   ├── Lollipop (3 variants)  — Lollipop collectibles
│   ├── Macaron 1              — Macaron collectible
│   ├── Marmelade (3 variants) — Marmelade collectibles
│   ├── Marshmallow            — Marshmallow collectible
│   ├── CandyBush              — Decorative candy bush
│   ├── Syrup                  — Slow trap obstacle
│   ├── SugarTrail             — Sugar particle effect
│   └── Tree_1 (2 variants)    — Environment trees
├── Food Pack-Demo/            — 55+ food 3D models and mesh colliders
├── LowPoly Environment Pack/  — Low-poly environment assets (trees, roads)
├── Materials/                 — Custom materials (Biscuit, Chocolate, Syrup, Skybox)
├── Settings/                  — URP render pipeline profiles
├── Scenes/
│   └── SampleScene.unity      — Main game scene
└── CookiePivot.controller     — Animator controller for cookie rotation
```

---

## Technical Details

| Component | Details |
|-----------|---------|
| Engine | Unity 6 (6000.4.6f1) |
| Render Pipeline | URP (Balanced / HighFidelity / Performant profiles) |
| UI System | TextMeshPro |
| Input | Legacy Input Manager (`Input.GetKeyDown`) |
| Level Generation | Procedural — segments spawn 100m ahead, destroyed after 30s |
| Collision System | Trigger-based with custom tags: `Item`, `Obstacle`, `Slow` |

---

## Game Architecture

```
PlayerMovement (Central Controller)
    ├── Handles movement, jumping, gravity
    ├── Manages game start / game over / restart
    ├── Processes all collisions (Item, Obstacle, Slow)
    └── Integrates with:
         ├── ScoreManager (Singleton) — score tracking and UI
         ├── SegementGenerator — spawns road + objects ahead
         ├── FloatingText — visual "+N" feedback on collect
         ├── ThiefRunner — companion follows player
         └── PopUpObstacle — obstacle spawn animations
```

---

## How to Run

1. Install **Unity 6** (version 6000.4.6f1 or compatible)
2. Clone this repository:
   ```bash
   git clone https://github.com/YOUR_USERNAME/EndlessRunner.git
   ```
3. Open the project in Unity Hub
4. Open `Assets/Scenes/SampleScene.unity`
5. Press **Play**

---

## Screenshots

> *Add gameplay screenshots here*

---

## Future Improvements

- [ ] High score save system (PlayerPrefs)
- [ ] Difficulty scaling (more obstacles over time)
- [ ] Audio system (background music, collect/hit SFX)
- [ ] Game Over UI screen with score display
- [ ] Object pooling for better performance
- [ ] Migrate to New Input System
- [ ] Mobile touch controls
- [ ] Power-ups (shield, magnet, multiplier)

---

## Credits

- **3D Models**: Food Pack-Demo, LowPoly Environment Pack (Asset Store)
- **Engine**: Unity Technologies
- **Development**: Student Project

---

## License

This project is for educational purposes.
