using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(SmartLocalizedText))]
[CanEditMultipleObjects]
public class SmartLocalizedTextEditor : UnityEditor.UI.TextEditor
{
    [MenuItem("GameObject/UI/Text - SmartLocalized")]
    public static void AddSmartLocalized()
    {
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            GameObject item = Selection.objects[i] as GameObject;

            GameObject newText = new GameObject("SmartText");
            newText.transform.SetParent(item.transform);
            newText.transform.ResetLocal();
            newText.AddComponent<SmartLocalizedText>();
        }
    }

    [MenuItem("GameObject/UI/ReplaceTextToSmart")]
    public static void ReplaceTextToSmart()
    {
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            GameObject item = Selection.objects[i] as GameObject;

            Text[] texts = item.GetComponentsInChildren<Text>(true);

            for (int j = 0; j < texts.Length; j++)
            {
                GameObject target = texts[j].gameObject;

                Font font = texts[j].font;
                int fontSize = texts[j].fontSize;
                TextAnchor anchor = texts[j].alignment;
                string context = texts[j].text;
                Color color = texts[j].color;
                bool bestFit = texts[j].resizeTextForBestFit;
                int minSize = texts[j].resizeTextMinSize;
                int maxSize = texts[j].resizeTextMaxSize;
                DestroyImmediate(texts[j]);
                SmartLocalizedText newText = target.AddComponent<SmartLocalizedText>();
                newText.font = font;
                newText.fontSize = fontSize;
                newText.alignment = anchor;
                ((Text) newText).text = context;
                newText.UseBestFit = bestFit;
                newText.MinSize = minSize;
                newText.MaxSize = maxSize;
                newText.color = color;
                EditorUtility.SetDirty(target);
            }
        }
    }

    [MenuItem("GameObject/UI/ReplaceSmartToText")]
    public static void ReplaceSmartToText()
    {
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            GameObject item = Selection.objects[i] as GameObject;

            SmartLocalizedText[] texts = item.GetComponentsInChildren<SmartLocalizedText>(true);

            for (int j = 0; j < texts.Length; j++)
            {
                GameObject target = texts[j].gameObject;

                Font font = texts[j].font;
                int fontSize = texts[j].fontSize;
                TextAnchor anchor = texts[j].alignment;
                string context = texts[j].text;
                Color color = texts[j].color;
                bool bestFit = texts[j].UseBestFit;
                int minSize = texts[j].MinSize;
                int maxSize = texts[j].MaxSize;
                DestroyImmediate(texts[j]);
                Text newText = target.AddComponent<Text>();
                newText.font = font;
                newText.fontSize = fontSize;
                newText.alignment = anchor;
                ((Text)newText).text = context;
                newText.resizeTextForBestFit = bestFit;
                newText.resizeTextMinSize = minSize;
                newText.resizeTextMaxSize = maxSize;
                newText.color = color;
                EditorUtility.SetDirty(target);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        DrawBestFit();
        DrawLanguageSetArea();
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void DrawBestFit()
    {
        SmartLocalizedText smartText = target as SmartLocalizedText;

        EditorGUILayout.LabelField("-------------------------Custom--------------------------");
        smartText.Tag_Localized = EditorGUILayout.TextField("Tag", smartText.Tag_Localized);

        if (GUILayout.Button("SearchTag"))
        {
            // LocalizationTagSearchWindow.SelectedTag((tagSelected, needAdd) =>
            // {
            //     smartText.Tag_Localized = tagSelected;
            //     if (needAdd)
            //     {
            //         TagAddHelper.AddTag(tagSelected, ((Text) smartText).text);
            //     }
            // });
        }

        smartText.UseBestFit = EditorGUILayout.Toggle("UseBestFit", smartText.UseBestFit);

        if (smartText.UseBestFit)
        {
//            EditorGUILayout.HelpBox("Best fit only fit single line,so set Wrap to over flow is better!",
//                MessageType.Info);
            EditorGUILayout.HelpBox("Number Don't use this.",
                MessageType.Info);

            smartText.MinSize = EditorGUILayout.IntField("  MinSize", smartText.MinSize);
            smartText.MaxSize = EditorGUILayout.IntField("  MaxSize", smartText.MaxSize);

            EditorGUILayout.HelpBox("Click this will cancel origin best fit", MessageType.Info);
            if (GUILayout.Button("Copy form Origin"))
            {
                smartText.MinSize = smartText.resizeTextMinSize;
                smartText.MaxSize = smartText.resizeTextMaxSize;
                smartText.resizeTextForBestFit = false;
                EditorUtility.SetDirty(smartText);
            }

            if (GUILayout.Button("Fit"))
            {
                smartText.FitBestSize();
                EditorUtility.SetDirty(smartText);
            }

            EditorGUILayout.Space();
        }
    }

    private void DrawLanguageSetArea()
    {
        if (LocalizationConfig.Instance.LanguageList == null)
            return;

        SmartLocalizedText smartText = target as SmartLocalizedText;

        int count = LocalizationConfig.Instance.LanguageList.Count / 4;
        EditorGUILayout.BeginVertical();
        for (int i = 0; i < count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = i * 4; j < (i + 1) * 4 && j < LocalizationConfig.Instance.LanguageList.Count; j++)
            {
                Language language = LocalizationConfig.Instance.LanguageList[j];

                if (GUILayout.Button("Set " + language.LanguageName))
                {
                    SetLanguageTextByTag(language.LangaugeCode, smartText);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        for (int i = count * 4; i < LocalizationConfig.Instance.LanguageList.Count; i++)
        {
            Language language = LocalizationConfig.Instance.LanguageList[i];

            if (GUILayout.Button("Set " + language.LanguageName))
            {
                SetLanguageTextByTag(language.LangaugeCode, smartText);
            }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private void SetLanguageTextByTag(string code, SmartLocalizedText smartText)
    {
        if (string.IsNullOrEmpty(smartText.Tag_Localized))
        {
            Debug.LogError("Your tag is Null or Empty!");
            return;
        }

        smartText.text = LocalizationSetHelper.GetTextByTag(smartText.Tag_Localized, code);
        smartText.font = LocalizationConfig.Instance.GetLanguage(code).FontData.Font;

        if (smartText.UseBestFit)
        {
            LanguageFontData fontData = LocalizationConfig.Instance.GetLanguage(code).FontData;
            smartText.FitFontSize(fontData);
        }

        EditorUtility.SetDirty(smartText);
    }
}