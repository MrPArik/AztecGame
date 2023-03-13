using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitboxColliderSkeleton : MonoBehaviour
{
        
        [SerializeField] Transform attackPoint;
       [SerializeField] float attackRange=0.5f;
        [SerializeField] LayerMask enemyLayers; 
        [SerializeField] int attackDamage=40;
        
       

    void AttackHit()
    {  
        

            

      Collider2D[] hitEnemis= Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);
 
        foreach(Collider2D enemy in hitEnemis)
        {
            if(enemy.GetComponent<PlayerMovement>().isDashing==false && enemy.GetComponent<PlayerMovement>().isAlive==true){
                enemy.GetComponent<Health>().GetDamage(attackDamage);
            }else{
                Debug.Log("Nem talált");
            }
           
        }
       

        
       
    }


     

    private void OnDrawGizmosSelected()
     {
        if(attackPoint==null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }//ez az attak point körul kirajzolja a kört hogy lássuk mekkora

}
