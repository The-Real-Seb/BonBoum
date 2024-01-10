using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPistol : Weapon
{
   public override void Shoot(Transform origin){


    if (ammo > 0 && _cdTime < 0)
        {
             Debug.Log("Basic  Pistol Teste");

            _cdTime = fireRate;

            GameObject bullet = PoolingManager.Instance.GetPooledObject();

            if (bullet != null) {

                if (bullet.TryGetComponent<Bullet>(out Bullet compBullet))
                {
                    bullet.SetActive(true);
                    compBullet.SetAimTransform(origin);
                }

                ammo--;
                UIManager.Instance.ChangeAmmoText(ammo.ToString());
                //ShootServerRpc();
            }
        }  
   }
}
