using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapItem : PickupItem
{
    //public Rigidbody storedBody;
	// Start is called before the first frame update
    void WorldClicked(GameObject hand) {
	//	if(storedBody || true) {
	//		Rigidbody rb = gameObject.AddComponent<Rigidbody>();
			// rb = Instantiate(storedBody);
	//		storedBody = null;
	//	}
		base.WorldClicked(hand);
	}
	
	void ShortClicked(RaycastHit target) {
		GameObject node = target.collider.gameObject;
		if(node.GetComponent("NodeSnap")) {
		//	storedBody = gameObject.GetComponent<Rigidbody>();
		//	Destroy(gameObject.GetComponent<Rigidbody>());
			
			gameObject.transform.SetParent(node.transform, false);
			gameObject.GetComponent<Rigidbody>().isKinematic = false;
			
		}
	}
}
