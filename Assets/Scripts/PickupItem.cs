using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour {
    private Rigidbody rb;
	// Start is called before the first frame update
    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
    }
	
	void WorldClicked(GameObject hand) {
	//	Debug.Log(hand);
		gameObject.transform.SetParent(hand.transform, false);
		if(rb && !rb.isKinematic) {
			rb.isKinematic = true;
		}
		gameObject.transform.localPosition = new Vector3();
		gameObject.transform.localRotation = Quaternion.identity;
	}
	void LongClicked() {
		gameObject.transform.SetParent(null, true);
		if(rb) { rb.isKinematic = false; }
	}
	
	void LongClickedSky() {
		LongClicked();
	}
}