using System.Collections;
using System.Collections.Generic;
using BaseTemplate.Behaviours;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
[Header("Pistol Infos")] 
    public int ammo = 10;
    public int maxAmmo = 10;
    public float timeForReload = 2f;
    public float fireRate;
    public ParticleSystem fxShoot;
    protected float _cdTime;

    public virtual void Shoot(Transform origin){}    
}
