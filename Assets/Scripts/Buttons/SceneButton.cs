using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
	public string scene;
	public bool confirm;
	public GameObject confirmBox;
	// Start is called before the first frame update
    void Start() {
		if(!confirmBox) { confirm = false; }
	}
	void WorldClicked() {
		if(confirm) {
			confirmBox.SetActive(true);
		} else {
			StartCoroutine(LoadAsync());
		}
	}
	void OnGUI() {
		
	}
	IEnumerator LoadAsync() {
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
		
        while (!asyncLoad.isDone) {
            yield return null;
        }
	}
}
