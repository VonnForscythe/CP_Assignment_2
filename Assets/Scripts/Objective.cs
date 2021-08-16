using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Objective : MonoBehaviour
{
    public LevelManager currentLevel;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Objective Met");
            if (SceneManager.GetActiveScene().name == "World1")
                SceneManager.LoadScene("VictoryScreen");
            else if (SceneManager.GetActiveScene().name == "EndScreen")
                SceneManager.LoadScene("TitleScreen");
        }
    }
}