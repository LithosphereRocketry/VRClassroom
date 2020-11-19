using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSnap : MonoBehaviour
{
    private Collider c;
	public GameObject model;
	void Start() {
		c = gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update() {
		bool isAttached = false;
		foreach(Transform t in transform) {
			if(t.gameObject != model && t.gameObject != gameObject) {
				isAttached = true;
				Debug.Log(t.gameObject);
			}
		}
		if(isAttached && c.enabled) {
			c.enabled = false;
			model.SetActive(false);
		} else if(!c.enabled && !isAttached) {
			c.enabled = true;
			model.SetActive(true);
		}
    }
}
