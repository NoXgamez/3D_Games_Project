using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Access the Player script (assuming it's attached to the same GameObject)
            Player playerScript = other.GetComponent<Player>();
            if (playerScript != null)
            {
                // Do something with the playerScript
                Debug.Log("Player collided!");
                playerScript.GetHit();
            }
            else
            {
                Debug.LogError("Player script not found on the colliding object!");
            }
        }
    }
}
