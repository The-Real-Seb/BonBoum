using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider))]
public class EnemyBase : MonoBehaviour
{
    public NavMeshAgent agent;   
    public float speed;
    public float attackDamage;
    public float cooldownAttack;
    private float actualCdTime;
    private bool canAttack;
    public int killValue;
    public Enemylife lifeComponent;

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
        lifeComponent = GetComponent<Enemylife>();        
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
        if (GameManager.Instance.isGamePaused) { agent.isStopped = true; return; }
        else if (player)
        {
            agent.isStopped = false;
            //agent.Move(player.transform.position);
            agent.SetDestination(player.transform.position);
        }

        if (actualCdTime >= 0)
        {
            actualCdTime -= Time.deltaTime;
        }
    }

   /* private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Damage");
            if (collision.TryGetComponent(out Idamageable player))
            {

                if (canAttack && actualCdTime <= 0)
                {
                    player.Damage(attackDamage);
                    actualCdTime = cooldownAttack;
                }
            }
        }
    }*/

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {            
            if (collision.TryGetComponent(out Idamageable player))
            {

                if (canAttack && actualCdTime <= 0)
                {
                    player.Damage(attackDamage);
                    actualCdTime = cooldownAttack;
                }
            }
        }
    }
      

    private void OnHit()
    {
        //Anim
        //ChangeMesh();
    }

    public void Death()
    {
        gameObject.SetActive(false);
        GameManager.Instance.AddPlayerPoints(killValue);
        WaveManager.Instance.EnemyGetKilled();
    }

       

    public void ChangeMesh(float life)
    {
        Debug.Log(life);
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
