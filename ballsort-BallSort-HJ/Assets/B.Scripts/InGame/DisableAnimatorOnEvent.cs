using UnityEngine;

public class DisableAnimatorOnEvent : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("��ǰ����û��Animator�����");
        }
    }

    // �����¼������ĺ��������Ʊ������¼����õ�Functionһ�£�
    public void DisableAnimator()
    {
        if (_animator != null)
        {
            _animator.enabled = false;
            Debug.Log("����������ɣ��ѽ���Animator���");
        }
    }
}