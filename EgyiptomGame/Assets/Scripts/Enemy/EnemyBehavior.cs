using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour


{
    [SerializeField] int EnemyCurrentHealth=0;
    [SerializeField] int EnemyMaxHealth=100;
    Animator EnemyAnimator;

private void Start() {
    EnemyCurrentHealth=EnemyMaxHealth;
    
    EnemyAnimator=GetComponent<Animator>();
}


    public void EnemyGetHit(int damage){
        EnemyAnimator.SetTrigger("IsHit");
        EnemyCurrentHealth=EnemyCurrentHealth-damage;
    }
}
