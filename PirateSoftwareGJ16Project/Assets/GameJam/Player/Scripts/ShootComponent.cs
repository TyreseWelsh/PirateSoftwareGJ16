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
    [SerializeField] private int MAX_SHOOT_COUNT = 8;
    private int shootCounter = 0;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float reloadSpeed = 1.2f;
    [SerializeField] private int baseCritRate = 0;
    
    [Header("")]
    [SerializeField] private GameObject mesh;
    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] private Transform cameraTransform;
    private StatManagerComponent statManager;
    
    [Header("BaseGun")] 
    [SerializeField] private GunScriptableObject baseGunData;
    [SerializeField] private GameObject baseMuzzle;

    [HideInInspector] public bool bHoldingTrigger;
    [HideInInspector] public bool bCanShoot = true;
    private Coroutine shootCoroutine;
    [SerializeField] protected ParticleSystem muzzleFlashParticles;
    protected ParticleSystem muzzleFlashObject;
    [SerializeField] List<AudioClip> shootSounds;
    
    private Coroutine reloadCoroutine;
    [SerializeField] List<AudioClip> reloadSounds;


    // "Key" dictates at what multiple the weapons will be fired e.g. Shotguns may be added to "3" meaning that they will fire every 3rd shot 
    private List<bool> nextGunOnRightSide = new List<bool>();
    private Dictionary<int, List<GameObject>> leftSideGuns = new Dictionary<int, List<GameObject>>();
    private Dictionary<int, List<GameObject>> rightSideGuns = new Dictionary<int, List<GameObject>>();

    private void Awake()
    {
        statManager = GetComponent<StatManagerComponent>();
        animator = mesh.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentAmmoCount = MAX_AMMO_COUNT;
        
        // At start, randomly decide where the first gun for each interval will be placed (we will then swap between right and left side for each gun on that interval)
        for (int i = 0; i < MAX_SHOOT_COUNT; i++)
        {
            nextGunOnRightSide.Add(Random.value < 0.5f);
            leftSideGuns.Add(i, new List<GameObject>());
            rightSideGuns.Add(i, new List<GameObject>());
        }
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
                FlashMuzzle();
                
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
                
                // Two if statements to gather all the guns (from both sides of the player) we are going to shoot from during this frame
                List<GameObject> currentGunList = new List<GameObject>();
                if (leftSideGuns.ContainsKey(shootCounter))
                {
                    currentGunList.AddRange(leftSideGuns[shootCounter]);
                }
                if (rightSideGuns.ContainsKey(shootCounter))
                {
                    currentGunList.AddRange(rightSideGuns[shootCounter]);
                }

                // Fire all guns at this "shoot interval" if any
                if (currentGunList.Count > 0)
                {
                    foreach (GameObject gun in currentGunList)
                    {
                        // Debug.Log("Firing gun at interval: " + shootCounter);
                        gun.GetComponent<GunScript>()?.Shoot(isCrit);
                    }
                }
                animator.SetTrigger(name: "isShooting");
                shootCoroutine = StartCoroutine(UntilNextShot());
            }
        }
    }

    private void FlashMuzzle()
    {
        if (baseMuzzle)
        {
            muzzleFlashObject = Instantiate(muzzleFlashParticles, baseMuzzle.transform.position, CalculateProjectileRotation());
            muzzleFlashObject.Play();
            Destroy(muzzleFlashObject.gameObject, 0.1f);
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
        PlayShootSound();

        if (projectileScript != null)
        {
            projectileScript.InitOwner(gameObject);
            projectileScript.isCrit = isCrit;
        }
    }

    private void PlayShootSound()
    {
        if (shootSounds.Count > 0)
        {
            int randSoundIndex = Random.Range(0, 3);
            float pitch = Random.Range(0.9f, 1.10f);
            float volume = Random.Range(0.85f, 1.0f);
            AudioClip shootSound = shootSounds[randSoundIndex];

            audioSource.pitch = pitch;
            audioSource.PlayOneShot(shootSound, volume);
        }
    }

    private Quaternion CalculateProjectileRotation()
    {
        Vector3 targetPoint = cameraTransform.position + cameraTransform.forward * 35f;
        Vector3 lookDirection = targetPoint - baseMuzzle.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        
        Quaternion projectileRotation = lookRotation;

        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward * 35, out hit))
        {
            if (Vector3.Distance(transform.position, hit.point) > 1.5f)
            {
                lookDirection = hit.point - baseMuzzle.transform.position;
                Debug.DrawRay(baseMuzzle.transform.position, lookDirection * 35, Color.red, 0.1f);
                projectileRotation = Quaternion.LookRotation(lookDirection.normalized);
            }
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
            PlayReloadSound();
            // Make sure player cant attempt to shoot during reload
            bCanShoot = false;
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
            
            reloadCoroutine = StartCoroutine(Reloading());
        }
    }

    private void PlayReloadSound()
    {
        if (reloadSounds.Count > 0)
        {
            int randSoundIndex = Random.Range(0, 3);
            float pitch = Random.Range(0.9f, 1.10f);
            float volume = Random.Range(0.85f, 1.0f);
            AudioClip reloadSound = reloadSounds[randSoundIndex];

            audioSource.pitch = pitch;
            audioSource.PlayOneShot(reloadSound, volume);
        }
    }
    
    IEnumerator Reloading()
    {
        yield return new WaitForSeconds( 2 - GetReloadSpeed(true));
        
        currentAmmoCount = MAX_AMMO_COUNT;
        shootCounter = 1;
        bCanShoot = true;
        Debug.Log("Weapons reloaded!" + (2 - GetReloadSpeed(true)));
        
        reloadCoroutine = null;
    }

    public void AddGun(GunScriptableObject newGunData)
    {
        int armInterval = newGunData.shootInterval - 1;
        
        GameObject newGun = SpawnNewGun(newGunData);
        
        // Looping through lists of guns, adding the to each multiple of its gun interval so it fires 
        // For example: a gun with shootInterval of 1, will be added to every single list of guns (as it should be fired every time) whereas a gun with shootInterval of 3 will be fired every 3 shots 
        while (armInterval < MAX_SHOOT_COUNT)
        {
            // Need to spawn new gun object in correct position, add the right GunData, and rotate it slightly so not all the same guns are facing in the same direction
            // For now add random values to position, to offset each gun
            if (nextGunOnRightSide.Count > 0)
            {
                if (nextGunOnRightSide[newGunData.shootInterval])
                {
                    rightSideGuns[armInterval].Add(newGun);
                }
                else
                {
                    leftSideGuns[armInterval].Add(newGun);
                }
                
            }

            armInterval += newGunData.shootInterval;
        }
        nextGunOnRightSide[newGunData.shootInterval] = !nextGunOnRightSide[newGunData.shootInterval];
    }

    private GameObject SpawnNewGun(GunScriptableObject newGunData)
    {
        GameObject newGun = Instantiate(newGunData.gunPrefab, mesh.transform);
        if(newGun)
        {
            GunScript newGunScript = newGun.GetComponent<GunScript>();
            if(newGunScript)
            {
                newGunScript.playerCameraTransform = cameraTransform;
                newGunScript.InitOwner(gameObject);
            }

            Vector3 gunOffset = FindGunOffset(newGunData.shootInterval);
            Debug.Log(gunOffset);
            newGun.transform.localPosition = new Vector3(newGun.transform.localPosition.x + gunOffset.x, newGun.transform.localPosition.y + gunOffset.y, newGun.transform.localPosition.z + gunOffset.z);
        }

        return newGun;
    }

    // TODO: Issue with gunIndex skipping values
    private Vector3 FindGunOffset(int _shootInterval)
    {
        float xOffset;
        if (nextGunOnRightSide.Count > 0)
        {
            float gunIndex = 0;

            // Spawn gun on right side if true, left if false
            if (nextGunOnRightSide[_shootInterval])
            {
                gunIndex = rightSideGuns[_shootInterval].Count;
                xOffset = 0.75f + (gunIndex / 3f);
            }
            else
            {
                gunIndex = leftSideGuns[_shootInterval].Count;
                xOffset = -0.75f - (gunIndex / 3f);
            }
            
            float yOffset = -0.2f + (_shootInterval / 2.3f) + Random.Range(-0.05f, 0.05f);
            float zOffset = Random.Range(-0.6f, 0.6f);
            
            return new Vector3(xOffset, yOffset, zOffset);
        }

        Debug.LogError("nextGunOnRightSide not initialized!", gameObject);

        return Vector3.zero;
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
