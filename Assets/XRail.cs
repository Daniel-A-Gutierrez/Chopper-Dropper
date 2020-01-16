using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRail : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void LateUpdate()
    {
        transform.position = new Vector3(GameManager.instance.Player.transform.position.x, transform.position.y,transform.position.z);
    }
}
