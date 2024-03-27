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

    [Header("Animation Infos")] 
    public Animator _animator; 

    private void Start()
    {
        //if (!IsOwner) return;
        _camera = Camera.main;
        UIManager.Instance.ChangeAmmoText(ammo.ToString());

        _animator = gameObject.GetComponent<Animator>();
    }

    private void OnEnable()
    {
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
        else if(_isPerformed)
        {
            
            Shoot();
        }        
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(timeForReload);
        //_animator?.SetTrigger("Reload");
        ammo = maxAmmo;
        UIManager.Instance.ChangeAmmoText(ammo.ToString());
    }

    

    public void Fire(InputAction.CallbackContext context)
    {
        //if (!IsOwner) return;
        //Listener du bouton de tir
        if (context.started)
        {
            //SetAim();
            _isPerformed = true;
            
            //Shoot();
        }
            
        if (context.canceled)
            _isPerformed = false;
    }

    void Shoot()
    {
        SetAim();
      
        
        //Tirs
        if (ammo > 0 && _cdTime < 0)
        {
            //_animator?.SetTrigger("Shoot");
            _cdTime = fireRate;
              fxShoot.Play();        
            GameObject bullet = PoolingManager.Instance.GetPooledObject();

            if (bullet != null) {
                
                if (bullet.TryGetComponent<Bullet>(out Bullet compBullet))
                {
                    bullet.SetActive(true);
                    compBullet.SetAimTransform(bulletOrigin);      
                }
                
                ammo--;
                UIManager.Instance.ChangeAmmoText(ammo.ToString());
                ScreenShaker.Instance.SetShake(0.01f,0.01f);
            }
        }

        if (_isPerformed && ammo <= 0)
        {
            StartCoroutine(Reload());
        }
    }
    
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
