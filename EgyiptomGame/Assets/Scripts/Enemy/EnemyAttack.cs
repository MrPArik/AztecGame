using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Greetings from Sid!

//Thank You for watching my tutorials
//I really hope you find my tutorials helpful and knowledgeable
//Appreciate your support.

public class EnemyAttack : MonoBehaviour
{
    #region Public Variables
    
    
    
    public float attackDistance; //Minimum distance for attack
    public float moveSpeed;
    public float timer; //Timer for cooldown between attacks
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
     public bool inRange; //Check if Player is in range
    public GameObject HotZone;
    public GameObject triggerArea;
    public bool SkeletonIsAlive=true;
   
    #endregion

    #region Private Variables
    
    
    private Animator anim;
    private float distance; //Store the distance b/w enemy and player
    private bool attackMode;
    
    private bool cooling; //Check if Enemy is cooling after attack
    private float intTimer;
    #endregion
    
    PlayerMovement playerMovement;

    void Awake()
    {
        SelectTarget();
        intTimer = timer; //Store the inital value of timer
        anim = GetComponent<Animator>();
        playerMovement=FindObjectOfType<PlayerMovement>();
        
    }

    void Update()
    {

        if(SkeletonIsAlive==true){

                if (!attackMode)
        {
            Move();
        }

        if (!InsideOfLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("SkeletonAttack"))
        {
            SelectTarget();
        }

        if(playerMovement.isAlive==false){
            //HotZone.SetActive(false);
           // triggerArea.SetActive(false);
            //inRange=false;
            StopAttack();
            inRange=false;
            //SelectTarget();
        }
       
        

        if (inRange)
        {
            EnemyLogic();
        }

        }
        
        
    }

    

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance && !anim.GetCurrentAnimatorStateInfo(0).IsName("SkeletonAttack"))
        {
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("CanAttack", false);
        }
    }

    void Move()
    {
        anim.SetBool("CanWalk", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SkeletonAttack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        timer = intTimer; //Reset Timer when Player enter Attack Range
        attackMode = true; //To check if Enemy can still attack or not

        anim.SetBool("CanWalk", false);
        anim.SetBool("CanAttack", true);
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    void StopAttack()
    {
        
        cooling = false;
        
        attackMode = false;
        anim.SetBool("CanAttack", false);
    }

    

    public void TriggerCooling()
    {
        cooling = true;
    }

    private bool InsideOfLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector3.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector3.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        //Ternary Operator
        //target = distanceToLeft > distanceToRight ? leftLimit : rightLimit;

        Flip();
    }

    public void Flip()
    {
        if(SkeletonIsAlive==true){
                Vector3 rotation = transform.eulerAngles;
         if (transform.position.x > target.position.x) 
            {
               rotation.y = 180;
         }
         else
         {
             Debug.Log("Twist");
             rotation.y = 0;
            }

        //Ternary Operator
        //rotation.y = (currentTarget.position.x < transform.position.x) ? rotation.y = 180f : rotation.y = 0f;

            transform.eulerAngles = rotation;
        }
        
    }

    public void SkeletonDied(){
        SkeletonIsAlive=false;
        StopAttack();
        anim.SetBool("IsDead",true);
    }


}
