using System;

namespace Libs
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class SingletonAttribute : Attribute
	{
		public bool DontDestroy
		{
			get;
			set;
		}
		//
		// Constructors
		//
		public SingletonAttribute()
		{
		}
		public SingletonAttribute(bool dontDestroy)
		{
			DontDestroy = dontDestroy;
		}
	}
}