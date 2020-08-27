using System;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float maxHealth = 100f;

    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        this.currentHealth = this.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeHit(float damage)
    {
        this.currentHealth = Math.Max(this.currentHealth - damage, 0f);
    }

    public float GetCurrentHealth()
    {
        return this.currentHealth;
    }

    public float GetMaxHealth()
    {
        return this.maxHealth;
    }
}
