using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;
using Xunit.Sdk;

namespace Tests.Core
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class InlineAutoMoqDataAttribute : CompositeDataAttribute
	{
		public InlineAutoMoqDataAttribute(params object[] values)
			: base(new DataAttribute[] {
			new InlineDataAttribute(values), new AutoMoqDataAttribute() })
		{
		}
	}
}
