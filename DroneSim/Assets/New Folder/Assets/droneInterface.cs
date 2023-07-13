using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class droneInterface : MonoBehaviour
{
    public Text currentThrottleText;
    
    public Text targetPitchText;
    public Text targetRollText;
    public Text targetYawText;

    public Text throttleInputText;
    public Text pitchInputText;
    public Text rollInputText;
    public Text yawInputText;
    public Text heightText;

    public Text positionXText;
    public Text positionYText;
    public Text positionZText;

    private droneControls quadrocopter;
    private float initialHeight;
    
    public Text speedText;
    public Rigidbody rb;
	public float sp;

    private void Start()
    {
        quadrocopter = GetComponentInParent<droneControls>();
        initialHeight = transform.position.y;
    }

    private void Update()
    {
        // Получение текущих значений targetPitch, targetRoll и targetYaw из скрипта quadrocopterScript
        float targetPitch = (float)quadrocopter.targetPitch;
        float targetRoll = (float)quadrocopter.targetRoll;
        float targetYaw = (float)quadrocopter.targetYaw;

        // Обновление текстовых элементов с целевыми значениями
        targetPitchText.text = "Тангаж: " + targetPitch.ToString("0.0");
        targetRollText.text = "Крен: " + (-1f * targetRoll).ToString("0.0");
        targetYawText.text = "Рыскание: " + targetYaw.ToString("0.0");

        // Получение ввода пользователя для осей pitch, roll, yaw и throttle
        float throttleInput = Input.GetAxis("Vertical");
        float pitchInput = Input.GetAxis("Horizontal");
        float rollInput = Input.GetAxis("Roll");
        float yawInput = Input.GetAxis("Yaw");

        // Преобразование ввода в проценты
        float throttleInputPercentage = throttleInput * 100f;
        float pitchInputPercentage = pitchInput * 100f;
        float rollInputPercentage = -1f * rollInput * 100f;
        float yawInputPercentage = yawInput * 100f;

        // Обновление текстовых элементов с вводом пользователя в процентах
        throttleInputText.text = "Подача (газ): " + throttleInputPercentage.ToString("0") + "%";
        pitchInputText.text = "Подача (тангаж): " + pitchInputPercentage.ToString("0") + "%";
        rollInputText.text = "Подача (крен): " + rollInputPercentage.ToString("0") + "%";
        yawInputText.text = "Подача (рысканье): " + yawInputPercentage.ToString("0") + "%";

        // Вычисление текущей высоты относительно начальной высоты
        float currentHeight = transform.position.y - initialHeight;
        heightText.text = "Высота: " + currentHeight.ToString("0.0") + "m";
        
        float currentThrottle = (float)quadrocopter.throttle;
        currentThrottleText.text = "Текущий газ: " + currentThrottle.ToString("0.0");

        // Вывод координат по X, Y и Z
        float positionX = transform.position.x;
        float positionY = transform.position.y;
        float positionZ = transform.position.z;

        positionXText.text = "Координата X: " + positionX.ToString("0.00");
        positionYText.text = "Координата Y: " + positionY.ToString("0.00");
        positionZText.text = "Координата Z: " + positionZ.ToString("0.00");

        float speed = rb.velocity.magnitude;
        speedText.text = "Скорость: " + speed.ToString("0.0") + " m/s";
    }
}
