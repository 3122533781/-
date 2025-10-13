using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fangtang;
using Fangtang.Utils;
using UnityEngine;

namespace ProjectSpace.Lei31Utils.Scripts.Framework.ElementKit
{
    public class SceneElementManager : Singleton<SceneElementManager>
    {
        public void Init()
        {
        }

        public void RegisterElement(SceneElement element)
        {
            RegisterElement(element, null);
        }

        public void RegisterElement(SceneElement element, object data)
        {
            if (TryAddElementToDictioinary(element))
            {
                InjectDependencies(element);
                element.Init(data);
            }
        }

        public void UnregisterElement(SceneElement element)
        {
            if (_idToElement.ContainsKey(element.ID))
            {
                element.Free();

                _idToElement.Remove(element.ID);
                _typeToElements[element.GetType()].Remove(element);
                if (_typeToElements[element.GetType()].Count == 0)
                {
                    _typeToElements.Remove(element.GetType());
                }
            }
            else
            {
                //Debug.LogError("Element manager doesn't contain an element with ID [" + element.ID +
                //               "]");
            }
        }

        public bool ContainsElement(SceneElement element)
        {
            return _idToElement.ContainsKey(element.ID);
        }

        public bool TryAddElementToDictioinary(SceneElement element)
        {
            if (_idToElement.ContainsKey(element.ID))
            {
                Debug.LogError("An element with the same id [" +
                               element.ID + "] already exists.");
                return false;
            }
            else
            {
                if (!_typeToElements.ContainsKey(element.GetType()))
                {
                    _typeToElements.Add(element.GetType(), new List<SceneElement>());
                }

                _typeToElements[element.GetType()].Add(element);
                _idToElement.Add(element.ID, element);

                return true;
            }
        }

        public void InjectDependencies(SceneElement targetElement)
        {
            if (targetElement == null) return;

            var members = targetElement.GetType().GetMembers();
            foreach (var memberInfo in members)
            {
                var injectAttribute =
                    memberInfo.GetCustomAttributes(typeof(InjectElementAttribute), true)
                        .FirstOrDefault() as InjectElementAttribute;
                if (injectAttribute != null)
                {
                    if (memberInfo is PropertyInfo)
                    {
                        var propertyInfo = memberInfo as PropertyInfo;

                        propertyInfo.SetValue(targetElement,
                            Resolve(propertyInfo.PropertyType, injectAttribute.Name), null);
                    }
                    else if (memberInfo is FieldInfo)
                    {
                        var fieldInfo = memberInfo as FieldInfo;
                        fieldInfo.SetValue(targetElement,
                            Resolve(fieldInfo.FieldType, injectAttribute.Name));
                    }
                }
            }
        }

        public T Resolve<T>() where T : SceneElement
        {
            return Resolve(typeof(T), null) as T;
        }

        public SceneElement Resolve(Type type, string id)
        {
            if (_typeToElements.ContainsKey(type))
            {
                List<SceneElement> elements = _typeToElements[type];
                if (elements.Count == 1 || string.IsNullOrEmpty(id))
                {
                    return elements[0];
                }
                else
                {
                    return _idToElement.ContainsKey(id) ? _idToElement[id] : null;
                }
            }

            return null;
        }

        public void ClearElements()
        {
            _typeToElements.Clear();
            _idToElement.Clear();
        }

        private SceneElementManager()
        {
            InitElements();
        }

        private void InitElements()
        {
            _typeToElements = new Dictionary<Type, List<SceneElement>>();
            _idToElement = new Dictionary<string, SceneElement>();
        }

        private Dictionary<Type, List<SceneElement>> _typeToElements;
        private Dictionary<string, SceneElement> _idToElement;
    }
}