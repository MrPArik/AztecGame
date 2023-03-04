using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHealth=100;
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth=MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -=damage;
        Debug.Log(currentHealth);
    }

}
