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
	public float defaultMoles;
	
	Dissolved[] disCations = new Dissolved[CATIONS];
	Dissolved[] disAnions = new Dissolved[ANIONS];
	List<Compound> solids = new List<Compound>();
	
	class Dissolved {
		public int index;
		public bool cation;
		public float moles;
		public Dissolved() {
			moles = 0;
			cation = true;
			index = -1;
		}
		public float getSolubility(Dissolved other, IonInfo info) {
			int cat, an;
			if(cation) { cat = index; an = other.index; }
			else { an = index; cat = other.index; }
			if(cat < 0 || an < 0) { return 0; }
			return info.solubility[cat, an];
		}
	}
	
	class Compound {
		float moles;
		Dissolved anion, cation;
		public Compound(Dissolved cat, Dissolved an, float moles) {
			
		}
		public float mass(IonInfo info) {
			return 0;
		}
	}
	
	void Precipitate(Dissolved d) {
		if(d.cation) {
			foreach(Dissolved s in disAnions) {
				float maxSol = d.getSolubility(s, this);
				float molarity = Mathf.Min(d.moles/nCations(d.index, s.index), s.moles/nAnions(d.index, s.index)) / waterVolume;
				if(molarity > maxSol) {
					solids.Add(new Compound(d, s, (molarity-maxSol)*waterVolume));
					Debug.Log("dumpC");
				}
			}
		} else {
			foreach(Dissolved s in disCations) {
				float maxSol = d.getSolubility(s, this);
				float molarity = Mathf.Min(s.moles/nCations(s.index, d.index), d.moles/nAnions(s.index, d.index)) / waterVolume;
				if(molarity > maxSol) {
					solids.Add(new Compound(s, d, (molarity-maxSol)*waterVolume));
					Debug.Log("dumpA");
				}
			}
		}
	}
	
	void Start() {
		base.Start();
        waterVolume = 0;
		for(int i = 0; i < CATIONS; i++) {
			disCations[i] = new Dissolved();
			disCations[i].index = i;
			disCations[i].cation = true;
		}
		for(int i = 0; i < ANIONS; i++) {
			disAnions[i] = new Dissolved();
			disAnions[i].index = i;
			disAnions[i].cation = false;
		}
    }
	void ProduceCompound(int[] indices) {
		waterVolume += 1;
		disCations[indices[0]].moles += defaultMoles * nCations(indices[0], indices[1]);
		disAnions[indices[1]].moles += defaultMoles * nCations(indices[0], indices[1]);
		Precipitate(disCations[indices[0]]);
		Precipitate(disAnions[indices[1]]);
	}

    void Update() {
        waterObject.SetActive(waterVolume > 0);
		solidObject.SetActive(solids.Count > 0);
    }
}
