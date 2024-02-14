using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider))]
public class EnemyBase : MonoBehaviour, Idamageable
{
    public NavMeshAgent agent;

    public float maxLife;
    public float life;
    public float attackDamage;
    public float cooldownAttack;
    private bool canAttack;
    public int killValue;

    public GameObject player;

    private void OnEnable()
    {
        Init();
        
    }

    public void Init()
    {
        agent = GetComponent<NavMeshAgent>();
        life = maxLife;
        canAttack = true;
        
        if(!player)
            player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (player)
        {
            //agent.Move(player.transform.position);
            agent.SetDestination(player.transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (TryGetComponent<Idamageable>(out Idamageable player))
            {
                Attack(player);
            }
        }
    }

    public void Damage(float damage)
    {
        if (life > damage)
        {
            life -= damage;
            OnHit();
        }
        else
        {
            Death();
        }
    }

    private void OnHit()
    {
        //Anim
    }

    private void Death()
    {
        gameObject.SetActive(false);
        GameManager.Instance.AddPlayerPoints(killValue);
        WaveManager.Instance.EnemyGetKilled();
    }

    private void Attack(Idamageable player)
    {
        if (canAttack)
        {
            player.Damage(attackDamage);
            StartCoroutine(CooldownAttack());
        }
    }

    IEnumerator CooldownAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(cooldownAttack);
        canAttack = true;
    }
}
