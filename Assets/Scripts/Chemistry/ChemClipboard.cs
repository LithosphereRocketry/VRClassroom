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
			molecule = addAtom(src.element, molecule);
			text.text = makeReadable(molecule);
		}
    }
	
	string addAtom(string atom, string mol) {
		string[] molbreak = molecule.Split(' ');
		string output = "";
		bool placed = false;
		for(int i = 0; i < molbreak.Length; i++) {
			string[] atombreak = molbreak[i].Split('_');
			if(atombreak.Length > 1) {
				if(atombreak[0] == atom) {
					atombreak[1] = (int.Parse(atombreak[1]) + 1).ToString();
					placed = true;
				}
				output += (atombreak[0]+"_"+atombreak[1]+" ");
			}
		}
		if(!placed) {
			output += atom+"_1 ";
		}
		return output;
	}
	
	string makeReadable(string molecule) {
		string[] molbreak = molecule.Split(' ');
		string output = "";
		for(int i = 0; i < molbreak.Length; i++) {
			string[] atombreak = molbreak[i].Split('_');
			if(atombreak.Length > 1) {
				output += char.ToUpper(atombreak[0][0]) + atombreak[0].Substring(1);
				int num = int.Parse(atombreak[1]);
				output += num>1 ? subscript(num) : "";
			}
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
