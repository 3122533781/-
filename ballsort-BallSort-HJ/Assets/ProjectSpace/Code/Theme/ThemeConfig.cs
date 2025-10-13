using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace RU
{
    public class ThemeConfig : ScriptableObject
    {
        public int[] face_unlock_levels;
        public int[] bg_unlock_levels;

        //[MenuItem("Mahjong/Russian/CreateThemeConfig")]
        //static void CreateLevelConfigAssetInstance()
        //{
        //    var ConfigAsset = CreateInstance<ThemeConfig>();

        //    AssetDatabase.CreateAsset(ConfigAsset, "Assets/Russian/Resources/Configs/ThemeConfig.asset");
        //    AssetDatabase.Refresh();
        //}
    }
}

