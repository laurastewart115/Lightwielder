using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour {

	public void openLevel(int sceneIndex) {
		SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
	}
}
