using UnityEditor;
using UnityEngine;

namespace Lei31
{
    public static class MathHelpers
    {
        /// <summary>
        /// Maps a value from some arbitrary range to the 0 to 1 range
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="min">Lminimum value.</param>
        /// <param name="max">maximum value</param>
        public static float map01(float value, float min, float max)
        {
            return (value - min) * 1f / (max - min);
        }


        /// <summary>
        /// mapps value (which is in the range leftMin - leftMax) to a value in the range rightMin - rightMax
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="leftMin">Left minimum.</param>
        /// <param name="leftMax">Left max.</param>
        /// <param name="rightMin">Right minimum.</param>
        /// <param name="rightMax">Right max.</param>
        public static float map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
        {
            return rightMin + (Mathf.Clamp(value, leftMin, leftMax) - leftMin) * (rightMax - rightMin) /
                   (leftMax - leftMin);
        }

        public static Vector3 GetMiddlePoint(Vector3 begin, Vector3 end, float delta = 0)
        {
            Vector3 center = Vector3.Lerp(begin, end, 0.5f);
            Vector3 beginEnd = end - begin;
            Vector3 perpendicular = new Vector3(-beginEnd.y, beginEnd.x, 0).normalized;
            Vector3 middle = center + perpendicular * delta;
            return middle;
        }

        ///舍弃个位数
        public static int GiveUpDigits(int value)
        {
            int left = value / 10;
            return left * 10;
        }

        /// <summary>
        /// Random point inside triangle 
        /// </summary>
        /// <returns></returns>
        public static Vector3 RandomTrianglePoint(Vector3 pointA, Vector3 pointB, Vector3 pointC)
        {
            float r1 = Random.Range(0, 1f);
            float r2 = Random.Range(0, 1f);
            float pointRandomX = (1 - Mathf.Sqrt(r1)) * pointA.x + (Mathf.Sqrt(r1) * (1 - r2)) * pointB.x +
                                 (Mathf.Sqrt(r1) * r2) * pointC.x;

            float pointRandomY = (1 - Mathf.Sqrt(r1)) * pointA.y + (Mathf.Sqrt(r1) * (1 - r2)) * pointB.y +
                                 (Mathf.Sqrt(r1) * r2) * pointC.y;
            float pointRandomZ = (1 - Mathf.Sqrt(r1)) * pointA.z + (Mathf.Sqrt(r1) * (1 - r2)) * pointB.z +
                                 (Mathf.Sqrt(r1) * r2) * pointC.z;

            return new Vector3(pointRandomX, pointRandomY, pointRandomZ);
        }

        public static Vector3 RandomEulerAngle()
        {
            float x = Random.Range(0, 360);
            float y = Random.Range(0, 360);
            float z = Random.Range(0, 360);

            return new Vector3(x, y, z);
        }

        public static Vector2 RandomCircleInLine(Vector2 center, float radius)
        {
            float x = Random.Range(-radius, radius);
            float y = Mathf.Sqrt(radius * radius - x * x);

            y = (Random.Range(0, 2) < 1 ? 1 : -1) * y;
            return new Vector2(center.x + x, center.y + y);
        }

        public static Vector2 RandomCircleInLineH(Vector2 center, float radius)
        {
            float x = Random.Range(-radius, radius);
            float y = Mathf.Sqrt(radius * radius - x * x);

            return new Vector2(center.x + x, center.y + y);
        }

        public static Vector3 RandomSpherePointInSurface(Vector3 center, float radius)
        {
            float angle1 = Random.Range(0, 360);
            float angle2 = Random.Range(0, 360);

            float x = radius * Mathf.Sin(angle1) * Mathf.Cos(angle2);
            float y = radius * Mathf.Sin(angle1) * Mathf.Sin(angle2);
            float z = radius * Mathf.Cos(angle1);
            return new Vector3(center.x + x, center.y + y, center.z + z);
        }

        public static Vector3 ProjectPointToPlane(Vector3 point, Transform plane, Vector3 normal)
        {
            Vector3 localPos = plane.InverseTransformPoint(point);
            Vector3 vecN = plane.InverseTransformDirection(normal) *
                           Vector3.Dot(localPos, plane.InverseTransformDirection(normal));

            localPos = localPos - vecN;
            return plane.TransformPoint(localPos);
        }
        
        public static int[] GetIndexRandomNum(int minValue, int maxValue)
        {
            System.Random random = new System.Random();
            int sum = Mathf.Abs(maxValue - minValue);
            int site = sum;
            int[] index = new int[sum];
            int[] result = new int[sum];
            int temp = 0;
            for (int i = minValue; i < maxValue; i++)
            {
                index[temp] = i;
                temp++;
            }
            for (int i = 0; i < sum; i++)
            {
                int id = random.Next(0, site - 1);
                result[i] = index[id];
                index[id] = index[site - 1];
                site --;
            }
            return result;
        }
    }
}