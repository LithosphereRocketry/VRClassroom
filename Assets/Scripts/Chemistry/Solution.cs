using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution : IonInfo
{
    float emptyMass;
	public float maxVolume;
	float waterVolume;
	public GameObject water;
	public GameObject solids;
	
	List<int> cations = new List<int>();
	List<int> anions = new List<int>();
	
	class Compound {
		float solubility;
		float mass;
		Compound(int cat, int an) {
			
		}
	}
	
	void Start() {
        waterVolume = 0;
    }

    void Update() {
        
    }
}
