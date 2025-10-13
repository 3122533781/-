using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using UnityEngine;

public class WaitingDialog : Dialog
{
    private bool _show = false;
    private float _runTime = 0f;

    private void OnEnable()
    {
        _runTime = 0f;
        _show = true;
    }

    private void OnDisable()
    {
        _runTime = 0f;
        _show = false;
    }

    private void Update()
    {
        if (_show)
        {
            _runTime += Time.deltaTime;
            if (_runTime >= 10f)
            {
                Deactivate();
            }
        }

    }
}
