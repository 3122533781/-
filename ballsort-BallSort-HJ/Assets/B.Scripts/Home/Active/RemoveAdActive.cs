using ProjectSpace.Lei31Utils.Scripts.Framework.App;

namespace _02.Scripts.Home.Active
{
    public class RemoveAdActive : DialogButtonItem
    {
        protected override void CheckIsCanShow(out bool isCanShow)
        {
            isCanShow = !Game.Instance.Model.AdRemovedIsBuy();
        }

        protected override void BindShowAction()
        {
            base.BindShowAction();
            Game.Instance.Model.RegisterAd(RefreshGameObj);
        }

        protected override void RemoveBindAction()
        {
            base.RemoveBindAction();
            Game.Instance.Model.UnRegisterAd(RefreshGameObj);
        }

        private void RefreshGameObj(bool arg1, bool arg2)
        {
            CheckIsCanShow(out var canShow);
            gameObject.SetActive(canShow);
        }

        protected override void RefreshItem()
        {
            if (Game.Instance.GameStatic.OpenGameTime.Value >= 3 && Game.Instance.LevelModel.MaxUnlockLevel.Value > 1)
            {
                ShowDialog();
            }
        }
    }
}