using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassComponent : MonoBehaviour
{
    GameObject surface;
	public float mass;
	
	void OnCollisionEnter(Collision other) {
		surface = other.gameObject;
	}
	void OnCollisionExit(Collision other) {
		surface = null;
	}

	void LiftCollision() {
		Debug.Log("lift");
		if(surface) { surface.SendMessage("LiftCollision", gameObject, SendMessageOptions.DontRequireReceiver); }
	}
	void LowerCollision() {
		if(surface) { surface.SendMessage("LowerCollision", gameObject, SendMessageOptions.DontRequireReceiver); }
	}
}
