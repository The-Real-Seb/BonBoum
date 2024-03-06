using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemylife : MonoBehaviour, Idamageable
{
    public float actualLife;
    public float maxLife;

    private void Start()
    {
        actualLife = maxLife;
    }

    public void Damage(float damage)
    {
        if (actualLife >= damage)
        {
            actualLife -= damage;
            Debug.Log("Damage");            
        }
        else
        {
            //Death
        }
    }
}
