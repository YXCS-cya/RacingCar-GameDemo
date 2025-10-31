using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//驱动模式的选择
public enum EDriveType
{
    frontDrive,   //前轮驱动
    backDrive,    //后轮驱动
    allDrive      //四驱
}


public class CarMoveControl : MonoBehaviour
{
    //-------------------------------------------
    //四个轮子的碰撞器
    public WheelCollider[] wheels;

    //网格的获取
    public GameObject[] wheelMesh;

    //初始化三维向量和四元数
    private Vector3 wheelPosition = Vector3.zero;
    private Quaternion wheelRotation = Quaternion.identity;
    //-------------------------------------------

    //驱动模式选择 _默认前驱
    public EDriveType DriveType = EDriveType.frontDrive;

    //----------车辆属性特征-----------------------

    //车刚体
    public Rigidbody rigidbody;
    //轮半径
    public float radius = 0.36f;
    //扭矩力度
    public float motorflaot = 8000f;
    //刹车力
    public float brakVualue = 800000f;
    //速度：每小时多少公里
    public int Km_H;
    //下压力
    public float downForceValue = 1000f;
    //侧向力
    public float sideForceValue = 1000f;

    //四个轮胎扭矩力的大小
    public float f_right;
    public float f_left;
    public float b_right;
    public float b_left;

    //车轮打滑参数识别
    public float[] slip;//用于刹车

    //质心
    public Vector3 CenterMass;

    //一些属性的初始化
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
        VerticalAttribute();//车辆物理属性管理
        WheelsAnimation(); //车轮动画
        VerticalContorl(); //驱动管理
        HorizontalContolr(); //转向管理
        HandbrakControl(); //手刹管理
        ShiftSpeed();


    }

    //车辆物理属性相关
    public void VerticalAttribute()
    {
        //---------------速度实时---------------
        //1m/s = 3.6km/h
        Km_H = (int)(rigidbody.velocity.magnitude * 3.6);
        Km_H = Mathf.Clamp(Km_H, 0, 180);   //油门速度为 0 到 200 Km/H之间

        //--------------扭矩力实时---------------
        //显示每个轮胎的扭矩
        f_right = wheels[0].motorTorque;
        f_left = wheels[1].motorTorque;
        b_right = wheels[2].motorTorque;
        b_left = wheels[3].motorTorque;

        //-------------下压力添加-----------------

        //速度越大，下压力越大，抓地更强
        if(Km_H >= 180)
            rigidbody.AddForce(-transform.up * downForceValue * rigidbody.velocity.magnitude  *0.6f );

        //-------------质量中心同步----------------

        //质量中心越贴下，越不容易翻
        rigidbody.centerOfMass = CenterMass;
    }

    //垂直轴方向运动管理（驱动管理）
    public void VerticalContorl()
    {

        switch (DriveType)
        {
            case EDriveType.frontDrive:
                //选择前驱
                if (InputManager.InputManagerment.vertical != 0) //当按下WS键时生效
                {
                    for (int i = 0; i < wheels.Length - 2; i++)
                    {
                        //扭矩力度
                        wheels[i].motorTorque = InputManager.InputManagerment.vertical * (motorflaot / 2); //扭矩马力归半
                    }
                }
                else
                {
                    for (int i = 0; i < wheels.Length - 2; i++)
                    {
                        //扭矩力度
                        wheels[i].motorTorque = 0;
                    }
                }
                break;
            case EDriveType.backDrive:
                //选择后驱
                if (InputManager.InputManagerment.vertical != 0) //当按下WS键时生效
                {
                    for (int i = 2; i < wheels.Length; i++)
                    {
                        //扭矩力度
                        wheels[i].motorTorque = InputManager.InputManagerment.vertical * (motorflaot / 2); //扭矩马力归半
                    }
                }
                else
                {
                    for (int i = 2; i < wheels.Length; i++)
                    {
                        //扭矩力度
                        wheels[i].motorTorque = 0;
                    }
                }
                break;
            case EDriveType.allDrive:
                //选择四驱
                if (InputManager.InputManagerment.vertical != 0) //当按下WS键时生效
                {
                    for (int i = 0; i < wheels.Length; i++)
                    {
                        //扭矩力度
                        wheels[i].motorTorque = InputManager.InputManagerment.vertical * (motorflaot / 4); //扭矩马力/4
                    }
                }
                else
                {
                    for (int i = 0; i < wheels.Length; i++)
                    {
                        //扭矩力度
                        wheels[i].motorTorque = 0;
                    }
                }
                break;
            default:
                break;
        }



    }

    //水平轴方向运动管理（转向管理）
    public void HorizontalContolr()
    {
        float turnPercent = InputManager.InputManagerment.horizontal;

        if (turnPercent != 0)
        {
            //后轮距尺寸设置为1.5f ，轴距设置为2.55f ，radius 默认为6，radius 越大旋转的角度看起来越小
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

    //手刹管理
    public void HandbrakControl()
    {
        if (InputManager.InputManagerment.handbanl)
        {
            //后轮刹车
            wheels[2].brakeTorque = brakVualue;
            wheels[3].brakeTorque = brakVualue;
        }
        else
        {
            wheels[2].brakeTorque = 0;
            wheels[3].brakeTorque = 0;
        }

        //------------刹车效果平滑度显示------------

        for (int i = 0; i < slip.Length; i++)
        {
            WheelHit wheelhit;

            wheels[i].GetGroundHit(out wheelhit);

            slip[i] = wheelhit.forwardSlip; //轮胎在滚动方向上打滑。加速滑移为负，制动滑为正
        }


    }

     //加速以及动画相关
    public void ShiftSpeed()
    {
        //按下shift加速键时
        if (InputManager.InputManagerment.shiftSpeed)
        {
            //向前增加一个力
            rigidbody.AddForce(transform.forward * 2000f );
        }
        else
        {
            rigidbody.AddForce(transform.forward * 0);
        }

    }



    //车轮动画相关
    public void WheelsAnimation()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            //获取当前空间的车轮位置 和 角度
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);

            //赋值给
            wheelMesh[i].transform.position = wheelPosition;

            wheelMesh[i].transform.rotation = wheelRotation * Quaternion.AngleAxis(0, Vector3.forward);

        }

    }
}

