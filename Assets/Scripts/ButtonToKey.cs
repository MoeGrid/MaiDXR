using System;
using UnityEngine;

public class ButtonToKey : MonoBehaviour
{
    public VirtualKeyCode keyToPress;

    private int _insideColliderCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        _insideColliderCount += 1;
        Debug.Log(string.Format("Enter Key {0}", Convert.ToByte(keyToPress)));
        CDD.Instance.PressKey(Convert.ToByte(keyToPress), true);
    }

    private void OnTriggerExit(Collider other)
    {
        _insideColliderCount -= 1;
        _insideColliderCount = Mathf.Max(0, _insideColliderCount);
        if (_insideColliderCount == 0)
        {
            Debug.Log(string.Format("Exit Key {0}", Convert.ToByte(keyToPress)));
            CDD.Instance.PressKey(Convert.ToByte(keyToPress), false);
        }
    }
}
