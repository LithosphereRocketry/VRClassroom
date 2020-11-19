using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapItem : MonoBehaviour
{
    // Start is called before the first frame update
    void ShortClicked(RaycastHit target) {
		GameObject node = target.collider.gameObject;
		if(node.GetComponent("NodeSnap")) {
			gameObject.transform.SetParent(node.transform, false);
		}
	}
}
