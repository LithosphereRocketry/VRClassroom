using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scale : MonoBehaviour
{
	public GameObject textCanvas;
	Text text;
	
	float currentMass;
	
	void Start() {
		text = textCanvas.GetComponent<Text>();
    }
	
    void OnCollisionEnter(Collision other) {
		GameObject g = other.gameObject;
		LowerCollision(g);
	}
	void OnCollisionExit(Collision other) {
		GameObject g = other.gameObject;
		LiftCollision(g);
	}
	void LowerCollision(GameObject g) {
		Component[] masses = g.transform.GetComponentsInChildren(typeof(MassComponent));
		foreach(Component m in masses) {
			currentMass += ((MassComponent) m).mass;
		}
		setMass();
	}
	void LiftCollision(GameObject g) {
		Component[] masses = g.transform.GetComponentsInChildren(typeof(MassComponent));
		foreach(Component m in masses) {
			currentMass -= ((MassComponent) m).mass;
		}
		setMass();
	}
	void setMass() {
		text.text = Mathf.Round(currentMass*1000)/1000 + " g";
	}
}
