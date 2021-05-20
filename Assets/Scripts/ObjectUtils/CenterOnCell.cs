using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOnCell : MonoBehaviour
{
    public Vector3 centerPos;
	
	void OnTriggerEnter(Collider other) {
		other.gameObject.transform.position = centerPos;
		Debug.Log("Centered object on cell.");
	}
}
