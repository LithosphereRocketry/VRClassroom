using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCenterOfMass : MonoBehaviour
{
    public GameObject center;
	void Start() {
		Vector3 pos = center.transform.localPosition;
		pos.Scale(transform.localScale);
        gameObject.GetComponent<Rigidbody>().centerOfMass = pos;
    }
}
