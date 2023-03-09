using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaCheck : MonoBehaviour
{

    private EnemyAttack enemyAttack;
    
    private void Awake() {
        enemyAttack=GetComponentInParent<EnemyAttack>();
    }

     void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            gameObject.SetActive(false);
            enemyAttack.target=other.transform;
            enemyAttack.inRange=true;
            enemyAttack.HotZone.SetActive(true);
        }
    }

}
