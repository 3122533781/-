using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using _02.Scripts.LevelEdit;
using UnityEngine;

namespace _02.Scripts.Common
{
    /// <summary>
    /// 栈
    /// 先进后出
    /// 后进先出
    /// </summary>
    [Serializable]
    public class Stack<T> : IReadOnlyCollection<T>,
        ISerializable, IDeserializationCallback,
        IEnumerable<T>, IEnumerable,
        IEnumerator<T>, IEnumerator,
        ICollection
    {
        /// <summary>
        /// 数组的容量
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// 数组现在有多少个元素
        /// </summary>
        [SerializeField] public int Count;

        public object Current => array[position];

        T IEnumerator<T>.Current => array[position];

        public object SyncRoot => throw new NotImplementedException();

        public bool IsSynchronized => false;

        /// <summary>
        /// 反序列化需要用到这个参数存储反序列化的类型
        /// </summary>
        private SerializationInfo info;

        /// <summary>
        /// 数组
        /// </summary>
        public T[] array;

        /// <summary>
        /// 迭代器的显示位置
        /// </summary>
        private int position;

        private int _count;
        private int _count1;

        /// <summary>
        /// 构造
        /// 默认数组的初始化容量是10
        /// </summary>
        public Stack()
            : this(10)
        {
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="capacity">数组初始化的容量</param>
        public Stack(int capacity)
        {
            array = new T[capacity];
            Capacity = capacity;
            Count = 0;
        }

        /// <summary>
        /// 反序列化调用的构造
        /// </summary>
        /// <param name="info">反序列化需要用到这个参数存储反序列化的类型</param>
        protected Stack(SerializationInfo info, StreamingContext context)
            : this(10)
        {
            this.info = info;
        }

        /// <summary>
        /// 看一下头部的元素，不移除栈
        /// </summary>
        /// <returns>返回的元素</returns>
        public T Peek()
        {
            if (Count == 0)
                // return new T();
                ThrowError("栈中元素为空，没有办法查看");
            return array[Count - 1];
        }

        /// <summary>
        /// 移除返回头部的元素
        /// </summary>
        /// <returns>返回的元素</returns>
        public T Pop()
        {
            if (Count == 0)
                ThrowError("栈中元素为空，没有办法取出");
            T temp = array[--Count];
            array[Count] = default(T);

            //缩容
            //如果容量小于数据的个数4分之1，就缩容一半的容量
            if (Count <= Capacity / 4)
                ResetCapacity(Capacity / 2);

            return temp;
        }

        /// <summary>
        /// 添加一个元素到头部
        /// </summary>
        /// <param name="value">需要添加的值</param>
        public void Push(T value)
        {
            array[Count++] = value;

            //扩容
            if (Count == Capacity)
                ResetCapacity(Capacity * 2);
        }


        /// <summary>
        /// 序列化回调这个方法
        /// </summary>
        /// <param name="info">需要把需要序列化的数据存储在这里返回出去</param>
        /// <param name="context"></param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                ThrowError("info为空");

            if (Count != 0)
            {
                T[] array = new T[Count];
                CopyTo(array, 0);
                info.AddValue("data", array, array.GetType());
            }
            else
            {
                Debug.Log($"序列化数据为空");
            }
        }

        /// <summary>
        /// 反序列化回调这个方法
        /// </summary>
        /// <param name="sender">谁调用了这个反序列化</param>
        void IDeserializationCallback.OnDeserialization(object sender)
        {
            if (info == null)
                ThrowError("info为空");

            array = (T[]) info.GetValue("data", typeof(T[]));
            Count = array.Length;
            Capacity = Count;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Reset();
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            Reset();
            return this;
        }

        public bool MoveNext()
        {
            return ++position < Count;
        }

        public void Reset()
        {
            position = -1;
        }

        public void Dispose()
        {
        }

        public void CopyTo(Array array, int index)
        {
            if (array == null)
                ThrowError("Array数组为空");
            else if (index < 0 || index > Count)
                ThrowError("索引不规范");

            Array.Copy(this.array, index, array, index, Count - index);
        }

        int ICollection.Count => _count1;

        /// <summary>
        /// 重置容量
        /// </summary>
        /// <param name="newCapacity">新的容量</param>
        private void ResetCapacity(int newCapacity)
        {
            T[] newArr = new T[newCapacity];
            Array.Copy(array, newArr, Count);
            array = newArr;
            Capacity = newCapacity;
        }

        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="message"></param>
        private void ThrowError(string message)
        {
            throw new AggregateException(message);
        }

        public T GetDataByIndex(int index)
        {
            return array[index];
        }

        int IReadOnlyCollection<T>.Count => _count;
    }
}