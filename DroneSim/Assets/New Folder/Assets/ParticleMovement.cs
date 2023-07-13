using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleMovement : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public float spawnDistance = 600f;
    public float detectionDistance = 30f;
    public Vector2 spawnArea = new Vector2(200f, 800f);
    public Text textMesh;

    private Transform quadcopterTransform;
    private Vector3 targetPosition;
    private bool isDetected = false;

    void Start()
    {
        // Находим компонент Transform квадрокоптера
        quadcopterTransform = GetComponent<Transform>();
        // Позиционируем Particle System в случайном месте
        MoveParticleSystem();
        textMesh.text = "";
    }

    void Update()
    {
        // Проверяем нажатие на LB на геймпаде
        float yawInput = Input.GetAxis("LB");
        if (yawInput > 0f)
        {
            // Вычисляем расстояние между квадрокоптером и Particle System
            float distance = Vector3.Distance(quadcopterTransform.position, targetPosition);
            // Проверяем, приблизился ли квадрокоптер к Particle System
            if (distance <= detectionDistance && !isDetected)
            {
                // Выводим текст "Обнаружено" на экран
                textMesh.text = "Координаты переданы";
                isDetected = true;
                MoveParticleSystem();

                // очистка текста через 2 секунды
                StartCoroutine(ClearTextAfterDelay(2f));
            }
        }
    }

    IEnumerator ClearTextAfterDelay(float delay)
    {
        // Ждем указанное время
        yield return new WaitForSeconds(delay);

        // Очищаем текст
        textMesh.text = "";

        // Сбрасываем флаг обнаружения
        isDetected = false;
    }

    void MoveParticleSystem()
    {
        // Generate random coordinates within the specified range
        float x = Random.Range(-spawnDistance, spawnDistance);
        float z = Random.Range(-spawnDistance, spawnDistance);

        // Check if the coordinates are within the spawnArea
        if (x >= spawnArea.x && x <= spawnArea.y && z >= spawnArea.x && z <= spawnArea.y)
        {
            particleSystem.Stop();
            particleSystem.Clear();
            // Calculate the target position
            targetPosition = new Vector3(x, 0f, z);

            // Move the Particle System to the target position
            particleSystem.transform.position = targetPosition;

            // Restart the Particle System
            particleSystem.Play();

            Debug.Log("Перемещено");
        }
        else
        {
            // Coordinates are outside the spawn area, generate new coordinates
            MoveParticleSystem();
        }
    }
}
