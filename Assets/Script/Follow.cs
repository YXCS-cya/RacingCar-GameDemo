using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContorl : MonoBehaviour
{
    //目标物体
    public Transform targetOb;
    public Transform[] target;
    private CarMoveControl Control;
    public float speed;
    //相机平滑值
    public float smoothTime;
    //镜头的选择
    public int ChooseIndex;



    [Header("----------加速时相机属性-------------")]
    //加速时的跟随力度
    [Range(1, 5)]
    public float shiftOff;

    //目标视野 （让其显示可见）
    [SerializeField]
    private float addFov;

    //当前视野
    private float startView;
    [Range(1, 10)]
    public float followLerp = 1;
    [Range(1, 10)]
    public float angleLerp = 1;
    //为一些属性初始化

    private void Start()
    {
        startView = Camera.main.fieldOfView; //将相机的开始属性赋入
        addFov = 30;

        //获取镜头位置
        for (int i = 0; i < 3; i++)
        {
            target[i] = targetOb.GetChild(0).GetChild(i);
        }
    }

    private void Update()
    {
        
    }
    void FixedUpdate()
    {
        TabViewChoose();  //选择镜头
        
    }

    void LateUpdate()
    {
        CameraAtrribute(); //相机属性显示
            
        FllowEffect(); //相机跟随功能
        

        FOXChange();  //加速时相机视野的变化
    }

    //选择场景
    private void TabViewChoose()
    {
        //按下Tab键时
        if (InputManager.InputManagerment.TabView)

            ChooseIndex = ChooseIndex > 1 ? 0 : ChooseIndex + 1;
    }

    //相机跟随功能
    private void FllowEffect()
    {
        //-------1.镜头的跟随方法一---------
        //实现从不同的角度切入观看
        //transform.position = target[ChooseIndex].position * (1 - smoothTime) + transform.position * smoothTime;
        //transform.LookAt(targetOb);
        ////速度不同跟随的角度不同
        //smoothTime = (Control.Km_H >= 150) ? (Mathf.Abs(Control.Km_H) / 150) - 0.85f : 0.45f;

        //-------2.镜头的跟随方法二---------

        transform.position = Vector3.Lerp(transform.position, target[ChooseIndex].position, Time.deltaTime * speed * 1f);
        transform.position = target[ChooseIndex].position;

        transform.LookAt(targetOb);

    }

    //加速时相机视野的变化
    private void FOXChange()
    {
        if (Input.GetKey(KeyCode.LeftShift)) //按下坐标shift键生效
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, startView + addFov, Time.deltaTime * shiftOff);
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, startView, Time.deltaTime * shiftOff);
        }

    }

    //相机属性显示
    private void CameraAtrribute()
    {
        //实时速度
        Control = targetOb.GetComponent<CarMoveControl>();

        speed = Mathf.Lerp(speed, Control.Km_H / 4, Time.deltaTime);

        speed = Mathf.Clamp(speed, 0, 55);   //对应最大200公里每小时

    }

}

