using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPredictionLine : MonoBehaviour
{
    Rigidbody2D rb2;
    LineRenderer lr;
    public Vector3 rockOffset;
    Vector3[] points;
    // Start is called before the first frame update
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        points= new Vector3[lr.positionCount];
    }

    // Update is called once per frame
    void Update()
    {
        lr.GetPositions(points);
        points[0] = transform.position + rockOffset;
        Vector3 vi = Vector3.down + transform.position;
        //Vector3 vi = rb2.velocity;
        // for(int i = 1 ; i < lr.positionCount; i++)
        // {
        //     points[i] = points[i-1] + vi*Time.fixedDeltaTime;
        //     vi += Physics.gravity*Time.fixedDeltaTime;
        // }
        points[1] = vi + Vector3.down*2;
        lr.SetPositions(points);
    }

    void OnDisable()
    {
        lr.enabled  = false;
    }

    void OnEnable()
    {
        GetComponent<LineRenderer>().enabled = true;
        lr = GetComponent<LineRenderer>();
    }
}
