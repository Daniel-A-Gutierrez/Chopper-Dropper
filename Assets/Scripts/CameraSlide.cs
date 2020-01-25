using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSlide : MonoBehaviour
{
    public Vector3[] waypoints;
    public float speed;
    public float startupTime;
    int index = 0;
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
        if(V2EQ((Vector2)transform.position,(Vector2)waypoints[index],.05f) )
        {
            index = (index + 1) % waypoints.Length;
        }
        if(waypoints[index]==transform.position)
            transform.Translate(.001f,.001f,.001f);
        transform.Translate( (waypoints[index]-transform.position).normalized*speed*Mathf.Lerp(0,speed,Time.time/startupTime)*Time.deltaTime );
    }

    bool V2EQ(Vector2 a, Vector2 b, float tolerance = 0.01f)
    {
        return FloatComp(a.x,b.x,tolerance)==0f &&  FloatComp(a.y,b.y,tolerance)==0f;
    }

    bool V3EQ(Vector3 a, Vector3 b, float tolerance = 0.01f)
    {
        return FloatComp(a.x,b.x,tolerance)==0f &&  FloatComp(a.y,b.y,tolerance)==0f && FloatComp(a.z,b.z,tolerance)==0f;   
    }

    float FloatComp(float a, float b, float tolerance = .000001f)
    {
        if(float.IsNaN(a) || float.IsNaN(b))
            return float.NaN;
        else if(float.IsInfinity(a) && float.IsInfinity(b))
            return float.PositiveInfinity;
        else if( Mathf.Abs(Mathf.Abs(a) - Mathf.Abs(b)) < tolerance)
            return 0f;
        else if( a - b > tolerance)
            return 1f;
        else if (b-a > tolerance)
            return -1f;
        else
            return float.NaN;

    }
}
