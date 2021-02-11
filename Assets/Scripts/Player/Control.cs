using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
	private bool canTP = false;
	private double downTime = -1;
	
	public GameObject body;
	public GameObject pointer;
	public GameObject navigator;
	public GameObject hand;
	public float floorTolerance = 0.01f;
	public float meshTolerance = 1.0f;
	public float reach = 2.0f;
	public float holdTime = 0.5f;
	public string environmentTag = "Environment";
	// Start is called before the first frame update
    void Start()
    {
        if(!hand) { hand = body; }
    }

    // Update is called once per frame
    void Update()
    {
		bool press = Google.XR.Cardboard.Api.IsTriggerPressed || Input.GetMouseButtonDown(0); // check if either input occurred
		bool release = !Google.XR.Cardboard.Api.IsTriggerPressed && Input.GetMouseButtonUp(0);
		
		bool click = false;
		bool hold = false;
		
		if(press && downTime == -1) {
			downTime = Time.time;
		}
		
		if(release && downTime != -1) {
			if(Time.time-downTime < holdTime) {
				click = true;
			} else {
				hold = true;
			}
			downTime = -1;
		}
		
		Ray look = new Ray(transform.position, transform.forward); // get look point
		
		RaycastHit lookPt;
		
		if(Physics.Raycast(look, out lookPt)) { // if we're looking at a surface...
			string tgt = lookPt.collider.gameObject.tag;
			if(tgt == environmentTag) { // check for target type...
				canTP = Teleport(lookPt, click);
			} else { canTP = false; }
			
			if(hand.transform.childCount > 0) {
				foreach(Transform held in hand.transform) {
					if(click && !canTP) {
						held.gameObject.SendMessage("ShortClicked", lookPt, SendMessageOptions.DontRequireReceiver);
					} else if(hold) {
						held.gameObject.SendMessage("LongClicked", lookPt, SendMessageOptions.DontRequireReceiver);
					}
				}
			} else {
				if(click && !canTP && lookPt.distance <= reach) {
					lookPt.collider.gameObject.SendMessage("WorldClicked", hand, SendMessageOptions.DontRequireReceiver);
					Application.Quit();
				}
			}
		} else {
			canTP = false;
			foreach(Transform held in hand.transform) {
				if(click) {
					held.gameObject.SendMessage("ShortClickedSky", look, SendMessageOptions.DontRequireReceiver);
				} else if(hold) {
					held.gameObject.SendMessage("LongClickedSky", look, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		pointer.SetActive(canTP);
    }
	
	
	
	bool Teleport(RaycastHit look, bool click) {
		Vector3 lookVector = look.point + Vector3.up * floorTolerance; // move up slightly to avoid falling through the floor
				
		RaycastHit landPt;
		Ray land = new Ray(lookVector, Vector3.down);
		if(Physics.Raycast(land, out landPt)) { // if we drop onto a floor...
			
			UnityEngine.AI.NavMeshHit meshPt;
			if(UnityEngine.AI.NavMesh.SamplePosition(landPt.point, out meshPt, 1.0f, UnityEngine.AI.NavMesh.AllAreas)) { // if we're near a NavMesh...
				
				UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
				if(UnityEngine.AI.NavMesh.CalculatePath(body.transform.position, meshPt.position, UnityEngine.AI.NavMesh.AllAreas, path)) { // if we can navigate there...
					pointer.transform.position = meshPt.position; // place the icon
					if(click) { body.transform.position = pointer.transform.position; } // if we click, teleport
					return true; // skip turning off the icon if it's somewhere valid
				}
			}
		}
		return false;
	}
}
