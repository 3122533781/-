using Prime31;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;


// when implementing this in your MonoBehaviours, wrap your using UnityEditor and
// OnInspectorGUI/OnSceneGUI methods in #if UNITY_EDITOR/#endif



namespace Prime31Editor
{
    /// <summary>
    /// for fields to work with the Vector3 inspector they must either be public or marked with SerializeField and have the Vector3Inspectable
    /// attribute.
    /// </summary>
    [CustomEditor(typeof(UnityEngine.Object), true)]
    [CanEditMultipleObjects]
    public class ObjectInspector : Editor
    {
        MethodInfo _onInspectorGuiMethod;
        MethodInfo _onSceneGuiMethod;
        List<MethodInfo> _buttonMethods = new List<MethodInfo>();
        MonoScript targetScript;

        // Vector3 editor
        bool _hasVector3Fields = false;
        IEnumerable<FieldInfo> _fields;
        IEnumerable<PropertyInfo> _propertyInfos;

        public void OnEnable()
        {
            var type = target.GetType();
            if (!typeof(IObjectInspectable).IsAssignableFrom(type))
                return;

            if (typeof(ScriptableObject).IsAssignableFrom(type))
            {
                targetScript = MonoScript.FromScriptableObject((ScriptableObject)target);
            }
            else if (typeof(MonoBehaviour).IsAssignableFrom(type))
            {
                targetScript = MonoScript.FromMonoBehaviour((MonoBehaviour)target);
            }

            _onInspectorGuiMethod = target.GetType().GetMethod("OnInspectorGUI", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            _onSceneGuiMethod = target.GetType().GetMethod("OnSceneGUI", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var meths = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.IsDefined(typeof(MakeButtonAttribute), false))
                .Where(m => !m.IsDefined(typeof(CategoryFieldAttribute), false));

            _normalFields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(f => (f.IsPublic || f.IsDefined(typeof(SerializeField), false)) && !f.IsDefined(typeof(CategoryFieldAttribute), false))
                .Where(f => !f.IsDefined(typeof(HideInInspector), false));

            foreach (var meth in meths)
            {
                _buttonMethods.Add(meth);
            }


            _propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            // the vector3 editor needs to find any fields with the Vector3Inspectable attribute and validate them
            _fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(f => f.IsDefined(typeof(Vector3Inspectable), false))
                .Where(f => f.IsPublic || f.IsDefined(typeof(SerializeField), false))
                .Where(f => !f.IsDefined(typeof(HideInInspector), false));
            _hasVector3Fields = _fields.Count() > 0;

            InitCategoryAboult(type);
        }

        public override void OnInspectorGUI()
        {
            var type = target.GetType();
            if (!typeof(IObjectInspectable).IsAssignableFrom(type))
            {
                DrawDefaultInspector();
                return;
            }
            GUI.enabled = false;
            targetScript = EditorGUILayout.ObjectField("Script:", targetScript, typeof(MonoScript), false) as MonoScript;
            GUI.enabled = true;
            serializedObject.Update();
            DrawNormalField();

            if (_onInspectorGuiMethod != null)
            {
                foreach (var eachTarget in targets)
                    _onInspectorGuiMethod.Invoke(eachTarget, new object[0]);
            }

            foreach (var meth in _buttonMethods)
            {
                if (GUILayout.Button(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Regex.Replace(meth.Name, "(\\B[A-Z])", " $1"))))
                    foreach (var eachTarget in targets)
                        meth.Invoke(eachTarget, new object[0]);
            }

            DrawCategory();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawNormalField()
        {
            if (_normalFields == null)
            {
                return;
            }

            foreach (FieldInfo fieldInfo in _normalFields)
            {
                if (fieldInfo.FieldType == typeof(System.Action))
                {
                    continue;
                }
                var property = serializedObject.FindProperty(fieldInfo.Name);
                if (property != null && FieldCanBeDraw(fieldInfo))
                {
                    DrawField(property);
                }
            }
        }

        private void DrawField(SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.Vector3)
            {
                DrawVector3Field(property);
            }
            else
            {
                EditorGUILayout.PropertyField(property, true);
            }
        }

        private void DrawVector3Field(SerializedProperty property)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(property, true);
            if (GUILayout.Button("p", GUILayout.Width(20f)))
            {
                property.vector3Value = GetCopyBufferVector3();
            }
            EditorGUILayout.EndHorizontal();
        }

        public static Vector3 GetCopyBufferVector3()
        {
            Vector3 result = new Vector3();
            string bufferString = EditorGUIUtility.systemCopyBuffer;
            string[] bufferArray = bufferString.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);

            if (bufferArray.Length == 3)
            {
                result.x = float.Parse(bufferArray[0]);
                result.y = float.Parse(bufferArray[1]);
                result.z = float.Parse(bufferArray[2]);
            }
            return result;
        }

        private bool FieldCanBeDraw(FieldInfo info)
        {
            var condition = info.GetCustomAttributes(typeof(IfContentAttribute),true).FirstOrDefault() as IfContentAttribute;
            if (condition != null)
            {
                foreach (PropertyInfo property in _propertyInfos)
                {
                    if (property.Name == condition.ConditionName && 
                        property.PropertyType == typeof(Boolean))
                    {
                        var ifCondition = property.GetValue(target,null);
                        if (ifCondition is Boolean)
                        {
                            return (Boolean)ifCondition;
                        }
                        return true;
                    }
                }
                return true;
            }
            return true;
        }

        protected virtual void OnSceneGUI()
        {
            if (_onSceneGuiMethod != null)
                _onSceneGuiMethod.Invoke(target, new object[0]);

            if (_hasVector3Fields)
                vector3OnSceneGUI();
        }


        #region Vector3 editor

        void vector3OnSceneGUI()
        {
            foreach (var field in _fields)
            {
                var value = field.GetValue(target);
                if (value is Vector3)
                {
                    Handles.Label((Vector3)value, field.Name);
                    var newValue = Handles.PositionHandle((Vector3)value, Quaternion.identity);
                    if (GUI.changed)
                    {
                        GUI.changed = false;
                        field.SetValue(target, newValue);
                    }
                }
                else if (value is List<Vector3>)
                {
                    var list = value as List<Vector3>;
                    var label = field.Name + ": ";

                    for (var i = 0; i < list.Count; i++)
                    {
                        Handles.Label(list[i], label + i);
                        list[i] = Handles.PositionHandle(list[i], Quaternion.identity);
                    }
                    Handles.DrawPolyLine(list.ToArray());
                }
                else if (value is Vector3[])
                {
                    var list = value as Vector3[];
                    var label = field.Name + ": ";

                    for (var i = 0; i < list.Length; i++)
                    {
                        Handles.Label(list[i], label + i);
                        list[i] = Handles.PositionHandle(list[i], Quaternion.identity);
                    }
                    Handles.DrawPolyLine(list);
                }
            }
        }

        #endregion

        #region Category editor
        private void InitCategoryAboult(Type type)
        {
            var categoryField = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(f => f.IsPublic || f.IsDefined(typeof(SerializeField), false))
                    .Where(f => f.IsDefined(typeof(CategoryFieldAttribute), false));

            var categoryMeth = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(f => f.IsDefined(typeof(CategoryFieldAttribute), false))
                .Where(f => f.IsDefined(typeof(MakeButtonAttribute), false));


            _categoryIDToCategoryInfo = new Dictionary<string, CategoryInfo>();
            _categoryLabels = new List<string>();
            foreach (FieldInfo info in categoryField)
            {
                var categoryInfos = info.GetCustomAttributes(typeof(CategoryFieldAttribute), true).FirstOrDefault() as CategoryFieldAttribute;
                if (categoryInfos != null)
                {
                    if (_categoryIDToCategoryInfo.ContainsKey(categoryInfos.ID))
                    {
                        _categoryIDToCategoryInfo[categoryInfos.ID].Fields.Add(info);
                    }
                    else
                    {
                        _categoryLabels.Add(categoryInfos.ID);
                        _categoryIDToCategoryInfo.Add(categoryInfos.ID
                            , new CategoryInfo() { Fields = new List<FieldInfo>() { info }, Meths = new List<MethodInfo>() });
                    }
                }
            }

            foreach (MethodInfo info in categoryMeth)
            {
                var categoryInfos = info.GetCustomAttributes(typeof(CategoryFieldAttribute), true).FirstOrDefault() as CategoryFieldAttribute;
                if (categoryInfos != null)
                {
                    if (_categoryIDToCategoryInfo.ContainsKey(categoryInfos.ID))
                    {
                        _categoryIDToCategoryInfo[categoryInfos.ID].Meths.Add(info);
                    }
                    else
                    {
                        _categoryLabels.Add(categoryInfos.ID);
                        _categoryIDToCategoryInfo.Add(categoryInfos.ID
                            , new CategoryInfo() { Fields = new List<FieldInfo>(), Meths = new List<MethodInfo>() { info } });
                    }
                }
            }
        }

        private void DrawCategory()
        {
            if (_categoryIDToCategoryInfo == null || _categoryIDToCategoryInfo.Count == 0)
            {
                return;
            }

            Rect region = EditorGUILayout.GetControlRect(false, 30);
            _selectedIndex = GUI.Toolbar(region, _selectedIndex, _categoryLabels.ToArray());
            CategoryInfo categoryInfo = _categoryIDToCategoryInfo.Values.ElementAt(_selectedIndex);


            foreach (FieldInfo fieldInfo in categoryInfo.Fields)
            {
                var property = serializedObject.FindProperty(fieldInfo.Name);

                if (FieldCanBeDraw(fieldInfo) && property != null)
                {
                    DrawField(property);
                }
            }

            foreach (MethodInfo meth in categoryInfo.Meths)
            {
                if (GUILayout.Button(meth.Name))
                {
                    foreach (var eachTarget in targets)
                    {
                        meth.Invoke(eachTarget, new object[0]);
                    }
                }
            }
        }

        private Dictionary<string, CategoryInfo> _categoryIDToCategoryInfo;
        private List<string> _categoryLabels;
        private int _selectedIndex;

        public class CategoryInfo
        {
            public List<FieldInfo> Fields;
            public List<MethodInfo> Meths;
        }
        #endregion

        private IEnumerable<FieldInfo> _normalFields;
    }
}