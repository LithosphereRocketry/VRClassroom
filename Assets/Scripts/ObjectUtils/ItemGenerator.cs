using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public GameObject item;
	public float wobbleSize;
	public float wobbleTime;
	public Transform child;
	
	void Update() {
		child = null;
        foreach(Transform t in transform) {
			if(t.gameObject != gameObject) {
				child = t;
			}
		}
		
		if(!child) {
			child = Instantiate(item).transform;
			child.SetParent(transform, false);
			child.transform.localPosition = new Vector3();
			child.transform.localRotation = Quaternion.identity;
			if(child.GetComponent<Rigidbody>() && !child.GetComponent<Rigidbody>().isKinematic) {
				child.GetComponent<Rigidbody>().isKinematic = true;
			}
		} else {
			float wobble = (wobbleSize * Mathf.Cos(Time.time * Mathf.PI * 2 / wobbleTime));
			child.transform.localPosition = wobble * new Vector3(0.0f, 1.0f, 0.0f);
		}
    }
}
