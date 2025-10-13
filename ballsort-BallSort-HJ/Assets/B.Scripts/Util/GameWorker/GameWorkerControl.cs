using System.Collections.Generic;
using SoyBean;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Soybean.GameWorker
{
    public class GameWorkerControl
    {
        public void Init(GameWorker context)
        {
            _context = context;
            _uiResult = new List<RaycastResult>();
            _context.Model.InitStart();
        }

        public void Update()
        {
            if (Time.frameCount % 120 == 0)
            {
                CheckCurrentUnit();
            }
        }

        private void CheckCurrentUnit()
        {
            for (int i = 0; i < _context.Model.ActiveAttachedUnits.Count; i++)
            {
                GameWorkerUnit attach = _context.Model.ActiveAttachedUnits[i];
                if (attach.State != UnitState.Active)
                    continue;

                if (TryGetButton(attach.ButtonPath, out Button findBtn01))
                {
                    JobUtils.Delay(0.3f, () =>
                    {
                        findBtn01.onClick.Invoke();
                        _context.Model.PassUnit(attach);
                    });
                    return;
                }
            }

            if (_context.Model.ActiveNessUnit.State == UnitState.Active &&
                TryGetButton(_context.Model.ActiveNessUnit.ButtonPath, out Button findBtn))
            {
                JobUtils.Delay(0.3f, () =>
                {
                    findBtn.onClick.Invoke();
                    _context.Model.PassUnit(_context.Model.ActiveNessUnit);
                });
            }
        }

        //如果按钮处于可以点击的状态，则可以返回
        private bool TryGetButton(string path, out Button btn)
        {
            GameObject target = GameObject.Find(path);

            if (target == null)
            {
                btn = null;
                return false;
            }

            Button targetBtn = target.GetComponent<Button>();

            if (target.activeSelf && targetBtn.interactable && IsTopUI(targetBtn))
            {
                btn = targetBtn;
                return true;
            }

            btn = null;
            return false;
        }

        private bool IsTopUI(Button btn)
        {
            PointerEventData data = new PointerEventData(EventSystem.current);
            data.position = UICamera.Instance.Camera.WorldToScreenPoint(btn.transform.position);
            EventSystem.current.RaycastAll(data, _uiResult);
            if (_uiResult.Count > 0)
            {
                GameObject topCastObject = _uiResult[0].gameObject;
                if (btn.transform.IsChildRecursionCheck(topCastObject))
                {
                    return true;
                }
            }

            return false;
        }

        private GameWorker _context;
        private List<RaycastResult> _uiResult;
    }
}