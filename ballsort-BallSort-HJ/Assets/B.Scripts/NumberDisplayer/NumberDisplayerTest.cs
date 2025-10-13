using System.Collections;
using System.Collections.Generic;
using Prime31;
using UnityEngine;

public class NumberDisplayerTest : MonoBehaviour, Prime31.IObjectInspectable
{
    [MakeButton]
    public void Add()
    {
        _displayer2.Number += 50;
    }
    
    [MakeButton]
    public void Set()
    {
        _displayer2.Number = _setValue;
    }

    [MakeButton]
    public void Recover()
    {
        _displayer2.Number = 0;
    }

    [SerializeField] private int _setValue = 10;
    [SerializeField] private IntNumberDisplayer _displayer2 = null;
}