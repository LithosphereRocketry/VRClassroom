using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		GameObject item = other.gameObject;
		if(item.GetComponent("TrashableItem") && (item.GetComponent<Rigidbody>() && !item.GetComponent<Rigidbody>().isKinematic)) {
			Destroy(other.gameObject);
		}
	}
}
