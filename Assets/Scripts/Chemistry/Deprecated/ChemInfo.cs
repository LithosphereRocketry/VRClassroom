using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// string format: number,abbr,name,charge

public class ChemInfo : MonoBehaviour
{
	const int ELEMENTS = 118;
	
	public TextAsset elementData;
   
    public class Element {
		public int atomicNum;
		public string name;
		public string abbr;
		public Element(int num, string a, string n) {
			atomicNum = num;
			name = n;
			abbr = a;
		}
		public override string ToString() {
			return name+" ["+atomicNum+"] ("+abbr+")"; 
		}
	}
	
	protected Element[] table = new Element[ELEMENTS];
	
	protected void Start() {
		string[] text = elementData.text.Split('\n');
		foreach(string element in text) {
			string[] e = element.Split(',');
			if(e.Length >= 3) {
				table[int.Parse(e[0])-1] = new Element(int.Parse(e[0]), e[1], e[2]);
			}
		}
	}
}
