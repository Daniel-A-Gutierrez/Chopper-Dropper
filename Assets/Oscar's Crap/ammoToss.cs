using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoToss : MonoBehaviour
{
    public GameObject ammo;
    public float tossForce = 1000f;
    bool shot = false;
    public float yOffset = 1f;
    ammoManager aM;
    void Awake()
    {
        aM = GameObject.FindObjectOfType(typeof(ammoManager)) as ammoManager;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            shot = true;
        }
    }

    void FixedUpdate()
    {
        if(shot)
        {
            if(aM.checkWalkerAmmo())
            {
                GameObject newAmmo;
                newAmmo = Instantiate(ammo, gameObject.transform.position + new Vector3(0f,yOffset,0f), Quaternion.identity);
                newAmmo.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,tossForce));
            }
            shot = false;
        }
    }
}
