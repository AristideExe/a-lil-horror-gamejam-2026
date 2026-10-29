using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepSounds;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootstepSound()
    {
        AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        _audioSource.volume = Random.Range(0.02f, 0.5f);
        _audioSource.pitch = Random.Range(0.8f, 1.2f);
        _audioSource.PlayOneShot(clip);
    }
}
