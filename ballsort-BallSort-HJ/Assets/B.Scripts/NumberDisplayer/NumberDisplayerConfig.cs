using System.Collections.Generic;
using UnityEngine;

public class NumberDisplayerConfig : ScriptableObject
{
    [SerializeField] public List<IntDisplaySpeed> IntSpeedDatas = null;
    [SerializeField] public List<FloatDisplaySpeed> FloatSpeedDatas = null;

    public IntDisplaySpeed GetIntSpeedData(DisplaySpeedType type)
    {
        if (_typeToIntSpeedData == null)
        {
            InitIntDic();
        }

        return _typeToIntSpeedData[type];
    }
    
    public FloatDisplaySpeed GetFloatSpeedData(DisplaySpeedType type)
    {
        if (_typeToFloatSpeedData == null)
        {
            InitFloatDic();
        }

        return _typeToFloatSpeedData[type];
    }

    private void InitIntDic()
    {
        _typeToIntSpeedData = new Dictionary<DisplaySpeedType, IntDisplaySpeed>();
        foreach (IntDisplaySpeed speed in IntSpeedDatas)
        {
            _typeToIntSpeedData.Add(speed.Type, speed);
        }
    }
    
    private void InitFloatDic()
    {
        _typeToFloatSpeedData = new Dictionary<DisplaySpeedType, FloatDisplaySpeed>();
        foreach (FloatDisplaySpeed speed in FloatSpeedDatas)
        {
            _typeToFloatSpeedData.Add(speed.Type, speed);
        }
    }
    
    private Dictionary<DisplaySpeedType, IntDisplaySpeed> _typeToIntSpeedData;
    private Dictionary<DisplaySpeedType, FloatDisplaySpeed> _typeToFloatSpeedData;

}
