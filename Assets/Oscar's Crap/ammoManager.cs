using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoManager : MonoBehaviour
{
    public int walkerAmmo = 0;
    public int flyerAmmo = 0;
    public Collider2D walker;
    public Collider2D flyer;

    void Update()
    {
        if(flyerAmmo>0)
        {
            if(!flyer.gameObject.GetComponent<RockPredictionLine>().enabled)
                flyer.gameObject.GetComponent<RockPredictionLine>().enabled=true;
        }
        else
        {
            if(flyer.gameObject.GetComponent<RockPredictionLine>().enabled)
                flyer.gameObject.GetComponent<RockPredictionLine>().enabled=false;
        }
    }

    public bool checkWalkerAmmo()
    {
        if(walkerAmmo > 0)
        {
            walkerAmmo -= 1;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool checkFlyerAmmo()
    {
        if(flyerAmmo > 0)
        {
            flyerAmmo -= 1;
            return true;
        }
        else
        {
            return false;
        }
    }
}