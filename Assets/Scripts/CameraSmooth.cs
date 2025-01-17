using UnityEngine;

public class CameraSmooth : MonoBehaviour {

	public Transform target;
	public float smoothSpeed = 0.125f;
	public Vector3 PositionOffset;
	void Update ()
	{
		transform.position = Vector3.Lerp(transform.position, target.position + PositionOffset, smoothSpeed);
		transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, smoothSpeed);
	}

}
