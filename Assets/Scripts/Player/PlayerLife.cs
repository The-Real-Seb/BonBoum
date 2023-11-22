using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerLife : NetworkBehaviour, Idamageable
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
            //HitServerRpc(damage);
        }
        else
        {
            //Death
        }
    }
    
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
    }
}
