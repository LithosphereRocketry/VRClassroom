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
		
		GameObject mr = MassRoot(gameObject);
		mr.SendMessage("LiftCollision", SendMessageOptions.DontRequireReceiver);
		base.WorldClicked(hand);
		mr.SendMessage("LowerCollision", SendMessageOptions.DontRequireReceiver);
	}
	void ShortClicked(RaycastHit target) {
		GameObject node = target.collider.gameObject;
		GameObject mr = MassRoot(node);
		mr.SendMessage("LiftCollision", SendMessageOptions.DontRequireReceiver);
		if(node.GetComponent("NodeSnap")) {
			gameObject.transform.SetParent(node.transform, false);
			mass = rb.mass;
			drag = rb.drag;
			angDrag = rb.angularDrag;
			cg = rb.centerOfMass;
			Destroy(rb);
		}
		mr.SendMessage("LowerCollision", SendMessageOptions.DontRequireReceiver);
	}
	GameObject MassRoot(GameObject start) {
		GameObject massRoot = start;
		bool foundRoot = false;
		while(!foundRoot) {
			Transform nextRootTransform = massRoot.transform.parent;
			if(nextRootTransform) {
				massRoot = nextRootTransform.gameObject;
				if(massRoot.GetComponent<Rigidbody>() && massRoot.GetComponent(typeof(MassComponent))) {
					foundRoot = true;
				}
			} else {
				massRoot = gameObject;
				foundRoot = true;
			}
		}
		return massRoot;
	}
}
