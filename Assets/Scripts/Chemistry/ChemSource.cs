using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemSource : ChemInfo {
	public int elementNum;
	public Element element;
	public GameObject tile;
	void Start() {
		base.Start();
		element = table[elementNum-1];
		Debug.Log(element);
		tile.GetComponent<Renderer>().material = Resources.Load("ElementTiles/Materials/"+element.abbr, typeof(Material)) as Material;
	}
}
