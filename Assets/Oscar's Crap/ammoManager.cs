using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ammoManager : MonoBehaviour
{
    public int walkerAmmo = 0;
    public int flyerAmmo = 0;
    public Collider2D walker;
    public Collider2D flyer;
    GameObject canvas;
    GameObject cam;
    Vector3 initPos;
    Vector3 lastPos;
    void Awake()
    {

        cam = GameObject.Find("Main Camera");
        canvas= GameObject.Find("Canvas");
        initPos = cam.transform.position;
        lastPos = cam.GetComponent<CameraSlide>().waypoints[cam.GetComponent<CameraSlide>().waypoints.Length-1];
    }

    void Update()
    {
        canvas.transform.Find("rocknum").GetComponent<TextMeshProUGUI>().text = ("" + flyerAmmo);
        float progress = (cam.transform.position-initPos).magnitude / (lastPos-initPos).magnitude;
        canvas.transform.Find("distance").GetComponent<TextMeshProUGUI>().text = ("" + (int)(progress*100) + "/100"); 

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