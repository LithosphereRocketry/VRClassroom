using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMoment : MonoBehaviour
{
    public Vector3 moment;
	
	void Start() {
        GetComponent<Rigidbody>().inertiaTensor = moment;
		GetComponent<Rigidbody>().maxAngularVelocity = Mathf.Infinity;
    }
}
