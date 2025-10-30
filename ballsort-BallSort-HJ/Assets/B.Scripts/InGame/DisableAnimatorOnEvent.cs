using UnityEngine;

public class DisableAnimatorOnEvent : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("当前物体没有Animator组件！");
        }
    }

    // 动画事件触发的函数（名称必须与事件配置的Function一致）
    public void DisableAnimator()
    {
        if (_animator != null)
        {
            _animator.enabled = false;
            Debug.Log("动画播放完成，已禁用Animator组件");
        }
    }
}