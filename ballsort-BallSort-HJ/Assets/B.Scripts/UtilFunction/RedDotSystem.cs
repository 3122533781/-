using System;
using System.Collections.Generic;
using Fangtang.Utils;
using UnityEngine;

namespace ProjectSpace.Lei31Utils.Scripts.Common
{
    #region 红点基础信息

    public static class RedPointConst
    {
        private const string None = "None";
        public const string Home = "Home";


        public static string TypeToString(AppriseType type)
        {
            switch (type)
            {
                case AppriseType.None:
                    return None;
                case AppriseType.Home:
                    return Home;
            }

            return "";
        }
    }

    public enum AppriseType
    {
        None = -1,
        Home = 0,
    }


    public class RedPointNode
    {
        public string NodeName;
        public int PointNum;
        public RedPointNode Parent;
        public readonly Dictionary<string, RedPointNode> ChildDictionary = new Dictionary<string, RedPointNode>();
        public RedDotSystem.OnPointNumChange NumChangeFunc;


        public void SetRedPointNum(int rpNum)
        {
            if (ChildDictionary.Count > 0)
            {
                Debug.LogError("only can set leaf node");
                return;
            }

            // if (useAdd)
            // {
            PointNum += rpNum;
            // }
            // else
            // {
            //     PointNum = rpNum;
            // }


            NotifyPointNumChange();

            if (Parent != null)
            {
                Parent.ChangeRedPointNum();
            }
        }

        private void NotifyPointNumChange()
        {
            if (NumChangeFunc != null)
            {
                NumChangeFunc.Invoke(this);
            }
        }

        private void ChangeRedPointNum()
        {
            int num = 0;
            foreach (var node in ChildDictionary.Values)
            {
                num += node.PointNum;
            }

            if (num != PointNum)
            {
                PointNum = num;
                NotifyPointNumChange();
            }

            //--
            if (Parent != null)
            {
                Parent.ChangeRedPointNum();
            }
        }
    }

    #endregion

    public class RedDotSystem : Singleton<RedDotSystem>
    {
        public delegate void OnPointNumChange(RedPointNode node);

        private bool _isInitData;

        private RedPointNode MRootNode { get; set; }

        static readonly List<string> LstRedPointTreeList = new List<string>();


        private RedDotSystem()
        {
        }

        internal static RedDotSystem CreateInstance()
        {
            return new RedDotSystem();
        }

        public void InitRedPointTreeNode()
        {
            if (_isInitData)
            {
                return;
            }

            _isInitData = true;
            foreach (var id in Enum.GetValues(typeof(AppriseType)))
            {
                var type = (AppriseType) id;
                if (type == AppriseType.None)
                {
                    continue;
                }

                LstRedPointTreeList.Add(RedPointConst.TypeToString(type));
            }


            MRootNode = new RedPointNode
            {
                NodeName = RedPointConst.Home,
                NumChangeFunc = (node) => { Debug.Log(node.NodeName + " rpNum change num = :" + node.PointNum); }
            };
            //--

            foreach (var s in LstRedPointTreeList)
            {
                var node = MRootNode;
                var treeNodeAy = s.Split('.');
                if (treeNodeAy[0] != MRootNode.NodeName)
                {
                    Debug.LogError("repointed root node error :" + treeNodeAy[0]);
                    continue;
                }

                if (treeNodeAy.Length > 1)
                {
                    for (int i = 1; i < treeNodeAy.Length; i++)
                    {
                        if (!node.ChildDictionary.ContainsKey(treeNodeAy[i]))
                        {
                            node.ChildDictionary.Add(treeNodeAy[i], new RedPointNode());
                        }

                        //--
                        node.ChildDictionary[treeNodeAy[i]].NodeName = node.NodeName + "." + treeNodeAy[i];
                        node.ChildDictionary[treeNodeAy[i]].Parent = node;

                        node = node.ChildDictionary[treeNodeAy[i]];
                    }
                }
            }
        }

        public RedPointNode GetRedPoint(AppriseType appriseType)
        {
            var strNode = RedPointConst.TypeToString(appriseType);
            var nodeList = strNode.Split('.');

            if (appriseType == AppriseType.None)
            {
                return null;
            }


            var node = MRootNode;
            for (int i = 0; i < nodeList.Length; i++)
            {
                if (nodeList[i] == RedPointConst.Home)
                {
                    continue;
                }

                node = node.ChildDictionary[nodeList[i]];
                if (i == nodeList.Length - 1)
                {
                    return node;
                }
            }

            return null;
        }


        public void SetRedPointNodeCallback(AppriseType appriseType, OnPointNumChange callBack)
        {
            if (appriseType == AppriseType.None)
            {
                return;
            }

            if (MRootNode == null)
            {
                InitRedPointTreeNode();
            }


            var strNode = RedPointConst.TypeToString(appriseType);
            var nodeList = strNode.Split('.');
            if (nodeList.Length == 1)
            {
                if (nodeList[0] != RedPointConst.Home)
                {
                    return;
                }
            }

            var node = MRootNode;
            for (int i = 1; i < nodeList.Length; i++)
            {
                if (!node.ChildDictionary.ContainsKey(nodeList[i]))
                {
                    Debug.LogError("does not contains child node :" + nodeList[i]);
                    return;
                }

                node = node.ChildDictionary[nodeList[i]];
                if (i == nodeList.Length - 1)
                {
                    node.NumChangeFunc = callBack;
                    return;
                }
            }
        }

        private void SetInvoke(AppriseType appriseType, int rpNum)
        {
            if (appriseType == AppriseType.None)
            {
                return;
            }

            if (MRootNode == null)
            {
                InitRedPointTreeNode();
            }

            var strNode = RedPointConst.TypeToString(appriseType);
            var nodeList = strNode.Split('.');
            if (nodeList.Length == 1)
            {
                if (nodeList[0] != RedPointConst.Home)
                {
                    Debug.LogError("get wrong root node , current is :" + nodeList[0]);
                    return;
                }
            }

            var node = MRootNode;
            for (int i = 1; i < nodeList.Length; i++)
            {
                if (!node.ChildDictionary.ContainsKey(nodeList[i]))
                {
                    Debug.LogError("does not contains child node :" + nodeList[i]);
                    return;
                }

                node = node.ChildDictionary[nodeList[i]];

                if (i == nodeList.Length - 1)
                {
                    node.SetRedPointNum(rpNum);
                }
            }
        }
    }
}