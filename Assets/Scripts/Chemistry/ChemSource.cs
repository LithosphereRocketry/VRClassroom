using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemSource : MonoBehaviour {
	public string element;
	public GameObject tile;
	void Start() {
		tile.GetComponent<Renderer>().material = Resources.Load("ElementTiles/Materials/"+element, typeof(Material)) as Material;
	}
}
