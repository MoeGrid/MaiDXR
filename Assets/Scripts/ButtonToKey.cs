using System;
using UnityEngine;

public class ButtonToKey : MonoBehaviour
{
    public VirtualKeyCode keyToPress;

    private int _insideColliderCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        _insideColliderCount += 1;
        CDD.Instance.PressKey(Convert.ToByte(keyToPress), true);
    }

    private void OnTriggerExit(Collider other)
    {
        _insideColliderCount -= 1;
        _insideColliderCount = Mathf.Max(0, _insideColliderCount);
        if (_insideColliderCount == 0)
            CDD.Instance.PressKey(Convert.ToByte(keyToPress), false);
    }
}
