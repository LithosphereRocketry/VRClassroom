﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution : IonInfo
{
    public float emptyMass;
	public float maxVolume;
	float waterVolume;
	public GameObject waterObject;
	public GameObject solidObject;
	public float defaultMoles;
	float ionInScalar = 1;
	Vector3 waterScale;
	MassComponent mc;
	
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
		public float mass(IonInfo info) {
			return moles * (cation ? info.cations[index].mass : info.anions[index].mass);
		}
	}
	
	public class Compound {
		float moles;
		public Dissolved anion, cation;
		public Compound(Dissolved cat, Dissolved an, float m) {
			cation = cat;
			anion = an;
			moles = m;
		}
		public float mass(IonInfo info) {
			return moles * (
				info.cations[cation.index].mass*info.nCations(cation.index, anion.index)
			  + info.anions[anion.index].mass  *info.nAnions(cation.index, anion.index)
			);
		}
	}
	
	void Precipitate(Dissolved d) {
		if(d.cation) {
			foreach(Dissolved s in disAnions) {
				float maxSol = d.getSolubility(s, this) * waterVolume;
				float molsIn = Mathf.Min(d.moles/nCations(d.index, s.index), s.moles/nAnions(d.index, s.index));
				if(molsIn > maxSol) {
					AddSolid(new Compound(d, s, molsIn-maxSol));
					d.moles -= (molsIn-maxSol)*nCations(d.index, s.index);
					s.moles -= (molsIn-maxSol)*nAnions(d.index, s.index);
					Debug.Log(d.moles+" "+s.moles);
				}
			}
		} else {
			foreach(Dissolved s in disCations) {
				float maxSol = d.getSolubility(s, this) * waterVolume;
				float molsIn = Mathf.Min(s.moles/nCations(s.index, d.index), d.moles/nAnions(s.index, d.index));
				if(molsIn > maxSol) {
					AddSolid(new Compound(s, d, molsIn-maxSol));
					s.moles -= (molsIn-maxSol)*nCations(s.index, d.index);
					d.moles -= (molsIn-maxSol)*nAnions(s.index, d.index);
					Debug.Log(d.moles+" "+s.moles);
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
		mc = (MassComponent) gameObject.GetComponent("MassComponent");
		
    }
	public void AddSolid(Compound c) {
			if(c.cation.index == 0 && c.anion.index == 5) { // water
				AddWater(c.mass(this)/1000f);
			} else {
				solids.Add(c);
			}
		}
	public void AddWater(float amt) {
		if(transform.parent) {
			if(transform.parent.parent) {
				if(transform.parent.parent.gameObject.GetComponent("Solution")) {
					((Solution) transform.parent.parent.gameObject.GetComponent("Solution")).AddWater(amt);
					return;
				}
			}
		}
		if(waterVolume + amt <= maxVolume) {
			ionInScalar = 1;
			waterVolume += amt;
		} else {
			ionInScalar = (maxVolume-waterVolume)/amt;
			waterVolume = maxVolume;
		}			
	}
	public void AddIon(IndQty ion) {
		if(transform.parent) {
			if(transform.parent.parent) {
				if(transform.parent.parent.gameObject.GetComponent("Solution")) {
					((Solution) transform.parent.parent.gameObject.GetComponent("Solution")).AddIon(ion);
					return;
				}
			}
		}
		if(ion.cation) {
			disCations[ion.ind].moles += ion.qty*ionInScalar;
			Precipitate(disCations[ion.ind]);
		} else {
			disAnions[ion.ind].moles += ion.qty*ionInScalar;
			Precipitate(disAnions[ion.ind]);
		}
	}
    void Update() {
        solidObject.SetActive(solids.Count > 0);
		if(maxVolume > 0) {
			waterObject.SetActive(waterVolume > 0);
			waterObject.transform.localScale = new Vector3(waterScale.x, waterScale.y, waterScale.z * waterVolume/maxVolume);
		}
		if(mc) { mc.mass = getTotalMass(); }
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
	float getTotalMass() {
		float mass = emptyMass;
		for(int i = 0; i < CATIONS; i++) { mass += disCations[i].mass(this); }
		for(int i = 0; i < ANIONS; i++) { mass += disAnions[i].mass(this); }
		foreach(Compound c in solids) { mass += c.mass(this); }
		mass += waterVolume*1000f;
		return mass;
	}
}
