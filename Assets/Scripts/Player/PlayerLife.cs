using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerLife : MonoBehaviour, Idamageable
{
    public float actualLife;
    public float maxLife;

    private void Start()
    {
        actualLife = maxLife;
        UIManager.Instance.ChangeLifeValue(actualLife);
    }

    public void Damage(float damage)
    {
        if (actualLife >= damage)
        {
            actualLife -= damage;
            UIManager.Instance.ChangeLifeValue(actualLife);            
        }
        else
        {
            GameManager.Instance.ShowEndScore();
        }
    }
    /*
    [ServerRpc(RequireOwnership = false)]
    private void HitServerRpc(float damage)
    {
        ShootClientRpc(damage);
    }

    [ClientRpc]
    private void ShootClientRpc(float damage)
    {
        if (actualLife >= damage)
        {
            actualLife -= damage;
        }
        else
        {
            //Death
        }
    }*/
}
