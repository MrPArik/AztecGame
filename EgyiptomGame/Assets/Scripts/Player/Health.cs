using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float CurrentHealth;
    public float MaxHealth=100f;
    bool SebezhetiCsapda=true;
    [SerializeField] CapsuleCollider2D myBodyCollider;
     [SerializeField] Animator myAnimator;
     [SerializeField] PlayerMovement playerMovement;

     bool isAlive=true;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth=MaxHealth;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive==true){
            Csapda();
         if(CurrentHealth<=0){
              Die();
         }
        }
        
    }

    void Csapda(){
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Csapda"))){
            playerMovement.CsapdaSebzesMethod();
            if(SebezhetiCsapda==true){
                   StartCoroutine(CsapdaSebzodes());
            }
           } 
        else{
             playerMovement.NincsCsapdaSebzesMethod();
        }
    }

    private IEnumerator CsapdaSebzodes()
    {
        SebezhetiCsapda=false;
        playerMovement.CsapdaSebzesMethod();
        myAnimator.SetTrigger("IsHurt");
        CurrentHealth=CurrentHealth-100;
        yield return new WaitForSeconds(2f);
         SebezhetiCsapda=true;
        
        
    }

    public void GetDamage(int damage){
        if(isAlive==true){
                CurrentHealth=CurrentHealth-damage;
        myAnimator.SetTrigger("IsHurt");
        }
        
    }

    void Die(){
            isAlive=false;
            playerMovement.Death();
    }

}
