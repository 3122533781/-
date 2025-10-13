using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntNumberDisplayer : MonoBehaviour, Prime31.IObjectInspectable
{
    public int Total = -1;

    public int Number
    {
        get => _number;
        set
        {
            if (_number != value)
            {
                int old = _number;
                _number = value;
                HandleAnim(old, value);
            }
        }
    }

    public void ResetNumber(int value)
    {
        _number = value;
        ChangeDirect(value);
    }

    private void HandleAnim(int oldValue, int newValue)
    {
        if (!enabled || oldValue > newValue)
        {
            ChangeDirect(newValue);
        }
        else
        {
            HandleOldAnim();
            ShowLight();
            //_currentCoroutine =
            //    StartCoroutine(TextPlusAnim(oldValue, newValue, _config.GetIntSpeedData(DisplaySpeedType.Slow)));
        }
    }

    private void HandleOldAnim()
    {
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);
    }


    private IEnumerator TextPlusAnim(int oldValue, int newValue, IntDisplaySpeed speedData)
    {
        int current = oldValue;
        float timer = 0;
        ShowLight(true);
        while (timer < speedData.Duration && current + speedData.Step <= newValue)
        {
            current += speedData.Step;
            TextFillData(current);
            timer += speedData.StepDuration;
            _anim?.FuncScale();
            yield return Yielders.Get(speedData.StepDuration);
        }

        TextFillData(newValue);
    }

    void ShowLight(bool show = false)
    {
    }

    private void ChangeDirect(int newValue)
    {
        TextFillData(newValue);
    }

    public void TextFillManual(int value, string manual = "")
    {
        _number = value;
        _text.Get(gameObject).text = manual;
    }

    public void TextFillData(int value)
    {
        if (Total != -1)
        {
            _text.Get(gameObject).text = $"{value}/{Total}";
        }
        else
        {
            if (value >= 10000)
            {
                _text.Get(gameObject).text = (value / 1000f).ToString("#0.0k");
            }
            else
            {
                _text.Get(gameObject).text = value.ToString();
            }
        }
    }

    [SerializeField] private NumberDisplayerConfig _config = null;
    [SerializeField] private NumberScaleAnim _anim = null;

    private DelayGetComponent<Text> _text = new DelayGetComponent<Text>();
    private int _number;

    private Coroutine _currentCoroutine;
}

[System.Serializable]
public class IntDisplaySpeed
{
    public DisplaySpeedType Type;
    public float Duration; //总共到达需要的时间
    public int Step; //每次跳跃数字大小
    public float StepDuration; //每次跳跃间隔时间
}

public enum DisplaySpeedType
{
    Slow,
    Medium1,
    Medium2,
    Fast,
}