using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog
{
    public class DialogManager : MonoSingleton<DialogManager>
    {
        public static List<Dialog> ListActiveDialog;
        private Dictionary<string, Dialog> _pool;
        public const string DialogResourcesPath = "Dialogs";

        private void Awake()
        {
            _pool = new Dictionary<string, Dialog>();
            ListActiveDialog = new List<Dialog>();
        }

        public void ClearAllDialog()
        {
            foreach (KeyValuePair<string, Dialog> pair in _pool)
            {
                Destroy(pair.Value.gameObject);
            }

            _pool.Clear();
        }

        public bool GetIsActiveDialog()
        {
            foreach (var pair in _pool)
            {
                if (pair.Value.gameObject.activeSelf)
                {
                    return true;
                }
            }

            return false;
        }

        public T GetDialog<T>() where T : Dialog
        {
            string dialogID = typeof(T).ToString();
            if (!ContainDialog(dialogID))
            {
                Dialog dialog = LoadDialog<T>();
                _pool.Add(dialogID, dialog);
            }

            _pool[dialogID].transform.SetAsLastSibling();
            return _pool[dialogID] as T;
        }

        public Dialog GetDialog(DialogName dialogName)
        {
            string dialogID = dialogName.ToString();
            if (!ContainDialog(dialogID))
            {
                Dialog dialog = LoadDialog(dialogName);
                _pool.Add(dialogID, dialog);
            }

            _pool[dialogID].transform.SetAsLastSibling();
            return _pool[dialogID] as Dialog;
        }


        private Dialog LoadDialog(DialogName dialogName)
        {
            string dialogPath = $"{dialogName}";//{DialogResourcesPath}/
            Dialog prefab = Resources.Load<Dialog>(dialogPath);
            Dialog result = Instantiate(prefab);
            result.transform.SetParent(transform);
            result.transform.ResetLocal();
            result.gameObject.SetActive(true);
            return result;
        }

        private Dialog LoadDialog<T>() where T : Dialog
        {
            string dialogPath = $"{typeof(T).Name}";//{DialogResourcesPath}/
            Dialog prefab = Resources.Load<Dialog>(dialogPath);
            LDebug.Log($"Load {typeof(T)} from {dialogPath}");
            Dialog result = Instantiate(prefab);
            result.transform.SetParent(transform);
            result.transform.ResetLocal();
            result.gameObject.SetActive(true);
            return result;
        }

        private bool ContainDialog(string id)
        {
            return _pool.ContainsKey(id);
        }

        #region 异步加载

        /// <summary>
        /// info:弹窗基础显示方法，不用传入内容用这
        /// </summary>
        /// <param name="dialogName"></param>
        public void ShowDialog(DialogName dialogName)
        {
            var res = GetDialogByName<Dialog>(dialogName.ToString());
            res.ShowDialog();
        }

        public void ShowDialogWithContext<T>(DialogName dialogName, T context) where T : DialogContent
        {
            var res = GetDialogByName<Dialog<T>>(dialogName.ToString());
            res.ShowDialogWithContext(context);
        }


        /// <summary>
        /// info:弹窗基础显示方法，不用传入内容用这
        /// </summary>
        /// <param name="dialogName"></param>
        public void CloseDialog(DialogName dialogName)
        {
            var res = GetDialogByName<Dialog>(dialogName.ToString());
            res.CloseDialog();
        }


        /// <summary>
        /// 异步加载弹窗物体
        /// </summary>
        /// <param name="dialogName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T GetDialogByName<T>(string dialogName) where T : Dialog
        {
            string dialogID = dialogName;
            if (!ContainDialog(dialogID))
            {
                Dialog dialog = LoadDialog<T>(dialogName);
                if (dialog)
                {
                    _pool.Add($"{dialogName}", dialog);
                }
            }

            var res = _pool[dialogID] as T;
            return res;
        }

        /// <summary>
        /// info:通用加在资源
        /// rule:只能加在文件类型，gameObj && Texture不能作用于脚本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private Dialog LoadDialog<T>(string dialogName) where T : Dialog
        {
            string dialogPath = $"{dialogName}";//{DialogResourcesPath}/
            Dialog prefab = Resources.Load<Dialog>(dialogPath);
            LDebug.Log($"Load {typeof(T)} from {dialogPath}");
            Dialog result = Instantiate(prefab, transform, true);
            result.transform.ResetLocal();
            return result;
        }

        #endregion



        #region 回调加载弹窗方法

        // public T GetDialogByName<T>() where T : Dialog
        // {
        //     string dialogID = typeof(T).Name;
        //     if (!ContainDialog(dialogID))
        //     {
        //         Dialog dialog = LoadDialog<T>();
        //         if (dialog)
        //         {
        //             _pool.Add(dialogID, dialog);
        //         }
        //     }
        //
        //     var res = _pool[dialogID] as T;
        //     return res;
        // }
        //
        // private T LoadDialog<T>()
        // {
        //     GameObject spawnObj = null;
        //     Addressables.InstantiateAsync($"{typeof(T).Name}").Completed += (handle) =>
        //     {
        //         if (handle.Status == AsyncOperationStatus.Succeeded)
        //         {
        //             var obj = handle.Result;
        //             spawnObj = Instantiate(obj, transform, true);
        //             spawnObj.transform.ResetLocal();
        //             spawnObj.gameObject.SetActive(true);
        //         }
        //     };
        //     return spawnObj.GetComponent<T>();
        // }

        #endregion
    }
}