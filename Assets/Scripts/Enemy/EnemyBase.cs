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
    public float speed;
    public float attackDamage;
    public float cooldownAttack;
    private bool canAttack;
    public int killValue;

    public GameObject meshLvl1;
    public GameObject meshLvl2;
    public GameObject meshLvl3;
    public GameObject meshLvl4;

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
        agent.speed = speed;
        meshLvl1.SetActive(true);
        meshLvl2.SetActive(false);
        meshLvl3.SetActive(false);
        meshLvl4.SetActive(false);
        
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
        ChangeMesh();
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

    private void ChangeMesh()
    {
        if (life <= 75 && life > 51)
        {
            meshLvl1.SetActive(false);
            meshLvl2.SetActive(true);
        }else if (life <= 50 && life > 26)
        {
            meshLvl2.SetActive(false);
            meshLvl3.SetActive(true);

            agent.speed = speed * 0.6f;
        }else if (life <= 25)
        {
            meshLvl3.SetActive(false);
            meshLvl4.SetActive(true);
            agent.speed = speed * 0.3f;
        }
    }
}
