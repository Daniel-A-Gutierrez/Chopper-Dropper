using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    [SerializeField] private string TagToKill = "Player";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == TagToKill)
        {
            Destroy(collision.gameObject);
        }
    }
}
