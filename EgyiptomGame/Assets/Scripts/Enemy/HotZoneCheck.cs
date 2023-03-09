using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneCheck : MonoBehaviour
{
    private EnemyAttack enemyAttack;
    bool inRange;
    Animator anim;
    
    private void Awake() {
        enemyAttack=GetComponentInParent<EnemyAttack>();
        anim=GetComponentInParent<Animator>();
    }

     void Update() {
        if(inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("SkeletonAttack")){
            enemyAttack.Flip();
        }
    }

     void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            inRange=true;
        }
    }

     void OnTriggerExit2D(Collider2D other) {
         if(other.gameObject.CompareTag("Player")){
            inRange=false;
            gameObject.SetActive(false);
            enemyAttack.triggerArea.SetActive(true);
            enemyAttack.inRange=false;
            enemyAttack.SelectTarget();
        }
    }
}
