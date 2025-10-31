using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//功能： 输入控制管理器

public class InputManager : MonoBehaviour
{
    //单例模式管理
    static private InputManager inputManagerment;
    static public InputManager InputManagerment => inputManagerment;

    public float horizontal;  //水平方向动力值
    public float vertical;    //垂直方向动力值
    public bool handbanl;    //手刹动力值
    public bool shiftSpeed;   //加速shift键
    public bool TabView;

    void Awake()
    {
        inputManagerment = this;
    }

    void Update()
    {
        //与Unity中输入管理器的值相互对应

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        handbanl = Input.GetAxis("Jump") != 0 ? true : false; //按下空格键时就是1，否则为0
        shiftSpeed = Input.GetKey(KeyCode.LeftShift) ? true : false;
        TabView = Input.GetKey(KeyCode.Tab) ? true : false;
    }
}

