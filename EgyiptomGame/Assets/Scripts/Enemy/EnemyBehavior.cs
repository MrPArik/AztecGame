using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour


{
    [SerializeField] int EnemyCurrentHealth=0;
    [SerializeField] int EnemyMaxHealth=100;
     [SerializeField] Animator EnemyAnimator;
     EnemyAttack enemyAttack;

private void Start() {
    EnemyCurrentHealth=EnemyMaxHealth;
    enemyAttack=GetComponentInParent<EnemyAttack>();
    
    //EnemyAnimator=GetComponent<Animator>();
}
private void Update() {
    if(EnemyCurrentHealth<=0){
        SkeletonDead();
    }
}


    public void EnemyGetHit(int damage){
            if(enemyAttack.SkeletonIsAlive==true){
        EnemyAnimator.SetTrigger("IsHit");
        EnemyCurrentHealth-=damage;
            }
        
    }

    void SkeletonDead(){
        enemyAttack.SkeletonDied();
    }
}
