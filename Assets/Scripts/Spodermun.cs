using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Spodermun : MonoBehaviour
{

    public bool swingingEnabled;
    Dictionary<string,Action> States;
    [SerializeField]
    string state = "DefaultState";
    Rigidbody2D rb;

    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode jump = KeyCode.Space;

    public float walkSpeed = 4;
    public float airControl = .1f;
    public float stunDuration = 1f;
    public float jumpMaxHeight = 250;//idk what the fuck units these are but it works
    public float jumpMinHeight = 150;
    public float jumpCooldown = .1f;
    public float gravityScale = .5f;
    public float maxAirSpeed = 16;
    public float fallSpeedCap = 32;
    public float RoofCheckOffset = .32f;
    public float GroundCheckOffset =.38f;
    [Range(0,4)]
    public int numJumps = 1;

    
    public Vector2 RoofCheckBounds;
    public Vector2 GroundCheckBounds;
    public string groundLayer;
    public string roofLayer;

    float currentGravity;
    float lastStunTime;
    bool gravityOn = true;
    bool grounded;
    bool roofed;
    float lastJumpTime;
    Vector2 moveVec;
    int jumpsRemaining;

    public GameObject hook;
    public float hookSpawnOffset;
    public float ropeLength;
    public float maxRopeLength;
    GameObject hookInstance;
    [SerializeField]
    Vector2 aimdir;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        States = new Dictionary<string, Action>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        States["DefaultState"] = DefaultState;
        States["JumpingState"] = JumpingState;
        States["FallingState"] = FallingState;
        States["StunnedState"] = StunnedState;
        ResetGravity();
    }

    void FixedUpdate()
    {
        if(hookInstance!=null)
            if(hookInstance.GetComponent<Hook>().affixed)
                Swinging();
        if(moveVec!=Vector2.zero)
            rb.MovePosition(rb.position+moveVec);
        if(gravityOn)
            FUApplyGravity();
    }
//
    void Update()
    {
        gravityOn=false;//override this in state if gravity should be on.
        States[state]();
        animator.SetFloat("YVelocity", (-moveVec.y/Mathf.Sqrt(2 * gravityScale * jumpMinHeight)*Time.fixedDeltaTime )*1.25f);
        GetComponent<SpriteRenderer>().flipX = lastNonZeroMoveDir.x < 0;
        if(swingingEnabled)
            SwingingStuff();
    }



    void SwingingStuff()
    {
        SetAimDir();
        if(Input.GetMouseButtonDown(0) && hookInstance == null)
        {
            FireHook();
        }
        if(Input.GetMouseButtonUp(0) && hookInstance != null)
        {
            ReleaseHook();
        }
    }

    public void SwingResetJump()
    {
        if(jumpsRemaining < numJumps)
        {
            jumpsRemaining ++;
        }
    }

    //swinging is not a state , but an action that can be performed under all states.
    void Swinging()
    {
        //yay linear algebra time. if im at max disance, cancel my velocity that is perpindicular to 
        //the line of tension. superceeds other movement. 

        if((hookInstance.transform.position-transform.position).magnitude > ropeLength)
        {
            moveVec -= (Vector2)(hookInstance.transform.position - transform.position).normalized* Mathf.Min( 0,
                Vector2.Dot( (hookInstance.transform.position - transform.position).normalized , moveVec ) );
            moveVec += (Vector2)(hookInstance.transform.position - transform.position).normalized*
                ((hookInstance.transform.position - transform.position).magnitude - ropeLength)*.01f;
        }
    }


//states

    void EnterDefaultState()
    {
        state = "DefaultState";
        jumpsRemaining = (int)numJumps;
        animator.Play("Idle");
        lastMoveDir=Vector2.zero;
        DefaultState();
    }
    Vector2 lastMoveDir = Vector2.zero;
    Vector2 lastNonZeroMoveDir = Vector2.zero;
    void DefaultState()
    {
        SetMoveDir();
        if(lastMoveDir==Vector2.zero && moveVec!=Vector2.zero)
        {
            animator.Play("Walk");
            GetComponent<AudioManager>().PlayIfNotPlaying("Walk");
        }
        else if(moveVec==Vector2.zero && lastMoveDir!=Vector2.zero)
        {
            animator.Play("Idle");
            GetComponent<AudioManager>().StopIfPlaying("Walk");
        }
        lastMoveDir=moveVec;
        if(moveVec!=Vector2.zero)
            lastNonZeroMoveDir = moveVec;
        CheckGrounded();
        CheckRoofed();
        ApplyGravity();
        if(!roofed && Input.GetKeyDown(jump)&&Time.time-lastJumpTime > jumpCooldown && jumpsRemaining>0 )
        {
            ExitDefaultState();
            EnterJumpingState();
            return;
        }
        if(!grounded)
        {
            ExitDefaultState();
            EnterFallingState();
            return;
        }
    }
    void ExitDefaultState()
    {

    }

    void EnterJumpingState()
    {
        lastJumpTime = Time.time;
        jumpsRemaining--;
        state = "JumpingState";
        GetComponent<AudioManager>().ForcePlay("Jump");
        //max height is going to be under lowered gravity, min under normal gravity.
        /*so v = root( 2ax ) */
        moveVec.y = Mathf.Sqrt(2 * gravityScale * jumpMinHeight)*Time.fixedDeltaTime;
        //but now i want it to only cancel once its reached max height
        currentGravity = moveVec.y *moveVec.y /  (2*jumpMaxHeight*Time.fixedDeltaTime*Time.fixedDeltaTime);
        animator.Play("Jump");
        JumpingState();
    }
    void JumpingState()
    {
        if(!Input.GetKey(jump))
        {
            ResetGravity();
        }
        CheckRoofed();
        CheckGrounded();
        
        if(grounded && Time.time - lastJumpTime > jumpCooldown)
        {
            ExitJumpingState();
            EnterDefaultState();
            return;
        }

        if(roofed || moveVec.y < 0)
        {
            ExitJumpingState();
            EnterFallingState();
            return;
        }

        ApplyGravity();
        SetMoveDirAir();
    }
    void ExitJumpingState()
    {
        ResetGravity();
        moveVec.y = 0;
    }

    void EnterFallingState()
    {
        state = "FallingState";
        animator.Play("Jump");
        FallingState();
    }
    void FallingState()
    {
        CheckGrounded();
        CheckRoofed();
        ApplyGravity();
        if(roofed)
        {
            ApplyGravity();
        }
        SetMoveDirAir();
        if(grounded)
        {
            ExitFallingState();
            EnterDefaultState();
            return;
        }
        if(!roofed && Input.GetKeyDown(jump)&&Time.time-lastJumpTime > jumpCooldown && jumpsRemaining>0 )
        {
            ExitDefaultState();
            EnterJumpingState();
            return;
        }
    }
    void ExitFallingState()
    {
        moveVec.y = 0;
    }

    void EnterStunnedState()//EnterStunnedState
    {
        if(state!="StunnedState")
        {
            lastStunTime = Time.time;
            state = "StunnedState";
            animator.Play("Idle");
            StunnedState();
        }
    }

    void StunnedState()
    {
        //graphical representation of stun.
        if(Time.time > lastStunTime+stunDuration)
        {
            ExitStunnedState();
            EnterDefaultState();
            return;
        }

        moveVec-= new Vector2(moveVec.x,0);
        lastMoveDir=moveVec;
        if(moveVec!=Vector2.zero)
            lastNonZeroMoveDir = moveVec;
        CheckGrounded();
        CheckRoofed();
        ApplyGravity();


    }

    void ExitStunnedState()
    {

    }

    


    void OnCollisionStay2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            EnterStunnedState();
            Destroy(col.gameObject); // make this a function later.
        }
    }









//useful functions


    void FireHook()
    {
        hookInstance = Instantiate(hook,transform.position + (Vector3)(aimdir) * hookSpawnOffset, Quaternion.identity );
        hookInstance.GetComponent<Hook>().Controller = this;
        hookInstance.transform.right=aimdir;
    }

    void ReleaseHook()
    {
        hookInstance.GetComponent<Hook>().Release();
    }

    public void DestroyHook()
    {
        hookInstance = null;
    }

    void SetAimDir()
    {
        Vector2 cursor = GameObject.FindGameObjectWithTag("MainCamera").
            GetComponent<Camera>().ScreenPointToRay(Input.mousePosition).origin;
        aimdir = (cursor - (Vector2)transform.position).normalized;
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

    Collider2D[] colliders;
    bool CheckBoxOverlap( Vector2 center, Vector2 size, float rotation, LayerMask checkLayers)
    {
        if(colliders == null)
            colliders = new Collider2D[24];
        ContactFilter2D c = new ContactFilter2D();
        c.layerMask = checkLayers;
        c.useLayerMask = true;
        return Physics2D.OverlapBox(center,size, rotation,c,colliders) > 0;
    }


    void ZeroMoveDir()
    {
        moveVec= Vector2.zero;
    }

    //moveVec
    void SetMoveDir()
    {
        moveVec = ((Input.GetKey(left) ? Vector2.left : Vector2.zero) +
         (Input.GetKey(right) ? Vector2.right : Vector2.zero)).normalized *walkSpeed*Time.fixedDeltaTime;
    }

    //applies input to moveVec without resetting Y. takes air control into account. 
    void SetMoveDirAir()
    {
       
        float apply = ((Input.GetKey(left) ? -1 : 0) +
            (Input.GetKey(right) ? 1 : 0))*walkSpeed*Time.fixedDeltaTime*airControl;
        if(  FloatComp( Mathf.Abs(moveVec.x) , Mathf.Abs(walkSpeed * Time.fixedDeltaTime)  ) < 0)
            {moveVec.x += apply;}
        else if( FloatComp(  moveVec.x , walkSpeed*Time.fixedDeltaTime) >= 0  && apply < 0) //allow decelleration but not accelleration above max. 
            {moveVec.x += apply; }
        else if( FloatComp ( moveVec.x , -walkSpeed*Time.fixedDeltaTime) <= 0 && apply > 0)
            {moveVec.x += apply;}
    }

    //moveVec
    void ApplyGravity() //rn gravity is linear
    {
        gravityOn = true;
    }

    void FUApplyGravity()
    {
        if(gravityOn && moveVec.y >= -fallSpeedCap*Time.fixedDeltaTime && !grounded)
            moveVec += Vector2.down * currentGravity * Time.fixedDeltaTime;
    }

    //grounded
    void CheckGrounded()
    {
        grounded = CheckBoxOverlap(transform.position + Vector3.up*GroundCheckOffset, GroundCheckBounds, 0f, 1 <<LayerMask.NameToLayer( groundLayer ));
    }

    //roofed
    void CheckRoofed()
    {
        roofed = CheckBoxOverlap( transform.position + Vector3.up*RoofCheckOffset, RoofCheckBounds, 0f, 1 << LayerMask.NameToLayer( roofLayer ));
    }

    //gravityScale , lastGravity
    void GravityOn()
    {
        currentGravity = gravityScale;
    }

    //gravityScale, lastGravity
    void GravityOff()
    {
        currentGravity = 0;
    }

    //gravityScale, lastGravity
    void SetGravity(float g)
    {
        if(g!=currentGravity)
        {
            currentGravity = g;
        }
    }

    void ResetGravity()
    {
        currentGravity = gravityScale;
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + Vector3.up*GroundCheckOffset , GroundCheckBounds);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.up*RoofCheckOffset , RoofCheckBounds);
    }

}
