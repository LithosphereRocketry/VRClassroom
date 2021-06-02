using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemMachine : IonInfo
{
    public float fluidQty;
	public float ionQty;
	public GameObject container;
	public GameObject animObject;
    private Animator anim;
	public float passiveSpeed;
	public float activeSpeed;
	public float dampTime;
	public float onTime;
	float currentTarget;
	int[] noCompound = {-1, -1};
	void Start() {
		base.Start();
        anim = animObject.GetComponent<Animator>();
		anim.SetFloat("SpinSpeed", passiveSpeed);
		currentTarget = passiveSpeed;
    }
	void Update() {
		anim.SetFloat("SpinSpeed", currentTarget, dampTime, Time.deltaTime);
	}
	void WorldClicked(GameObject hand) {
		StartCoroutine(ProduceCompound(noCompound));
	}

    IEnumerator ProduceCompound(int[] ionInputs) {
		int cat = ionInputs[0];
		int an = ionInputs[1];
		currentTarget = activeSpeed;
		yield return new WaitForSeconds(onTime);
		GameObject gen = ((ItemGenerator) container.GetComponent("ItemGenerator")).child.gameObject;
		gen.SendMessage("AddWater", fluidQty);
		if(cat>0) { gen.SendMessage("AddIon", new IndQty(cat, ionQty*nCations(cat, an), true), SendMessageOptions.RequireReceiver); }
		if(an>0) { gen.SendMessage("AddIon", new IndQty(an, ionQty*nAnions(cat, an), false), SendMessageOptions.RequireReceiver); }
		gen.SendMessage("AddWater", 0, SendMessageOptions.RequireReceiver);
		currentTarget = passiveSpeed;
    }
	
}
