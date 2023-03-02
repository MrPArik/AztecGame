using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float RunSpeed=7f;
    [SerializeField] float JumpSpeed=7f;
    const float groundCheckRadius=0.3f;
     [SerializeField] bool isGrounded=false;
    [SerializeField] LayerMask GroundLayer;
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D myFeetCollider;
    Animator myAnimator;
    [SerializeField] Transform groundCheckCollider;

    

    bool canDash=true;  //mehet-e a dash
    bool isDashing=false;	//most megy-e a dash
    float dashingPower=12f; //mekkroa távot teszl vele
    float dashingTime=0.2f; //meddig tart
    float dashingColdown=1f; 

    float GravityAtStart;
    [SerializeField] float climbSpeed=7f;
   

   


   
    void Start()
    {
        myRigidbody=GetComponent<Rigidbody2D>();
         myAnimator=GetComponent<Animator>();
         myCapsuleCollider=GetComponent<CapsuleCollider2D>();
         myFeetCollider=GetComponent<BoxCollider2D>();
         GravityAtStart=myRigidbody.gravityScale;
    }

  
    void Update()
    {
       if(isDashing==true){
            return; //ha épp dashelünk akkor semmi mást se tudunk csinálni
        }
        
        
       
        Run();
        ClimbWall();

       
            GroundCheck();
        
        
        FlipSprite();
       
       myAnimator.SetFloat("yVelocity",myRigidbody.velocity.y);
        
    }

    void OnMove(InputValue value)
    {
        moveInput=value.Get<Vector2>();
        
    }

    void Run()
    {

        
        Vector2 playerVelocity=new Vector2(moveInput.x*RunSpeed,myRigidbody.velocity.y);
        
        myRigidbody.velocity=playerVelocity;

        if(Mathf.Abs(myRigidbody.velocity.x)>Mathf.Epsilon)   
        {
            myAnimator.SetBool("IsRunning",true);
        }
        else 
        {
            myAnimator.SetBool("IsRunning",false);
        }
        
        

    }

    void OnJump(InputValue value){
        if( myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Talaj")))//vagy if(isGrounded==true)
        {  

        if(value.isPressed){
            myRigidbody.velocity=new Vector2(myRigidbody.velocity.x,JumpSpeed);
            
           
        }
        }
    }

    void OnRoll(InputValue value){
        if(isGrounded==true)
        {  
        if(value.isPressed && canDash) 
            {
		//ha élünk és megnyonjuka  gombot plusz dashelhetünk
                  StartCoroutine(Dash()); //meghivjuk a dash fügvényt
                 
                
            }  

       
        }

    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = myRigidbody.gravityScale;
       myRigidbody.gravityScale = 0f;
       myAnimator.SetTrigger("IsRolling");
        myRigidbody.velocity = new Vector2(transform.localScale.x * dashingPower, 0f); //megkapjuk merre nézünk és arra megyünk a – azért van mert //nekem forditva van valamiért 
       
        yield return new WaitForSeconds(dashingTime);
        
        myRigidbody.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingColdown);
        canDash = true;
    }

    void ClimbWall(){
        myAnimator.SetBool("IsCLimbing",false);
    
        if(myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("MaszoFal"))){
           
            
                myRigidbody.gravityScale=0f;
            Vector2 playerVelocity=new Vector2(myRigidbody.velocity.x,climbSpeed*moveInput.y);
            myRigidbody.velocity=playerVelocity;
        
           

           bool playerHasVerticalSpeed=Mathf.Abs(myRigidbody.velocity.y)>Mathf.Epsilon;

                if(playerHasVerticalSpeed){
                myAnimator.SetBool("IsCLimbing",true);
               } 
            
            else{
                
                
                myAnimator.SetBool("IsCLimbing",false);
            }
        }else{
                myRigidbody.gravityScale=GravityAtStart;

        }
            
               
            }
        
        
    


    void FlipSprite()
    {
        bool playerHasHorizantalSpeed= Mathf.Abs(myRigidbody.velocity.x)>Mathf.Epsilon;

        if(playerHasHorizantalSpeed)
        {
            transform.localScale=new Vector2(Mathf.Sign(myRigidbody.velocity.x),1f);

        }

        
    }

    

    void GroundCheck(){
       
 if(myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("MaszoFal"))){
            isGrounded=true;
             myAnimator.SetBool("IsJumping",!isGrounded);
        }
        else{
            isGrounded=false;

        Collider2D[] colliders=Physics2D.OverlapCircleAll(groundCheckCollider.position,groundCheckRadius,GroundLayer);
        if(colliders.Length>0){
            isGrounded=true;
            
        }
        myAnimator.SetBool("IsJumping",!isGrounded);

        }
        }

        
        
    }



