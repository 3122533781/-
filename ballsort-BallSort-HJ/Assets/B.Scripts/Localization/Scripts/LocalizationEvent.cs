using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationEvent
{
    public static Action<string> OnLanguageChangedEvent = delegate { };
}