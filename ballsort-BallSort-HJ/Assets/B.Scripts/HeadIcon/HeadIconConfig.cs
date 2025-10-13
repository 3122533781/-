using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class HeadIconConfig : ScriptableSingleton<HeadIconConfig>, Prime31.IObjectInspectable
{
    [SerializeField] public List<HeadIconData> DefaultHeadIcons;
    [SerializeField] public List<HeadIconData> MaleHeads;
    [SerializeField] public List<HeadIconData> FemaleHeads;

    public HeadIconData RandomGet(HeadSex sex = HeadSex.All)
    {
        if (_idToData == null)
        {
            InitDic();
        }
        
        switch (sex)
        {
            case HeadSex.All:
                return _allHeadIcons[Random.Range(0, _allHeadIcons.Count)];
            case HeadSex.Default:
                return DefaultHeadIcons[Random.Range(0, DefaultHeadIcons.Count)];
            case HeadSex.Female:
                return FemaleHeads[Random.Range(0, FemaleHeads.Count)];
            case HeadSex.Male:
                return MaleHeads[Random.Range(0, MaleHeads.Count)];
        }

        throw new Exception("error exception");
    }

    public HeadIconData GetDataByID(int id)
    {
        if (_idToData == null)
        {
            InitDic();
        }

        return _idToData[id];
    }

    private void InitDic()
    {
        _allHeadIcons = new List<HeadIconData>();
        _allHeadIcons.AddRange(DefaultHeadIcons);
        _allHeadIcons.AddRange(MaleHeads);
        _allHeadIcons.AddRange(FemaleHeads);

        _idToData = new Dictionary<int, HeadIconData>();
        for (int i = 0; i < _allHeadIcons.Count; i++)
        {
            _idToData.Add(_allHeadIcons[i].ID, _allHeadIcons[i]);
        }
    }

    protected override void HandleOnEnable()
    {
        if (_idToData == null)
        {
            InitDic();
        }
    }

#if UNITY_EDITOR
    [Prime31.MakeButton]
    public void Update()
    {
        DefaultHeadIcons.Clear();
        MaleHeads.Clear();
        FemaleHeads.Clear();
        int id = 0;
        string[] files1 = Directory.GetFiles("Assets/WordAquarium/Textures/Faces/Default", "*.png");
        foreach (string filePath in files1)
        {
            HeadIconData data = new HeadIconData();
            data.ID = id++;
            Sprite sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(filePath);
            data.Icon = sprite;
            DefaultHeadIcons.Add(data);
        }

        string[] files2 = Directory.GetFiles("Assets/WordAquarium/Textures/Faces/Female", "*.jpg");
        foreach (string filePath in files2)
        {
            HeadIconData data = new HeadIconData();
            data.ID = id++;
            Sprite sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(filePath);
            data.Icon = sprite;
            FemaleHeads.Add(data);
        }

        string[] files3 = Directory.GetFiles("Assets/WordAquarium/Textures/Faces/Male", "*.jpg");
        foreach (string filePath in files3)
        {
            HeadIconData data = new HeadIconData();
            data.ID = id++;
            Sprite sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(filePath);
            data.Icon = sprite;
            MaleHeads.Add(data);
        }

        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

    private Dictionary<int, HeadIconData> _idToData;
    private List<HeadIconData> _allHeadIcons;
}

[System.Serializable]
public class HeadIconData
{
    public int ID;
    public Sprite Icon;
}