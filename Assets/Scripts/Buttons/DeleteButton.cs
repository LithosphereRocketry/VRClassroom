using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    public GameObject target;
	void Start() {
        if(!target) { target = gameObject; }
    }

    void WorldClicked() {
        target.SetActive(false);
    }
}
