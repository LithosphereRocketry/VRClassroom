using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotation : MonoBehaviour
{
	public Vector3 vertAxis;
	Quaternion rotation;
	void Awake() {
		rotation = transform.rotation;
	}
	void LateUpdate() {
		transform.rotation = rotation;
	}
	void UpdateOrientation(Vector3 dir) {
		transform.LookAt(dir, vertAxis);
		rotation = transform.rotation;
	}
}
