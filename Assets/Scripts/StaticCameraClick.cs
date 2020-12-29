using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class StaticCameraClick : MonoBehaviour
{
    public GameObject hand;
	public GameObject crosshair;
    private Camera cam;
	private bool enabled;
	
	void Start() {
		enabled = !XRSettings.enabled;
        cam = Camera.main;
		if(!hand) { hand = gameObject; }
		if(enabled) {
			Destroy(crosshair);
			Cursor.lockState = CursorLockMode.None;
		}
    }

	
	void Update() {
		if(enabled && Input.GetMouseButtonDown(0)) {
			Ray look = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit lookPt;
			if(Physics.Raycast(look, out lookPt)) {
				lookPt.collider.gameObject.SendMessage("WorldClicked", hand, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
