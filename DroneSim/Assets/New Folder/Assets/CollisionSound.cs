using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioClip collisionClip; // Звук столкновения
    private AudioSource audioSource; 

    void Start()
    {
        // Получаем компонент AudioSource
        audioSource = GetComponent<AudioSource>();
        // Устанавливаем звук столкновения в качестве аудиоклипа
        audioSource.clip = collisionClip;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Проигрываем звук столкновения
        audioSource.Play();
    }
}
