
[System.Serializable]
public class FloatDisplaySpeed
{
    public DisplaySpeedType Type;
    public float Duration; //总共到达需要的时间
    public float Step; //每次跳跃数字大小
    public float StepDuration; //每次跳跃间隔时间
}