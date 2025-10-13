using UnityEngine;

public static class ColorExtend
{
    public static Color Alpha0(this Color origin)
    {
        return new Color(origin.r, origin.g, origin.b, 0);
    }
    
    public static Color Alpha1(this Color origin)
    {
        return new Color(origin.r, origin.g, origin.b, 1);
    }

    public static void SetAlpha0(this Color origin)
    {
        origin = new Color(origin.r, origin.g, origin.b, 0);
    }

    public static Color SetAlpha(this Color origin, float alpha)
    {
        return new Color(origin.r, origin.g, origin.b, alpha);
    }
}