using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FloatingWindow : MonoSingleton<FloatingWindow>, Prime31.IObjectInspectable
{
    [Prime31.MakeButton]
    public void Test()
    {
        Show("功能暂未开放");
    }

    public void Show(string text, float duration)
    {
        _isUseSetTime = true;
        _setWaitTime = duration;
        _text.text = text;
        _display.Show();
        StopCoroutine("Wait");
        StartCoroutine("Wait");
    }

    public void Show(string text)
    {
        AudioClipHelper.Instance.PlaySound(AudioClipEnum.Tips);
        _isUseSetTime = false;
        _text.text = text;
        _display.Show();
        StopCoroutine("Wait");
        StartCoroutine("Wait");
    }

    private IEnumerator Wait()
    {
        yield return Yielders.Get(_isUseSetTime ? _setWaitTime : _waitTime);
        _display.Hide(() => { });
    }

    [SerializeField] private float _waitTime = 0.5f;
    [SerializeField] private Text _text = null;
    [SerializeField] private GroupDisplay _display;

    private bool _isUseSetTime;
    private float _setWaitTime;
}