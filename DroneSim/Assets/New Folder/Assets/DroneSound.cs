using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSound : MonoBehaviour
{
    public AudioSource audioSource;
    public Rigidbody quadrocopter;
    public float minPitch = 0.5f; // минимальный тон звука двигателя
    public float maxPitch = 2f; // максимальный тон звука двигателя

    private float initialPitch;

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        initialPitch = audioSource.pitch;
    }

    private void Update()
    {
        if (quadrocopter != null)
        {
            float currentThrottle = (float)quadrocopter.GetComponent<droneControls>().throttle;
            float normalizedThrottle = Mathf.InverseLerp(0f, 30f, currentThrottle);
            float pitch = Mathf.Lerp(minPitch, maxPitch, normalizedThrottle) * initialPitch;

            audioSource.pitch = pitch;
        }
    }
}
