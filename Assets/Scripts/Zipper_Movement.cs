using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zipper_Movement : MonoBehaviour
{
    [SerializeField] Transform goToPosition;
    [SerializeField] Transform startPosition;
    [SerializeField] float moveSpeed;
    private bool goingUp = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void FixedUpdate() 
    {
        
        if(goingUp)
        {
            if(this.gameObject.transform.position.y >= goToPosition.transform.position.y)
            {
                goingUp = false;
            }
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, goToPosition.transform.position, moveSpeed);
        }
        else if(!goingUp)
        {
            if(this.gameObject.transform.position.y <= startPosition.transform.position.y)
            {
                goingUp = true;
            }
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, startPosition.transform.position, moveSpeed);            
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            //Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
}
