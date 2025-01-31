using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField] protected GunScriptableObject gunData;
    [SerializeField] public GameObject gunMuzzle;
    [SerializeField] protected ParticleSystem muzzleFlashParticles;
    protected ParticleSystem muzzleFlashObject;

    private GameObject player;
    [HideInInspector] public Transform playerCameraTransform;

    private AudioSource audioSource;
    [SerializeField] List<AudioClip> gunSounds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected void Update()
    {
        if (playerCameraTransform)
        {
            Vector3 targetPoint = playerCameraTransform.position + playerCameraTransform.forward * 50f;
            Quaternion gunRotation = Quaternion.LookRotation(targetPoint - gunMuzzle.transform.position);
            transform.rotation = gunRotation;
        }
    }

    public void InitOwner(GameObject newPlayer)
    {
        player = newPlayer;
        audioSource = player.GetComponent<AudioSource>();
    }
    
    /*protected void CalculateProjectileRotation()
    {
        Vector3 targetPoint = playerCameraTransform.position + playerCameraTransform.forward * 50f;
        Vector3 lookDirection = targetPoint - gunMuzzle.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        
        projectileRotation = lookRotation;

        
        RaycastHit hit;
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward * 100, out hit))
        {
            lookDirection = hit.point - gunMuzzle.transform.position;
            Debug.DrawRay(gunMuzzle.transform.position, lookDirection * 100, Color.red, 1f);
            projectileRotation = Quaternion.LookRotation(lookDirection.normalized);
        }
    }*/
    
    public virtual void Shoot(bool isCrit)
    {
        FlashMuzzle();
        
        GameObject newProjectile = Instantiate(gunData.projectilePrefab, gunMuzzle.transform.position, transform.rotation);
        PistolBulletScript projectileScript = newProjectile.GetComponent<PistolBulletScript>();

        if (projectileScript != null)
        {
            projectileScript.InitOwner(player);
            projectileScript.isCrit = isCrit;
        }
    }

    protected void FlashMuzzle()
    {
        if (gunMuzzle && muzzleFlashParticles)
        {
            muzzleFlashObject = Instantiate(muzzleFlashParticles, gunMuzzle.transform.position, transform.rotation);
            muzzleFlashObject.Play();
            Destroy(muzzleFlashObject.gameObject, 0.1f);
        }
    }

    protected void PlayAltGunSounds()
    {
        if (gunSounds.Count > 0)
        {
            int randSoundIndex = Random.Range(0, (gunSounds.Count - 1));
            float pitch = Random.Range(0.9f, 1.10f);
            float volume = Random.Range(0.1f, 0.3f);

            audioSource.pitch = pitch;
            audioSource.PlayOneShot(gunSounds[randSoundIndex], volume);
        }
    }

}
