using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras; // Массив с доступными камерами
    private int currentCameraIndex; // Индекс текущей активной камеры

    void Start()
    {
        currentCameraIndex = 0; // Устанавливаем начальный индекс активной камеры
        ActivateCamera(currentCameraIndex); // Активируем начальную камеру
    }

    void Update()
    {
        // Проверяем нажатие кнопки для переключения камеры
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Увеличиваем индекс камеры на 1
            currentCameraIndex++;

            // Если достигли конца массива камер, возвращаемся к началу
            if (currentCameraIndex >= cameras.Length)
            {
                currentCameraIndex = 0;
            }

            // Активируем новую камеру
            ActivateCamera(currentCameraIndex);
        }
    }

    void ActivateCamera(int cameraIndex)
    {
        // Деактивируем все камеры
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        // Активируем выбранную камеру
        cameras[cameraIndex].gameObject.SetActive(true);
    }
}
