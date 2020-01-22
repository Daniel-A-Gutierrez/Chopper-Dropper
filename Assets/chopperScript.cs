using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chopperScript : MonoBehaviour
{

    [SerializeField] string rightKey;
    [SerializeField] string leftKey;
    [SerializeField] float moveSpeed;
    [SerializeField] string dropKey;
    [SerializeField] GameObject rock;
    //[SerializeField] ammoManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //movement stuff
        if(Input.GetKey(rightKey))
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(1 * moveSpeed, 0, 0);
        }
        else if(Input.GetKey(leftKey))
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(-1 * moveSpeed, 0, 0);
        }

        //rock drop stuff
        if(Input.GetKeyDown(dropKey))
        {
            if(true)//gameManager.checkFlyerAmmo())
            {
                GameObject droppedRock = Instantiate(rock, this.gameObject.transform.position, Quaternion.identity);
                droppedRock.GetComponent<Rigidbody2D>().velocity = this.gameObject.GetComponent<Rigidbody2D>().velocity;
            }
            else
            {
                //do something
                Debug.Log("Out of ammo");
            }
        }
    }
}
