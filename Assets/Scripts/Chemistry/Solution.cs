using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution : IonInfo
{
    float emptyMass;
	public float maxVolume;
	float waterVolume;
	public GameObject waterObject;
	public GameObject solidObject;
	
	List<int> dissolvedCations = new List<int>();
	List<int> dissolvedAnions = new List<int>();
	List<Compound> solids = new List<Compound>();
	
	class Compound {
		float solubility;
		float mass;
		public Compound(int cat, int an) {
			
		}
	}
	
	void Start() {
		base.Start();
        waterVolume = 0;
    }

    void Update() {
        waterObject.SetActive(waterVolume > 0);
		solidObject.SetActive(solids.Count > 0);
    }
	void ProduceCompound(int[] ions) {
		int nc = -anions[ions[1]].charge; // calculate empirical formula via GCD
		int na = cations[ions[0]].charge;
		int scalar = gcd(nc, na);
		nc /= scalar;
		na /= scalar;
		for(int i = 0; i < nc; i++) { AddIon(ions[0], true); }
		for(int i = 0; i < na; i++) { AddIon(ions[1], false); }
	}
	void AddIon(int ion, bool cation) {
		waterVolume = maxVolume;
		if(cation) {
			dissolvedCations.Add(ion);
			for(int i = 0; i < dissolvedAnions.Count; i++) {
				if(getSolubility(ion, dissolvedAnions[i]) < 1) { // this is jank, fix later
					solids.Add(new Compound(ion, i));
					dissolvedCations.RemoveAt(dissolvedCations.Count-1);
					dissolvedAnions.RemoveAt(i);
					i--;
				}
			}
		} else {
			dissolvedAnions.Add(ion);
			for(int i = 0; i < dissolvedCations.Count; i++) {
				if(getSolubility(dissolvedCations[i], ion) < 1) { // this is jank, fix later
					solids.Add(new Compound(i, ion));
					dissolvedAnions.RemoveAt(dissolvedAnions.Count-1);
					dissolvedCations.RemoveAt(i);
					i--;
				}
			}
		}
	}		
}
