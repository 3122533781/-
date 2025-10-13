using System;
using System.Reflection;

namespace Fangtang.Utils
{
	public class Singleton<T> : IDisposable where T : Singleton<T>
	{
		private static object _lock = new object();
		private static volatile T _instance;

		public static T Instance
		{
			get
			{
				if ((object) Singleton<T>._instance == null)
				{
					lock (Singleton<T>._lock)
					{
						if ((object) Singleton<T>._instance == null)
						{
							ConstructorInfo constructor = typeof (T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, (Binder) null, new Type[0], (ParameterModifier[]) null);
							if ((object) constructor == null || constructor.IsAssembly)
								throw new Exception(string.Format("A private or protected constructor is missing for '{0}'.", (object) typeof (T).Name));
							Singleton<T>._instance = (T) constructor.Invoke((object[]) null);
						}
					}
				}
				return Singleton<T>._instance;
			}
		}

		public void Dispose()
		{
			this.OnDispose();
			Singleton<T>._instance = (T) null;
		}

		protected virtual void OnDispose()
		{
		}
	}
}