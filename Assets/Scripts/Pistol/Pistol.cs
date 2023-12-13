using System.Collections;
using System.Collections.Generic;
using BaseTemplate.Behaviours;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pistol : Weapon
{
    [Header("Basics")]
    public Transform bulletOrigin;
    public float fireRate;
    public ParticleSystem fxShoot;
    private bool _isPerformed = false;

    [Header("Pistol Infos")] 
    
    private float _cdTime;
    

    [Header("Aim Infos")] 
    public GameObject aim;
    public LayerMask layerMask;
    private Camera _camera;

    
    
    private void Start()
    {
        
        _camera = Camera.main;
        UIManager.Instance.ChangeAmmoText(ammo.ToString());
    }

    private void Update()
    {
        //CoolDown entre les tirs
        if (_cdTime >= 0)
        {
            _cdTime -= Time.deltaTime;
            
        }
        else
        {
            SetAim();
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
        //Listener du bouton de tir
        if (context.started)
        {
            SetAim();
            _isPerformed = true;
            fxShoot.Play();
        }
            
        if (context.canceled)
            _isPerformed = false;
        
    }


    public Transform SetAim()                                                                                                                                                                                             
    {
        //Tir d'un rayon
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit,layerMask))
        {
            //Ajustement de notre Aim en fonction du hitPoint, fonctionne avec un AimConstraint
            aim.transform.position = hit.point;
            hitPoint = hit.point;
        }
        return bulletOrigin;
    }
    
    public Vector3 hitPoint;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitPoint, 0.5f); 
    }
}
