using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialItem : MonoBehaviour
{
    public string type;
	public float potential;
	public float lineEquivLength;
	
    void Start() {
        foreach(PotentialGrid p in Object.FindObjectsOfType<PotentialGrid>()) {
			p.gameObject.SendMessage("UpdatePotentials", gameObject);
		}
    }
}
