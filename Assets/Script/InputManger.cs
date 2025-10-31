using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ܣ� ������ƹ�����

public class InputManager : MonoBehaviour
{
    //����ģʽ����
    static private InputManager inputManagerment;
    static public InputManager InputManagerment => inputManagerment;

    public float horizontal;  //ˮƽ������ֵ
    public float vertical;    //��ֱ������ֵ
    public bool handbanl;    //��ɲ����ֵ
    public bool shiftSpeed;   //����shift��
    public bool TabView;

    void Awake()
    {
        inputManagerment = this;
    }

    void Update()
    {
        //��Unity�������������ֵ�໥��Ӧ

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        handbanl = Input.GetAxis("Jump") != 0 ? true : false; //���¿ո��ʱ����1������Ϊ0
        shiftSpeed = Input.GetKey(KeyCode.LeftShift) ? true : false;
        TabView = Input.GetKey(KeyCode.Tab) ? true : false;
    }
}

