using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;

public class Bullet : NetworkBehaviour
{
    [Header("BulletComponents")] 
    private TrailRenderer _trail;
    
    [Header("Bullet Stats")]
    public float bulletSpeed;
    public float speedMultiplier;
    public float bulletDmg;
    public float dmgMultiplier;
    public float delayToDestroy;

    private float cooldown;
    
    private Rigidbody _rb;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = Vector3.zero;
        _trail = GetComponent<TrailRenderer>();
        _trail.Clear();
        cooldown = delayToDestroy;
    }

    private void OnDisable()
    {
        ResetBulletInfo();
    }


    void Update()
    {
        _rb.velocity = transform.forward * (bulletSpeed * speedMultiplier * Time.deltaTime);
        cooldown -= Time.deltaTime;

        if (cooldown < 0)
        {
            Destroy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Idamageable>(out Idamageable comp))
            {
                comp.Damage(bulletDmg);
            }
            Destroy();
        }
    }

    public void Destroy()
    {
        transform.gameObject.SetActive(false);
        //DeactivateBulletServerRpc();
    }
    
    public void ResetBulletInfo()
    {
        bulletSpeed = 1000;
        speedMultiplier = 1;
        bulletDmg = 10;
        dmgMultiplier = 1;
    }
    
    
    [ServerRpc(RequireOwnership = false)]
    public void ActivateBulletServerRpc(Vector3 position, Quaternion rotation)
    {
        ActivateBulletClientRpc(position, rotation);
    }

    [ClientRpc]
    private void ActivateBulletClientRpc(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        gameObject.SetActive(true);
        
        _rb.velocity = Vector3.zero;
        _trail.Clear();
        // Initialiser d'autres paramètres de la balle ici
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeactivateBulletServerRpc()
    {
        DeactivateBulletClientRpc();
    }

    [ClientRpc]
    private void DeactivateBulletClientRpc()
    {
        gameObject.SetActive(false);
    }
}
