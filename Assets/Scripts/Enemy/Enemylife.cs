using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemylife : MonoBehaviour, Idamageable
{
    public float actualLife;
    public float maxLife;

    private EnemyBase enemyBase;

    private void Start()
    {
        actualLife = maxLife;
        enemyBase = GetComponent<EnemyBase>();
    }

    public void Damage(float damage)
    {
        if (actualLife >= damage)
        {
            actualLife -= damage;
            Debug.Log("Damage");
            enemyBase.ChangeMesh(actualLife);
        }
        else
        {
            enemyBase.Death();
        }
    }
}
