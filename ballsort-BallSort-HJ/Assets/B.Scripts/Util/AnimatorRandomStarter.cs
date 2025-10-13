using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorRandomStarter : MonoBehaviour
{
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return Yielders.Get(Random.Range(_randomRange.x, _randomRange.y));
        _animator.enabled = true;
    }

    [SerializeField] private Vector2 _randomRange = new Vector2(0, 1f);

    private Animator _animator;
}