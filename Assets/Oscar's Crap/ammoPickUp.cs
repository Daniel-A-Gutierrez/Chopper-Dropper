using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoPickUp : MonoBehaviour
{
    ammoManager aM;

    void Awake()
    {
        aM = GameObject.FindObjectOfType(typeof(ammoManager)) as ammoManager;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other == aM.walker)
        {
            aM.walkerAmmo += 1;
            Destroy(gameObject);
        }
        else if(other == aM.flyer)
        {
            aM.flyerAmmo += 1;
            Destroy(gameObject);
        }
    }
}
