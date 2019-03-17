namespace DotNetCoreInjectorExtensions.Components
{
	public abstract class Singleton<T> where T: class, new()
	{
		private static T _instance;

		private static readonly object Locker = new object();

		public static T Current
		{
			get
			{
				if (_instance == null)
				{
					lock (Locker)
					{
						if (_instance == null)
							_instance = new T();
					}
				}

				return _instance;
			}
		}
	}
}
