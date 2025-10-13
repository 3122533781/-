using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class User
    {
        public bool IsFetchFormService;
        public UserType Type;

        public int user_id;
        public string username;
        public string avatar_url;
        public int balance;
        public int balance_token;
        public int balance_energy;
        public string device_id;
        public string facebook_id;
        public string gp_id;
        public string city;
        public string country;
        public int gender;
        public string email;
        public DateTime last_login;
        public string token;
        public string refer_code;
        public int refer_user;
        public string profile;

        // 邀请
        public bool isInvited;
        public string InvitedCode;

        // token排名
        public int ranking_tokens;
        public int ranking;

        public bool can_redeem;
        public long server_time;

        public User()
        {
            IsFetchFormService = false;
            Type = PlayerDataStorage.GetUserType();
        }

        /// <summary>
        /// 默认的用户名是包含Guest的
        /// </summary>
        /// <returns></returns>
        public bool NeedChangeName()
        {
            return string.IsNullOrEmpty(username) ||
                username.Contains("Guest");
        }

        public bool IsOwnHead()
        {
            return !string.IsNullOrEmpty(avatar_url);
        }

        public int GetHeadID()
        {
            if (int.TryParse(avatar_url, out int headID))
            {
                return headID;
            }
            else
            {
                return 1;
            }
        }

        public Sprite GetHead()
        {
            //PlayerHeadData headData = PlayerHeadConfig.Instance.GetConfigByID(GetHeadID());
            //return headData != null ? headData.HeadImg : null;
            return null;
        }

        public string GetUserName()
        {
            //if (string.IsNullOrEmpty(username))
            //{
            //    return LocalizationManager.Instance.GetTextByTag(Lei31.Localizetion.LocalizationConst.YOU);
            //}
            //else
            {
                return username;
            }
        }

        public DateTime GetLoginTime()
        {
            return DataFormater.GetDateTime(server_time);
        }

        public bool HasLoginTime()
        {
            return server_time > 0;
        }

        public SoyProfileUnit GetServerProfile()
        {
            return !string.IsNullOrEmpty(profile) ? new SoyProfileUnit(profile) : null;
        }

        public bool HasBindFacebook()
        {
            return !string.IsNullOrEmpty(facebook_id);
        }

        ///// <summary>
        ///// 通过profile 获取玩家存储的战斗力
        ///// 需要注意profile只在登录时候刷新，如果是获取当前玩家的实时数据，
        ///// 需要通过PlayerDataManager.Instance.GetFightScore()
        ///// </summary>
        ///// <returns></returns>
        //public int GetProfileFightScore()
        //{
        //    SoyProfileUnit soyProfile = GetServerProfile();
        //    string key = PlayerDataManager.Instance.GetKeyPlayerData(PlayerDataType.FightScore);
        //    return soyProfile == null ? 0 : SoyProfile.Get<int>(soyProfile, key, 0);
        //}

        //public int GetProfilePlayerLevel()
        //{
        //    SoyProfileUnit soyProfile = GetServerProfile();
        //    string key = PlayerDataManager.Instance.GetKeyPlayerData(PlayerDataType.PlayerLevel);
        //    return soyProfile == null ? 1 : SoyProfile.Get<int>(soyProfile, key, 1);
        //}

        //public List<Equip> GetProfileLoadedEquips()
        //{
        //    SoyProfileUnit soyProfile = GetServerProfile();
        //    string key = PlayerDataManager.Instance.GetKeyPlayerData(PlayerDataType.EquipLoaded);
        //    return soyProfile == null ? null : PlayerDataManager.Instance.GetLoadedEquips(SoyProfile.Get<string>(soyProfile, key, ""));
        //}
    }
}