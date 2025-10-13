using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraShake : MonoBehaviour
{
    public enum ShakeSpace
    {
        Screen,
        World
    }
    [Space]
    public bool useMainCamera = true;
    public List<Camera> cameras = new List<Camera>();
    [Space]
    public float delay = 0.0f;
    public float duration = 1.0f;
    public ShakeSpace shakeSpace = ShakeSpace.Screen;
    public Vector3 shakeStrength = new Vector3(0.1f, 0.1f, 0.1f);
    public AnimationCurve shakeCurve = AnimationCurve.Linear(0, 1, 1, 0);
    [Space]
    [Range(0, 0.1f)] public float shakesDelay = 0;

    [System.NonSerialized] public bool isShaking;
    Dictionary<Camera, Vector3> camerasPreRenderPosition = new Dictionary<Camera, Vector3>();
    Vector3 shakeVector;
    float delaysTimer;
    float time;
    bool shake;

    const float GLOBAL_CAMERA_SHAKE_MULTIPLIER = 1.0f;
    bool s_CallbackRegistered;
    List<CameraShake> s_CameraShakes = new List<CameraShake>();

    void Awake()
    {
        fetchCameras();
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
        StopShake();
    }

    void Update()
    {
        if(!shake) return;
        time += Time.deltaTime;
        Animate(time);
    }

    public void Shake()
    {
        shake = true;
    }

    public void Animate(float time)
    {
#if UNITY_EDITOR
        if (!isShaking)
        {
            fetchCameras();
        }
#endif
        animate(time);
    }

#if UNITY_2019_1_OR_NEWER
    void OnPreRenderCamera_Static_URP(ScriptableRenderContext context, Camera cam)
    {
        OnPreRenderCamera_Static(cam);
    }
    void OnPostRenderCamera_Static_URP(ScriptableRenderContext context, Camera cam)
    {
        OnPostRenderCamera_Static(cam);
    }
#endif

    void OnPreRenderCamera_Static(Camera cam)
    {
        int count = s_CameraShakes.Count;
        for (int i = 0; i < count; i++)
        {
            var ss = s_CameraShakes[i];
            ss.onPreRenderCamera(cam);
        }
    }

    void OnPostRenderCamera_Static(Camera cam)
    {
        int count = s_CameraShakes.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            var ss = s_CameraShakes[i];
            ss.onPostRenderCamera(cam);
        }
    }

    void RegisterStaticCallback(CameraShake cameraShake)
    {
        s_CameraShakes.Add(cameraShake);

        if (!s_CallbackRegistered)
        {
#if UNITY_2019_1_OR_NEWER
#if UNITY_2019_3_OR_NEWER
            if (GraphicsSettings.currentRenderPipeline == null)
#else
				if (GraphicsSettings.renderPipelineAsset == null)
#endif
            {
                // Built-in Render Pipeline
                Camera.onPreRender += OnPreRenderCamera_Static;
                Camera.onPostRender += OnPostRenderCamera_Static;
            }
            else
            {
                // URP
                RenderPipelineManager.beginCameraRendering += OnPreRenderCamera_Static_URP;
                RenderPipelineManager.endCameraRendering += OnPostRenderCamera_Static_URP;
            }
#else
				Camera.onPreRender += OnPreRenderCamera_Static;
				Camera.onPostRender += OnPostRenderCamera_Static;
#endif

            s_CallbackRegistered = true;
        }
    }

    void UnregisterStaticCallback(CameraShake cameraShake)
    {
        s_CameraShakes.Remove(cameraShake);

        if (s_CallbackRegistered && s_CameraShakes.Count == 0)
        {
#if UNITY_2019_1_OR_NEWER
#if UNITY_2019_3_OR_NEWER
            if (GraphicsSettings.currentRenderPipeline == null)
#else
			if (GraphicsSettings.renderPipelineAsset == null)
#endif
            {
                // Built-in Render Pipeline
                Camera.onPreRender -= OnPreRenderCamera_Static;
                Camera.onPostRender -= OnPostRenderCamera_Static;
            }
            else
            {
                // URP
                RenderPipelineManager.beginCameraRendering -= OnPreRenderCamera_Static_URP;
                RenderPipelineManager.endCameraRendering -= OnPostRenderCamera_Static_URP;
            }
#else
				Camera.onPreRender -= OnPreRenderCamera_Static;
				Camera.onPostRender -= OnPostRenderCamera_Static;
#endif

            s_CallbackRegistered = false;
        }
    }

    void onPreRenderCamera(Camera cam)
    {
#if UNITY_EDITOR
        //add scene view camera if necessary
        if (SceneView.currentDrawingSceneView != null && SceneView.currentDrawingSceneView.camera == cam && !camerasPreRenderPosition.ContainsKey(cam))
        {
            camerasPreRenderPosition.Add(cam, cam.transform.localPosition);
        }
#endif

        if (isShaking && camerasPreRenderPosition.ContainsKey(cam))
        {
            camerasPreRenderPosition[cam] = cam.transform.localPosition;

            if (Time.timeScale <= 0) return;

            switch (shakeSpace)
            {
                case ShakeSpace.Screen: cam.transform.localPosition += cam.transform.rotation * shakeVector; break;
                case ShakeSpace.World: cam.transform.localPosition += shakeVector; break;
            }
        }
    }

    void onPostRenderCamera(Camera cam)
    {
        if (camerasPreRenderPosition.ContainsKey(cam))
        {
            cam.transform.localPosition = camerasPreRenderPosition[cam];
        }
    }

    public void fetchCameras()
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlayingOrWillChangePlaymode)
        {
            return;
        }
#endif

        foreach (var cam in cameras)
        {
            if (cam == null) continue;

            camerasPreRenderPosition.Remove(cam);
        }

        cameras.Clear();

        if (useMainCamera && Camera.main != null)
        {
            cameras.Add(Camera.main);
        }

        foreach (var cam in cameras)
        {
            if (cam == null) continue;

            if (!camerasPreRenderPosition.ContainsKey(cam))
            {
                camerasPreRenderPosition.Add(cam, Vector3.zero);
            }
        }
    }

    public void StartShake()
    {
        if (isShaking)
        {
            StopShake();
        }

        isShaking = true;
        RegisterStaticCallback(this);
    }

    public void StopShake()
    {
        isShaking = false;
        shakeVector = Vector3.zero;
        UnregisterStaticCallback(this);
        shake = false;
        time = 0f;
    }

    public void animate(float time)
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            shakeVector = Vector3.zero;
            return;
        }
#endif

        float totalDuration = duration + delay;
        if (time < totalDuration)
        {
            if (time < delay)
            {
                return;
            }

            if (!isShaking)
            {
                this.StartShake();
            }

            // duration of the camera shake
            float delta = Mathf.Clamp01(time / totalDuration);

            // delay between each camera move
            if (shakesDelay > 0)
            {
                delaysTimer += Time.deltaTime;
                if (delaysTimer < shakesDelay)
                {
                    return;
                }
                else
                {
                    while (delaysTimer >= shakesDelay)
                    {
                        delaysTimer -= shakesDelay;
                    }
                }
            }

            var randomVec = new Vector3(Random.value, Random.value, Random.value);
            var shakeVec = Vector3.Scale(randomVec, shakeStrength) * (Random.value > 0.5f ? -1 : 1);
            shakeVector = shakeVec * shakeCurve.Evaluate(delta) * GLOBAL_CAMERA_SHAKE_MULTIPLIER;
        }
        else if (isShaking)
        {
            StopShake();
        }
    }
}