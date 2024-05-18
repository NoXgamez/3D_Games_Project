using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        string path = $"{Application.dataPath}/StatData.json";
        Stats Data = GameUtilities.Load<Stats>(path);
        GameUtilities.Save<Stats>(Data, path);

        if(timer > 1)
            if (other.tag == "Player")
                SceneManager.LoadScene("MainMenuScene");
    }
}
