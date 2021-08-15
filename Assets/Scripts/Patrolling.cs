using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolling : MonoBehaviour
{
    public Transform[] points;
    int current;
    public float speed;


  
    void Start()
    {
        current = 0;
    }


    void Update()
    {
        if (transform.position != points[current].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[current].position, speed * Time.deltaTime);
        }
        else
            //current = (current + 1) % points.Length;
            current = (Random.Range(0, 4)) % points.Length;
    }
            
}
