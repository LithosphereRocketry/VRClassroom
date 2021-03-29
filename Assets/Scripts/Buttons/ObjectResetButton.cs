using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectResetButton : MonoBehaviour
{
    public GameObject target;
	public Vector3 resetPosition;
	public Vector3 resetEulers;
	Quaternion resetRotation;
	public GameObject timer;
	private Rigidbody body;
	
	void Start() {
		body = target.GetComponent<Rigidbody>();
		resetRotation = Quaternion.Euler(resetEulers);
	}
	
	void WorldClicked() {
        body.position = resetPosition;
		body.rotation = resetRotation;
		body.velocity = Vector3.zero;
		body.angularVelocity = Vector3.zero;
		if(timer) {
			timer.SendMessage("StartTimer", target, SendMessageOptions.DontRequireReceiver);
		}
    }
}
