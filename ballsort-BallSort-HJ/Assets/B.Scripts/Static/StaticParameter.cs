public class StaticParameter
{
    public string Name;
    public string StringValue;
    public int IntValue;

    public StaticParameterType Type;

    public StaticParameter(string name, string stringValue)
    {
        Name = name;
        StringValue = stringValue;
        Type = StaticParameterType.String;
    }

    public StaticParameter(string name, int intValue)
    {
        Name = name;
        IntValue = intValue;
        Type = StaticParameterType.Int;
    }

    public string GetString()
    {
        if (Type == StaticParameterType.Int)
        {
            return IntValue.ToString();
        }
        else
        {
            return StringValue;
        }
    }
}

public enum StaticParameterType
{
    Int,
    String,
}