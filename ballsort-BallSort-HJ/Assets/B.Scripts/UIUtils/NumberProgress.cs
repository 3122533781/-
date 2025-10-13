using UnityEngine;
using UnityEngine.UI;

public class NumberProgress : MonoBehaviour
{
    public void Reset(int max)
    {
        _max = max;
        if (_type == DisplayType.NumToNum)
        {
            _textDisplay.text = $"{Mathf.RoundToInt(Progress.CurrentPercentage * _max)}/{_max}";
        }
        else
        {
            _textDisplay.text = $"{Mathf.RoundToInt(Progress.CurrentPercentage * 100)}";
        }
    }

    private void Update()
    {
        if (_type == DisplayType.NumToNum)
        {
            _textDisplay.text = $"{Mathf.RoundToInt(Progress.CurrentPercentage * _max)}/{_max}";
        }
        else
        {
            _textDisplay.text = $"{Mathf.RoundToInt(Progress.CurrentPercentage * 100)}";
        }
    }

    private IProgress Progress => _progressInterface as IProgress;

    [SerializeField] private MonoBehaviour _progressInterface;
    [SerializeField] private Text _textDisplay = null;
    [SerializeField] private DisplayType _type = DisplayType.NumToNum;

    private int _max = 10;

    private enum DisplayType
    {
        NumToNum,
        Percentage,
    }
}