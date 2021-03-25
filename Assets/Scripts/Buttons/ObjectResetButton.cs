using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectResetButton : MonoBehaviour
{
    public GameObject target;
	public Vector3 resetPosition;
	public GameObject timer;
	private Rigidbody body;
	
	void Start() {
		body = target.GetComponent<Rigidbody>();
	}
	
	void WorldClicked() {
        body.position = resetPosition;
		body.velocity = Vector3.zero;
		body.angularVelocity = Vector3.zero;
		if(timer) {
			timer.SendMessage("StartTimer", target, SendMessageOptions.DontRequireReceiver);
		}
    }
}
