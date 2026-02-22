using UnityEngine;

/// <summary>
/// Mobile UI Controller
/// Touch controls and settings for mobile VR
/// </summary>
public class MobileUIController : MonoBehaviour
{
    [Header("=== REFERENCES ===")]
    public VRCameraRig vrCameraRig;
    public HandGestureController handGestureController;
    
    [Header("=== UI SETTINGS ===")]
    public bool showMobileUI = true;
    
    [Header("=== PERFORMANCE ===")]
    [Range(30, 60)]
    public int targetFrameRate = 60;
    
    private bool settingsMenuOpen = false;

    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
        
        if (vrCameraRig == null)
            vrCameraRig = FindObjectOfType<VRCameraRig>();
        
        if (handGestureController == null)
            handGestureController = FindObjectOfType<HandGestureController>();
        
        Debug.Log("Mobile UI Controller ready!");
    }

    void Update()
    {
        HandleTouchInput();
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            settingsMenuOpen = ! settingsMenuOpen;
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input. GetTouch(0);
            
            if (touch.tapCount == 2 && touch.phase == TouchPhase.Ended)
            {
                if (vrCameraRig != null)
                {
                    vrCameraRig.RecenterView();
                }
            }
            
            if (Input.touchCount == 3 && touch.phase == TouchPhase. Began)
            {
                settingsMenuOpen = ! settingsMenuOpen;
            }
        }
    }

    void OnGUI()
    {
        if (!showMobileUI) return;
        
        float buttonSize = 80f;
        float quarterScreen = Screen.width / 4f;
        
        if (! settingsMenuOpen)
        {
            if (GUI.Button(new Rect(quarterScreen - buttonSize - 10, 10, buttonSize, buttonSize), "Settings"))
            {
                settingsMenuOpen = true;
            }
        }
        
        if (settingsMenuOpen)
        {
            DrawSettingsMenu();
        }
    }

    void DrawSettingsMenu()
    {
        float menuWidth = 400f;
        float menuHeight = 450f;
        float menuX = (Screen.width - menuWidth) / 2f;
        float menuY = (Screen.height - menuHeight) / 2f;
        
        GUI.Box(new Rect(menuX, menuY, menuWidth, menuHeight), "");
        
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle. fontSize = 28;
        titleStyle. fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor. MiddleCenter;
        titleStyle. normal.textColor = Color.white;
        
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle. fontSize = 24;
        
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = 20;
        labelStyle.normal.textColor = Color.white;
        
        float y = menuY + 20;
        float buttonHeight = 50f;
        float spacing = 60f;
        
        GUI.Label(new Rect(menuX, y, menuWidth, 40), "Settings", titleStyle);
        y += 50;
        
        if (vrCameraRig != null)
        {
            string vrText = vrCameraRig. enableVRMode ?  "VR Mode: ON" : "VR Mode: OFF";
            if (GUI.Button(new Rect(menuX + 20, y, menuWidth - 40, buttonHeight), vrText, buttonStyle))
            {
                vrCameraRig. ToggleVRMode();
            }
            y += spacing;
            
            string headText = vrCameraRig.enableHeadTracking ?  "Head Tracking:  ON" : "Head Tracking: OFF";
            if (GUI.Button(new Rect(menuX + 20, y, menuWidth - 40, buttonHeight), headText, buttonStyle))
            {
                vrCameraRig.enableHeadTracking = !vrCameraRig.enableHeadTracking;
            }
            y += spacing;
            
            GUI.Label(new Rect(menuX + 20, y, menuWidth - 40, 30), "Eye Distance: " + (vrCameraRig.eyeDistance * 1000f).ToString("F1") + "mm", labelStyle);
            y += 30;
            vrCameraRig.eyeDistance = GUI.HorizontalSlider(new Rect(menuX + 20, y, menuWidth - 40, 30), vrCameraRig.eyeDistance, 0.055f, 0.075f);
            y += spacing;
            
            if (GUI.Button(new Rect(menuX + 20, y, menuWidth - 40, buttonHeight), "Recenter View", buttonStyle))
            {
                vrCameraRig.RecenterView();
            }
            y += spacing;
        }
        
        if (GUI.Button(new Rect(menuX + 20, y, menuWidth - 40, buttonHeight), "Close", buttonStyle))
        {
            settingsMenuOpen = false;
        }
    }
}