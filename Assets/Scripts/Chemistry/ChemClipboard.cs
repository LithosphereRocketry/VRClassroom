using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChemClipboard : MonoBehaviour
{
    public GameObject textCanvas;
	Text text;
	string molecule = "";
	
	void Start() {
		text = textCanvas.GetComponent<Text>();
	}
    
	void ShortClicked(RaycastHit point) {
		ChemSource src = (ChemSource) point.collider.gameObject.GetComponent("ChemSource");
        if(src) {
			molecule += src.element;
			text.text = molecule;
			Debug.Log(subscript(1234));
		}
    }
	
	string subscript(int n) {
		char d = (char) (0x2080 + n%10);
		if(n >= 10) {
			return subscript(n/10)+d.ToString();
		} else {
			return d.ToString();
		}
	}
}
