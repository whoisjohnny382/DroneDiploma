using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryScript : MonoBehaviour
{

    private float GetChargeChangeRate(float currentSpeed)
    {
        float maxSpeed = 20f; // Максимальная скорость, при которой заряд будет меняться максимально
        float chargeChangeRate = 0f;

        if (currentSpeed > 0f)
        {
                    // Модифицируем скорость, чтобы обратно пропорционально влиять на скорость изменения заряда
        float modifiedSpeed = maxSpeed / currentSpeed;
            
            // Рассчитываем пропорциональное изменение заряда в зависимости от текущей скорости
            chargeChangeRate = currentSpeed / maxSpeed;
        }

        return chargeChangeRate;
    }
    
    public Text batteryPercentageText;
    public Text batteryCapacityText;
    public Text distanceText;
        
    private float maxCapacity = 5350f; // Максимальная емкость батареи в мАч
    private float currentDistance = 0f; // Текущий пробег в метрах
    public float currentCapacity; // Текущая емкость батареи
    

    private float updateInterval = 1f; // Интервал обновления пробега (в секундах)
    private float updateTimer = 0f; // Таймер обновления пробега

    private droneControls quadrocopter;
    private Vector3 lastPosition; // Последняя позиция дрона
    
    void Start()
    {
        // Запоминаем начальную позицию дрона
        quadrocopter = GetComponentInParent<droneControls>();
        lastPosition = transform.position;
        currentCapacity = maxCapacity;
    }

    void Update()
    {
        // Обновляем пробег с учетом скорости дрона
        updateTimer += Time.deltaTime;
        if (updateTimer >= updateInterval)
        {
            // Вычисляем пройденное расстояние между последним и текущим положением дрона
            float distanceDelta = Vector3.Distance(transform.position, lastPosition);
            currentDistance += distanceDelta;

            // Сбрасываем таймер обновления
            updateTimer = 0f;

            // Запоминаем текущую позицию дрона
            lastPosition = transform.position;
        }

            // Получаем текущий газ дрона
        float currentThrottle = (float)quadrocopter.throttle;

                // чтобы при нулевом throttle некий расход оставался
        float throttleRate = (currentThrottle + 0.1f) / 35f;
        
        // Получаем текущую скорость дрона
        float currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;

        // Получаем коэффициент изменения заряда
        float chargeChangeRate = GetChargeChangeRate(currentSpeed);

        // Вычисляем изменение емкости батареи
        float capacityChange = 0.1f * chargeChangeRate * throttleRate;

        // Вычисление текущего заряда в процентах и текущей емкости
        currentCapacity = Mathf.Max(0f, currentCapacity - capacityChange);
        float batteryPercentage = (currentCapacity / maxCapacity) * 100f;

        // Обновление текстовых элементов
        batteryPercentageText.text = "Заряд: " + batteryPercentage.ToString("0") + "%";
        batteryCapacityText.text = "Текущая емкость: " + currentCapacity.ToString("0") + " mAh";
        distanceText.text = "Пробег: " + currentDistance.ToString("0") + " m";
    }
}


