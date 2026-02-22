using System. Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

/// <summary>
/// Hand Gesture Controller - COMPLETE ENHANCED VERSION
/// 
/// Features:
/// ✅ Grab & Drag (1: 1 movement)
/// ✅ Two-hand Scaling
/// ✅ Three-finger Rotation (X + Y axes)
/// ✅ Trail Effect
/// ✅ Haptic Shake Feedback
/// ✅ Visual Glow Effect
/// ✅ UI Display
/// ✅ Performance Optimized
/// 
/// Gestures: 
/// - Two-finger pinch near cube → Grab & Drag
/// - Two hands pinch → Scale
/// - Three-finger pinch + move → Rotate (X & Y axes)
/// </summary>
public class HandGestureController : MonoBehaviour
{
    [Header("=== REFERENCES ===")]
    public GameObject solutionGameObject;
    public Transform targetCube;
    
    [Header("=== PINCH DETECTION ===")]
    [Range(0.02f, 0.15f)]
    public float pinchThreshold = 0.08f;
    
    [Header("=== GRAB SETTINGS ===")]
    [Range(0.5f, 10f)]
    [Tooltip("How close to cube you must pinch to grab it")]
    public float grabRadius = 3f;
    
    [Header("=== SCALING SETTINGS ===")]
    [Range(0.1f, 1f)]
    public float minScale = 0.2f;
    
    [Range(1f, 5f)]
    public float maxScale = 3f;
    
    [Range(1f, 10f)]
    public float scaleSpeed = 3f;
    
    [Header("=== ROTATION SETTINGS ===")]
    [Tooltip("Enable three-finger rotation")]
    public bool enableRotation = true;
    
    [Range(50f, 300f)]
    public float rotationSpeed = 150f;
    
    [Tooltip("Enable X-axis rotation (up/down tilt)")]
    public bool enableRotationX = true;
    
    [Tooltip("Enable Y-axis rotation (left/right spin)")]
    public bool enableRotationY = true;
    
    [Header("=== TRAIL EFFECT ===")]
    [Tooltip("Show trail when dragging cube")]
    public bool enableTrail = true;
    
    public Color trailStartColor = Color.cyan;
    public Color trailEndColor = new Color(0f, 1f, 1f, 0f);
    
    [Range(0.1f, 2f)]
    public float trailTime = 0.5f;
    
    [Range(0.05f, 0.5f)]
    public float trailStartWidth = 0.2f;
    
    [Range(0.01f, 0.2f)]
    public float trailEndWidth = 0.05f;
    
    [Header("=== HAPTIC FEEDBACK ===")]
    [Tooltip("Shake effect when grabbing")]
    public bool enableHapticFeedback = true;
    
    [Range(0.01f, 0.1f)]
    public float shakeIntensity = 0.03f;
    
    [Range(0.1f, 0.5f)]
    public float shakeDuration = 0.2f;
    
    [Header("=== VISUAL FEEDBACK ===")]
    [Tooltip("Glow/pulse effect when hovering")]
    public bool enableGlowEffect = true;
    
    [Range(1f, 10f)]
    public float glowSpeed = 5f;
    
    [Range(0.1f, 0.5f)]
    public float glowIntensity = 0.3f;
    
    [Header("=== COLORS ===")]
    public Color normalColor = Color.cyan;
    public Color hoverColor = Color.yellow;
    public Color grabbedColor = Color. green;
    public Color rotatingColor = new Color(1f, 0.5f, 0f); // Orange
    public Color twoHandColor = Color.magenta;
    
    [Header("=== UI DISPLAY ===")]
    public bool showUI = true;
    public bool showDebugLogs = false;
    
    // Private - Camera & Rendering
    private Camera mainCamera;
    private Renderer cubeRenderer;
    private Material cubeMaterial;
    private TrailRenderer trailRenderer;
    private Vector3 originalScale;
    
    // Private - Landmark indices
    private const int WRIST = 0;
    private const int THUMB_TIP = 4;
    private const int INDEX_TIP = 8;
    private const int MIDDLE_TIP = 12;
    
    // Private - MediaPipe access
    private Component handTrackingSolution;
    private object annotationController;
    private FieldInfo landmarkDataField;
    private PropertyInfo landmarkProperty;
    private bool isInitialized = false;
    
    // Private - State tracking
    private bool isGrabbed = false;
    private bool isHovering = false;
    private bool isRotating = false;
    private bool isTwoHandPinching = false;
    private float initialTwoHandDistance = 0f;
    private Vector3 initialCubeScale;
    
    // Private - Movement tracking
    private Vector3 lastHandScreenPos;
    private bool hasLastPos = false;
    
    // Private - Haptic feedback
    private float shakeTimer = 0f;
    private bool isShaking = false;
    
    // Private - Performance optimization (cached properties)
    private Dictionary<Type, Dictionary<string, PropertyInfo>> propertyCache = 
        new Dictionary<Type, Dictionary<string, PropertyInfo>>();
    
    // Private - Stats for UI
    private int frameCount = 0;
    private float fps = 0f;
    private float fpsTimer = 0f;
    private string currentGesture = "None";

    // ═══════════════════════════════════════════════════════════════════
    // START
    // ═══════════════════════════════════════════════════════════════════
    void Start()
    {
        // Validate references
        if (solutionGameObject == null)
        {
            Debug.LogError("❌ Solution GameObject not assigned!");
            enabled = false;
            return;
        }
        
        if (targetCube == null)
        {
            Debug.LogError("❌ Target Cube not assigned!");
            enabled = false;
            return;
        }
        
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("❌ Main Camera not found!");
            enabled = false;
            return;
        }
        
        // Setup cube
        originalScale = targetCube.localScale;
        initialCubeScale = originalScale;
        
        cubeRenderer = targetCube.GetComponent<Renderer>();
        if (cubeRenderer != null)
        {
            cubeMaterial = cubeRenderer.material;
            cubeMaterial.color = normalColor;
        }
        
        // Setup trail
        SetupTrailRenderer();
        
        // Disable annotations (remove rectangles)
        DisableAnnotations();
        
        // Initialize landmark access
        InitializeLandmarkAccess();
        
        if (isInitialized)
        {
            Debug.Log("═══════════════════════════════════════════");
            Debug.Log("✅ Enhanced HandGestureController ready!");
            Debug. Log("═══════════════════════════════════════════");
            Debug.Log("📋 Features enabled:");
            Debug. Log($"   • Rotation: {enableRotation} (X:  {enableRotationX}, Y: {enableRotationY})");
            Debug.Log($"   • Trail: {enableTrail}");
            Debug.Log($"   • Haptic:  {enableHapticFeedback}");
            Debug.Log($"   • Glow: {enableGlowEffect}");
            Debug. Log($"   • UI: {showUI}");
            Debug.Log("═══════════════════════════════════════════");
            Debug.Log("🎮 GESTURE GUIDE:");
            Debug. Log("   ✌️ Two-finger pinch near cube → Grab & Drag");
            Debug. Log("   🤏🤏 Both hands pinch → Scale");
            Debug. Log("   🤟 Three-finger pinch + move → Rotate");
            Debug.Log("═══════════════════════════════════════════");
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    // SETUP TRAIL RENDERER
    // ═══════════════════════════════════════════════════════════════════
    void SetupTrailRenderer()
    {
        // Get or create trail renderer
        trailRenderer = targetCube.GetComponent<TrailRenderer>();
        
        if (trailRenderer == null && enableTrail)
        {
            trailRenderer = targetCube.gameObject.AddComponent<TrailRenderer>();
        }
        
        if (trailRenderer != null)
        {
            // Configure trail
            trailRenderer. time = trailTime;
            trailRenderer.startWidth = trailStartWidth;
            trailRenderer. endWidth = trailEndWidth;
            trailRenderer.startColor = trailStartColor;
            trailRenderer.endColor = trailEndColor;
            trailRenderer.autodestruct = false;
            trailRenderer.emitting = false;
            
            // Create material if needed
            if (trailRenderer.material == null || trailRenderer.material.shader. name == "Standard")
            {
                Material trailMat = new Material(Shader.Find("Sprites/Default"));
                trailMat.color = trailStartColor;
                trailRenderer.material = trailMat;
            }
            
            // Set gradient
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { 
                    new GradientColorKey(trailStartColor, 0.0f), 
                    new GradientColorKey(trailEndColor, 1.0f) 
                },
                new GradientAlphaKey[] { 
                    new GradientAlphaKey(1.0f, 0.0f), 
                    new GradientAlphaKey(0.0f, 1.0f) 
                }
            );
            trailRenderer.colorGradient = gradient;
            
            // Set sorting order so trail appears behind cube
            trailRenderer.sortingOrder = -1;
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    // DISABLE ANNOTATIONS (Remove rectangles)
    // ═══════════════════════════════════════════════════════════════════
    void DisableAnnotations()
    {
        if (solutionGameObject == null) return;
        
        Transform solutionTransform = solutionGameObject.transform;
        for (int i = 0; i < solutionTransform. childCount; i++)
        {
            Transform child = solutionTransform.GetChild(i);
            string name = child.name. ToLower();
            if (name.Contains("annotation") || name.Contains("landmark") || 
                name.Contains("rect") || name.Contains("detection"))
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    // INITIALIZE LANDMARK ACCESS
    // ═══════════════════════════════════════════════════════════════════
    void InitializeLandmarkAccess()
    {
        var allComponents = solutionGameObject. GetComponents<Component>();
        
        foreach (var component in allComponents)
        {
            if (component. GetType().Name == "HandTrackingSolution")
            {
                handTrackingSolution = component;
                var solutionType = component.GetType();
                var fields = solutionType.GetFields(BindingFlags. NonPublic | BindingFlags.Public | BindingFlags. Instance);
                
                foreach (var field in fields)
                {
                    if (field.Name. ToLower().Contains("landmark") && field.Name.ToLower().Contains("controller"))
                    {
                        annotationController = field.GetValue(component);
                        
                        if (annotationController != null)
                        {
                            if (FindLandmarkDataField(annotationController))
                            {
                                isInitialized = true;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }

    bool FindLandmarkDataField(object controller)
    {
        var controllerType = controller. GetType();
        var fields = controllerType.GetFields(BindingFlags. NonPublic | BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var field in fields)
        {
            string name = field.Name. ToLower();
            if (name. Contains("target") || name.Contains("current") || name.Contains("landmark"))
            {
                if (typeof(IList).IsAssignableFrom(field.FieldType) || field.FieldType. IsGenericType)
                {
                    landmarkDataField = field;
                    return true;
                }
            }
        }
        return false;
    }

    // ═══════════════════════════════════════════════════════════════════
    // UPDATE (Main Loop)
    // ═══════════════════════════════════════════════════════════════════
    void Update()
    {
        if (! isInitialized || annotationController == null) return;
        
        // FPS calculation
        UpdateFPS();
        
        var handLandmarks = GetHandLandmarks();
        
        if (handLandmarks == null || handLandmarks. Count == 0)
        {
            ResetState();
            UpdateVisuals();
            return;
        }
        
        // Process based on hand count
        if (handLandmarks.Count >= 2)
        {
            ProcessTwoHands(handLandmarks[0], handLandmarks[1]);
        }
        else
        {
            isTwoHandPinching = false;
            initialTwoHandDistance = 0f;
            ProcessSingleHand(handLandmarks[0]);
        }
        
        // Update haptic feedback
        UpdateHapticFeedback();
        
        // Update visuals
        UpdateVisuals();
        
        // Update trail
        UpdateTrail();
    }

    // ═══════════════════════════════════════════════════════════════════
    // FPS CALCULATION
    // ═══════════════════════════════════════════════════════════════════
    void UpdateFPS()
    {
        frameCount++;
        fpsTimer += Time. deltaTime;
        
        if (fpsTimer >= 1f)
        {
            fps = frameCount / fpsTimer;
            frameCount = 0;
            fpsTimer = 0f;
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    // GET HAND LANDMARKS
    // ═══════════════════════════════════════════════════════════════════
    List<object> GetHandLandmarks()
    {
        if (landmarkDataField == null || annotationController == null) return null;
        
        try
        {
            var value = landmarkDataField.GetValue(annotationController);
            if (value != null && value is IList list && list.Count > 0)
            {
                var result = new List<object>();
                foreach (var item in list)
                    result.Add(item);
                return result;
            }
        }
        catch { }
        return null;
    }

    // ═══════════════════════════════════════════════════════════════════
    // PROCESS SINGLE HAND
    // ═══════════════════════════════════════════════════════════════════
    void ProcessSingleHand(object handLandmarks)
    {
        try
        {
            if (landmarkProperty == null)
                landmarkProperty = handLandmarks.GetType().GetProperty("Landmark");
            
            if (landmarkProperty == null) return;
            
            var landmarks = landmarkProperty. GetValue(handLandmarks) as IList;
            if (landmarks == null || landmarks.Count < 21) return;
            
            var thumbTip = landmarks[THUMB_TIP];
            var indexTip = landmarks[INDEX_TIP];
            var middleTip = landmarks[MIDDLE_TIP];
            
            float pinchDist = GetLandmarkDistance(thumbTip, indexTip);
            float threeFingerDist = GetLandmarkDistance(indexTip, middleTip);
            
            bool isPinching = pinchDist < pinchThreshold;
            bool isThreeFingerPinch = isPinching && threeFingerDist < pinchThreshold && enableRotation;
            
            // Get hand screen position (normalized 0-1)
            float handX = (GetLandmarkX(thumbTip) + GetLandmarkX(indexTip)) / 2f;
            float handY = (GetLandmarkY(thumbTip) + GetLandmarkY(indexTip)) / 2f;
            
            // Convert to screen coordinates
            Vector3 handScreenPos = new Vector3(
                handX * Screen.width,
                (1f - handY) * Screen.height,
                0f
            );
            
            // Get cube screen position
            Vector3 cubeWorldPos = targetCube.position;
            Vector3 cubeScreenPos = mainCamera.WorldToScreenPoint(cubeWorldPos);
            
            // Distance in screen pixels
            float screenDist = Vector2.Distance(
                new Vector2(handScreenPos.x, handScreenPos.y),
                new Vector2(cubeScreenPos.x, cubeScreenPos.y)
            );
            
            bool isNearCube = screenDist < (grabRadius * 100f);
            isHovering = isNearCube && ! isGrabbed && !isRotating;
            
            // ═══════════════════════════════════════════════════════════
            // THREE-FINGER ROTATION (X + Y AXES)
            // ═══════════════════════════════════════════════════════════
            if (isThreeFingerPinch && isNearCube)
            {
                isRotating = true;
                isGrabbed = false;
                hasLastPos = false;
                currentGesture = "Rotating";
                
                // Calculate rotation on BOTH axes
                // Hand X position (0-1) controls Y-axis rotation (horizontal spin)
                // Hand Y position (0-1) controls X-axis rotation (vertical tilt)
                
                float rotationY = 0f;
                float rotationX = 0f;
                
                if (enableRotationY)
                {
                    rotationY = (handX - 0.5f) * rotationSpeed * Time.deltaTime;
                }
                
                if (enableRotationX)
                {
                    rotationX = (handY - 0.5f) * rotationSpeed * Time.deltaTime;
                }
                
                // Apply both rotations in world space
                targetCube. Rotate(rotationX, rotationY, 0f, Space.World);
                
                if (showDebugLogs)
                {
                    Debug. Log($"🔄 Rotating:  X={rotationX: F2}°, Y={rotationY:F2}°");
                }
            }
            // ═══════════════════════════════════════════════════════════
            // TWO-FINGER PINCH (GRAB & DRAG)
            // ═══════════════════════════════════════════════════════════
            else if (isPinching && ! isThreeFingerPinch)
            {
                isRotating = false;
                
                if (isGrabbed)
                {
                    currentGesture = "Dragging";
                    
                    if (hasLastPos)
                    {
                        // Calculate screen delta
                        Vector3 screenDelta = handScreenPos - lastHandScreenPos;
                        
                        // Convert to world movement
                        Vector3 currentWorldPos = mainCamera. ScreenToWorldPoint(
                            new Vector3(cubeScreenPos.x, cubeScreenPos.y, cubeScreenPos.z)
                        );
                        Vector3 newWorldPos = mainCamera.ScreenToWorldPoint(
                            new Vector3(cubeScreenPos.x + screenDelta.x, cubeScreenPos.y + screenDelta.y, cubeScreenPos.z)
                        );
                        
                        Vector3 worldDelta = newWorldPos - currentWorldPos;
                        
                        // Apply movement directly (1:1)
                        targetCube.position += worldDelta;
                        
                        if (showDebugLogs)
                        {
                            Debug.Log($"✊ Dragging | Delta: ({screenDelta.x:F1}, {screenDelta.y:F1})");
                        }
                    }
                }
                else if (isNearCube)
                {
                    // Start grab
                    isGrabbed = true;
                    StartHapticFeedback();
                    currentGesture = "Grabbed";
                    
                    if (showDebugLogs)
                    {
                        Debug.Log($"✅ GRABBED!");
                    }
                }
                else
                {
                    currentGesture = "Pinching (far)";
                    
                    if (showDebugLogs)
                    {
                        Debug.Log($"⚠️ Too far to grab!  Distance: {screenDist:F0}px");
                    }
                }
                
                // Store position for next frame
                lastHandScreenPos = handScreenPos;
                hasLastPos = true;
            }
            // ═══════════════════════════════════════════════════════════
            // NOT PINCHING
            // ═══════════════════════════════════════════════════════════
            else
            {
                if (isGrabbed)
                {
                    if (showDebugLogs)
                    {
                        Debug.Log($"👋 RELEASED!");
                    }
                }
                
                isGrabbed = false;
                isRotating = false;
                hasLastPos = false;
                
                currentGesture = isHovering ? "Hovering" : "None";
            }
        }
        catch (Exception e)
        {
            if (showDebugLogs)
                Debug.LogWarning($"⚠️ Error:  {e.Message}");
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    // PROCESS TWO HANDS (SCALING)
    // ═══════════════════════════════════════════════════════════════════
    void ProcessTwoHands(object hand1, object hand2)
    {
        try
        {
            if (landmarkProperty == null) return;
            
            var landmarks1 = landmarkProperty.GetValue(hand1) as IList;
            var landmarks2 = landmarkProperty.GetValue(hand2) as IList;
            
            if (landmarks1 == null || landmarks2 == null) return;
            if (landmarks1.Count < 21 || landmarks2.Count < 21) return;
            
            var thumb1 = landmarks1[THUMB_TIP];
            var index1 = landmarks1[INDEX_TIP];
            var thumb2 = landmarks2[THUMB_TIP];
            var index2 = landmarks2[INDEX_TIP];
            
            float dist1 = GetLandmarkDistance(thumb1, index1);
            float dist2 = GetLandmarkDistance(thumb2, index2);
            
            bool hand1Pinching = dist1 < pinchThreshold;
            bool hand2Pinching = dist2 < pinchThreshold;
            
            if (hand1Pinching && hand2Pinching)
            {
                // BOTH hands pinching - SCALE mode
                isGrabbed = false;
                isRotating = false;
                hasLastPos = false;
                isTwoHandPinching = true;
                currentGesture = "Scaling";
                
                var wrist1 = landmarks1[WRIST];
                var wrist2 = landmarks2[WRIST];
                float currentDist = GetLandmarkDistance(wrist1, wrist2);
                
                if (initialTwoHandDistance == 0f)
                {
                    // First frame of two-hand pinch
                    initialTwoHandDistance = currentDist;
                    initialCubeScale = targetCube.localScale;
                    StartHapticFeedback();
                    
                    if (showDebugLogs)
                    {
                        Debug.Log($"🤌 SCALING START!");
                    }
                }
                
                // Calculate scale ratio
                float ratio = currentDist / initialTwoHandDistance;
                Vector3 newScale = initialCubeScale * ratio;
                
                // Clamp scale
                float clampedScale = Mathf.Clamp(newScale. x, minScale, maxScale);
                newScale = Vector3.one * clampedScale;
                
                // Apply scale smoothly
                targetCube.localScale = Vector3.Lerp(targetCube.localScale, newScale, Time.deltaTime * scaleSpeed);
                
                // Rotate for visual feedback
                targetCube.Rotate(Vector3.up, Time.deltaTime * 30f);
                
                if (showDebugLogs)
                {
                    Debug. Log($"🤌 Scaling:  {clampedScale:F2}");
                }
            }
            else
            {
                // Not both pinching - check for single hand gestures
                isTwoHandPinching = false;
                initialTwoHandDistance = 0f;
                
                if (hand1Pinching)
                {
                    ProcessSingleHand(hand1);
                }
                else if (hand2Pinching)
                {
                    ProcessSingleHand(hand2);
                }
                else
                {
                    isGrabbed = false;
                    isRotating = false;
                    hasLastPos = false;
                    currentGesture = "None";
                }
            }
        }
        catch (Exception e)
        {
            if (showDebugLogs)
                Debug. LogWarning($"⚠️ Error: {e. Message}");
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    // HAPTIC FEEDBACK (Shake Effect)
    // ═══════════════════════════════════════════════════════════════════
    void StartHapticFeedback()
    {
        if (enableHapticFeedback)
        {
            isShaking = true;
            shakeTimer = shakeDuration;
        }
    }

    void UpdateHapticFeedback()
    {
        if (! enableHapticFeedback || !isShaking) return;
        
        if (shakeTimer > 0f)
        {
            // Apply shake
            Vector3 shake = new Vector3(
                UnityEngine.Random. Range(-shakeIntensity, shakeIntensity),
                UnityEngine. Random.Range(-shakeIntensity, shakeIntensity),
                UnityEngine.Random.Range(-shakeIntensity, shakeIntensity)
            );
            
            // Decrease intensity over time
            float intensity = shakeTimer / shakeDuration;
            targetCube.position += shake * intensity;
            
            shakeTimer -= Time. deltaTime;
        }
        else
        {
            isShaking = false;
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    // VISUAL FEEDBACK (Colors & Glow)
    // ═══════════════════════════════════════════════════════════════════
    void UpdateVisuals()
    {
        if (cubeMaterial == null) return;
        
        Color targetColor = normalColor;
        
        if (isTwoHandPinching)
        {
            targetColor = twoHandColor;
        }
        else if (isRotating)
        {
            targetColor = rotatingColor;
        }
        else if (isGrabbed)
        {
            targetColor = grabbedColor;
        }
        else if (isHovering && enableGlowEffect)
        {
            // Pulsing glow effect
            float pulse = Mathf. Sin(Time.time * glowSpeed) * glowIntensity + 1f;
            targetColor = hoverColor * pulse;
        }
        else if (isHovering)
        {
            targetColor = hoverColor;
        }
        
        // Smooth color transition
        cubeMaterial.color = Color.Lerp(cubeMaterial.color, targetColor, Time. deltaTime * 10f);
    }

    // ═══════════════════════════════════════════════════════════════════
    // TRAIL EFFECT
    // ═══════════════════════════════════════════════════════════════════
    void UpdateTrail()
    {
        if (trailRenderer == null || ! enableTrail) return;
        
        // Enable trail when dragging
        trailRenderer.emitting = isGrabbed;
    }

    // ══════════════════════════════════════════════════════════════════���
    // RESET STATE
    // ═══════════════════════════════════════════════════════════════════
    void ResetState()
    {
        if (isGrabbed && showDebugLogs)
        {
            Debug.Log($"👋 RELEASED (no hands)!");
        }
        
        isGrabbed = false;
        isRotating = false;
        isHovering = false;
        isTwoHandPinching = false;
        hasLastPos = false;
        initialTwoHandDistance = 0f;
        currentGesture = "No hands";
        
        if (cubeMaterial != null)
        {
            cubeMaterial.color = Color. Lerp(cubeMaterial.color, normalColor, Time.deltaTime * 5f);
        }
        
        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    // PERFORMANCE OPTIMIZED PROPERTY ACCESS
    // ═══════════════════════════════════════════════════════════════════
    PropertyInfo GetCachedProperty(object obj, string propertyName)
    {
        Type type = obj.GetType();
        
        if (! propertyCache.ContainsKey(type))
        {
            propertyCache[type] = new Dictionary<string, PropertyInfo>();
        }
        
        if (!propertyCache[type].ContainsKey(propertyName))
        {
            propertyCache[type][propertyName] = type.GetProperty(propertyName);
        }
        
        return propertyCache[type][propertyName];
    }

    float GetLandmarkX(object landmark)
    {
        var prop = GetCachedProperty(landmark, "X");
        return prop != null ? Convert.ToSingle(prop.GetValue(landmark)) : 0f;
    }

    float GetLandmarkY(object landmark)
    {
        var prop = GetCachedProperty(landmark, "Y");
        return prop != null ? Convert. ToSingle(prop.GetValue(landmark)) : 0f;
    }

    float GetLandmarkZ(object landmark)
    {
        var prop = GetCachedProperty(landmark, "Z");
        return prop != null ?  Convert.ToSingle(prop.GetValue(landmark)) : 0f;
    }

    float GetLandmarkDistance(object a, object b)
    {
        float dx = GetLandmarkX(a) - GetLandmarkX(b);
        float dy = GetLandmarkY(a) - GetLandmarkY(b);
        float dz = GetLandmarkZ(a) - GetLandmarkZ(b);
        return Mathf. Sqrt(dx * dx + dy * dy + dz * dz);
    }

    // ═══════════════════════════════════════════════════════════════════
    // UI DISPLAY
    // ═══════════════════════════════════════════════════════════════════
    void OnGUI()
    {
        if (! showUI) return;
        
        // Background box - Status
        GUI.Box(new Rect(10, 10, 280, 160), "");
        
        // Styles
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 16;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = Color.white;
        
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = 14;
        labelStyle.normal.textColor = Color.white;
        
        GUIStyle gestureStyle = new GUIStyle(GUI. skin.label);
        gestureStyle.fontSize = 18;
        gestureStyle.fontStyle = FontStyle. Bold;
        
        // Set gesture color based on state
        switch (currentGesture)
        {
            case "Grabbed":
            case "Dragging":
                gestureStyle.normal.textColor = grabbedColor;
                break;
            case "Scaling":
                gestureStyle.normal.textColor = twoHandColor;
                break;
            case "Rotating": 
                gestureStyle.normal.textColor = rotatingColor;
                break;
            case "Hovering":
                gestureStyle.normal. textColor = hoverColor;
                break;
            default:
                gestureStyle.normal.textColor = Color.gray;
                break;
        }
        
        // Draw status info
        int y = 15;
        int lineHeight = 22;
        
        GUI.Label(new Rect(20, y, 260, 25), "🎮 Hand Gesture Controller", titleStyle);
        y += lineHeight + 5;
        
        GUI.Label(new Rect(20, y, 260, 25), $"Gesture: {currentGesture}", gestureStyle);
        y += lineHeight;
        
        GUI.Label(new Rect(20, y, 260, 25), $"📍 Position: ({targetCube.position. x:F1}, {targetCube.position.y:F1}, {targetCube.position. z:F1})", labelStyle);
        y += lineHeight;
        
        GUI.Label(new Rect(20, y, 260, 25), $"📏 Scale: {targetCube.localScale.x:F2}", labelStyle);
        y += lineHeight;
        
        GUI.Label(new Rect(20, y, 260, 25), $"🔄 Rotation: ({targetCube.eulerAngles.x:F0}°, {targetCube.eulerAngles.y:F0}°, {targetCube.eulerAngles.z:F0}°)", labelStyle);
        y += lineHeight;
        
        GUI.Label(new Rect(20, y, 260, 25), $"⚡ FPS: {fps: F1}", labelStyle);
        
        // Background box - Gesture Guide
        GUI.Box(new Rect(10, 180, 280, 110), "");
        y = 185;
        
        GUI.Label(new Rect(20, y, 260, 25), "📖 Gesture Guide", titleStyle);
        y += lineHeight;
        
        GUIStyle guideStyle = new GUIStyle(GUI.skin.label);
        guideStyle.fontSize = 12;
        guideStyle.normal.textColor = Color.white;
        
        GUI.Label(new Rect(20, y, 260, 20), "✌️ Two-finger pinch near cube → Grab & Drag", guideStyle);
        y += 18;
        GUI.Label(new Rect(20, y, 260, 20), "🤏🤏 Both hands pinch → Scale (spread/pinch)", guideStyle);
        y += 18;
        GUI.Label(new Rect(20, y, 260, 20), "🤟 Three-finger pinch → Rotate (move hand)", guideStyle);
        y += 18;
        GUI.Label(new Rect(20, y, 260, 20), "   ↔️ Left/Right = Y rotation | ↕️ Up/Down = X rotation", guideStyle);
    }
}