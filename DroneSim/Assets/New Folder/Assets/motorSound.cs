using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class motorSound : MonoBehaviour
{
    public AudioSource audioSource;
    public Rigidbody quadrocopter;
    public float minPitch = 0.5f; // Adjust the minimum pitch value
    public float maxPitch = 1.5f; // Adjust the maximum pitch value

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
            double power = quadrocopter.GetComponent<motorScript>().power;
            float normalizedPower = Mathf.InverseLerp(0f, 50f, (float)power);
            float pitch = Mathf.Lerp(minPitch, maxPitch, normalizedPower) * initialPitch;

            audioSource.pitch = pitch;
        }
    }
}
