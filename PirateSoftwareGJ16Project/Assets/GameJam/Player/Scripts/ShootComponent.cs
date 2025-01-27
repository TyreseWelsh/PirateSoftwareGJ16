using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using System.Reflection;
using Random = UnityEngine.Random;

public class ShootComponent : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public int MAX_AMMO_COUNT = 40;
    [HideInInspector]public int currentAmmoCount = 0;
    [SerializeField] private int MAX_SHOOT_COUNT = 4;
    private int shootCounter = 0;
    [SerializeField] private float fireRate = 0.3f;
    [SerializeField] private float reloadSpeed = 1f;
    [SerializeField] private int baseCritRate = 0;
    
    [Header("")]
    [SerializeField] private GameObject mesh;
    [SerializeField] private Transform cameraTransform;
    private StatManagerComponent statManager;
    
    [Header("BaseGun")] 
    [SerializeField] private GunScriptableObject baseGunData;
    [SerializeField] private GameObject baseMuzzle;
    
    private bool bHoldingTrigger = false;
    private bool bCanShoot = true;
    private Coroutine shootCoroutine;
    
    private Coroutine reloadCoroutine;
    
    // "Key" dictates at what multiple the weapons will be fired e.g. Shotguns may be added to "3" meaning that they will fire every 3rd shot 
    private Dictionary<int, List<GameObject>> currentGuns = new Dictionary<int, List<GameObject>>();


    private void Awake()
    {
        statManager = GetComponent<StatManagerComponent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentAmmoCount = MAX_AMMO_COUNT;
    }

    // Update is called once per frame
    void Update()
    {
        if (bHoldingTrigger && bCanShoot)
        {
            // Automatically reload when attempting to shoot with no ammo
            if (currentAmmoCount <= 0)
            {
                StartReload();
            }
            else
            {
                bCanShoot = false;
                if (shootCounter >= MAX_SHOOT_COUNT)
                {
                    shootCounter = 0;
                }
                shootCounter++;
                currentAmmoCount--;
                //Debug.Log("Ammo left: " + currentAmmoCount);
                //Debug.Log("Shoot all " + shootCounter + " weapons!");
                
                // Check if crit
                int critNum = Random.Range(1, 100);
                bool isCrit = critNum < GetCritRate(true);
                ShootBaseGun(isCrit);
                
                if (currentGuns.ContainsKey(shootCounter))
                {
                    List<GameObject> currentGunList = currentGuns[shootCounter];
                    
                    if (currentGunList.Count > 0)
                    {
                        foreach (GameObject gun in currentGunList)
                        {
                            Debug.Log("Firing gun at interval: " + shootCounter);
                            gun.GetComponent<GunScript>()?.Shoot(isCrit);
                        }
                    }
                }

                shootCoroutine = StartCoroutine(UntilNextShot());
            }
        }
    }

    public float GetFireRate(bool modified)
    {
        if (statManager)
        {
            if (modified)
            {
                return statManager.ApplyStatIncrease("FireRate", fireRate);
            }
        }
        
        return fireRate;
    }

    public float GetReloadSpeed(bool modified)
    {
        if (statManager)
        {
            if (modified)
            {
                return statManager.ApplyStatIncrease("ReloadSpeed", reloadSpeed);
            }
        }
        
        return reloadSpeed;
    }

    public int GetCritRate(bool modified)
    {
        if (statManager)
        {
            if (modified)
            {
                return Mathf.CeilToInt(statManager.ApplyStatIncrease("CritRate", baseCritRate));
            }
        }
        
        return baseCritRate;
    }
    
    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            bHoldingTrigger = true;
        }

        if (context.canceled)
        {
            bHoldingTrigger = false;
        }
    }

    IEnumerator UntilNextShot()
    {
        yield return new WaitForSeconds(GetFireRate(true));
        bCanShoot = true;
    }

    private void ShootBaseGun(bool isCrit)
    {
        GameObject baseProjectile = Instantiate(baseGunData.projectilePrefab, baseMuzzle.transform.position, CalculateProjectileRotation());
        PistolBulletScript projectileScript = baseProjectile.GetComponent<PistolBulletScript>();

        if (projectileScript != null)
        {
            projectileScript.InitOwner(gameObject);
            projectileScript.isCrit = isCrit;
        }
    }
    
    private Quaternion CalculateProjectileRotation()
    {
        Vector3 targetPoint = cameraTransform.position + cameraTransform.forward * 30f;
        Vector3 lookDirection = targetPoint - baseMuzzle.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        
        Quaternion projectileRotation = lookRotation;

        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward * 15, out hit))
        {
            lookDirection = hit.point - baseMuzzle.transform.position;
            Debug.DrawRay(baseMuzzle.transform.position, lookDirection * 15, Color.red, 1f);
            projectileRotation = Quaternion.LookRotation(lookDirection.normalized);
        }

        return projectileRotation;
    }
    
    public void Reload(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartReload();
        }
    }

    private void StartReload()
    {
        // If not reloading currently
        if (reloadCoroutine == null)
        {
            ClearLog();
            // Will start reload timer/coroutine which plays animations, sounds, resets ammo count to MAX_AMMO
            Debug.Log("Start reload!");
            
            // Make sure player cant attempt to shoot during reload
            bCanShoot = false;
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
            
            reloadCoroutine = StartCoroutine(Reloading());
        }
    }
    
    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(GetReloadSpeed(true));
        currentAmmoCount = MAX_AMMO_COUNT;
        shootCounter = 1;
        bCanShoot = true;
        Debug.Log("Weapons reloaded!");
        
        reloadCoroutine = null;
    }

    public void AddGun(GunScriptableObject newGunData)
    {
        int armInterval = newGunData.shootInterval;
        
        GameObject newGun = Instantiate(newGunData.gunPrefab, mesh.transform);
        if(newGun)
        {
            GunScript newGunScript = newGun.GetComponent<GunScript>();
            if(newGunScript)
            {
                newGunScript.playerCameraTransform = cameraTransform;
                newGunScript.InitOwner(gameObject);
            }
            newGun.GetComponent<GunScript>().playerCameraTransform = cameraTransform;
            newGun.transform.localPosition = new Vector3(newGun.transform.localPosition.x + Random.Range(-2f, 2f), newGun.transform.localPosition.y + Random.Range(-0.75f, 0.75f), newGun.transform.localPosition.z + Random.Range(-1f, 1f));
        }

        while (armInterval <= MAX_SHOOT_COUNT)
        {
            // Need to spawn new gun object in correct position, add the right GunData, and rotate it slightly so not all the same guns are facing in the same direction
            // For now add random values to position, to offset each gun

            if (!currentGuns.ContainsKey(armInterval))
            {
                currentGuns.Add(armInterval, new List<GameObject>());
            }
            currentGuns[armInterval].Add(newGun);
            Debug.Log("Added new gun to interval: " + armInterval);
            
            armInterval += newGunData.shootInterval;
        }
        
        Debug.Log("Armed with new " + newGunData.name+ "...");
    }
    
    // UREGENT NOTE: COMMENT OUT WHEN BUILDING
    // Source: https://stackoverflow.com/questions/40577412/clear-editor-console-logs-from-script
    private void ClearLog()
    {
        /*var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);*/
    }
}
