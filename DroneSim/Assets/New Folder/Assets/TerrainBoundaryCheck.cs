using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainBoundaryCheck : MonoBehaviour
{
    public Terrain terrain;
    public float boundaryRadius = 50f;
    private bool hasApproachedBoundary = false;
    public droneControls droneControls;
    public Text textMesh;


    private void Start()
    {
        // Получаем ссылку на компонент DroneControls
        droneControls = GetComponent<droneControls>();
    }

    private void Update()
    {
        // Проверяем приближение к границе
        if (IsApproachingTerrainBoundary())
        {
            if (!hasApproachedBoundary)
            {
                textMesh.text = "Потеря сигнала! Возврат в зону";
                 // очистка текста через 2 секунды
                StartCoroutine(ClearTextAfterDelay(2f));
                hasApproachedBoundary = true;
                StartCoroutine(ApplyYawCorrection());
                
            }
        }
        else
        {
            hasApproachedBoundary = false;
        }
    }

        IEnumerator ClearTextAfterDelay(float delay)
    {
        // Ждем указанное время
        yield return new WaitForSeconds(delay);

        // Очищаем текст
        textMesh.text = "";
    }

    public bool IsApproachingTerrainBoundary()
    {
        // Получаем размеры Terrain
        Vector3 terrainSize = terrain.terrainData.size;

        // Получаем позицию объекта
        Vector3 objectPosition = transform.position;

        // Вычисляем минимальные и максимальные координаты для радиуса вокруг объекта
        float minX = objectPosition.x - boundaryRadius;
        float maxX = objectPosition.x + boundaryRadius;
        float minZ = objectPosition.z - boundaryRadius;
        float maxZ = objectPosition.z + boundaryRadius;

        // Проверяем приближение к границе в радиусе
        if (minX <= 0f || maxX >= terrainSize.x ||
            minZ <= 0f || maxZ >= terrainSize.z)
        {
            return true; // Приближается к границе
        }

        return false; // Не приближается к границе
    }

    private IEnumerator ApplyYawCorrection()
    {
        for (int i = 0; i < 6; i++)
        {
            // Получаем текущее значение Yaw
            double currentYaw = droneControls.targetYaw;

            // Добавляем 30 градусов к текущему значению Yaw 6 раз в течение цикла
            // один поворот на 180 слишком резкий и нереалистичный
            double correctedYaw = currentYaw + 30.0;

            // Применяем скорректированное значение Yaw в droneControls
            droneControls.targetYaw = correctedYaw; // Здесь предполагается, что у вас есть метод SetYaw для установки нового значения Yaw

            yield return new WaitForSeconds(0.3f);
        }
    }
}
