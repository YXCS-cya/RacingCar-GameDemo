using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//����ģʽ��ѡ��
public enum EDriveType
{
    frontDrive,   //ǰ������
    backDrive,    //��������
    allDrive      //����
}


public class CarMoveControl : MonoBehaviour
{
    //-------------------------------------------
    //�ĸ����ӵ���ײ��
    public WheelCollider[] wheels;

    //����Ļ�ȡ
    public GameObject[] wheelMesh;

    //��ʼ����ά��������Ԫ��
    private Vector3 wheelPosition = Vector3.zero;
    private Quaternion wheelRotation = Quaternion.identity;
    //-------------------------------------------

    //����ģʽѡ�� _Ĭ��ǰ��
    public EDriveType DriveType = EDriveType.frontDrive;

    //----------������������-----------------------

    //������
    public Rigidbody rigidbody;
    //�ְ뾶
    public float radius = 0.36f;
    //Ť������
    public float motorflaot = 8000f;
    //ɲ����
    public float brakVualue = 800000f;
    //�ٶȣ�ÿСʱ���ٹ���
    public int Km_H;
    //��ѹ��
    public float downForceValue = 1000f;
    //������
    public float sideForceValue = 1000f;

    //�ĸ���̥Ť�����Ĵ�С
    public float f_right;
    public float f_left;
    public float b_right;
    public float b_left;

    //���ִ򻬲���ʶ��
    public float[] slip;//����ɲ��

    //����
    public Vector3 CenterMass;

    //һЩ���Եĳ�ʼ��
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        slip = new float[4];

    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        VerticalAttribute();//�����������Թ���
        WheelsAnimation(); //���ֶ���
        VerticalContorl(); //��������
        HorizontalContolr(); //ת�����
        HandbrakControl(); //��ɲ����
        ShiftSpeed();


    }

    //���������������
    public void VerticalAttribute()
    {
        //---------------�ٶ�ʵʱ---------------
        //1m/s = 3.6km/h
        Km_H = (int)(rigidbody.velocity.magnitude * 3.6);
        Km_H = Mathf.Clamp(Km_H, 0, 180);   //�����ٶ�Ϊ 0 �� 200 Km/H֮��

        //--------------Ť����ʵʱ---------------
        //��ʾÿ����̥��Ť��
        f_right = wheels[0].motorTorque;
        f_left = wheels[1].motorTorque;
        b_right = wheels[2].motorTorque;
        b_left = wheels[3].motorTorque;

        //-------------��ѹ�����-----------------

        //�ٶ�Խ����ѹ��Խ��ץ�ظ�ǿ
        if(Km_H >= 180)
            rigidbody.AddForce(-transform.up * downForceValue * rigidbody.velocity.magnitude  *0.6f );

        //-------------��������ͬ��----------------

        //��������Խ���£�Խ�����׷�
        rigidbody.centerOfMass = CenterMass;
    }

    //��ֱ�᷽���˶�������������
    public void VerticalContorl()
    {

        switch (DriveType)
        {
            case EDriveType.frontDrive:
                //ѡ��ǰ��
                if (InputManager.InputManagerment.vertical != 0) //������WS��ʱ��Ч
                {
                    for (int i = 0; i < wheels.Length - 2; i++)
                    {
                        //Ť������
                        wheels[i].motorTorque = InputManager.InputManagerment.vertical * (motorflaot / 2); //Ť���������
                    }
                }
                else
                {
                    for (int i = 0; i < wheels.Length - 2; i++)
                    {
                        //Ť������
                        wheels[i].motorTorque = 0;
                    }
                }
                break;
            case EDriveType.backDrive:
                //ѡ�����
                if (InputManager.InputManagerment.vertical != 0) //������WS��ʱ��Ч
                {
                    for (int i = 2; i < wheels.Length; i++)
                    {
                        //Ť������
                        wheels[i].motorTorque = InputManager.InputManagerment.vertical * (motorflaot / 2); //Ť���������
                    }
                }
                else
                {
                    for (int i = 2; i < wheels.Length; i++)
                    {
                        //Ť������
                        wheels[i].motorTorque = 0;
                    }
                }
                break;
            case EDriveType.allDrive:
                //ѡ������
                if (InputManager.InputManagerment.vertical != 0) //������WS��ʱ��Ч
                {
                    for (int i = 0; i < wheels.Length; i++)
                    {
                        //Ť������
                        wheels[i].motorTorque = InputManager.InputManagerment.vertical * (motorflaot / 4); //Ť������/4
                    }
                }
                else
                {
                    for (int i = 0; i < wheels.Length; i++)
                    {
                        //Ť������
                        wheels[i].motorTorque = 0;
                    }
                }
                break;
            default:
                break;
        }



    }

    //ˮƽ�᷽���˶�����ת�����
    public void HorizontalContolr()
    {
        float turnPercent = InputManager.InputManagerment.horizontal;

        if (turnPercent != 0)
        {
            //���־�ߴ�����Ϊ1.5f ���������Ϊ2.55f ��radius Ĭ��Ϊ6��radius Խ����ת�ĽǶȿ�����ԽС
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * InputManager.InputManagerment.horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * InputManager.InputManagerment.horizontal;

            if(turnPercent > 0.8 || turnPercent < -0.8)
                {
               
                rigidbody.AddForce(-transform.up * downForceValue * rigidbody.velocity.magnitude );
                rigidbody.AddForce(transform.right * sideForceValue * rigidbody.velocity.magnitude * turnPercent * 1.25f);
                rigidbody.AddForce(transform.forward * sideForceValue * rigidbody.velocity.magnitude * turnPercent * 0.6f);
                //Km_H = Mathf.Clamp(Km_H, 0, 120);
            }
            else
            {
                if(turnPercent >0.5 || turnPercent < -0.5)
                {
                    rigidbody.AddForce(-transform.up * downForceValue * rigidbody.velocity.magnitude);
                    rigidbody.AddForce(transform.right * sideForceValue * rigidbody.velocity.magnitude * turnPercent * 1.25f);
                    rigidbody.AddForce(transform.forward * sideForceValue * rigidbody.velocity.magnitude * turnPercent * 0.6f);
                }
            }
            
        }

        else
        {
            rigidbody.AddForce(0,0,0);
            wheels[0].steerAngle = 0;
            wheels[1].steerAngle = 0;
        }

    }

    //��ɲ����
    public void HandbrakControl()
    {
        if (InputManager.InputManagerment.handbanl)
        {
            //����ɲ��
            wheels[2].brakeTorque = brakVualue;
            wheels[3].brakeTorque = brakVualue;
        }
        else
        {
            wheels[2].brakeTorque = 0;
            wheels[3].brakeTorque = 0;
        }

        //------------ɲ��Ч��ƽ������ʾ------------

        for (int i = 0; i < slip.Length; i++)
        {
            WheelHit wheelhit;

            wheels[i].GetGroundHit(out wheelhit);

            slip[i] = wheelhit.forwardSlip; //��̥�ڹ��������ϴ򻬡����ٻ���Ϊ�����ƶ���Ϊ��
        }


    }

     //�����Լ��������
    public void ShiftSpeed()
    {
        //����shift���ټ�ʱ
        if (InputManager.InputManagerment.shiftSpeed)
        {
            //��ǰ����һ����
            rigidbody.AddForce(transform.forward * 2000f );
        }
        else
        {
            rigidbody.AddForce(transform.forward * 0);
        }

    }



    //���ֶ������
    public void WheelsAnimation()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            //��ȡ��ǰ�ռ�ĳ���λ�� �� �Ƕ�
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);

            //��ֵ��
            wheelMesh[i].transform.position = wheelPosition;

            wheelMesh[i].transform.rotation = wheelRotation * Quaternion.AngleAxis(0, Vector3.forward);

        }

    }
}

