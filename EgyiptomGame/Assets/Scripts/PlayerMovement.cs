using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float RunSpeed=7f;
    [SerializeField] float JumpSpeed=7f;
    const float groundCheckRadius=0.3f;
     public bool isGrounded=false;
    [SerializeField] LayerMask GroundLayer;
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D myFeetCollider;
    Animator myAnimator;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] bool isAlive=true;

    

    bool canDash=true;  //mehet-e a dash
    bool isDashing=false;	//most megy-e a dash
    float dashingPower=12f; //mekkroa távot teszl vele
    float dashingTime=0.2f; //meddig tart
    float dashingColdown=1f; 

    float GravityAtStart;
    [SerializeField] float climbSpeed=7f;
   

   public Transform ledgeCheck;
   public Transform wallCheck;
   bool isTouchingLedge;
   bool isTouchingWall;
   bool canClimbLedge;
   bool ledgeDetected;
    bool isFacingRight;

    Vector2 ledgePosBot;
    Vector2 ledgePos1;
    Vector2 ledgePos2; 

    [SerializeField] float wallCheckDistance;
    [SerializeField] float ledgeClimbXOffset1=0f;
    [SerializeField] float ledgeClimbYOffset1=0f;
    [SerializeField] float ledgeClimbXOffset2=0f;
    [SerializeField] float ledgeClimbYOffset2=0f;


    public bool CsapdaSebzes=false;

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
        if(isAlive){

            if(isDashing==true){
            return; //ha épp dashelünk akkor semmi mást se tudunk csinálni
        }
        
        
       FacingRight();
        Run();
        ClimbWall();

        CheckSurroundings();
        CheckLedgeClimb();
        if(CsapdaSebzes==false)
        {
            GroundCheck();
        }else{
            myAnimator.SetBool("IsJumping",false);
        }
            
        
        
        FlipSprite();
       
       myAnimator.SetFloat("yVelocity",myRigidbody.velocity.y);

        }
           
            
                        
            
        }
       
        
    

    void FacingRight(){
        if(transform.localScale.x>0){
            isFacingRight=true;
            
        }else{
            isFacingRight=false;
            
        }
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
        if(isAlive){
            if( myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Talaj")))//vagy if(isGrounded==true)
        {  

        if(value.isPressed){
            myRigidbody.velocity=new Vector2(myRigidbody.velocity.x,JumpSpeed);
            
           
        }
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

        void CheckLedgeClimb(){
            if(ledgeDetected && !canClimbLedge){
                canClimbLedge=true;

                if(isFacingRight){
                    ledgePos1=new Vector2(Mathf.Floor(ledgePosBot.x+wallCheckDistance)-ledgeClimbXOffset1,Mathf.Floor(ledgePosBot.y)+ledgeClimbYOffset1);
                  ledgePos2=new Vector2(Mathf.Floor(ledgePosBot.x+wallCheckDistance)+ledgeClimbXOffset2,Mathf.Floor(ledgePosBot.y)+ledgeClimbYOffset2);
                }else{
                    ledgePos1=new Vector2(Mathf.Ceil(ledgePosBot.x-wallCheckDistance)+ledgeClimbXOffset1,Mathf.Floor(ledgePosBot.y)+ledgeClimbYOffset1);
                  ledgePos2=new Vector2(Mathf.Ceil(ledgePosBot.x-wallCheckDistance)-ledgeClimbXOffset2,Mathf.Floor(ledgePosBot.y)+ledgeClimbYOffset2);
                }
                isDashing=true;

                myAnimator.SetBool("canClimbLedge",canClimbLedge);
            }
            
            if(canClimbLedge){
                transform.position=ledgePos1;
                myRigidbody.velocity=new Vector2(0f,0f);
                myRigidbody.gravityScale=0f; //mivel wallclimbe be van rakva ha nem érjük a falat a gravitáció alap lesz ezért ezt nem kell vissza csinélni
            }
        }

        public void FinnishLedgeCLimb(){
            canClimbLedge=false;
            transform.position=ledgePos2;
            isDashing=false;
            ledgeDetected=false;
             myAnimator.SetBool("canClimbLedge",canClimbLedge);
        }

        
        void CheckSurroundings(){
            if(isFacingRight){
                 isTouchingLedge=Physics2D.Raycast(ledgeCheck.position,transform.right,wallCheckDistance,GroundLayer);
           isTouchingWall=Physics2D.Raycast(wallCheck.position,transform.right,wallCheckDistance,GroundLayer);
            }else if(!isFacingRight){
                  isTouchingLedge=Physics2D.Raycast(ledgeCheck.position,-transform.right,wallCheckDistance,GroundLayer);
           isTouchingWall=Physics2D.Raycast(wallCheck.position,-transform.right,wallCheckDistance,GroundLayer);
            }
           

            if(isTouchingWall && !isTouchingLedge && !ledgeDetected){
                ledgeDetected=true;
                ledgePosBot=wallCheck.position;
            }

        }

        public void CsapdaSebzesMethod(){
            CsapdaSebzes=true;
        }
         public void NincsCsapdaSebzesMethod(){
            CsapdaSebzes=false;
        }

        public void Death(){
            isAlive=false;
            myAnimator.SetBool("IsRunning",false);
            myAnimator.SetBool("IsJumping",false);
            myAnimator.SetBool("IsDead",true);
            myRigidbody.velocity=new Vector2(0f,-6f);
            
             if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Talaj"))){
                myRigidbody.velocity=new Vector2(0f,0f);
                myRigidbody.gravityScale=0f;
                myCapsuleCollider.enabled=false;
                this.enabled=false;

            }
        
        
    }
}


