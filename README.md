# Mediapipe Cube Moving using Hand Gesture

A real-time interactive Unity application that uses the **MediaPipe Unity Plugin** and your device's camera to detect hand landmarks and translate finger gestures into intuitive 3D cube manipulation — grab, drag, scale, and rotate a virtual cube using only your hands.

---

## Project Overview

This project demonstrates how computer vision and machine learning-based hand tracking can be used to create a touchless, gesture-driven 3D interaction experience inside Unity. The application captures a live camera feed, passes each frame to the MediaPipe Hands model, extracts 21 hand landmarks per hand, and maps specific pinch gestures to cube transformations in 3D space.

The project also supports **mobile VR** (Google Cardboard-style split-screen), gyroscope-based head tracking, and an Android camera permission workflow, making it suitable as a prototype for AR/VR gesture interfaces.

---

## Demo Description

When the application runs, the device camera feed is processed every frame by MediaPipe's hand-tracking pipeline. A coloured 3D cube is rendered in the scene. As you perform gestures in front of the camera:

- Pinching two fingers close to the cube **grabs** it and lets you drag it across the screen in real time.
- Spreading or closing both pinching hands **scales** the cube up or down.
- Forming a three-finger pinch and moving your hand **rotates** the cube on its X and Y axes.
- Hovering near the cube without pinching triggers a **yellow glow pulse** effect.
- Every grab or scale-start produces a brief **haptic shake** on the cube for tactile feedback.
- A cyan **trail** follows the cube as you drag it.
- An on-screen HUD displays the current gesture, cube position, scale, rotation, and live FPS.

---

## Features

- ✅ **Real-time hand landmark detection** via MediaPipe Unity Plugin (21 landmarks per hand)
- ✅ **Grab & Drag** — two-finger pinch near the cube moves it 1:1 with hand movement
- ✅ **Two-hand Scaling** — both hands pinching scales the cube proportionally
- ✅ **Three-finger Rotation** — three-finger pinch controls X-axis (up/down tilt) and Y-axis (left/right spin) rotation simultaneously
- ✅ **Trail Effect** — cyan motion trail while the cube is being dragged
- ✅ **Haptic Shake Feedback** — brief shake animation on grab/scale start
- ✅ **Visual Glow Effect** — pulsing colour when hovering near the cube
- ✅ **Colour-coded state** — Normal (cyan), Hover (yellow), Grabbed (green), Rotating (orange), Scaling (magenta)
- ✅ **On-screen HUD** — live gesture name, position, scale, rotation, and FPS
- ✅ **Mobile VR Mode** — split-screen stereoscopic rendering for Google Cardboard
- ✅ **Gyroscope Head Tracking** — gyro-driven camera rotation on mobile
- ✅ **Android Camera Permission Handler** — runtime permission request on Android

---

## Technologies Used

| Technology | Role |
|---|---|
| **Unity 2021.3 LTS** | Game engine / rendering / scene management |
| **C#** | Scripting language for all game logic |
| **MediaPipe Unity Plugin** (`com.github.homuler.mediapipe` v0.14.4) | Hand landmark detection via ML pipeline |
| **MediaPipe Hands Model** | 21-point 3D hand landmark estimation |
| **Unity TrailRenderer** | Visual trail effect on cube movement |
| **Unity Gyroscope API** | Head tracking on mobile devices |
| **Android / iOS Camera API** | Runtime camera access |

---

## System Workflow

```
┌─────────────────────────────────────────────────────────────────┐
│  1. Camera Feed                                                  │
│     Device camera captured every frame by MediaPipe solution    │
├─────────────────────────────────────────────────────────────────┤
│  2. Hand Landmark Detection                                      │
│     MediaPipe Hands model infers 21 3D landmarks per hand       │
├─────────────────────────────────────────────────────────────────┤
│  3. Gesture Classification                                       │
│     HandGestureController reads landmark coordinates:           │
│     • Thumb tip (4), Index tip (8), Middle tip (12), Wrist (0)  │
│     • Computes inter-landmark distances to detect pinch state   │
├─────────────────────────────────────────────────────────────────┤
│  4. Gesture → Cube Transform Mapping                            │
│     • 2-finger pinch near cube  → Grab & Drag (translate)       │
│     • 2-hand pinch              → Scale (min 0.2x – max 3x)     │
│     • 3-finger pinch + movement → Rotate (X & Y axes)           │
├─────────────────────────────────────────────────────────────────┤
│  5. Visual & Haptic Feedback                                     │
│     Colour, glow, trail, and shake effects applied each frame   │
├─────────────────────────────────────────────────────────────────┤
│  6. HUD Overlay                                                  │
│     OnGUI renders gesture name, position, scale, rotation, FPS  │
└─────────────────────────────────────────────────────────────────┘
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
4. Place your hand in front of the camera and perform gestures.

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

| Gesture | Action |
|---|---|
| ✌️ Two-finger pinch (near cube) | Grab the cube and drag it |
| 🤏🤏 Both hands pinching | Scale the cube (spread = bigger, pinch = smaller) |
| 🤟 Three-finger pinch + move hand left/right | Rotate cube around Y-axis |
| 🤟 Three-finger pinch + move hand up/down | Rotate cube around X-axis |
| 🖐️ Open hand near cube | Hover — triggers yellow glow effect |

---

## Folder Structure

```
Mediapipe-Cube-Moving-using-Hand-Gesture/
├── Assets/
│   ├── scripts/
│   │   └── HandGestureController.cs   # Core gesture detection & cube manipulation logic
│   ├── Scenes/
│   │   └── SampleScene.unity          # Main Unity scene
│   ├── Material/
│   │   └── CubeMaterial.mat           # Material applied to the interactive cube
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

| Parameter | Default | Description |
|---|---|---|
| `pinchThreshold` | 0.08 | Normalised distance at which a pinch is detected |
| `grabRadius` | 3.0 | Screen-space radius (×100 px) within which pinch grabs the cube |
| `minScale` / `maxScale` | 0.2 / 3.0 | Scale limits during two-hand scaling |
| `scaleSpeed` | 3.0 | Lerp speed for scale transitions |
| `rotationSpeed` | 150 °/s | Rotation speed during three-finger gesture |
| `enableRotationX/Y` | true | Toggle individual rotation axes |
| `enableTrail` | true | Enable/disable motion trail |
| `enableHapticFeedback` | true | Enable/disable shake-on-grab effect |
| `enableGlowEffect` | true | Enable/disable hover glow pulse |
| `showUI` | true | Show/hide the on-screen HUD |

---

## Possible Improvements

- 🎲 **Full 3D object interaction** — extend gestures to manipulate multiple objects or complex meshes
- 🖐️ **Additional gestures** — swipe-to-throw, two-finger twist, or custom sign-language commands
- 🥽 **VR/MR integration** — port to OpenXR or Meta Quest for fully immersive hand interaction
- 🎮 **Game mechanics** — build a puzzle or physics game controlled entirely by hand gestures
- 🌐 **Multi-user support** — synchronise cube state across devices via Unity Netcode
- 📊 **Gesture recording & playback** — record hand landmark sequences for animation
- 🤖 **Custom gesture classifier** — train a neural network on top of landmark data for complex gesture sets
- 📱 **AR overlay** — integrate with AR Foundation to place the cube in a real-world environment

---

## License

This project is open source. See the repository for licensing details.
