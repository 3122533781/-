using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
	public class GiftReward
	{
		public int[] daily_bonus;
		public int bind_facebook;
		public int watch_video;
	}
    [Serializable]
    public class Cost
    {
        public int reward0;
        public int reward1;
        public int reward2;
        public int lotto;
    }
	
    [Serializable]
    public class Daily
    {
        public int scratch;
        public int lotto;
        public int reward;
        public int video;
        public int share;
    }
    [Serializable]
    public class Global
    {
        public bool canRedeem;
        public int minRedeemAmount;
        public int[] daily_ranking_reward;
    }


    [Serializable]
	public class GameConfigs
	{
		public int[] lottoReward;
        public string[] lottoRewardType;
        public GiftReward giftReward;
		public bool canRedeem;
        public Cost cost;
        public Daily daily;
        public Global global;
		public GameConfigs()
		{
		}
	}
}

