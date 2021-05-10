using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IonClipboard : IonInfo
{
	public GameObject textCanvas;
	Text text;
	
	int cationIndex = -1;
	int anionIndex = -1;
	
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
		
		ChemMachine machine = (ChemMachine) node.GetComponent("ChemMachine");
		
		if(machine && cationIndex >= 0 && anionIndex >= 0) {
			int[] ions = {cationIndex, anionIndex};
			machine.gameObject.SendMessage("ProduceCompound", ions);
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
