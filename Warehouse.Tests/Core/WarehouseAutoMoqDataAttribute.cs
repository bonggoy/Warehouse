using AutoMapper;
using Tests.Core;


namespace Warehouse.Tests
{
	internal class WarehouseAutoMoqDataAttribute : AutoMoqDataAttribute
	{
		public WarehouseAutoMoqDataAttribute(int repeatCount = 3)
			: base(repeatCount)
		{
			// dynamic map
			var config = new MapperConfiguration(x => x.CreateMissingTypeMaps = true);
			//{
			//	x.CreateMap<Data.Article, Admin.Frontend.Models.Article>();
			//});

			Fixture.Customize<IMapper>(x => x.FromFactory(() => config.CreateMapper()));
		}
	}
}
