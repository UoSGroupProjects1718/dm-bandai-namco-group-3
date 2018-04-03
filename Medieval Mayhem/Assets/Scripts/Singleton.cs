using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	#region  Variables

	protected static bool Quitting { get; private set; }

	private static readonly object Lock = new object();
	private static Dictionary<System.Type, Singleton<T>> _instances;

	public static T Instance
	{
		get
		{
			if (Quitting)
			{
				return null;
			}
			lock (Lock)
			{
				if (_instances == null)
					_instances = new Dictionary<System.Type, Singleton<T>>();

				if (_instances.ContainsKey(typeof(T)))
					return (T) _instances[typeof(T)];
				else
					return null;
			}
		}
	}
}

#endregion