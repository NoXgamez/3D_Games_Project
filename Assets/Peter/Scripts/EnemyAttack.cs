using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject hitbox;

    public void SpawnHitbox()
    {
        Debug.Log("hitbox");
        // Activate the hitbox GameObject
        hitbox.SetActive(true);

    }
    public void DespawnHitbox()
    {
        GetComponentInChildren<EnemyAttack>();
        hitbox.SetActive(false);
    }
}
