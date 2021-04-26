using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStop : MonoBehaviour
{
    public GameObject timer;
	void OnTriggerEnter(Collider other) {
		timer.SendMessage("StopTimer", other.gameObject, SendMessageOptions.DontRequireReceiver);
	}
}
