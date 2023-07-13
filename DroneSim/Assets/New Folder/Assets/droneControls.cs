using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class droneControls : MonoBehaviour
{
    private Rigidbody rb;

    // фактические параметры
    private double pitch; // Тангаж
    private double roll; // Крен
    private double yaw; // Рыскание
    public double throttle; // Газ

    // требуемые параметры
    public double targetPitch;
    public double targetRoll;
    public double targetYaw;

    // PID регуляторы, которые будут стабилизировать углы
    private PID pitchPID = new PID(100, 0, 20);
    private PID rollPID = new PID(100, 0, 20);
    private PID yawPID = new PID(100, 10, 20);

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void readRotation()
    {
        /* Получение фактической ориентации нашего квадрокоптера.
В        реальном квадрокоптере эти данные получаются из акселерометра, 
        и гироскопа*/
        Vector3 rot = GameObject.Find("Frame").GetComponent<Transform>().rotation.eulerAngles;
        pitch = rot.x;
        yaw = rot.y;
        roll = rot.z;
    }
    /* Функция стабилизации квадрокоптера.
    С использованием PID регуляторов мы настраиваем мощность моторов таким образом, 
    чтобы достичь желаемых значений углов. */
    void stabilize()
    {
        /* Для получения разницы между желаемым и текущим углами необходимо вычислить их разность.
        Эта разница ограничена в промежутке от -180 до 180 градусов, чтобы обеспечить корректную работу 
        PID регуляторов.*/
        
        double dPitch = targetPitch - pitch;
        double dRoll = targetRoll - roll;
        double dYaw = targetYaw - yaw;

        dPitch -= Math.Ceiling(Math.Floor(dPitch / 180.0) / 2.0) * 360.0;
        dRoll -= Math.Ceiling(Math.Floor(dRoll / 180.0) / 2.0) * 360.0;
        dYaw -= Math.Ceiling(Math.Floor(dYaw / 180.0) / 2.0) * 360.0;

        // Моторы 1 и 2 расположены спереди, а моторы 3 и 4 находятся сзади.
        double power1 = throttle;
        double power2 = throttle;
        double power3 = throttle;
        double power4 = throttle;

        // Управление тангажем:
        double pitchForce = -pitchPID.calc(0, dPitch / 180.0);
        power1 += pitchForce;
        power2 += pitchForce;
        power3 += -pitchForce;
        power4 += -pitchForce;

        // Управление креном:
        double rollForce = -rollPID.calc(0, dRoll / 180.0);
        power1 += rollForce;
        power2 += -rollForce;
        power3 += -rollForce;
        power4 += rollForce;

        // управление рысканием:
        double yawForce = yawPID.calc(0, dYaw / 180.0);
        power1 += yawForce;
        power2 += -yawForce;
        power3 += yawForce;
        power4 += -yawForce;

        // Устанавливаем мощность для каждого из моторов модели 
        GameObject.Find("Motor1").GetComponent<motorScript>().power = power1;
        GameObject.Find("Motor2").GetComponent<motorScript>().power = power2;
        GameObject.Find("Motor3").GetComponent<motorScript>().power = power3;
        GameObject.Find("Motor4").GetComponent<motorScript>().power = power4;
    }

    void FixedUpdate()
    {
        readRotation();
        // Получение пользовательского ввода
        float currentThrottle = Input.GetAxis("Vertical");
        float currentPitch = Input.GetAxis("Horizontal");
        float currentRoll = Input.GetAxis("Roll");
        float currentYaw = Input.GetAxis("Yaw");

        // стабилизация
        stabilize();    

        currentThrottle = Input.GetAxis("Vertical") * 25;
        /* ввод значения с оси Vertical осуществляется
        в формате [-1;1], поступает вещественное число.
        Для масштабирования относительно макс. газа умножаем на число */
        /* если текущий газ >0 - мы поднимаемся
        переменные amount и rate смягчают ввод со стиков, делая его менее ступенчатым
        как для increase, так и для decrease */
        if (currentThrottle >= 0)
        {
            float increasePercentage = Mathf.Abs(currentThrottle) / 35f;
            double increaseAmount = 35 * increasePercentage;
            double increaseRate = 1.5; // Контроль скорости подъема
            throttle = Mathf.Lerp((float)throttle, (float)(throttle + increaseAmount), (float)(increaseRate * Time.fixedDeltaTime));
            throttle = Math.Max(throttle, currentThrottle);
        }
        else
        {
            float decreasePercentage = Mathf.Abs(currentThrottle) / 35f;
            double decreaseAmount = 35 * decreasePercentage;
            double decreaseRate = 1.5; // Контроль скорости снижения
            throttle = Mathf.Lerp((float)throttle, (float)(throttle - decreaseAmount), (float)(decreaseRate * Time.fixedDeltaTime));
        }
        
        /* ограничение значения переменной throttle в диапазоне от 0 до 35.
        для реализации ограничения макс скорости

        ввод пользователя отрицательным быть может, 
        а общий газ квадрокоптера - нет */
        throttle = Mathf.Clamp((float)throttle, 0f, 35f);

        // тангаж
        float pitchInput = Input.GetAxis("Horizontal");
        float pitchSpeed = 1f; // смягчение тангажа, аналогично газу
        /* здесь и далее [-42;42] - максимально возможные углы
        согласно спецификации дрона DJI Phantom 4 PRO 2.0 */
        targetPitch = Mathf.Lerp((float)targetPitch, pitchInput * 42f, pitchSpeed * Time.fixedDeltaTime);
        targetPitch = Mathf.Clamp((float)targetPitch, -42f, 42f);
        
        // крен
        float rollInput = Input.GetAxis("Roll");
        float rollSpeed = 2f; // смягчение угла крена
        targetRoll = Mathf.Lerp((float)targetRoll, rollInput * 42f, rollSpeed * Time.fixedDeltaTime);
        targetRoll = Mathf.Clamp((float)targetRoll, -42, 42f);

        // рысканье
        float yawInput = Input.GetAxis("Yaw");
        float yawSpeed = 50f; // смягчение угла рыскания
        targetYaw = Mathf.Lerp((float)targetYaw, (float)targetYaw + yawInput * yawSpeed, Time.fixedDeltaTime);
        targetYaw = (float)((double)targetYaw % 360.0); 
        //Сбросить значение  на 0, когда оно достигает 360 градусов.
        targetYaw = Mathf.Clamp((float)targetYaw, -360f, 360f); 
    }
}

//реализация PID-регуляторов согласно описанной в записке формуле
public class PID {
	
	private double P;
	private double I;
	private double D;
	
	private double prevErr;
	private double sumErr;
	
	public PID (double P, double I, double D) {
		this.P = P;
		this.I = I;
		this.D = D;
	}
	
	public double calc (double current, double target) {
		
		double dt = Time.fixedDeltaTime;
		
		double err = target - current;
		this.sumErr += err;
		
		double force = this.P * err + this.I * this.sumErr * dt + this.D * (err - this.prevErr) / dt;
		
		this.prevErr = err;
		return force;
	}
}
