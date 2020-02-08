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
    [SerializeField] ammoManager gameManager;
    [SerializeField] float yOffset = -1f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //movement stuff
        if(Input.GetAxis("Horizontal_Chopper")!= 0)
        {
            GetComponent<Rigidbody2D>().AddForce(Input.GetAxis("Horizontal_Chopper") * moveSpeed * Vector2.right); 
        }

        //rock drop stuff
        if(Input.GetKeyDown(dropKey))
        {
            if(gameManager.checkFlyerAmmo())
            {
                GameObject droppedRock = Instantiate(rock, this.gameObject.transform.position + new Vector3(0f,yOffset,0f), Quaternion.identity);
                //droppedRock.GetComponent<Rigidbody2D>().velocity = this.gameObject.GetComponent<Rigidbody2D>().velocity;
            }
            else
            {
                //do something
                Debug.Log("Out of ammo");
            }
        }
    }
}
