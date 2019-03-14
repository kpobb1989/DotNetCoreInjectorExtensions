namespace DotNetCoreInjectorExtensions.Components
{
	public abstract class Singleton<T> where T: class, new()
	{
		private static T _resolver;

		private static readonly object Locker = new object();

		public static T Current
		{
			get
			{
				if (_resolver == null)
				{
					lock (Locker)
					{
						if (_resolver == null)
							_resolver = new T();
					}
				}

				return _resolver;
			}
		}
	}
}
