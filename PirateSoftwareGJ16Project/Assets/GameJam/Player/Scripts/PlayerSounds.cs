using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> footstepSounds;
    [SerializeField] private AudioClip splatSound;
    [SerializeField] private AudioClip boingSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootstepSound()
    {
        int randSoundIndex = Random.Range(0, 1);
        float pitch = Random.Range(0.9f, 1.10f);
        float volume = Random.Range(0.2f, 0.4f);
        AudioClip footstepSound = footstepSounds[randSoundIndex];

        audioSource.pitch = pitch;
        audioSource.PlayOneShot(footstepSound, volume);
    }

    public void PlaySplatSound()
    {
        int randSoundIndex = Random.Range(0, 1);
        float pitch = Random.Range(0.9f, 1.10f);
        float volume = Random.Range(0.50f, 0.60f);
      

        audioSource.pitch = pitch;
        audioSource.PlayOneShot(splatSound, volume);
    }

    public void PlayBoingSound()
    {
        int randSoundIndex = Random.Range(0, 1);
        float pitch = Random.Range(0.9f, 1.10f);
        float volume = Random.Range(0.6f, 0.7f);


        audioSource.pitch = pitch;
        audioSource.PlayOneShot(boingSound, volume);
    }
}

