using System.Xml.Linq;
using UnityEngine;

public class LocalizationSetHelper
{
    public static string GetTextByTag(string tag, string langCode)
    {
        bool isSuccess = ReadFormXML(langCode, out XDocument xDoc);
        if (!isSuccess)
        {
            Debug.LogError("Read xml error.");
        }

        XElement root = xDoc.Element("Resources");

        foreach (XElement childElement in root.Elements())
        {
            string name = childElement.Attribute("name").Value;
            string text = childElement.Attribute("text").Value;

            if (name.Equals(tag.Trim()))
            {
                return text;
            }
        }

        Debug.LogError($"Your tag:{tag} is not in language file:{langCode}");
        return "";
    }

    public void LoadAllText(XElement rootElement)
    {
    }

    private static bool ReadFormXML(string langCode, out XDocument xDoc)
    {
        var txtFile = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(AssetPath(langCode.ToLower()));

        if (txtFile == null)
        {
            txtFile = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(AssetPath());
            xDoc = XDocument.Parse(txtFile.text.ToString());
            return false;
        }
        else
        {
            xDoc = XDocument.Parse(txtFile.text.ToString());
            return true;
        }
    }

    private static string AssetPath(string lang = "en")
    {
        return $"Assets/ProjectSpace/Localization/Resources_moved/strings-{lang}.xml";
    }
}