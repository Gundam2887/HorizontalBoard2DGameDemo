using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnmeyAttack : MonoBehaviour
{
    Parameter parameter = new Parameter();
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name);
            other.GetComponent<PlayerLife>().ChangeLife(10);
            other.GetComponent<Player>().getHit = true;
        }
    }
}
