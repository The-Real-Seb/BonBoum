using System.Collections;
using System.Collections.Generic;
using BaseTemplate.Behaviours;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using UnityEngine.Animations;

public class Pistol : MonoBehaviour
{
    [Header("Basics")]
    public Transform bulletOrigin;
    private bool _isPerformed = false;

    [Header("Pistol Infos")] 
    public int ammo = 10;
    public int maxAmmo = 10;
    public float timeForReload = 2f;
    public float fireRate;
    public ParticleSystem fxShoot;
    private float _cdTime;
    

    [Header("Aim Infos")] 
    public GameObject aim;
    public LayerMask layerMask;
    private Camera _camera;
    private AimConstraint _constraint;
    
    public Vector3 hitPoint;

    
    private void Start()
    {
        //if (!IsOwner) return;
        _camera = Camera.main;
        UIManager.Instance.ChangeAmmoText(ammo.ToString());
    }

    private void Update()
    {
        //if (!IsOwner) return;
        //CoolDown entre les tirs
        if (_cdTime >= 0)
        {
            _cdTime -= Time.deltaTime;
            
        }
        else
        {
            //SetAim();
        }

        if (_isPerformed && ammo <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(timeForReload);
        ammo = maxAmmo;
        UIManager.Instance.ChangeAmmoText(ammo.ToString());
    }

    public void Fire(InputAction.CallbackContext context)
    {
        //if (!IsOwner) return;
        //Listener du bouton de tir
        if (context.started)
        {
            SetAim();
            _isPerformed = true;
            fxShoot.Play();
            Shoot();
        }
            
        if (context.canceled)
            _isPerformed = false;
    }

    void Shoot()
    {
        //Tirs
        if (ammo > 0 && _cdTime < 0)
        {
            _cdTime = fireRate;
            GameObject bullet = PoolingManager.Instance.GetPooledObject();

            if (bullet != null) {
                
                if (bullet.TryGetComponent<Bullet>(out Bullet compBullet))
                {
                    bullet.SetActive(true);
                    compBullet.SetAimTransform(bulletOrigin);
                }
                
                ammo--;
                UIManager.Instance.ChangeAmmoText(ammo.ToString());
                //ShootServerRpc();
            }
        }
    }
    
    /*
    [ServerRpc]
    private void ShootServerRpc()
    {
        ShootClientRpc();
    }

    [ClientRpc]
    private void ShootClientRpc()
    {
        if (IsOwner) return; // Ignore sur le client qui a tiré

        //Calcul de la visée, Reset du CD de tir et deduction d'une balle
        //SetAim();
        _cdTime = fireRate;
            
        //Tir Basic, recupere une balle du PoolingManager
        GameObject bullet = PoolingManager.Instance.GetPooledObject();
            

        //Set la position de la balle au bout du canon du pistolet
        if (bullet != null) {
            bullet.transform.position = bulletOrigin.position;
            bullet.transform.rotation = bulletOrigin.rotation;

            //Active la balle
            bullet.SetActive(true);
        }
    }*/
    
    public void SetAim()                                                                                                                                                                                             
    {
        //Tir d'un rayon
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit,layerMask))
        {
            aim.transform.position = hit.point;
            hitPoint = hit.point;
            bulletOrigin.LookAt(aim.transform.position);
        }
    }
    
   
}
