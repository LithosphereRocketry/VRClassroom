using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    void WorldClicked() {
		Debug.Log("Exiting...");
		Application.Quit();
	}
}
