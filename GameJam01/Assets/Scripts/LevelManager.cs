using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;

public class LevelManager : NetworkBehaviour {

    public void ChangeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
