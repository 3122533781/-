using UnityEngine;

public class GameHeartBeat : MonoSingleton<GameHeartBeat>
{
    private void Update()
    {
        _timer += Time.deltaTime;
        Game.Instance.GetSystem<ADStrategySystem>().PluseIntersistalTime(Time.deltaTime);
        if (Game.Instance.GetSystem<ADStrategySystem>().NeedShowSubLevelIntersistal() && Game.Instance.LevelModel.MaxUnlockLevel.Value > 2)
        {
            //ADMudule.ShowInterstitialAds("NoADShowAD", null);
            //Game.Instance.GetSystem<ADStrategySystem>().ResetIntersistalTime();
        }
        if (_timer >= _frequency)
        {
            _timer -= _frequency;
            StaticModule.GameHeartBeat();
        }
    }

    /// <summary>
    /// 单位分钟
    /// </summary>
    [SerializeField] private int _frequency = 60;

    private float _timer;
}