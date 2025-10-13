using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonDownEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler,
    IPointerEnterHandler
{
    private bool isDownButton;
    private Tweener tweener;

    [SerializeField] private bool isZoomIn = true;
    [SerializeField] private bool isPlaySound = true;

    [SerializeField] private Vector3 zoomInScale = new Vector3(0.9f, 0.9f, 0.9f);
    [SerializeField] private Vector3 zoomOutScale = new Vector3(1f, 1f, 1f);

    private Vector3 originalScale;
    private Button selfButton;

    private void Awake()
    {
        originalScale = transform.localScale;
        selfButton = transform.GetComponent<Button>();
        if (selfButton != null && isPlaySound)
        {
            selfButton.onClick.AddListener(() =>
            {
                AudioClipHelper.Instance.PlayButtonTap();
                VibrationManager.Instance.SelectedBlockImpact();
            });
        }
    }

    private void OnDestroy()
    {
        tweener.Kill();
        tweener = null;
        if (selfButton != null)
        {
            selfButton.onClick.RemoveAllListeners();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (selfButton != null && selfButton.interactable)
        {
            tweener.Kill();
            tweener = null;

            isDownButton = true;

            var targetScale = isZoomIn
                ? CalculationTool(zoomInScale, originalScale)
                : CalculationTool(zoomOutScale, originalScale);

            tweener = transform.DOScale(targetScale, 0.1f);
            if (isZoomIn)
            {
                transform.SetAsLastSibling();
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (selfButton != null && selfButton.interactable)
        {
            tweener.Kill();
            tweener = null;

            isDownButton = false;
            tweener = transform.DOScale(originalScale, 0.1f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (selfButton != null && selfButton.interactable && isDownButton)
        {
            tweener.Kill();
            tweener = null;

            tweener = transform.DOScale(originalScale, 0.1f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (selfButton != null && selfButton.interactable && isDownButton)
        {
            tweener.Kill();
            tweener = null;

            var targetScale = isZoomIn
                ? CalculationTool(zoomInScale, originalScale)
                : CalculationTool(zoomOutScale, originalScale);

            tweener = transform.DOScale(targetScale, 0.1f);
            if (isZoomIn)
            {
                transform.SetAsLastSibling();
            }
        }
    }

    private Vector3 CalculationTool(Vector3 zoomScale, Vector3 origScale)
    {
        return new Vector3(zoomScale.x * origScale.x, zoomScale.y * origScale.y, zoomScale.z * origScale.z);
    }
}