using UnityEngine;

public class LogSwitcher : ScriptableSingleton<LogSwitcher>
{
	[SerializeField]
	public bool Open = true;
}