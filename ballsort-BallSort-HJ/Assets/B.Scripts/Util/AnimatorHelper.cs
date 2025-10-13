using UnityEngine;

public static class AnimatorHelper
{
    public static bool IsPlaying(Animator targetAnimator, string name)
    {
        if (targetAnimator != null && targetAnimator.isActiveAndEnabled)
        {
            AnimatorStateInfo info = targetAnimator.GetCurrentAnimatorStateInfo(0);
            return info.IsName(name);
        }
        return false;
    }

    public static void SetTrigger(Animator targetAnimator, string name)
    {
        if (targetAnimator != null && targetAnimator.isActiveAndEnabled)
        {
            targetAnimator.SetTrigger(name);
        }
    }

    public static void SetParameter(Animator targetAnimator, string name, float parameter)
    {
        if (targetAnimator != null && targetAnimator.isActiveAndEnabled)
        {
            targetAnimator.SetFloat(name, parameter);
        }
    }

    public static void SetParameter(Animator targetAnimator, string name, int parameter)
    {
        if (targetAnimator != null && targetAnimator.isActiveAndEnabled)
        {
            targetAnimator.SetInteger(name, parameter);
        }
    }

    public static void SetParameter(Animator targetAnimator, string name, bool parameter)
    {
        if (targetAnimator != null && targetAnimator.isActiveAndEnabled)
        {
            targetAnimator.SetBool(name, parameter);
        }
    }

    public static float GetFloat(Animator targetAnimator, string name)
    {
        if (targetAnimator != null && targetAnimator.isActiveAndEnabled)
        {
            return targetAnimator.GetFloat(name);
        }
        return 0;
    }

    public static int GetInteger(Animator targetAnimator, string name)
    {
        if (targetAnimator != null && targetAnimator.isActiveAndEnabled)
        {
            return targetAnimator.GetInteger(name);
        }
        return 0;
    }

    public static bool GetBool(Animator targetAnimator, string name)
    {
        if (targetAnimator != null && targetAnimator.isActiveAndEnabled)
        {
            return targetAnimator.GetBool(name);
        }
        return false;
    }

    public static void Play(Animator targetAnimator, string name)
    {
        if (targetAnimator != null && targetAnimator.gameObject.activeInHierarchy)
        {
            targetAnimator.Play(name);
        }
    }

    public static void SetIntegerIfExitParameter(Animator targetAnimator, string name, int parameter)
    {
        if (targetAnimator != null && targetAnimator.gameObject.activeInHierarchy &&
            IsParameterExit(targetAnimator, name))
        {
            targetAnimator.SetInteger(name, parameter);
        }
    }

    public static void PlayIfExitState(Animator targetAnimator, string stateName)
    {
        if (targetAnimator != null && targetAnimator.gameObject.activeInHierarchy &&
            IsStateExit(targetAnimator, stateName))
        {
            targetAnimator.Play(stateName);
        }
    }

    //layer 0
    public static bool IsStateExit(Animator targetAnimator, string stateName)
    {
        if (targetAnimator != null)
        {
            return targetAnimator.HasState(0, Animator.StringToHash(stateName));
        }
        return false;
    }

    public static bool IsParameterExit(Animator targetAnimator, string name)
    {
        if (targetAnimator != null)
        {
            for (int i = 0; i < targetAnimator.parameters.Length; i++)
            {
                if (targetAnimator.parameters[i].name == name)
                {
                    return true;
                }
            }
        }
        return false;
    }
}