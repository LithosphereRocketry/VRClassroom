using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashableItem : MonoBehaviour {
    void ShortClicked(RaycastHit target) {
		GameObject node = target.collider.gameObject;
		if(node.GetComponent("TrashCan")) {
			RecursiveUnhook(transform, transform.parent.gameObject);
			Destroy(gameObject);
		}
	}
	void RecursiveUnhook(Transform obj, GameObject hand) {
		foreach(Transform t in obj) {
			if(t != obj) {
				if(t.gameObject.GetComponent("PickupItem")) {
					t.gameObject.SendMessage("WorldClicked", hand);
					t.gameObject.SendMessage("LongClickedSky", new Ray(new Vector3(), Vector3.up));
				} else {
					RecursiveUnhook(t, hand);
				}
			}
		}
	}
}
