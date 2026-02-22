using UnityEngine;

/// <summary>
/// VR Camera Rig for Mobile VR Box
/// Creates split-screen stereoscopic view
/// </summary>
public class VRCameraRig :  MonoBehaviour
{
    [Header("=== VR SETTINGS ===")]
    public bool enableVRMode = true;
    
    [Range(0.03f, 0.08f)]
    public float eyeDistance = 0.064f;
    
    [Header("=== CAMERAS ===")]
    public Camera leftEyeCamera;
    public Camera rightEyeCamera;
    
    [Header("=== HEAD TRACKING ===")]
    public bool enableHeadTracking = true;
    
    [Range(0.1f, 2f)]
    public float headTrackingSensitivity = 1f;
    
    private Camera mainCamera;
    private Gyroscope gyro;
    private Quaternion gyroInitialRotation;
    private bool gyroSupported = false;

    void Start()
    {
        mainCamera = Camera.main;
        
        if (enableVRMode)
        {
            SetupVRCameras();
        }
        
        if (enableHeadTracking)
        {
            SetupGyroscope();
        }
        
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        Debug.Log("VR Camera Rig initialized!");
    }

    void SetupVRCameras()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }
        
        mainCamera.enabled = false;
        
        if (leftEyeCamera == null)
        {
            GameObject leftEyeObj = new GameObject("LeftEyeCamera");
            leftEyeObj.transform.SetParent(mainCamera.transform);
            leftEyeObj.transform.localPosition = new Vector3(-eyeDistance / 2f, 0f, 0f);
            leftEyeObj.transform. localRotation = Quaternion.identity;
            
            leftEyeCamera = leftEyeObj.AddComponent<Camera>();
            leftEyeCamera.CopyFrom(mainCamera);
        }
        
        if (rightEyeCamera == null)
        {
            GameObject rightEyeObj = new GameObject("RightEyeCamera");
            rightEyeObj. transform.SetParent(mainCamera.transform);
            rightEyeObj.transform.localPosition = new Vector3(eyeDistance / 2f, 0f, 0f);
            rightEyeObj.transform.localRotation = Quaternion.identity;
            
            rightEyeCamera = rightEyeObj. AddComponent<Camera>();
            rightEyeCamera.CopyFrom(mainCamera);
        }
        
        leftEyeCamera. rect = new Rect(0f, 0f, 0.5f, 1f);
        rightEyeCamera. rect = new Rect(0.5f, 0f, 0.5f, 1f);
        
        leftEyeCamera. enabled = true;
        rightEyeCamera.enabled = true;
        
        Debug.Log("VR Cameras created (split-screen mode)");
    }

    void SetupGyroscope()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            gyroSupported = true;
            gyroInitialRotation = GyroToUnity(gyro. attitude);
            
            Debug.Log("Gyroscope enabled for head tracking");
        }
        else
        {
            gyroSupported = false;
            Debug.LogWarning("Gyroscope not supported on this device");
        }
    }

    void Update()
    {
        if (enableHeadTracking && gyroSupported)
        {
            UpdateHeadTracking();
        }
        
        if (enableVRMode && leftEyeCamera != null && rightEyeCamera != null)
        {
            leftEyeCamera. transform.localPosition = new Vector3(-eyeDistance / 2f, 0f, 0f);
            rightEyeCamera.transform.localPosition = new Vector3(eyeDistance / 2f, 0f, 0f);
        }
    }

    void UpdateHeadTracking()
    {
        if (mainCamera == null) return;
        
        Quaternion gyroRotation = GyroToUnity(gyro.attitude);
        Quaternion relativeRotation = Quaternion.Inverse(gyroInitialRotation) * gyroRotation;
        
        mainCamera.transform.localRotation = Quaternion.Slerp(
            mainCamera.transform.localRotation,
            relativeRotation,
            headTrackingSensitivity * Time.deltaTime * 10f
        );
    }

    Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q. y, -q.z, -q.w);
    }

    public void RecenterView()
    {
        if (gyroSupported)
        {
            gyroInitialRotation = GyroToUnity(gyro. attitude);
            Debug.Log("View recentered!");
        }
    }

    public void ToggleVRMode()
    {
        enableVRMode = !enableVRMode;
        
        if (enableVRMode)
        {
            SetupVRCameras();
        }
        else
        {
            if (leftEyeCamera != null) leftEyeCamera.enabled = false;
            if (rightEyeCamera != null) rightEyeCamera.enabled = false;
            if (mainCamera != null)
            {
                mainCamera.enabled = true;
                mainCamera.rect = new Rect(0f, 0f, 1f, 1f);
            }
        }
        
        Debug.Log("VR Mode:  " + enableVRMode);
    }
}