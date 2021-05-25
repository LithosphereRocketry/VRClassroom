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
	float ionInScalar = 1;
	Vector3 waterScale;
	
	Dissolved[] disCations = new Dissolved[CATIONS];
	Dissolved[] disAnions = new Dissolved[ANIONS];
	List<Compound> solids = new List<Compound>();
	
	public class Dissolved {
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
		public IndQty pack() {
			return new IndQty(index, moles, cation);
		}
	}
	
	public class Compound {
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
				float maxSol = d.getSolubility(s, this) * waterVolume;
				float molsIn = Mathf.Min(d.moles/nCations(d.index, s.index), s.moles/nAnions(d.index, s.index));
				if(molsIn > maxSol) {
					AddSolid(new Compound(d, s, molsIn-maxSol));
				}
			}
		} else {
			foreach(Dissolved s in disCations) {
				float maxSol = d.getSolubility(s, this) * waterVolume;
				float molsIn = Mathf.Min(s.moles/nCations(s.index, d.index), d.moles/nAnions(s.index, d.index));
				if(molsIn > maxSol) {
					AddSolid(new Compound(s, d, molsIn-maxSol));
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
		waterScale = waterObject.transform.localScale;
    }
	public void AddSolid(Compound c) { solids.Add(c); } // dumb but lets me keep list private
	public void AddWater(float amt) {
		if(waterVolume + amt <= maxVolume) {
			ionInScalar = 1;
			waterVolume += amt;
		} else {
			ionInScalar = (maxVolume-waterVolume)/amt;
			waterVolume = maxVolume;
		}
	}
	public void AddIon(IndQty ion) {
		if(ion.cation) {
			disCations[ion.ind].moles += ion.qty*ionInScalar;
			Precipitate(disCations[ion.ind]);
		} else {
			disAnions[ion.ind].moles += ion.qty*ionInScalar;
			Precipitate(disAnions[ion.ind]);
		}
	}
    void Update() {
        waterObject.SetActive(waterVolume > 0);
		solidObject.SetActive(solids.Count > 0);
		waterObject.transform.localScale = new Vector3(waterScale.x, waterScale.y, waterScale.z * waterVolume/maxVolume);
    }
	void ShortClicked(RaycastHit point) {
		Solution other = (Solution) point.collider.gameObject.GetComponent("Solution");
		if(other) {
			other.AddWater(waterVolume);
			for(int i = 0; i < CATIONS; i++) {
				other.AddIon(disCations[i].pack());
				disCations[i].moles = 0;
			}
			for(int i = 0; i < ANIONS; i++) {
				other.AddIon(disAnions[i].pack());
				disAnions[i].moles = 0;
			}
			other.AddWater(0);
			waterVolume = 0; 
			foreach(Compound c in solids) {
				other.AddSolid(c);
			}
			solids.Clear();
		}
	}
}
