using System.Collections;

public class JobUtils
{
    public delegate void Task();

    public static void Delay(float delay, Task task)
    {
        Job.Make(DoTask(task, delay));
    }

    public static void DelayNextFrame(Task task)
    {
        Job.Make(DoTaskNextFrame(task));
    }

    private static IEnumerator DoTaskNextFrame(Task task)
    {
        yield return Yielders.EndOfFrame;
        task();
    }

    private static IEnumerator DoTask(Task task, float delay)
    {
        yield return Yielders.Get(delay);
        task();
    }
}