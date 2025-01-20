using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;

using System.Reflection;
using Unity.VisualScripting;

public class ShootComponent : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int MAX_AMMO_COUNT = 40;
    private int currentAmmoCount = 0;
    [SerializeField] private int MAX_SHOOT_COUNT = 4;
    private int shootCounter = 0;
    [SerializeField] private float fireRate = 0.3f;
    [SerializeField] private float reloadSpeed = 1f;

    [Header("BaseGun")] 
    [SerializeField] private GunScriptableObject baseGunData;
    [SerializeField] private GameObject baseFiringPoint;
    
    private bool bHoldingTrigger = false;
    private bool bCanShoot = true;
    private Coroutine shootCoroutine;
    
    private Coroutine reloadCoroutine;
    
    // "Key" dictates at what multiple the weapons will be fired e.g. Shotguns may be added to "3" meaning that they will fire every 3rd shot 
    private Dictionary<int, List<GameObject>> currentGuns = new Dictionary<int, List<GameObject>>();
    
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
                
                ShootBaseGun();
                if (currentGuns.ContainsKey(shootCounter))
                {
                    foreach (GameObject gun in currentGuns[shootCounter])
                    {
                        Debug.Log("Firing gun at interval: " + shootCounter);
                        gun.GetComponent<GunScript>()?.Shoot();
                    }
                }

                shootCoroutine = StartCoroutine(UntilNextShot());
            }
        }
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
        yield return new WaitForSeconds(fireRate);
        bCanShoot = true;
    }

    private void ShootBaseGun()
    {
        GameObject baseProjectile = Instantiate(baseGunData.projectilePrefab, baseFiringPoint.transform.position, transform.rotation);
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
        yield return new WaitForSeconds(reloadSpeed);
        currentAmmoCount = MAX_AMMO_COUNT;
        shootCounter = 1;
        bCanShoot = true;
        Debug.Log("Weapons reloaded!");
        
        reloadCoroutine = null;
    }

    public void AddGun(GunScriptableObject newGunData)
    {
        int armInterval = newGunData.shootInterval;
        GameObject newGun = Instantiate(newGunData.gunPrefab, transform);
        newGun.transform.localPosition = new Vector3(newGun.transform.localPosition.x + Random.Range(0.75f, 2f), newGun.transform.localPosition.x + Random.Range(-0.6f, 1.6f), newGun.transform.localPosition.z + Random.Range(-1f, 1f));

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
    
    // Source: https://stackoverflow.com/questions/40577412/clear-editor-console-logs-from-script
    private void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}
