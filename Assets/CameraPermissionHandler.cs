using UnityEngine;
using System.Collections;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

/// <summary>
/// Handles camera permission request on Android
/// </summary>
public class CameraPermissionHandler :  MonoBehaviour
{
    [Header("Settings")]
    public bool requestOnStart = true;
    
    [Header("Status")]
    public bool hasPermission = false;

    void Start()
    {
        if (requestOnStart)
        {
            StartCoroutine(RequestCameraPermission());
        }
    }

    public IEnumerator RequestCameraPermission()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            hasPermission = true;
            Debug.Log("Camera permission already granted!");
            yield break;
        }
        
        Debug.Log("Requesting camera permission...");
        Permission.RequestUserPermission(Permission.Camera);
        
        yield return new WaitForSeconds(0.5f);
        
        float timer = 0f;
        while (! Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            yield return new WaitForSeconds(0.5f);
            timer += 0.5f;
            
            if (timer > 30f)
            {
                Debug.LogError("Camera permission denied or timed out!");
                yield break;
            }
        }
        
        hasPermission = true;
        Debug.Log("Camera permission granted!");
#else
        hasPermission = true;
        Debug.Log("Camera permission (Editor mode)");
        yield return null;
#endif
    }

    void OnGUI()
    {
#if UNITY_ANDROID && ! UNITY_EDITOR
        if (!hasPermission)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 32;
            style.normal. textColor = Color. white;
            style.alignment = TextAnchor.MiddleCenter;
            
            GUI.Label(
                new Rect(0, Screen.height / 2 - 50, Screen.width, 100),
                "Camera permission required\nPlease allow camera access",
                style
            );
        }
#endif
    }
}