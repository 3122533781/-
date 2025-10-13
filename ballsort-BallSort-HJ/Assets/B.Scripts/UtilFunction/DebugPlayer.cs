using _02.Scripts.Config;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.Framework.ElementKit;
using ProjectSpace.Lei31Utils.Scripts.IAPModule;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using UnityEngine;

public class DebugPlayer : IDebugPage
{
    public string Title
    {
        get { return "Player"; }
    }

    public void Draw()
    {
        GUILayout.Label($"language:{LocalizationManager.Instance.Model.CurrentLanguage.LanguageName}");

        if (GUILayout.Button("Coin * 1000"))
        {
            Game.Instance.CurrencyModel.AddGoodCount(GoodType.Coin, 0, 1000);
        }

        if (GUILayout.Button("RevocationTool * 10"))
        {
            var reward = new RewardData(GoodType.Tool, 10, (int)GoodSubType.RevocationTool);
            RewardClaimHandle.ClaimReward(reward, "ad", IapStatus.Free);
        }

        if (GUILayout.Button("AddPipe * 10"))
        {
            var reward = new RewardData(GoodType.Tool, 10, (int)GoodSubType.AddPipe);
            RewardClaimHandle.ClaimReward(reward, "ad", IapStatus.Free);
        }
        if (GUILayout.Button("球排序胜利"))
        {
            if (inGame2 == null)
            {
                inGame2 = SceneElementManager.Instance.Resolve<InGame>();
            }
            inGame2.Win();
        }
        if (GUILayout.Button("打开礼物弹窗"))
        {
            Game.Instance.LevelModel.RestartStoreGold();

            var randomReward = InGameRewardConfig.Instance.GetRandomRewardData();
            RewardClaimHandle.ClaimReward(randomReward, "InGameBox", IapStatus.Free);
        }

        if (GUILayout.Button("停止BGM"))
        {
            AudioManager.Instance.StopBGM();
        }

        if (GUILayout.Button("缓存清除"))
        {
            SoyProfile.DelaySet(SoyProfileConst.NormalLevel, SoyProfileConst.NormalLevel_Default, 1);
            SoyProfile.DelaySet(SoyProfileConst.SpecialLevel, SoyProfileConst.NormalLevel_Default, 1);
            SoyProfile.DelaySet(SoyProfileConst.HaveCoin, SoyProfileConst.HaveCoinDefault, 1);
            SoyProfile.DelaySet(SoyProfileConst.ADRemoved, SoyProfileConst.ADRemoved_Default, 1);
            SoyProfile.DelaySet(SoyProfileConst.CoinShopProgress, SoyProfileConst.CoinShopProgressDefault, 1);
            SoyProfile.DelaySet(SoyProfileConst.PurchaseTime, SoyProfileConst.PurchaseTimeDefault, 1);
            SoyProfile.DelaySet(SoyProfileConst.GameToolProgress, SoyProfileConst.GameToolProgressDefault, 1);

            PlayerPrefs.DeleteAll();
            GameDataManager.Instance.ClearAllData();

        }

        if (GUILayout.Button("Open Redeem"))
        {
            Game.Instance.Model.CanRedeem.Value = true;
            Game.Instance.isSDKInitCompleted = true;
        }
      


    }

    private InGame inGame2;
}