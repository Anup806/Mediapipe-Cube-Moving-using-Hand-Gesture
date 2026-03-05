# Mediapipe Cube Moving using Hand Gesture

> **Portfolio Project** — A real-time interactive Unity application that uses **MediaPipe Unity Plugin** and computer vision to enable **touchless 3D cube manipulation** using hand gestures.

[![Unity](https://img.shields.io/badge/Unity-2021.3_LTS-black.svg)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-9.0-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![MediaPipe](https://img.shields.io/badge/MediaPipe-0.14.4-orange.svg)](https://mediapipe.dev/)

---

## 📋 Table of Contents

- [Overview](#overview)
- [Project Overview](#project-overview)
- [Demo Description](#demo-description)
- [Key Features](#key-features)
- [Technologies Used](#technologies-used)
- [System Workflow](#system-workflow)
- [Gesture Reference](#gesture-reference)
- [Implementation Details](#implementation-details)
- [Use Cases](#use-cases)
- [Future Enhancements](#future-enhancements)
- [License](#license)

---

## Overview

This project demonstrates real-time **gesture-driven 3D object manipulation** in Unity, combining computer vision and machine learning-based hand tracking. The system enables users to control a 3D cube naturally using hand gestures—**no controllers, no touchscreen, just hands**.

The primary functionality allows intuitive **grab-and-drag interaction** using a two-finger pinch gesture, with hand movements translated 1:1 to cube position in real-time. Extended features include two-hand scaling, three-finger rotation, visual feedback systems, and mobile VR support.

---

## Project Overview

This application showcases **touchless gesture-driven 3D cube movement** powered by MediaPipe's hand tracking pipeline and Unity's rendering engine. The core interaction paradigm enables users to:

1. **Grab a 3D cube** using a natural two-finger pinch gesture
2. **Move it freely** across the screen with real-time position mapping
3. **Scale and rotate** using multi-finger and multi-hand gestures
4. **Receive visual and haptic feedback** during all interactions

Beyond the primary cube-moving capability, the project implements comprehensive 3D manipulation including scaling, rotation, visual state indicators, and cross-platform mobile VR support—making it a versatile prototype for AR/VR gesture-based interaction systems.

---

## Demo Description

When the application runs, the device camera feed is processed every frame by MediaPipe's hand-tracking pipeline. A colored 3D cube appears in the scene. Users perform gestures in front of the camera to interact:

### Core Feature: Cube Moving
- 🎯 **Two-finger pinch near the cube** — Grabs the cube and enables **real-time drag movement**, following hand position precisely
- A cyan **trail renderer** follows the cube during movement
- The cube turns **green** when grabbed (visual feedback)

### Extended Manipulation Features
- 🔍 **Two-hand pinch (both hands)** — Scales the cube proportionally (spread hands = bigger, pinch = smaller)
- 🔄 **Three-finger pinch + hand movement** — Rotates the cube on X and Y axes simultaneously
- ✨ **Hover near cube** (without pinching) — Triggers a **yellow glow pulse** effect
- 📳 **Haptic shake feedback** — Brief shake animation when grabbing or starting scale gesture

### Real-time HUD Overlay
An on-screen diagnostic display shows:
- Current gesture name and state
- Cube position (X, Y, Z)
- Cube scale and rotation values
- Live FPS counter

---

## Key Features

### Computer Vision & Gesture Recognition
- ✅ **Real-time hand landmark detection** via MediaPipe Unity Plugin (21 3D landmarks per hand)
- ✅ **Multi-finger pinch detection** — Two-finger and three-finger gesture classification
- ✅ **Distance-based gesture triggering** — Threshold-based pinch detection using landmark Euclidean distance
- ✅ **Proximity-based interaction** — Grab radius determines valid interaction zone around cube

### Primary Interaction (Cube Movement)
- ✅ **Grab & Drag** — Intuitive two-finger pinch to move the cube anywhere on screen
- ✅ **Real-time position mapping** — Hand landmark coordinates mapped to Unity world space
- ✅ **Trail Effect** — Cyan motion trail visualization during movement (Unity TrailRenderer)
- ✅ **Visual state feedback** — Color-coded cube states:
  - **Cyan** (normal/idle)
  - **Yellow** (hover)
  - **Green** (grabbed/moving)
  - **Orange** (rotating)
  - **Magenta** (scaling)

### Advanced Manipulation
- ✅ **Two-hand scaling** — Both hands pinching scales the cube proportionally (min 0.2x – max 3x)
- ✅ **Three-finger rotation** — Three-finger pinch controls X-axis and Y-axis rotation simultaneously
- ✅ **Haptic shake feedback** — Physical feedback animation on grab/scale events
- ✅ **Visual glow effect** — Pulsing shader effect when hovering near the cube
- ✅ **Configurable parameters** — Pinch threshold, grab radius, scale limits, rotation speed exposed in Inspector

### Platform & Mobile Support
- ✅ **Cross-platform compatibility** — Windows, macOS, Android, iOS
- ✅ **Mobile VR mode** — Split-screen stereoscopic rendering for Google Cardboard
- ✅ **Gyroscope head tracking** — Gyro-driven camera rotation on mobile devices
- ✅ **Android camera permission handler** — Automatic runtime permission request system
- ✅ **On-screen HUD** — Live gesture name, position, scale, rotation, and FPS display

---

## Technologies Used

| Technology | Version | Role |
|-----------|---------|------|
| **Unity** | 2021.3 LTS | Game engine, rendering pipeline, scene management |
| **C#** | 9.0 | Scripting language for all game logic and gesture detection |
| **MediaPipe Unity Plugin** | 0.14.4 | Hand landmark detection via ML-based tracking pipeline |
| **MediaPipe Hands Model** | — | 21-point 3D hand landmark estimation (thumb, fingers, wrist) |
| **Unity TrailRenderer** | — | Visual trail effect during cube movement |
| **Unity Gyroscope API** | — | Mobile head tracking for VR mode |
| **Android/iOS Camera API** | — | Runtime camera access and permission handling |

---

## System Workflow

```
┌──────────────────────────────────────────────────────────────────────┐
│  1. Camera Feed                                                      │
│     Device camera captured every frame by MediaPipe solution         │
├──────────────────────────────────────────────────────────────────────┤
│  2. Hand Landmark Detection                                          │
│     MediaPipe Hands model infers 21 3D landmarks per hand            │
│     • Wrist (0), Thumb (1–4), Index (5–8), Middle (9–12), etc.       │
├──────────────────────────────────────────────────────────────────────┤
│  3. Gesture Classification                                           │
│     HandGestureController.cs reads landmark coordinates:             │
│     • Computes Euclidean distance between:                           │
│       - Thumb tip (landmark 4) and Index tip (landmark 8)            │
│       - Thumb tip and Middle tip (landmark 12)                       │
│     • Distance < pinchThreshold → PINCH DETECTED                     │
├──────────────────────────────────────────────────────────────────────┤
│  4. Cube Movement & Transform Mapping                                │
│     PRIMARY FEATURE: 2-finger pinch near cube                        │
│     → Grab & Move (translate cube in world space)                    │
│     • Hand position mapped from camera space to screen space         │
│     • Cube follows hand movement in real-time via Transform.position │
│                                                                      │
│     EXTENDED FEATURES:                                               │
│     • 2-hand pinch → Scale (min 0.2x – max 3x)                       │
│     • 3-finger pinch + movement → Rotate (X & Y axes)                │
├──────────────────────────────────────────────────────────────────────┤
│  5. Visual & Haptic Feedback                                         │
│     • Color changes (Material.color)                                 │
│     • Trail rendering (TrailRenderer component)                      │
│     • Glow effect (Emission shader property)                         │
│     • Shake animation (Transform.localScale oscillation)             │
├──────────────────────────────────────────────────────────────────────┤
│  6. HUD Overlay Rendering                                            │
│     OnGUI() renders diagnostic information:                          │
│     • Gesture name, cube position, scale, rotation, FPS              │
└──────────────────────────────────────────────────────────────────────┘
```

### Gesture Detection Algorithm

```csharp
// Simplified pseudocode
float thumbIndexDistance = Vector3.Distance(thumbTip, indexTip);
float thumbMiddleDistance = Vector3.Distance(thumbTip, middleTip);

if (thumbIndexDistance < pinchThreshold && thumbMiddleDistance > pinchThreshold) {
    // Two-finger pinch detected
    if (DistanceToCube(handPosition) < grabRadius) {
        GrabCube();
        MoveCube(handPosition);
    }
} else if (thumbIndexDistance < pinchThreshold && thumbMiddleDistance < pinchThreshold) {
    // Three-finger pinch detected
    RotateCube(handMovementDelta);
}
```

---

## Gesture Reference

This table shows all supported gestures and their corresponding actions. **Recruiters:** This demonstrates the gesture recognition system's capabilities.

| Gesture | Action | Priority | Visual Feedback |
|---------|--------|----------|-----------------|
| ✌️ **Two-finger pinch** (near cube) | **Grab and move the cube** — drag in real-time | **PRIMARY** | Green cube + cyan trail |
| 🖐️ **Open hand near cube** | Hover — proximity detection | Secondary | Yellow glow pulse |
| 🤏🤏 **Both hands pinching** | Scale cube (spread = bigger, pinch = smaller) | Extended | Magenta cube + shake feedback |
| 🤟 **Three-finger pinch** + move left/right | Rotate cube around Y-axis | Extended | Orange cube |
| 🤟 **Three-finger pinch** + move up/down | Rotate cube around X-axis | Extended | Orange cube |

### Gesture Detection Parameters

| Parameter | Default Value | Description |
|-----------|---------------|-------------|
| **pinchThreshold** | 0.08 | Normalized distance at which a pinch is detected |
| **grabRadius** | 3.0 | Screen-space radius (×100 px) within which pinch grabs the cube |
| **minScale** / **maxScale** | 0.2 / 3.0 | Scale limits during two-hand scaling |
| **rotationSpeed** | 150 °/s | Rotation speed during three-finger gesture |

---

## Implementation Details

### Core Architecture

The project is built around a single main controller script (`HandGestureController.cs`) that:

1. **Interfaces with MediaPipe** — Receives hand landmark data every frame
2. **Processes gesture logic** — Calculates distances, detects pinches, classifies gestures
3. **Manipulates cube transforms** — Updates position, rotation, scale based on hand input
4. **Manages visual feedback** — Changes materials, enables/disables effects
5. **Renders diagnostic HUD** — Displays real-time interaction data

### Hand Landmark Coordinate System

MediaPipe outputs 21 landmarks per hand in normalized coordinates (0–1 range):

- **Landmark 0:** Wrist
- **Landmarks 1–4:** Thumb (CMC, MCP, IP, TIP)
- **Landmarks 5–8:** Index finger (MCP, PIP, DIP, TIP)
- **Landmarks 9–12:** Middle finger (MCP, PIP, DIP, TIP)
- **Landmarks 13–20:** Ring and pinky fingers

The controller converts these normalized coordinates to Unity world space for interaction.

### Visual Feedback System

| State | Color | Effect | Trigger Condition |
|-------|-------|--------|-------------------|
| **Idle** | Cyan | None | No hand detected or hand far from cube |
| **Hover** | Yellow | Pulsing glow | Open hand within grab radius |
| **Grabbed** | Green | Trail renderer active | Two-finger pinch + within grab radius |
| **Scaling** | Magenta | Shake animation | Both hands pinching |
| **Rotating** | Orange | None | Three-finger pinch + hand movement |

---

## Use Cases

### Primary Application: Gesture-Based Interaction Research
- **Touchless interaction prototyping** — Control 3D objects without physical contact
- **Accessibility interface design** — Alternative input method for users with limited mobility
- **Interactive demonstrations** — Exhibition installations with natural hand-based controls
- **Computer vision education** — Teaching gesture recognition and hand tracking concepts

### Extended Applications

#### 🎮 **Game Development**
- Puzzle games with gesture-based mechanics
- Physics-based object manipulation challenges
- VR hand interaction prototypes

#### 🥽 **AR/VR Systems**
- Foundation for immersive hand-tracking experiences
- Touchless UI navigation in mixed reality
- Virtual object manipulation in 3D space

#### 🎨 **Creative Tools**
- 3D modeling interfaces with gesture controls
- Spatial design applications
- Interactive art installations

#### 🏥 **Specialized Environments**
- Medical applications requiring sterile input
- Industrial control systems (hands-free operation)
- Assistive technology for motor-impaired users

---

## Future Enhancements

### Core Movement Improvements
- 🎯 **Z-axis depth control** — Full 3D space navigation using hand depth from camera
- 🖐️ **Multi-object selection** — Simultaneous manipulation of multiple cubes
- ⚡ **Kalman filtering** — Gesture smoothing for more stable movement
- 🎮 **Custom gesture mapping** — User-defined gesture-to-action bindings

### Advanced Features
- 🎲 **Physics-based interaction** — Apply forces instead of direct position control
- 🖐️ **Extended gesture vocabulary** — Swipe-to-throw, two-finger twist, fist gestures
- 🥽 **OpenXR integration** — Port to Meta Quest, HoloLens 2, or Vision Pro
- 🌐 **Networked multi-user** — Synchronize cube state across devices (Unity Netcode/Mirror)
- 📊 **Gesture recording system** — Record and replay hand landmark sequences
- 🤖 **ML gesture classifier** — Train custom TensorFlow models for complex gestures
- 📱 **AR Foundation support** — Place and manipulate cubes in real-world environments via smartphone AR

### Platform Extensions
- **WebGL build** — Browser-based demo using MediaPipe Web
- **Cross-platform ML optimization** — ONNX model export for better performance
- **Hand occlusion handling** — Robust tracking when hands overlap or partially visible

---

## License

**Copyright © 2026 Anup806. All Rights Reserved.**

### Usage Restrictions

This software and associated documentation are provided for **portfolio demonstration and professional evaluation purposes only**.

**Prohibited without explicit written permission:**
- ❌ Copying, reproducing, or distributing this code
- ❌ Modifying or creating derivative works
- ❌ Using this software in commercial or personal projects
- ❌ Forking this repository for purposes other than evaluation

**Permitted actions:**
- ✅ Viewing the code and documentation for assessment purposes
- ✅ Evaluating this work for hiring, academic review, or research citation

### Intellectual Property Notice

This project contains proprietary implementations of:
- Custom gesture detection algorithms and threshold tuning
- Multi-hand interaction state machine logic
- Visual feedback system architecture
- Mobile VR and gyroscope integration patterns

All code, algorithms, UI designs, and documentation are protected by copyright law.

### Contact for Permissions

For inquiries regarding licensing, collaboration, or academic citation:

- 📧 **Email:** raianup806@gmail.com
- 💼 **LinkedIn:** [Anup Rai](https://www.linkedin.com/in/anup-rai-095695343/)

---

**Project Status:** Active Development | Portfolio Demonstration  
**Repository:** [Anup806/Mediapipe-Cube-Moving-using-Hand-Gesture](https://github.com/Anup806/Mediapipe-Cube-Moving-using-Hand-Gesture)

---

### Acknowledgments

Built with [MediaPipe Unity Plugin](https://github.com/homuler/MediaPipeUnityPlugin) by homuler, powered by Google's MediaPipe framework.
