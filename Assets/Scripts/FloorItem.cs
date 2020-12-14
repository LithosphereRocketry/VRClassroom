using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorItem : PickupItem {
    void ShortClicked(RaycastHit target) {
		GameObject node = target.collider.gameObject;
		if(node.GetComponent("FloorSnap")) {
			rb.isKinematic = true;
			Vector3 lookDir = gameObject.transform.parent.parent.position;
			gameObject.transform.SetParent(null);
			gameObject.transform.position = target.point;
			lookDir.y = gameObject.transform.position.y;
			gameObject.transform.LookAt(lookDir, Vector3.up);
			
		}
	}
}
