using UnityEngine;

namespace Soybean.GameWorker
{
    public class GameWorkerGUI
    {
        public void Init(GameWorker context)
        {
            _context = context;
            _windowRect = new Rect(0f, 60f, WIDTH, HEIGHT - 60f);
            _titleRect = new Rect(0f, 0f, _windowRect.width, 24f);
        }

        public void Draw()
        {
            // GUI.skin = DebugConsoleControl.Instance.Skin;
            float ratio = Screen.width / WIDTH < Screen.height / HEIGHT ? Screen.width / WIDTH : Screen.height / HEIGHT;
            if (ratio != _ratio)
            {
                _ratio = ratio;
                _guiMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(ratio, ratio, 1));
            }

            GUI.matrix = _guiMatrix;
            _windowRect = GUI.Window(0, _windowRect, WindowFunction, "GameWorker Console");
        }


        private void WindowFunction(int windowID)
        {
            GUI.DragWindow(_titleRect);

            GUILayout.Space(5);

            _scrollDebug = GUILayout.BeginScrollView(_scrollDebug);
            {
                //Draw
                GameWorkerGUIMain.Draw(_context);

                GUIUtility.ScaleAroundPivot(Vector2.one, Vector2.zero);
            }
            GUILayout.EndScrollView();
        }

        private static Texture2D _staticRectTexture;
        private static GUIStyle _staticRectStyle;

        private Vector2 _scrollDebug;
        private Rect _windowRect;
        private Rect _titleRect;
        private float _ratio;
        private Matrix4x4 _guiMatrix;

        private float WIDTH = Screen.width / 2;
        private float HEIGHT = Screen.height / 2;
        private GameWorker _context;
    }
}