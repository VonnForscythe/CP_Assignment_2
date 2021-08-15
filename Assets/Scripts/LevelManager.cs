using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int startingLives;
    public Transform spawnLocation;

    
    void Start()
    {
       // GameManager.instance.SpawnPlayer(spawnLocation);
        GameManager.instance.currentLevel = GetComponent<LevelManager>();
    }


}