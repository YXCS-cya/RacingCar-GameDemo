using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    private MeshRenderer[] WheelMesh;
    private WheelCollider[] wheel;
    private float maxAngel; // 转弯角度
    private float maxTorque; // 最大扭矩（动力）
    private float decelerationTorque; // 减速时的制动扭矩
    private float v, h;

    void Start()
    {
        maxAngel = 60f;
        maxTorque = 1500f; // 增大最大动力以提升加速度
        decelerationTorque = 100f; // 调整减速时的制动力（较小以减少能量流失）
        WheelMesh = transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();
        wheel = transform.GetChild(1).GetComponentsInChildren<WheelCollider>();

        ConfigureWheelFriction(); // 调整轮胎摩擦模型
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (Mathf.Abs(v) > 0 || Mathf.Abs(h) > 0)
        {
            Move(); // 如果有输入，移动车辆
        }
        else
        {
            Decelerate(); // 如果没有输入，减速
        }
    }

    private void Move() // 移动
    {
        // 前轮转弯
        for (int i = 0; i < 2; i++)
        {
            wheel[i].steerAngle = h * maxAngel;
        }

        // 所有轮子加动力
        foreach (var o in wheel)
        {
            o.motorTorque = maxTorque * v;
            o.brakeTorque = 0; // 移动时不施加制动
        }

        // 模型转弯
        for (int i = 0; i < 4; i++)
        {
            WheelMesh[i].transform.localRotation =
                Quaternion.Euler(wheel[i].rpm * 360 / 60, wheel[i].steerAngle, 0);
        }
    }

    private void Decelerate() // 减速
    {
        foreach (var o in wheel)
        {
            o.motorTorque = 0; // 不再提供动力
            o.brakeTorque = decelerationTorque; // 施加制动扭矩
        }
    }

    private void ConfigureWheelFriction()
    {
        foreach (var o in wheel)
        {
            WheelFrictionCurve forwardFriction = o.forwardFriction;
            WheelFrictionCurve sidewaysFriction = o.sidewaysFriction;

            // 调整前向摩擦
            forwardFriction.stiffness = 1.5f; // 增大抓地力以避免滑动
            forwardFriction.extremumSlip = 0.4f; // 调整滑移极限值
            forwardFriction.asymptoteSlip = 0.8f; // 延迟摩擦力衰减

            // 调整侧向摩擦
            sidewaysFriction.stiffness = 1.5f; // 增大侧向抓地力
            sidewaysFriction.extremumSlip = 0.3f; // 更高滑移允许值
            sidewaysFriction.asymptoteSlip = 0.6f;

            o.forwardFriction = forwardFriction;
            o.sidewaysFriction = sidewaysFriction;
        }
    }
}
