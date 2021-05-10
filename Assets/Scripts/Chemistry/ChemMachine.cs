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
	public float deltaTime;
	public float onTime;
	void Start() {
        anim = animObject.GetComponent<Animator>();
		anim.SetFloat("SpinSpeed", passiveSpeed);
    }

    void ProduceCompound(int[] ions) {
        anim.SetFloat("SpinSpeed", activeSpeed, dampTime, deltaTime);
		yield return new WaitForSeconds(onTime);
        anim.SetFloat("SpinSpeed", passiveSpeed, dampTime, deltaTime);
    }
}
