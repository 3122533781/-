using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
[AddComponentMenu("UI/Text - Localized", 10)]
public class SmartLocalizedText : Text
{
    [SerializeField] public string Tag_Localized;

    [Header("Custom BestFit")] [SerializeField]
    public bool UseBestFit = false;

    [SerializeField] public int MinSize;
    [SerializeField] public int MaxSize;
    private System.Object[] Param { get; set; } = new object[] { };

    public override string text
    {
        get { return base.text; }
        set
        {
            base.text = value;
            if (UseBestFit)
            {
                FitBestSize();
            }
        }
    }

    public void FitBestSize()
    {
        LanguageFontData fontData = LocalizationManager.Instance.Model.CurrentLanguage.FontData;
        FitFontSize(fontData);
    }

    public void FitFontSize(LanguageFontData fontData)
    {
        RectTransform rect = GetComponent<RectTransform>();

        float rectWidth = rect.rect.width;
        float rectHeight = rect.rect.height;

        int contextLength = base.text.Length;
        float widthToSize = rectWidth / (fontData.FontTextUnitWidth * contextLength);
        float heightToSize = rectHeight / fontData.FontTextUnitHeight; //仅支持单行

        float minSize = Mathf.Min(widthToSize, heightToSize);

        fontSize = Mathf.Clamp(Mathf.FloorToInt(minSize), MinSize, MaxSize);
        CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
//        Invoke(nameof(SetLocalizedText), 0.02F);
        if (Application.isPlaying)
        {
            SetLocalizedText();
            LocalizationEvent.OnLanguageChangedEvent += OnLanguageChangedEvent;
            base.RegisterDirtyLayoutCallback(OnTextChange);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (Application.isPlaying)
        {
            LocalizationEvent.OnLanguageChangedEvent -= OnLanguageChangedEvent;
            base.UnregisterDirtyLayoutCallback(OnTextChange);
        }
    }

    private void OnTextChange()
    {
        if (LocalizationManager.Instance.Model.IsUsingChinese && base.text.Contains(" "))
        {
            base.text = base.text.Replace(" ", "\u00A0");
        }
    }

    private void OnLanguageChangedEvent(string langCode)
    {
        SetLocalizedText();
    }

    private void SetLocalizedText()
    {
        if (!string.IsNullOrEmpty(Tag_Localized))
        {
            font = LocalizationManager.Instance.Model.CurrentLanguage.FontData.Font;
            text = LocalizationManager.Instance.GetTextByTag(Tag_Localized,Param);
        }
    }

    public void SetLocalizedText(String tag,params System.Object[] param)
    {
        font = LocalizationManager.Instance.Model.CurrentLanguage.FontData.Font;
        if (param == null)
        {
            param = new object[] {""};
        }
        text = LocalizationManager.Instance.GetTextByTag(tag, param);
        Param = param;
        Tag_Localized = tag;
    }

    #region 实现超框时再缩小字体，适配多语言
    /// <summary>
    /// 当前可见的文字行数
    /// </summary>
    public int VisibleLines { get; private set; }

    private void _UseFitSettings()
    {
        TextGenerationSettings settings = GetGenerationSettings(rectTransform.rect.size);
        settings.resizeTextForBestFit = false;

        if (!resizeTextForBestFit)
        {
            cachedTextGenerator.PopulateWithErrors(text, settings, gameObject);
            return;
        }

        int minSize = resizeTextMinSize;
        int txtLen = text.Length;
        for (int i = resizeTextMaxSize; i >= minSize; --i)
        {
            settings.fontSize = i;
            cachedTextGenerator.PopulateWithErrors(text, settings, gameObject);
            if (cachedTextGenerator.characterCountVisible == txtLen) break;
        }
    }

    private readonly UIVertex[] _tmpVerts = new UIVertex[4];
    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        if (null == font) return;

        m_DisableFontTextureRebuiltCallback = true;
        _UseFitSettings();

        // Apply the offset to the vertices
        IList<UIVertex> verts = cachedTextGenerator.verts;
        float unitsPerPixel = 1 / pixelsPerUnit;
        int vertCount = verts.Count;

        // We have no verts to process just return (case 1037923)
        if (vertCount <= 0)
        {
            toFill.Clear();
            return;
        }

        Vector2 roundingOffset = new Vector2(verts[0].position.x, verts[0].position.y) * unitsPerPixel;
        roundingOffset = PixelAdjustPoint(roundingOffset) - roundingOffset;
        toFill.Clear();
        if (roundingOffset != Vector2.zero)
        {
            for (int i = 0; i < vertCount; ++i)
            {
                int tempVertsIndex = i & 3;
                _tmpVerts[tempVertsIndex] = verts[i];
                _tmpVerts[tempVertsIndex].position *= unitsPerPixel;
                _tmpVerts[tempVertsIndex].position.x += roundingOffset.x;
                _tmpVerts[tempVertsIndex].position.y += roundingOffset.y;
                if (tempVertsIndex == 3)
                    toFill.AddUIVertexQuad(_tmpVerts);
            }
        }
        else
        {
            for (int i = 0; i < vertCount; ++i)
            {
                int tempVertsIndex = i & 3;
                _tmpVerts[tempVertsIndex] = verts[i];
                _tmpVerts[tempVertsIndex].position *= unitsPerPixel;
                if (tempVertsIndex == 3)
                    toFill.AddUIVertexQuad(_tmpVerts);
            }
        }

        m_DisableFontTextureRebuiltCallback = false;
        VisibleLines = cachedTextGenerator.lineCount;
    }
        #endregion
}