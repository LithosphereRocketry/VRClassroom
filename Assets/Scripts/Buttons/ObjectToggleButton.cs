using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToggleButton : MonoBehaviour
{
    public GameObject target;
	void WorldClicked() {
        target.SetActive(!target.activeSelf);
    }
}
