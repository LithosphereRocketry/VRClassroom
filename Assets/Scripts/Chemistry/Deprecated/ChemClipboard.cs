using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChemClipboard : ChemInfo
{
    public GameObject textCanvas;
	Text text;
	
	List<Element> elements = new List<Element>();
	List<int> counts = new List<int>();
	
	void Start() {
		base.Start();
		text = textCanvas.GetComponent<Text>();
	}
    
	void ShortClicked(RaycastHit point) {
		GameObject node = point.collider.gameObject;
		
		ChemSource src = (ChemSource) node.GetComponent("ChemSource");
        if(src) {
			//molecule = addAtom(src.element.abbr, molecule);
			addAtomDynamic(src.element);
			text.text = makeReadableDynamic();
		}
		
		if(node.GetComponent("TrashCan")) {
			elements = new List<Element>();
			counts = new List<int>();
			text.text = "";
		}
    }
	
	void addAtomDynamic(Element atom) {
		for(int i = 0; i < elements.Count; i++) {
			if(elements[i] == atom) {
				counts[i] ++;
				return;
			}
		}
		elements.Add(atom);
		counts.Add(1);
	}
	string makeReadableDynamic() {
		string output = "";
		for(int i = 0; i < elements.Count; i++) {
			output += char.ToUpper(elements[i].abbr[0]) + elements[i].abbr.Substring(1) + (counts[i] > 1 ? subscript(counts[i]) : "");
		}
		return output;
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
