using System.Runtime.InteropServices;
using UnityEngine;

internal class CDD
{
    private const string name = "DD64";

    [DllImport(name, EntryPoint = "DD_btn")]
    private static extern int DD_btn(int btn);

    [DllImport(name, EntryPoint = "DD_key")]
    private static extern int DD_key(int ddcode, int flag);

    [DllImport(name, EntryPoint = "DD_todc")]
    private static extern int DD_todc(int vkcode);

    private CDD()
    {
        var ret = DD_btn(0);
        if(ret != 1)
            Debug.LogErrorFormat("DD driver failed to load!");
    }

    private static CDD _Instance = null;

    public static CDD Instance
    { 
        get {
            if (_Instance == null)
                _Instance = new CDD();
            return _Instance; 
        }
        private set {
        }
    }

    public void PressKey(int keyCode, bool press)
    {
        DD_key(DD_todc(keyCode), press ? 1: 2);
    }

}
