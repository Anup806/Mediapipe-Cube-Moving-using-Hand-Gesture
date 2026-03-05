# Mediapipe Cube Moving using Hand Gesture

A real-time interactive Unity application that uses **MediaPipe Unity Plugin** and your device's camera to **move a 3D cube using hand gestures**. Simply pinch your fingers and drag to move the cube across your screen in real-time — no controllers, no touch, just your hands!

---

## Project Overview

This project demonstrates **touchless gesture-driven 3D cube movement** powered by computer vision and machine learning-based hand tracking in Unity. The core functionality allows you to **grab and move a cube naturally using a two-finger pinch gesture**, with your hand movements translated 1:1 to cube position in real-time.

Beyond the primary cube-moving capability, the project extends into comprehensive 3D manipulation including scaling, rotation, visual feedback, and mobile VR support — making it a versatile prototype for AR/VR gesture-based interaction systems.

---

## Demo Description

When the application runs, your device camera feed is processed every frame by MediaPipe's hand-tracking pipeline. A coloured 3D cube appears in the scene. Perform gestures in front of the camera to interact:

### Core Feature: Cube Moving
- 🎯 **Two-finger pinch near the cube** — Grabs the cube and lets you **drag it across the screen** in real-time, following your hand movement precisely
- A cyan **trail** follows the cube as you move it
- The cube turns **green** when grabbed

### Extended Manipulation Features
- 🔍 **Two-hand pinch (both hands)** — Scales the cube up or down (spread = bigger, pinch = smaller)
- 🔄 **Three-finger pinch + hand movement** — Rotates the cube on its X and Y axes
- ✨ **Hover near the cube** (without pinching) — Triggers a **yellow glow pulse** effect
- 📳 **Haptic shake feedback** — Brief shake animation when you grab or start scaling

### Real-time HUD
An on-screen display shows:
- Current gesture name
- Cube position, scale, and rotation
- Live FPS counter

---

## Features

### Primary Features (Cube Moving)
- ✅ **Real-time hand landmark detection** via MediaPipe Unity Plugin (21 landmarks per hand)
- ✅ **Grab & Drag** — Intuitive two-finger pinch to move the cube anywhere on screen
- ✅ **Trail Effect** — Cyan motion trail visualization during movement
- ✅ **Visual feedback** — Colour-coded states: Normal (cyan), Hover (yellow), Grabbed (green)

### Advanced Features
- ✅ **Two-hand Scaling** — Both hands pinching scales the cube proportionally (min 0.2x – max 3x)
- ✅ **Three-finger Rotation** — Three-finger pinch controls X-axis and Y-axis rotation simultaneously
- ✅ **Haptic Shake Feedback** — Physical feedback on grab/scale events
- ✅ **Visual Glow Effect** — Pulsing effect when hovering near the cube
- ✅ **Rotating state** (orange) and **Scaling state** (magenta) visual indicators

### Platform & Mobile Support
- ✅ **On-screen HUD** — Live gesture name, position, scale, rotation, and FPS display
- ✅ **Mobile VR Mode** — Split-screen stereoscopic rendering for Google Cardboard
- ✅ **Gyroscope Head Tracking** — Gyro-driven camera rotation on mobile devices
- ✅ **Android Camera Permission Handler** — Automatic runtime permission request

---

## Technologies Used

| Technology | Role |
|---|---|
| **Unity 2021.3 LTS** | Game engine / rendering / scene management |
| **C#** | Scripting language for all game logic |
| **MediaPipe Unity Plugin** (`com.github.homuler.mediapipe` v0.14.4) | Hand landmark detection via ML pipeline |
| **MediaPipe Hands Model** | 21-point 3D hand landmark estimation |
| **Unity TrailRenderer** | Visual trail effect during cube movement |
| **Unity Gyroscope API** | Head tracking on mobile devices |
| **Android / iOS Camera API** | Runtime camera access |

---

## System Workflow

```
┌──────────────────────────────────────────────────────────────────┐
│  1. Camera Feed                                                  │
│     Device camera captured every frame by MediaPipe solution     │
├──────────────────────────────────────────────────────────────────┤
│  2. Hand Landmark Detection                                      │
│     MediaPipe Hands model infers 21 3D landmarks per hand        │
├──────────────────────────────────────────────────────────────────┤
│  3. Gesture Classification                                       │
│     HandGestureController reads landmark coordinates:            │
│     • Thumb tip (4), Index tip (8), Middle tip (12), Wrist (0)   │
│     • Computes inter-landmark distances to detect pinch state    │
├──────────────────────────────────────────────────────────────────┤
│  4. Cube Movement & Transform Mapping                            │
│     PRIMARY: 2-finger pinch near cube → Grab & Move (translate)  │
│     • Hand position mapped to screen space                       │
│     • Cube follows hand movement in real-time                    │
│                                                                  │
│     EXTENDED:                                                    │
│     • 2-hand pinch → Scale (min 0.2x – max 3x)                  │
│     • 3-finger pinch + movement → Rotate (X & Y axes)           │
├──────────────────────────────────────────────────────────────────┤
│  5. Visual & Haptic Feedback                                     │
│     Colour, trail, glow, and shake effects applied each frame    │
├──────────────────────────────────────────────────────────────────┤
│  6. HUD Overlay                                                  │
│     OnGUI renders gesture name, position, scale, rotation, FPS   │
└──────────────────────────────────────────────────────────────────┘
```

---

## Requirements

| Requirement | Version |
|---|---|
| Unity Editor | 2021.3.x LTS (tested on 2021.3.45f2) |
| MediaPipe Unity Plugin | 0.14.4 |
| Platform | Windows / macOS (editor) · Android / iOS (device) |
| Camera | Built-in or USB webcam (desktop) · front/rear camera (mobile) |

---

## Installation Guide

### 1. Clone the Repository

```bash
git clone https://github.com/Anup806/Mediapipe-Cube-Moving-using-Hand-Gesture.git
cd Mediapipe-Cube-Moving-using-Hand-Gesture
```

### 2. Download the MediaPipe Unity Plugin

1. Download `com.github.homuler.mediapipe-0.14.4.tgz` from the [MediaPipe Unity Plugin releases](https://github.com/homuler/MediaPipeUnityPlugin/releases/tag/v0.14.4).
2. Place the `.tgz` file in a stable location on your machine (e.g., `C:/UnityPackages/`).

### 3. Open in Unity

1. Open **Unity Hub**.
2. Click **Open → Add project from disk** and select the cloned folder.
3. Unity will import all assets. If prompted about missing packages, proceed to step 4.

### 4. Install the MediaPipe Package

1. In Unity, open **Window → Package Manager**.
2. Click the **+** button → **Add package from tarball…**
3. Select the downloaded `com.github.homuler.mediapipe-0.14.4.tgz` file.
4. Wait for the import to complete.

> **Note:** The `Packages/manifest.json` references the plugin via a local file path. Update this path if you place the `.tgz` in a different location:
> 
> ```json
> "com.github.homuler.mediapipe": "file:YOUR/PATH/com.github.homuler.mediapipe-0.14.4.tgz"
> ```

### 5. Open the Scene

1. In the **Project** panel, navigate to `Assets/Scenes/`.
2. Double-click `SampleScene.unity` to open it.

---

## How to Run the Project

### In the Unity Editor (Desktop)

1. Ensure a webcam is connected.
2. Press the **▶ Play** button in the Unity Editor toolbar.
3. Allow camera access if prompted.
4. Place your hand in front of the camera and **pinch two fingers together near the cube to grab and move it**.

### On Android

1. Connect an Android device with **USB Debugging** enabled.
2. In Unity, open **File → Build Settings**.
3. Select **Android** and click **Switch Platform**.
4. Click **Build & Run**.
5. Grant camera permission when prompted on the device.

### On iOS

1. In Unity, open **File → Build Settings**.
2. Select **iOS** and click **Switch Platform**.
3. Click **Build**, then open the generated Xcode project.
4. Set your Team ID and click **Run** in Xcode.

---

## Gesture Reference

| Gesture | Action | Priority |
|---|---|---|
| ✌️ **Two-finger pinch** (near cube) | **Grab and move the cube** — primary feature | 🎯 **PRIMARY** |
| 🖐️ Open hand near cube | Hover — triggers yellow glow effect | Secondary |
| 🤏🤏 Both hands pinching | Scale the cube (spread = bigger, pinch = smaller) | Extended |
| 🤟 Three-finger pinch + move left/right | Rotate cube around Y-axis | Extended |
| 🤟 Three-finger pinch + move up/down | Rotate cube around X-axis | Extended |

---

## Folder Structure

```
Mediapipe-Cube-Moving-using-Hand-Gesture/
├── Assets/
│   ├── scripts/
│   │   └── HandGestureController.cs   # Core gesture detection & cube movement logic
│   ├── Scenes/
│   │   └── SampleScene.unity          # Main Unity scene with interactive cube
│   ├── Material/
│   │   └── CubeMaterial.mat           # Material applied to the movable cube
│   ├── Samples/
│   │   └── MediaPipe Unity Plugin/    # MediaPipe sample assets (v0.13.1 & v0.14.4)
│   ├── CameraPermissionHandler.cs     # Android runtime camera permission handler
│   ├── MobileUIController.cs          # Mobile UI (settings menu, touch controls)
│   ├── VRCameraRig.cs                 # Split-screen VR & gyroscope head tracking
│   └── TrailMaterial.mat              # Material for the cube's motion trail
├── Packages/
│   └── manifest.json                  # Unity package dependencies
├── ProjectSettings/                   # Unity project configuration files
└── README.md
```

---

## Configurable Parameters

All key parameters are exposed in the Unity Inspector on the `HandGestureController` component:

### Core Movement Parameters
| Parameter | Default | Description |
|---|---|---|
| `pinchThreshold` | 0.08 | Normalised distance at which a pinch is detected |
| `grabRadius` | 3.0 | Screen-space radius (×100 px) within which pinch grabs the cube |

### Extended Manipulation Parameters
| Parameter | Default | Description |
|---|---|---|
| `minScale` / `maxScale` | 0.2 / 3.0 | Scale limits during two-hand scaling |
| `scaleSpeed` | 3.0 | Lerp speed for scale transitions |
| `rotationSpeed` | 150 °/s | Rotation speed during three-finger gesture |
| `enableRotationX/Y` | true | Toggle individual rotation axes |

### Visual & Feedback Settings
| Parameter | Default | Description |
|---|---|---|
| `enableTrail` | true | Enable/disable motion trail during movement |
| `enableHapticFeedback` | true | Enable/disable shake-on-grab effect |
| `enableGlowEffect` | true | Enable/disable hover glow pulse |
| `showUI` | true | Show/hide the on-screen HUD |

---

## Use Cases

### Primary Use Case: Gesture-Based Cube Movement
- **Touchless interaction** — Control 3D objects without physical contact
- **Accessibility interfaces** — Alternative input method for users with limited mobility
- **Demos and exhibitions** — Interactive installations with natural hand-based controls
- **Educational tools** — Teaching computer vision and gesture recognition concepts

### Extended Applications
- 🎮 **Game mechanics** — Build puzzle or physics games controlled by hand gestures
- 🥽 **VR/AR prototyping** — Foundation for immersive hand-tracking experiences
- 🎨 **3D modelling interfaces** — Gesture-based object manipulation for creative tools
- 🏥 **Medical/industrial applications** — Sterile or hands-free control environments

---

## Possible Improvements

### Core Movement Enhancements
- 🎯 **Depth-based movement** — Add Z-axis control for full 3D space navigation
- 🖐️ **Multi-object selection** — Move multiple cubes simultaneously
- ⚡ **Gesture smoothing** — Apply Kalman filtering for smoother movement
- 🎮 **Customizable gestures** — User-defined gesture mappings

### Extended Features
- 🎲 **Complex object interaction** — Extend to manipulate meshes, UI elements, or physics objects
- 🖐️ **Additional gestures** — Swipe-to-throw, two-finger twist, or sign-language commands
- 🥽 **Full VR/XR integration** — Port to OpenXR or Meta Quest for immersive hand interaction
- 🌐 **Multi-user support** — Synchronise cube state across devices via Unity Netcode
- 📊 **Gesture recording & playback** — Record hand landmark sequences for animation
- 🤖 **Custom gesture classifier** — Train ML models for complex gesture recognition
- 📱 **AR Foundation integration** — Place and move cubes in real-world environments

---

## License

This project is open source. See the repository for licensing details.

---

## Acknowledgments

Built with [MediaPipe Unity Plugin](https://github.com/homuler/MediaPipeUnityPlugin) by homuler, powered by Google's MediaPipe framework.