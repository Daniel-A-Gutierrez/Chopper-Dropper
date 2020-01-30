using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chomper : MonoBehaviour
{
    //note: DO NOT make left negative. assign it in terms of absolute value of the units you want this boy to go left.
    public float moveLeft;
    public float moveRight;
    public Rigidbody2D chomperBody;
    public bool movingLeft;
    public SpriteRenderer chomperRenderer;

    private float initialX;
 

    // Start is called before the first frame update
    void Start()
    {
      if (!chomperBody)
        {
            Debug.Log("No rigid body in chomper prefb script.");
        }

      if (!chomperRenderer)
        {
            Debug.Log("No sprite renderer in chomper prefab");
        }
        initialX = chomperBody.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        print("czxcxz");
        if (movingLeft)
        {
            chomperBody.velocity = new Vector2(-10, 0);
            if(chomperBody.position.x <= initialX - moveLeft)
            {
                movingLeft = false;
                chomperRenderer.flipX = true;
            }
           
        }

        else if (!movingLeft)
        {
            chomperBody.velocity = new Vector2(10, 0);
            if (chomperBody.position.x >= initialX + moveRight)
            {
                movingLeft = true;
                chomperRenderer.flipX = false;
            }
            
        }

        else
        {
            Debug.Log("didn't enter moving case");
        }


    }
}
