using UnityEngine;

public class CarCollision : MonoBehaviour
{
    // ��������
    public AudioClip collisionSound;  // ��ײ��Ч
    private AudioSource audioSource; // ��Ч�������

    void Start()
    {
        // ��ȡ��Ч�������
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && collisionSound != null)
        {
            // ���û�� AudioSource����̬���һ��
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    // ��ײ���
    void OnCollisionEnter(Collision collision)
    {
        // ��ӡ��ײ��Ϣ
        Debug.Log($"С�������ǽ��������ײ��");
        //{collision.gameObject.name}
        // ������Ч
        if (collisionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(collisionSound);
        }


        // ִ�������߼������������ֵ�������¼��ȣ�
    }


}
