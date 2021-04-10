using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemy : MonoBehaviour
{
    [SerializeField]
    EnemySkeleton enemySkeleton;   
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Hero"))
        {
            enemySkeleton.HeroTrigger = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Hero"))
        {
            enemySkeleton.HeroTrigger = true;
        }
    }

}
