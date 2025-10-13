using UnityEngine;

public class DebugConsole : MonoBehaviour
{
    public static void DrawCover()
    {
        GUIDrawRect(new Rect(0, 0, Screen.width, Screen.height), Color.gray);
    }

    private void Awake()
    {
        _debugPages = new IDebugPage[]
        {
            // new DebugLevel(),
            new DebugPlayer(),
            new DebugAD(),
            // new DebugGoldenKey(),
        };

        _debugPageDescriptors = new string[_debugPages.Length];

        for (int i = 0; i < _debugPages.Length; i++)
        {
            _debugPageDescriptors[i] = _debugPages[i].Title;
        }

        _currentPageSelectedIdx = 0;
        _currentPageSelected = _debugPages[0];

        _windowRect = new Rect(0f, 60f, WIDTH, HEIGHT - 60f);
        _titleRect = new Rect(0f, 0f, _windowRect.width, 24f);
    }

    private void OnGUI()
    {
        // GUI.skin = DebugConsoleControl.Instance.Skin;
        float ratio = Screen.width / WIDTH < Screen.height / HEIGHT ? Screen.width / WIDTH : Screen.height / HEIGHT;
        if (ratio != _ratio)
        {
            _ratio = ratio;
            _guiMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(ratio, ratio, 1));
        }

        GUI.matrix = _guiMatrix;
        _windowRect = GUI.Window(0, _windowRect, WindowFunction, "Debug Console");
    }

    private void WindowFunction(int windowID)
    {
        GUI.DragWindow(_titleRect);

        int sel = GUILayout.SelectionGrid(_currentPageSelectedIdx, _debugPageDescriptors, _debugPageDescriptors.Length);
        if (sel != _currentPageSelectedIdx)
        {
            sel = Mathf.Clamp(sel, 0, _debugPages.Length - 1);
            _currentPageSelectedIdx = sel;
            _currentPageSelected = _debugPages[sel];
        }

        GUILayout.Space(5);

        _scrollDebug = GUILayout.BeginScrollView(_scrollDebug);
        {
            if (_currentPageSelected != null)
            {
                _currentPageSelected.Draw();
            }

            GUIUtility.ScaleAroundPivot(Vector2.one, Vector2.zero);
        }
        GUILayout.EndScrollView();
    }

    private static Texture2D _staticRectTexture;
    private static GUIStyle _staticRectStyle;

    private static void GUIDrawRect(Rect position, Color color)
    {
        if (_staticRectTexture == null)
        {
            _staticRectTexture = new Texture2D(1, 1);
        }

        if (_staticRectStyle == null)
        {
            _staticRectStyle = new GUIStyle();
        }

        _staticRectTexture.SetPixel(0, 0, color);
        _staticRectTexture.Apply();

        _staticRectStyle.normal.background = _staticRectTexture;

        GUI.Box(position, GUIContent.none, _staticRectStyle);
    }

    private int _currentPageSelectedIdx;
    private string[] _debugPageDescriptors;
    private IDebugPage[] _debugPages = new IDebugPage[0];
    private IDebugPage _currentPageSelected;
    private Vector2 _scrollDebug;

    private Rect _windowRect;
    private Rect _titleRect;
    private float _ratio;
    private Matrix4x4 _guiMatrix;

    private float WIDTH = Screen.width / 2;
    private float HEIGHT = Screen.height / 2;
    private const float NEW_REF_MILLISECONDS = 5000f;
}