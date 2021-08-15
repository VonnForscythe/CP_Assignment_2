using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceTrap : MonoBehaviour
{
    public int jumpSpeed;
    GameObject player;

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Bounce")
        {
            player = collision.gameObject;
            Bounce();
        }
    }

    private void Bounce()
    {
        Rigidbody rig = player.GetComponent<Rigidbody>();
        rig.AddForce(player.transform.up * jumpSpeed, ForceMode.Impulse);
    }
}
