using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{

    public Animator myAnimatorAttack;
    public bool isAttacking=false;
    public static Attack instance;
    [SerializeField] PlayerMovement playerMovement;
    
    
    
    public Transform attackPoint;
    public float attackRange=0.5f;
    public LayerMask enemyLayers; //ide az enemy layert kell kiválasztani
    int attackDamage=20;

    [SerializeField]  float AttackRate=2f; //hányszor üthetünk egy másodperc alatt
    float nextAttackTime=0f; 

    
    private void Awake() {
        instance=this;
    }

    
    

    public void OnFire(InputValue value){
        if(Time.time >=nextAttackTime){ 

        if(value.isPressed && !isAttacking && playerMovement.isGrounded==true){
            StartCoroutine(attacking());

        }
          nextAttackTime=Time.time+0.5f/AttackRate; //a jelenlegi idő meg ½ //jelenleg azaz 0.5sec

        }
     } 
    
    
    private IEnumerator attacking()
    {
        playerMovement.Attacking();
            isAttacking=true;
            
        yield return new WaitForSeconds(0.2f);
        
         playerMovement.NotAttacking();
        isAttacking=false;
        
    }

    public void AnimationAttack(){
                   
    { 
       

      Collider2D[] hitEnemis= Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);
 //ez csinél egy kört az attack point körül a surát az attackRange és amilyen layerek benne vannak ebbe azokat megjegyzi és a hitEnemies colliderbe menti el öket.
        foreach(Collider2D enemy in hitEnemis)
        {
           enemy.GetComponent<EnemyBehavior>().EnemyGetHit(attackDamage);
           Debug.Log("talat");
        }


    }
    

    }
        private void OnDrawGizmosSelected()
     {
        if(attackPoint==null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }//

    
    

}
