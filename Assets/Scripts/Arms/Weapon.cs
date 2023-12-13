using System.Collections;
using System.Collections.Generic;
using BaseTemplate.Behaviours;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon: MonoSingleton<Weapon>
{
[Header("Pistol Infos")] 
    public int ammo = 10;
    public int maxAmmo = 10;
    public float timeForReload = 2f;
    public float fireRate;
    public ParticleSystem fxShoot;
    private float _cdTime;

    public virtual void Fire(Transform origin){   
         
        if (ammo > 0 && _cdTime < 0)
        {
            _cdTime = fireRate;
            GameObject bullet = PoolingManager.Instance.GetPooledObject();

            if (bullet != null) {

                if (bullet.TryGetComponent<Bullet>(out Bullet compBullet))
                {
                    bullet.SetActive(true);
                    compBullet.SetAimTransform(origin));
                }

                ammo--;
                UIManager.Instance.ChangeAmmoText(ammo.ToString());
                //ShootServerRpc();
            }
        }  
    }    
}
