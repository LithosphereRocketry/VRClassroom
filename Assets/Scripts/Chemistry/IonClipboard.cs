using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IonClipboard : IonInfo
{
	public GameObject textCanvas;
	Text text;
	
	int anionIndex = -1;
	int cationIndex = -1;
	
    void Start() {
		base.Start();
		text = textCanvas.GetComponent<Text>();
    }

    void ShortClicked(RaycastHit point) {
		GameObject node = point.collider.gameObject;
		
		IonSource src = (IonSource) node.GetComponent("IonSource");
        if(src) {
			if(src.isCation) { cationIndex = src.index; }
			else { anionIndex = src.index; }
			text.text = UpdateText();
		}
		
		if(node.GetComponent("TrashCan")) {
			cationIndex = -1;
			anionIndex = -1;
			text.text = UpdateText();
		}
    }
	string UpdateText() {
		if(cationIndex == -1) {
			if(anionIndex == -1) {
				return "";
			} else {
				return anions[anionIndex].ToString();
			}
		} else {
			if(anionIndex == -1) {
				return cations[cationIndex].ToString();
			} else {
				return getReadable(cations[cationIndex], anions[anionIndex]);
			}
		}
	}
}
