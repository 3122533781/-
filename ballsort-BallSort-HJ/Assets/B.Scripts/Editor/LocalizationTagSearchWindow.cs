// using System;
// using System.Collections.Generic;
// using System.Text.RegularExpressions;
// using System.Xml.Linq;
// using UnityEditor;
// using UnityEngine;
//
// public class LocalizationTagSearchWindow : EditorWindow
// {
//     public static void SelectedTag(Action<string, bool> onBack)
//     {
//         _window = GetWindow<LocalizationTagSearchWindow>("Search Tag");
//         _window.Show();
//         _window.maxSize = new Vector2(200, 300);
//         _window._onSelected = onBack;
//     }
//
//     // [SerializeField] private SearchUtils.AutocompleteSearchField autocompleteSearchField;
//
//     private void OnEnable()
//     {
//         LoadAllTag();
//         if (autocompleteSearchField == null) autocompleteSearchField = new SearchUtils.AutocompleteSearchField();
//         autocompleteSearchField.onInputChanged = OnInputChanged;
//         autocompleteSearchField.onConfirm = OnConfirm;
//     }
//
//     private void OnGUI()
//     {
//         GUILayout.Label("Search AssetDatabase", EditorStyles.boldLabel);
//         autocompleteSearchField.OnGUI();
//
//         if (!_initializedPosition)
//         {
//             Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
//             position = new Rect(mousePos.x, mousePos.y, position.width, position.height);
//             _initializedPosition = true;
//         }
//     }
//
//     private void OnInputChanged(string searchString)
//     {
//         _currentSearching = searchString;
//         autocompleteSearchField.ClearResults();
//         if (!string.IsNullOrEmpty(searchString))
//         {
//             foreach (string tagItem in _tagList)
//             {
//                 if (IsMatch(searchString, tagItem))
//                 {
//                     autocompleteSearchField.AddResult(tagItem);
//                 }
//             }
//
//             if (autocompleteSearchField.ResultCount <= 0)
//             {
//                 autocompleteSearchField.AddResult(AddString);
//             }
//         }
//     }
//
//     private void OnConfirm(string result)
//     {
//         if (result.Equals(AddString))
//         {
//             _onSelected.Invoke(_currentSearching, true);
//         }
//         else
//         {
//             _onSelected.Invoke(result, false);
//         }
//
//         CloseWindow();
//     }
//
//     private bool IsMatch(string input, string existTag)
//     {
//         string pattern = $"^{input}[A-Za-z0-9_]*$";
//         return Regex.IsMatch(existTag, pattern);
//     }
//
//     private void LoadAllTag()
//     {
//         TextAsset txtFile = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(AssetPath);
//         XDocument xDoc = XDocument.Parse(txtFile.text.ToString());
//         XElement root = xDoc.Element("Resources");
//         _tagList.Clear();
//         foreach (XElement childElement in root.Elements())
//         {
//             string name = childElement.Attribute("name").Value;
//             string text = childElement.Attribute("text").Value;
//             _tagList.Add(name);
//         }
//     }
//
//     private static string AssetPath
//     {
//         get
//         {
//             return "Assets/ProjectSpace/Localization/Resources_moved/strings-en.xml";
//         }
//     }
//
//     private void OnLostFocus()
//     {
//         CloseWindow();
//     }
//
//     private void CloseWindow()
//     {
//         if (!_isClosing)
//         {
//             _isClosing = true;
//             _window.Close();
//         }
//     }
//
//     public LocalizationTagSearchWindow()
//     {
//         _tagList = new HashSet<string>();
//     }
//
//     private const string AddString = "-Add new Tag-";
//     private static LocalizationTagSearchWindow _window;
//     private bool _initializedPosition = false;
//     private HashSet<string> _tagList;
//     private bool _isClosing;
//     private Action<string, bool> _onSelected;
//     private string _currentSearching;
// }