using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Unity.XR.CoreUtils;
using uWindowCapture;
public class SettingsManager : MonoBehaviour
{
    string JsonPath;
    string JsonStr;
    Settings Setting = new Settings();  
    void Start()
    {
        JsonPath = Path.GetDirectoryName(Application.dataPath) + "/Settings.json";
        Debug.Log(JsonPath);
        if (!File.Exists (JsonPath))
        {
            Settings Setting = new Settings()
            {
                HandSize = 8f,
                HandPositionX = 2f,
                HandPositionY = -2f,
                HandPositionZ = 7f,
                PlayerHigh = 180f,
                CaptureFrameRate = 90,
                TouchRefreshRate = 90,
                CameraSmooth = 0.1f,
                HapticDuration = 0.15f,
                HapticAmplitude = 1
            };
            JsonStr = JsonConvert.SerializeObject(Setting, Formatting.Indented);
            Debug.Log(JsonStr);
            File.AppendAllText(JsonPath, JsonStr);
        }
        else
        {
            JsonStr = File.ReadAllText(JsonPath);
            Setting = JsonConvert.DeserializeObject<Settings>(JsonStr);
        }
        UpdateFromFile();
    }
    bool FocusChecked = true;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5) | !FocusChecked)
        {
            if (Application.isFocused)
            {
                FocusChecked=true;
                JsonStr = File.ReadAllText(JsonPath);
                Setting = JsonConvert.DeserializeObject<Settings>(JsonStr); 
                UpdateFromFile();
                Debug.Log("Setting Updated");
            }  
        }
        if (!Application.isFocused)
            FocusChecked=false;
    }
    public GameObject LHandObj;
    public GameObject RHandObj;
    public GameObject ScreenObj;
    public GameObject SmoothCameraObj;
    public GameObject XROriginObj;

    void UpdateFromFile()
    {
        LHandObj.transform.localScale = new Vector3(Setting.HandSize/100,Setting.HandSize/100,Setting.HandSize/100);
        RHandObj.transform.localScale = new Vector3(Setting.HandSize/100,Setting.HandSize/100,Setting.HandSize/100);
        LHandObj.transform.localPosition = new Vector3(Setting.HandPositionX/100,Setting.HandPositionY/100,Setting.HandPositionZ/100);
        RHandObj.transform.localPosition = new Vector3(Setting.HandPositionX/-100,Setting.HandPositionY/100,Setting.HandPositionZ/100);
        XROrigin XROriginScp = XROriginObj.GetComponent<XROrigin>();
        XROriginScp.CameraYOffset = Setting.PlayerHigh;
        UwcWindowTexture ScreenScp = ScreenObj.GetComponent<UwcWindowTexture>();
        ScreenScp.captureFrameRate = Setting.CaptureFrameRate;
        CameraSmooth CameraSmoothScp = SmoothCameraObj.GetComponent<CameraSmooth>();
        CameraSmoothScp.smoothSpeed = Setting.CameraSmooth;
        Controller LHandScp = LHandObj.GetComponent<Controller>();
        LHandScp.amplitude = Setting.HapticAmplitude;
        Controller RHandScp = RHandObj.GetComponent<Controller>();
        RHandScp.amplitude = Setting.HapticAmplitude;
        XROriginScp.CameraYOffset = Setting.PlayerHigh/100;
        Time.fixedDeltaTime = 1/Setting.TouchRefreshRate;
    }

    
    
}
public class Settings
{
    public float HandSize { get; set; }
    public float HandPositionX { get; set; }
    public float HandPositionY { get; set; }
    public float HandPositionZ { get; set; }
    public float PlayerHigh { get; set; }
    public int CaptureFrameRate { get; set; }
    public float TouchRefreshRate { get; set; }
    public float CameraSmooth { get; set; }
    public float HapticDuration { get; set; }
    public float HapticAmplitude { get; set; }
}