using System;
using System.Collections.Generic;
using System.Linq;
using _02.Scripts.Config;
using _02.Scripts.LevelEdit;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.Util
{
    public static class UtilClass
    {
        public static List<T> Clone<T>(List<T> list) where T : new()
        {
            var str = JsonConvert.SerializeObject(list);
            return JsonConvert.DeserializeObject<List<T>>(str);
        }

        public static T Clone<T>(T data) where T : new()
        {
            var str = JsonConvert.SerializeObject(data);
            return JsonConvert.DeserializeObject<T>(str);
        }

        public static List<Vector3> GetCubicBezierPoint(Vector3 startP, Vector3 endP, float fHeight, float fAngle = 90,
            bool isTop = true, int num = 10)
        {
            float x = Mathf.Min(startP.x, endP.x) + Mathf.Abs(startP.x - endP.x) / 2f;
            float y = Mathf.Min(startP.y, endP.y) + Mathf.Abs(startP.y - endP.y) / 2f;
            float z = startP.z;
            Vector3 midP = new Vector3(x, y, z);
            Vector3 p2, p3;
            LineFromMidpoint(fAngle, midP, fHeight, out p2, out p3);
            List<Vector3> arrRecPos = new List<Vector3>();
            arrRecPos.Add(startP);
            arrRecPos.Add(isTop ? p2 : p3);
            arrRecPos.Add(endP);

            List<Vector3> point = new List<Vector3>();
            for (int i = 0; i <= num; i++)
            {
                var t = i * 1.0f / num;
                Vector3 find = QuardaticOverPointBezier(t, arrRecPos[0], arrRecPos[1], arrRecPos[2]);
                point.Add(find);
            }

            return point;
        }

        public static void LineFromMidpoint(float A, Vector3 p1, float dist, out Vector3 p2, out Vector3 p3)
        {
            float startX = p1.x - dist * (float) Math.Cos(-A * Math.PI / 180);
            float endX = p1.x + dist * (float) Math.Cos(-A * Math.PI / 180);

            float startY = p1.y - dist * (float) Math.Sin(-A * Math.PI / 180);
            float endY = p1.y + dist * (float) Math.Sin(-A * Math.PI / 180);

            p2 = new Vector3(startX, startY);
            p3 = new Vector3(endX, endY);
        }

        public static Vector3 QuardaticOverPointBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            Vector3 a = p0;
            Vector3 b = p1;
            Vector3 c = p2;
            b = p1 - Mathf.Sqrt((p0 - p1).magnitude * (p2 - p1).magnitude) *
                ((p0 - p1) / (p0 - p1).magnitude + (p2 - p1) / (p2 - p1).magnitude) / 2;
            Vector3 aa = a + (b - a) * t;
            Vector3 bb = b + (c - b) * t;
            return aa + (bb - aa) * t;
        }

        public static void DoNumber(Text numberText, float end, float duration, Ease easeType = Ease.Linear)
        {
            float start = 0;
            float.TryParse(numberText.text, out start);
            DoNumber(numberText, start, end, duration, easeType);
        }

        public static void DoNumber(Text numberText, float start, float end, float duration,
            Ease easeType = Ease.Linear)
        {
            DOTween.To(
                    () => start, //起始值
                    x => { numberText.text = Mathf.Floor(x).ToString(); }, //变化值
                    end, //终点值
                    duration)
                .SetEase(easeType)
                .SetUpdate(true)
                .onComplete = () => { numberText.text = end.ToString(); };
        }

        public static void DoNumber(Text numberText, float start, float end, float duration, string conString,
            Ease easeType = Ease.Linear)
        {
            DOTween.To(
                    () => start, //起始值
                    x => { numberText.text = $"{Mathf.Floor(x)}{conString}"; }, //变化值
                    end, //终点值
                    duration)
                .SetEase(easeType)
                .SetUpdate(true)
                .onComplete = () => { numberText.text = $"{end}{conString}"; };
        }

        public static void ShakeScccreen(float delay, int num)
        {
            Camera.main.DOShakePosition(delay, new Vector3(0.3f, 0.3f), (int) (num * delay));
        }

        public static void ShakeRotationTransform(Transform tran, float delay, int num)
        {
            tran.DOPunchRotation(new Vector3(0, 0, -10), delay, (int) (num * delay));
        }

        public static void SetPersistenceReceived(PersistenceData<string> data, DateTime date, int received)
        {
            int y = date.Year, m = date.Month, d = date.Day;
            data.Value = $"{y}-{m}-{d}|{received}";
        }

        public static void AddPersistenceReceived(PersistenceData<string> data, DateTime date, int add = 1)
        {
            var num = GetPersistenceReceived(data, date);
            int y = date.Year, m = date.Month, d = date.Day;
            data.Value = $"{y}-{m}-{d}|{num + add}";
        }

        public static int GetPersistenceReceived(PersistenceData<string> data, DateTime date)
        {
            int y = date.Year, m = date.Month, d = date.Day;
            var str = data.Value.Split('|');
            if (str[0].Equals($"{y}-{m}-{d}"))
            {
                return int.Parse(str[1]);
            }

            return 0;
        }

        public static List<string> AddPersistenceSplit(PersistenceData<string> data, List<string> ids)
        {
            List<string> newL = new List<string>();
            string str = "";
            if (data.Value.Equals(str))
            {
                newL = Clone(ids);
                str = string.Join("|", ids);
            }
            else
            {
                var list = GetPersistenceSplit(data);
                for (int i = 0; i < ids.Count; i++)
                {
                    if (list.IndexOf(ids[i]) == -1)
                    {
                        newL.Add(ids[i]);
                        list.Add(ids[i]);
                    }
                }

                str = string.Join("|", list);
            }

            data.Value = str;
            return newL;
        }

        public static bool HasPersistenceSplit(PersistenceData<string> data, string id)
        {
            var str = data.Value.Split('|').ToList();
            return str.IndexOf(id) != -1;
        }

        public static List<string> GetPersistenceSplit(PersistenceData<string> data)
        {
            var str = data.Value.Split('|');
            return str.ToList();
        }

        public static Color CreateColor(int r, int g, int b, float alpha = 1)
        {
            return new Color(r / 255f, g / 255f, b / 255f, alpha);
        }

        public static Color CreateColor(int hex, float alpha = 1)
        {
            hex &= 0xFFFFFF;
            return CreateColor(hex >> 16, (hex & 0xFFFF) >> 8, hex & 0xFF, alpha);
        }

        public static Color HexToColor(string hex)
        {
            hex = hex.Replace("0x", ""); //in case the string is formatted 0xFFFFFF
            hex = hex.Replace("#", ""); //in case the string is formatted #FFFFFF
            byte a = 255; //assume fully visible unless specified in hex
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            //Only use alpha if the string has enough characters
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return new Color32(r, g, b, a);
        }

        public static Color RandomColorRGB()
        {
            float r = UnityEngine.Random.Range(0f, 1f);
            float g = UnityEngine.Random.Range(0f, 1f);
            float b = UnityEngine.Random.Range(0f, 1f);
            Color color = new Color(r, g, b);
            return color;
        }

        public static Vector2 GetPointPos()
        {
            Vector2 pos;
#if UNITY_EDITOR || UNITY_STANDALONE
            pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
#else
        pos = Input.touches[0].position;
#endif
            return pos;
        }

        public static bool GetIsButton()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return Input.GetMouseButton(0);
#else
        return 1 == Input.touchCount && 
        (Input.touches[0].phase == TouchPhase.Began || Input.touches[0].phase == TouchPhase.Stationary || Input.touches[0].phase == TouchPhase.Moved);
#endif
        }

        public static bool GetIsButtonUp()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return Input.GetMouseButtonUp(0);
#else
        return 1 == Input.touchCount && Input.touches[0].phase == TouchPhase.Ended;
#endif
        }

        public static string GetTimeToHour(int seconds)
        {
            int hour = seconds / 3600;
            int minute = seconds % 3600 / 60;
            seconds = seconds % 3600 % 60;
            var str = $"{hour:D2}:{minute:D2}:{seconds:D2}";
            return str;
        }

        public static string GetTimeToHour(float seconds)
        {
            var str = GetTimeToHour((int) seconds);
            return str;
        }

        public static string GetTimeToMinute(int seconds)
        {
            int minute = seconds / 60;
            seconds = seconds % 3600 % 60;
            var str = $"{minute:D2}:{seconds:D2}";
            return str;
        }

        public static string GetTimeToMinute(float seconds)
        {
            var str = GetTimeToMinute((int) seconds);
            return str;
        }


        public static Vector2 GetPipeSize(PipeCapacity pipeCapacity)
        {
            switch (pipeCapacity)
            {
                case PipeCapacity.Capacity3:
                    return new Vector2(154, 424);

                case PipeCapacity.Capacity4:
                    return new Vector2(154, 536);

                case PipeCapacity.Capacity5:
                    return new Vector2(138, 570);
            }

            return new Vector2(154, 424);
        }


        public static PipeAndBallData GetSizeFitter(PipeNumber pipeNumber, PipeCapacity pipeCapacity)
        {
            var subType = (int) pipeCapacity * 100 + (int) pipeNumber;
            return PipeAndBallConfig.Instance.All.Find(x => x.subType == subType);
        }

        /// <summary>
        /// 销毁所有处于激活状态的Children
        /// </summary>
        public static void DestroyActiveChildren(Transform parent)
        {
            var count = parent.childCount;
            for (int i = 0; i < count; i++)
            {
                var child = parent.GetChild(i);
                if (child.gameObject.activeInHierarchy)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }


        /// <summary>
        /// return a random value from list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        /// <example>
        /// ...
        /// var list = new List<int>(); 
        /// ...
        /// var item = Utils.ListRandom(list);
        /// ...
        public static T ListRandom<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                UnityEngine.Debug.LogError("List is empty");
                return default(T);
            }
            else if (list.Count == 1)
            {
                return list[0];
            }

            return list[UnityEngine.Random.Range(0, list.Count)];
        }


        /// <summary>
        /// 获取概率落入位置
        /// </summary>
        /// <param name="probitList">概率数组</param>
        /// <param name="totalNum"></param>
        /// <returns></returns>
        public static int GetProbitIndex(List<int> probitList, int totalNum = -1)
        {
            var max = totalNum;
            if (max == -1)
            {
                foreach (var item in probitList)
                {
                    max += item;
                }
            }

            var num = UnityEngine.Random.Range(0, max);
            max = 0;
            for (int i = 0; i < probitList.Count; i++)
            {
                max += probitList[i];
                if (num < max)
                {
                    return i;
                }
            }

            return 0;
        }
        
        public static bool IsFirstEnter(string position)
        {
            string key = "Unity.Extend.Utils.IsFirstEnter" + position;
            var rt = !PlayerPrefs.HasKey(key);
            PlayerPrefs.SetInt(key, 1);
            return rt;
        }
        
        /// <summary>
        /// 随机指定范围指定数量
        /// </summary>
        /// <param name="beginNum">起始数</param>
        /// <param name="endNum">结束数</param>
        /// <param name="getCount">随机的数量</param>
        /// <returns></returns>
        public static List<int> GetRandomNumberList(int beginNum, int endNum, int getCount)
        {
            List<int> resultArray = new List<int>();
            List<int> originalArray = new List<int>();
            for (int i = beginNum; i <= endNum; i++)
            {
                originalArray.Add(i);
            }
            int randomCount = originalArray.Count;
            int randomIndex = 0, count = randomCount, temp = 0;
            for (int i = 0; i < getCount; i++)
            {
                randomIndex = UnityEngine.Random.Range(0, count);
                resultArray.Add(originalArray[randomIndex]);
                if (randomIndex != count - 1)
                {
                    temp = originalArray[randomIndex];
                    originalArray[randomIndex] = originalArray[count - 1];
                    originalArray[count - 1] = temp;
                }
                count--;
            }
            return resultArray;
        }
    }
}