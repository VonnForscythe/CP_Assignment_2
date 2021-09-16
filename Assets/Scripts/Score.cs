using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int Scores = 0;
    //GameObject box;


    private void OnGUI()
    {
        GUI.Box(new Rect(50, 50, 50, 50), Scores.ToString());
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        box.SetActive(false);
    //    }
    //}
}