using UnityEngine;
using WhiteEggTart;

public class DebugConsoleControl : MonoSingleton<DebugConsoleControl>
{
    [SerializeField] public GUISkin Skin = null;

    [RegisterCommand(Help = "打开Debug窗口", Name = "opendebug")]
    public static void CommandOpenDebugConsole(bool isOpen)
    {
        DebugConsoleControl.Instance.enabled = isOpen;
    }

    private void Awake()
    {
#if UNITY_EDITOR
       // enabled = false;
#endif
    }

    private void OnGUI()
    {
        GUI.skin = Skin;
        if (_openButton && GUILayout.Button("                debug", GUILayout.Height(50), GUILayout.Width(250)))
        {
            SwitchDebug();
        }
    }

    private void OnDebugClick()
    {
        _needToPause = true;
        SwitchDebug();
    }

    public void SwitchDebug()
    {
        if (_debugConsole.enabled)
        {
            Unpause();
        }
        else
        {
            if (_needToPause)
            {
                Pause();
            }
        }

        _debugConsole.enabled = !_debugConsole.enabled;
    }

    private void Update()
    {
        _needToPause = Input.GetKey(KeyCode.LeftAlt);

        if (Input.touchCount == 3)
        {
            _openTimer += Time.deltaTime;

            if (_openTimer >= 5)
            {
                _openButton = !_openButton;
                _openTimer = 0f;
            }
        }
        else
        {
            _openTimer = 0f;
        }
    }

    private void Pause()
    {
        _timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0;
    }

    private void Unpause()
    {
        Time.timeScale = _timeScaleBeforePause > 0 ? _timeScaleBeforePause : 1.0f;
    }

    [SerializeField] private DebugConsole _debugConsole = null;


    private float _timeScaleBeforePause;
    private bool _needToPause = false;

    private bool _openButton = true;

    private float _openTimer;
}