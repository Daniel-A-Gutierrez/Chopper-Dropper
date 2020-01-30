using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string enemyTag = "Enemy";


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == enemyTag)
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject); 
        }
        if(collision.gameObject.tag == playerTag)
        {
            //Stun player
            Destroy(this.gameObject);
        }
    }
}
