
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

public class CreateAtlas
{
    /// <summary>
    /// 图片根目录 -- 需要打包图集的文件夹父级
    /// 适用目录结构：根部文件夹
    ///                -> 图片文件夹1
    ///                -> 图片文件夹2
    ///                ... 
    /// </summary>
    private static string pathRoot = "Assets/C.Sprites/";

    /// <summary>
    /// 图集存储路径
    /// </summary>
    private static string atlasStoragePath = "Assets/I.Atlas";

    
    [MenuItem("工具/DeleteAllPrefsData")]
    public static void DeleteAllPrefsData()
    {
        PlayerPrefs.DeleteAll();
    }
    /// <summary>
    /// 每个需要打图集的文件夹名 -- 即图集名
    /// </summary>
    private static string spritefilePathName;

    [MenuItem("工具/打包图集")]
    public static void CreateAllSpriteAtlas()
    {
        Debug.Log("打包图集开始执行");

        DirectoryInfo info = new DirectoryInfo(pathRoot);
        int index = 0;
        // 遍历根目录
        foreach (DirectoryInfo item in info.GetDirectories())
        {
            spritefilePathName = item.Name;

            SpriteAtlas spriteAtlas = AssetDatabase.LoadAssetAtPath(atlasStoragePath + "/" + spritefilePathName + ".spriteatlas", typeof(Object)) as SpriteAtlas;

            // 不存在则创建后更新图集
            if (spriteAtlas == null)
            {
                spriteAtlas = CreateSpriteAtlas(spritefilePathName);
            }

            string spriteFilePath = pathRoot + "/" + spritefilePathName;
            UpdateAtlas(spriteAtlas, spriteFilePath);

            // 打包进度
            EditorUtility.DisplayProgressBar("打包图集中...", "正在处理:" + item, index / info.GetDirectories().Length);
            index++;
        }

        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();

        Debug.Log("打包图集执行结束");
    }

    /// <summary>
    /// 创建图集
    /// </summary>
    /// <param name="atlasName">图集名字</param>
    private static SpriteAtlas CreateSpriteAtlas(string atlasName)
    {
        SpriteAtlas atlas = new SpriteAtlas();

        #region 图集基础设置

        SpriteAtlasPackingSettings packSetting = new SpriteAtlasPackingSettings()
        {
            blockOffset = 1,
            enableRotation = false,
            enableTightPacking = false,
            padding = 8,
        };
        atlas.SetPackingSettings(packSetting);

        #endregion

        #region 图集纹理设置

        SpriteAtlasTextureSettings textureSettings = new SpriteAtlasTextureSettings()
        {
            readable = false,
            generateMipMaps = false,
            sRGB = true,
            filterMode = FilterMode.Bilinear,
        };
        atlas.SetTextureSettings(textureSettings);

        #endregion

        #region 分平台设置图集格式

        TextureImporterPlatformSettings platformSetting = atlas.GetPlatformSettings(GetPlatformName(BuildTarget.iOS));
        platformSetting.overridden = true;
        platformSetting.maxTextureSize = 2048;
        platformSetting.textureCompression = TextureImporterCompression.Compressed;
        platformSetting.format = TextureImporterFormat.PVRTC_RGBA4;
        atlas.SetPlatformSettings(platformSetting);

        // 需要多端同步，就在写一份
        platformSetting = atlas.GetPlatformSettings(GetPlatformName(BuildTarget.Android));
        platformSetting.overridden = true;
        platformSetting.maxTextureSize = 2048;
        platformSetting.textureCompression = TextureImporterCompression.Compressed;
        platformSetting.format = TextureImporterFormat.ASTC_4x4;
        atlas.SetPlatformSettings(platformSetting);

        #endregion

        string atlasPath = atlasStoragePath + "/" + atlasName + ".spriteatlas";
        AssetDatabase.CreateAsset(atlas, atlasPath);
        AssetDatabase.SaveAssets();

        return atlas;
    }

    /// <summary>
    /// 每个图集的所有图片路径  --  记得用之前清空
    /// </summary>
    private static List<string> textureFullName = new List<string>();

    /// <summary>
    /// 更新图集内容
    /// </summary>
    /// <param name="atlas">图集</param>
    static void UpdateAtlas(SpriteAtlas atlas, string spriteFilePath)
    {
        textureFullName.Clear();
        FileName(spriteFilePath);

        // 获取图集下图片清除
        var pack = atlas.GetPackables();
        atlas.Remove(pack);

        foreach (string item in textureFullName)
        {
            // 加载指定目录
            Object spriteObj = AssetDatabase.LoadAssetAtPath(item, typeof(Object));
            Debug.Log("存png和jpg后缀的图片: " + item);
            atlas.Add(new Object[] { spriteObj });
        }
    }

    /// <summary>
    /// 递归文件夹下的图
    /// </summary>
    /// <param name="folderPath"></param>
    static void FileName(string folderPath)
    {
        DirectoryInfo info = new DirectoryInfo(folderPath);
        // foreach (DirectoryInfo item in info.GetDirectories())
        // {
        //     FileName(item.FullName);
        // }
        foreach (FileInfo item in info.GetFiles())
        {
            var dir = item.FullName.Replace('\\', '/');
            // 存png和jpg后缀的图片
            if (dir.EndsWith(".png", StringComparison.Ordinal)
                || dir.EndsWith(".jpg", StringComparison.Ordinal))
            {
                textureFullName.Add("Assets" + dir.Replace(Application.dataPath, ""));
            }
        }
    }

    /// <summary>
    /// 不同平台枚举对应的值
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    static string GetPlatformName(BuildTarget target)
    {
        string platformName = "";
        switch (target)
        {
            case BuildTarget.Android:
                platformName = "Android";
                break;
            case BuildTarget.iOS:
                platformName = "iPhone";
                break;
            case BuildTarget.PS4:
                platformName = "PS4";
                break;
            case BuildTarget.XboxOne:
                platformName = "XboxOne";
                break;
            case BuildTarget.NoTarget:
                platformName = "DefaultTexturePlatform";
                break;
            default:
                platformName = "Standalone";
                break;
        }
        return platformName;
    }    
}
