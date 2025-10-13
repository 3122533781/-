using System.Collections;
using System.Collections.Generic;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using UnityEngine;
using UnityEngine.UI;

public class RatingClickableStar : MonoBehaviour
{
    public void ShowBlink()
    {
        SetAnimator(true);
    }

    private void Awake()
    {
        _starButton = GetComponent<Button>();
    }
    private void OnEnable()
    {
        Game.Instance.GetSystem<RatingSystem>().Model.OnRatingStarChanged += OnRatingStarChanged;
        _starButton.onClick.AddListener(OnStarClick);
        InitUI();
    }

    private void OnDisable()
    {
        _starButton.onClick.RemoveListener(OnStarClick);
        Game.Instance.GetSystem<RatingSystem>().Model.OnRatingStarChanged -= OnRatingStarChanged;
    }

    private void InitUI()
    {
        SetStar(false);
        SetAnimator(false);
    }

    private void OnRatingStarChanged(int star)
    {
        SetStar(_starIndex <= star);
        SetAnimator(false);
    }

    private void SetStar(bool on)
    {
        _starOnImg.gameObject.SetActive(on);
        _starOffImg.gameObject.SetActive(!on);
    }

    private void SetAnimator(bool enable)
    {
        if(_reminderAnimator != null && _reminderAnimator.gameObject.activeSelf)
            _reminderAnimator.SetBool("IsBlink", enable);
    }

    private void OnStarClick()
    {
        Game.Instance.GetSystem<RatingSystem>().Model.CurrentRatingStar = _starIndex;
    }

    [SerializeField] private Image _starOnImg = null;
    [SerializeField] private Image _starOffImg = null;
    [SerializeField] private int _starIndex = 0;
    [SerializeField] private Animator _reminderAnimator = null;

    private Button _starButton = null;
}
