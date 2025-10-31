using UnityEngine;

public class CarCollision : MonoBehaviour
{
    // 公共属性
    public AudioClip collisionSound;  // 碰撞音效
    private AudioSource audioSource; // 音效播放组件

    void Start()
    {
        // 获取音效播放组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && collisionSound != null)
        {
            // 如果没有 AudioSource，动态添加一个
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    // 碰撞检测
    void OnCollisionEnter(Collision collision)
    {
        // 打印碰撞信息
        Debug.Log($"小车与空气墙发生了碰撞！");
        //{collision.gameObject.name}
        // 播放音效
        if (collisionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(collisionSound);
        }


        // 执行其他逻辑（如减少生命值、触发事件等）
    }


}
