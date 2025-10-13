using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FloatNumberDisplayer : MonoBehaviour
{
    public int Total = -1;

    public float Number
    {
        get => _number;
        set
        {
            if (_number != value)
            {
                float old = _number;
                _number = value;
                HandleAnim(old, value);
            }
        }
    }

    public void ResetNumber(float value)
    {
        _number = value;
        ChangeDirect(value);
    }

    private void HandleAnim(float oldValue, float newValue)
    {
        if (!enabled || oldValue > newValue)
        {
            ChangeDirect(newValue);
        }
        else
        {
            HandleOldAnim();
            _currentCoroutine =
                StartCoroutine(TextPlusAnim(oldValue, newValue, _config.GetFloatSpeedData(DisplaySpeedType.Slow)));
        }
    }

    private void HandleOldAnim()
    {
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);
    }

    private IEnumerator TextPlusAnim(float oldValue, float newValue, FloatDisplaySpeed speedData)
    {
        float current = oldValue;
        float timer = 0;
        while (timer < speedData.Duration && current + speedData.Step <= newValue)
        {
            current += speedData.Step;
            TextFillData(current);
            // _text.Get(gameObject).text = current.ToString("0.00");
            timer += speedData.StepDuration;
            _anim?.FuncScale();
            yield return Yielders.Get(speedData.StepDuration);
        }

        _text.Get(gameObject).text = $"{constantShow}{newValue.ToString("0.00")}";
    }

    private void ChangeDirect(float newValue)
    {
        _text.Get(gameObject).text = $"{constantShow}{newValue.ToString("0.00")}";
    }

    private void OnEnable()
    {
        _text.Get(gameObject).text = $"{constantShow}{Number.ToString("0.00")}";
    }

    public void TextFillData(float value)
    {
        if (Total != -1)
        {
            _text.Get(gameObject).text = $"{value}/{Total}";
        }
        else
        {
            if (value >= 10000)
            {
                _text.Get(gameObject).text = $"{constantShow}{(value / 1000f).ToString("0.00")}";
            }
            else
            {
                _text.Get(gameObject).text = $"{constantShow}{value.ToString("0.00")}";
            }
        }
    }

    [SerializeField] private NumberDisplayerConfig _config = null;
    [SerializeField] private NumberScaleAnim _anim = null;
    [SerializeField] private string constantShow;

    private DelayGetComponent<Text> _text = new DelayGetComponent<Text>();
    private float _number;
    private Coroutine _currentCoroutine;
}