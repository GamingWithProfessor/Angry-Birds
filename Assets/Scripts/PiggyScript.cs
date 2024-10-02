using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiggyScript : MonoBehaviour
{
    [SerializeField] private float _MaxHealth = 3f;
    [SerializeField] private float _DamageThreshold = 0.2f;

    private float _CurrentHealth;

    private void Awake()
    {
        _CurrentHealth = _MaxHealth;
    }

    public void DamagePiggy(float DamageAmount)
    {
        _CurrentHealth -= DamageAmount;

        if (_CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.RemovePiggy(this);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactVelocity = collision.relativeVelocity.magnitude;

        if (impactVelocity > _DamageThreshold)
        {
            DamagePiggy(impactVelocity);
        } 
    }
}
