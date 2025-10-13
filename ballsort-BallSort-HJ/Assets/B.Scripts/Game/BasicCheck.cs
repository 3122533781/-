using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using UnityEngine;

public class BasicCheck : MonoBehaviour
{
    public const string BasicComponentPath = "BasicComponent";

    [SerializeField] private Transform _coinTrans;
    [SerializeField] private Transform _magicTrans;
    [SerializeField] private Transform _moneyTrans;
    [SerializeField] private Transform _starTrans;
    [SerializeField] private Transform _bigTurnTrans;
    [SerializeField] private Transform _diamondTrans;

    private void Awake()
    {
        if (!Game.Instance.IsBasicComponentLoaded)
        {
            GameObject prefab = Resources.Load(BasicComponentPath) as GameObject;
           GameObject target = Instantiate(prefab, null);
            target.transform.position = Vector3.zero;
        }

        Game.Instance.CoinTrans = _coinTrans;
        Game.Instance.MagicTrans = _magicTrans;
        Game.Instance.MoneyTrans = _moneyTrans;
        Game.Instance.StarTrans = _starTrans;
        Game.Instance.BigTurnTrans = _bigTurnTrans;
        Game.Instance.DiamondTrans = _diamondTrans;
    }
}