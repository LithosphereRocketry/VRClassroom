using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemMachine : IonInfo
{
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
		((ItemGenerator) container.GetComponent("ItemGenerator")).child.SendMessage("ProduceCompound", ionInputs);
		currentTarget = passiveSpeed;
    }
	
}
