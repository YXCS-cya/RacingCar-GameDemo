using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    private MeshRenderer[] WheelMesh;
    private WheelCollider[] wheel;
    private float maxAngel; // ת��Ƕ�
    private float maxTorque; // ���Ť�أ�������
    private float decelerationTorque; // ����ʱ���ƶ�Ť��
    private float v, h;

    void Start()
    {
        maxAngel = 60f;
        maxTorque = 1500f; // ������������������ٶ�
        decelerationTorque = 100f; // ��������ʱ���ƶ�������С�Լ���������ʧ��
        WheelMesh = transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();
        wheel = transform.GetChild(1).GetComponentsInChildren<WheelCollider>();

        ConfigureWheelFriction(); // ������̥Ħ��ģ��
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (Mathf.Abs(v) > 0 || Mathf.Abs(h) > 0)
        {
            Move(); // ��������룬�ƶ�����
        }
        else
        {
            Decelerate(); // ���û�����룬����
        }
    }

    private void Move() // �ƶ�
    {
        // ǰ��ת��
        for (int i = 0; i < 2; i++)
        {
            wheel[i].steerAngle = h * maxAngel;
        }

        // �������ӼӶ���
        foreach (var o in wheel)
        {
            o.motorTorque = maxTorque * v;
            o.brakeTorque = 0; // �ƶ�ʱ��ʩ���ƶ�
        }

        // ģ��ת��
        for (int i = 0; i < 4; i++)
        {
            WheelMesh[i].transform.localRotation =
                Quaternion.Euler(wheel[i].rpm * 360 / 60, wheel[i].steerAngle, 0);
        }
    }

    private void Decelerate() // ����
    {
        foreach (var o in wheel)
        {
            o.motorTorque = 0; // �����ṩ����
            o.brakeTorque = decelerationTorque; // ʩ���ƶ�Ť��
        }
    }

    private void ConfigureWheelFriction()
    {
        foreach (var o in wheel)
        {
            WheelFrictionCurve forwardFriction = o.forwardFriction;
            WheelFrictionCurve sidewaysFriction = o.sidewaysFriction;

            // ����ǰ��Ħ��
            forwardFriction.stiffness = 1.5f; // ����ץ�����Ա��⻬��
            forwardFriction.extremumSlip = 0.4f; // �������Ƽ���ֵ
            forwardFriction.asymptoteSlip = 0.8f; // �ӳ�Ħ����˥��

            // ��������Ħ��
            sidewaysFriction.stiffness = 1.5f; // �������ץ����
            sidewaysFriction.extremumSlip = 0.3f; // ���߻�������ֵ
            sidewaysFriction.asymptoteSlip = 0.6f;

            o.forwardFriction = forwardFriction;
            o.sidewaysFriction = sidewaysFriction;
        }
    }
}
