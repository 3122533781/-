using Fangtang;
using ProjectSpace.BubbleMatch.Scripts.Util;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using Sirenix.OdinInspector;
using UnityEngine;

public class InGameMenu : GenericSceneElement<InGameMenu, InLineBeeGameState>, Prime31.IObjectInspectable
{
    [SerializeField] public Camera GameCamera = null;

    //public InGameLevelModel LevelModel { get; private set; }


    protected override void OnInit(object data)
    {




    }

    private void Start()
    {
      
        SpriteManager.Instance.InitSkin();
    }

    public void GameWin()
    {
    
    }

    public void GameFailed()
    {
    
    }
}
public enum InLineBeeGameState
{
    Null,
    Standby,
    Playing,
    Win,
    Failed,
}

