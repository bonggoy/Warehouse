namespace Warehouse.Data.Attributes
{
	using System;

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class SqlDefaultValueAttribute : Attribute
	{
		public string DefaultValue { get; set; }
	}
}
