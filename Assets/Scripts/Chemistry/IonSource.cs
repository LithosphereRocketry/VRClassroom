using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IonSource : IonInfo {
    public bool isCation;
	public int index;
	public Ion ion;
	public GameObject tile;
	void Start() {
		base.Start();
        if(isCation) {
			ion = cations[index];
		} else {
			ion = anions[index];
		}
		Texture2D t = Resources.Load("ElementTiles/"+ion.abbr, typeof(Texture2D)) as Texture2D;
		if(t != null) {
			tile.GetComponent<Renderer>().material.SetTexture("_MainTex", t);
			tile.GetComponent<Renderer>().material.SetTexture("_EmissionMap", t);
		} else {
			Debug.Log("Failed to load texture for compound "+ion.abbr);
		}
		
    }
}
