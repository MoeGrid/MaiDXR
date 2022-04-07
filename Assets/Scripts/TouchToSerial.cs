using UnityEngine;

public class TouchToSerial : MonoBehaviour
{
    public int Area;

    private int _insideColliderCount = 0;
    private TouchSerial Serial;

    private void Start()
    {
        Serial = GameObject.Find("TouchSerial1P").GetComponent<TouchSerial>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _insideColliderCount += 1;
        Serial.ChangeTouch(Area, true);
    }

    private void OnTriggerExit(Collider other)
    {
        _insideColliderCount -= 1;
        _insideColliderCount = Mathf.Max(0, _insideColliderCount);
        if (_insideColliderCount == 0)
        {
            Serial.ChangeTouch(Area, false);
        }
    }
}
