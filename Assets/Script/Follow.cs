using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContorl : MonoBehaviour
{
    //Ŀ������
    public Transform targetOb;
    public Transform[] target;
    private CarMoveControl Control;
    public float speed;
    //���ƽ��ֵ
    public float smoothTime;
    //��ͷ��ѡ��
    public int ChooseIndex;



    [Header("----------����ʱ�������-------------")]
    //����ʱ�ĸ�������
    [Range(1, 5)]
    public float shiftOff;

    //Ŀ����Ұ ��������ʾ�ɼ���
    [SerializeField]
    private float addFov;

    //��ǰ��Ұ
    private float startView;
    [Range(1, 10)]
    public float followLerp = 1;
    [Range(1, 10)]
    public float angleLerp = 1;
    //ΪһЩ���Գ�ʼ��

    private void Start()
    {
        startView = Camera.main.fieldOfView; //������Ŀ�ʼ���Ը���
        addFov = 30;

        //��ȡ��ͷλ��
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
        TabViewChoose();  //ѡ��ͷ
        
    }

    void LateUpdate()
    {
        CameraAtrribute(); //���������ʾ
            
        FllowEffect(); //������湦��
        

        FOXChange();  //����ʱ�����Ұ�ı仯
    }

    //ѡ�񳡾�
    private void TabViewChoose()
    {
        //����Tab��ʱ
        if (InputManager.InputManagerment.TabView)

            ChooseIndex = ChooseIndex > 1 ? 0 : ChooseIndex + 1;
    }

    //������湦��
    private void FllowEffect()
    {
        //-------1.��ͷ�ĸ��淽��һ---------
        //ʵ�ִӲ�ͬ�ĽǶ�����ۿ�
        //transform.position = target[ChooseIndex].position * (1 - smoothTime) + transform.position * smoothTime;
        //transform.LookAt(targetOb);
        ////�ٶȲ�ͬ����ĽǶȲ�ͬ
        //smoothTime = (Control.Km_H >= 150) ? (Mathf.Abs(Control.Km_H) / 150) - 0.85f : 0.45f;

        //-------2.��ͷ�ĸ��淽����---------

        transform.position = Vector3.Lerp(transform.position, target[ChooseIndex].position, Time.deltaTime * speed * 1f);
        transform.position = target[ChooseIndex].position;

        transform.LookAt(targetOb);

    }

    //����ʱ�����Ұ�ı仯
    private void FOXChange()
    {
        if (Input.GetKey(KeyCode.LeftShift)) //��������shift����Ч
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, startView + addFov, Time.deltaTime * shiftOff);
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, startView, Time.deltaTime * shiftOff);
        }

    }

    //���������ʾ
    private void CameraAtrribute()
    {
        //ʵʱ�ٶ�
        Control = targetOb.GetComponent<CarMoveControl>();

        speed = Mathf.Lerp(speed, Control.Km_H / 4, Time.deltaTime);

        speed = Mathf.Clamp(speed, 0, 55);   //��Ӧ���200����ÿСʱ

    }

}

