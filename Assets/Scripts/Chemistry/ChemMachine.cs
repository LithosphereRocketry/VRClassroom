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
	void Start() {
        anim = animObject.GetComponent<Animator>();
		anim.SetFloat("SpinSpeed", passiveSpeed);
		currentTarget = passiveSpeed;
    }
	void Update() {
		anim.SetFloat("SpinSpeed", currentTarget, dampTime, Time.deltaTime);
	}

    IEnumerator ProduceCompound(int[] ionInputs) {
		currentTarget = activeSpeed;
		yield return new WaitForSeconds(onTime);
		GameObject gen = ((ItemGenerator) container.GetComponent("ItemGenerator")).child.gameObject;
		gen.SendMessage("AddWater", fluidQty);
		gen.SendMessage("AddIon", new IndQty(ionInputs[0], ionQty, true), SendMessageOptions.RequireReceiver);
		gen.SendMessage("AddIon", new IndQty(ionInputs[1], ionQty, false), SendMessageOptions.RequireReceiver);
		gen.SendMessage("AddWater", 0, SendMessageOptions.RequireReceiver);
		currentTarget = passiveSpeed;
    }
	
}
