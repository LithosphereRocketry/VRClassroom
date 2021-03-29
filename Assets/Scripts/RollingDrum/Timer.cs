using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    GameObject target;
	float startTime;
	float startX;
	bool tracking;
	
	public GameObject textCanvas;
	Text text;
	
	void Start() {
		tracking = false;
		text = textCanvas.GetComponent<Text>();
	}
	
	void StartTimer(GameObject t) {
		target = t;
		tracking = true;
		startTime = Time.time;
		startX = target.GetComponent<Rigidbody>().position.x;
	}
	void StopTimer(GameObject t) {
		if(target == t) {
			tracking = false;
		}
	}
	
    void Update() {
        if(tracking) {
			text.text = "" + Mathf.Round((Time.time - startTime)*1000)/1000 + " s\n"
						   + Mathf.Round((startX-target.transform.position.x)*1000/0.98480775301f)/1000.0 + " m";
		}
    }
}
