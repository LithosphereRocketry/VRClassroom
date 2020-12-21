using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapItem : PickupItem
{
    protected float mass;
	protected float drag;
	protected float angDrag;
	protected Vector3 cg;
	void Start() {
		base.Start();
		if(rb) {		
			mass = rb.mass;
			drag = rb.drag;
			angDrag = rb.angularDrag;
			cg = rb.centerOfMass;
		}
	}
	void WorldClicked(GameObject hand) {
		if(!rb) {
			rb = gameObject.AddComponent<Rigidbody>();
		}
		rb.mass = mass;
		rb.drag = drag;
		rb.angularDrag = angDrag;
		rb.centerOfMass = cg;
		base.WorldClicked(hand);
	}
	void ShortClicked(RaycastHit target) {
		GameObject node = target.collider.gameObject;
		if(node.GetComponent("NodeSnap")) {
		//	storedBody = gameObject.GetComponent<Rigidbody>();
		//	Destroy(gameObject.GetComponent<Rigidbody>());
			
			gameObject.transform.SetParent(node.transform, false);
			mass = rb.mass;
			drag = rb.drag;
			angDrag = rb.angularDrag;
			cg = rb.centerOfMass;
			Destroy(rb);
		}
	}
}
