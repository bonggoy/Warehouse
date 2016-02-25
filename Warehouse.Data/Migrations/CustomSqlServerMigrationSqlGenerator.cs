using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;

namespace Warehouse.Data.Migrations
{
	public class CustomSqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
	{
		protected override void Generate(AddColumnOperation addColumnOperation)
		{
			SetAnnotatedColumn(addColumnOperation.Column);

			base.Generate(addColumnOperation);
		}

		protected override void Generate(CreateTableOperation createTableOperation)
		{
			SetAnnotatedColumns(createTableOperation.Columns);

			base.Generate(createTableOperation);
		}

		private static void SetAnnotatedColumns(IEnumerable<ColumnModel> columns)
		{
			foreach (var columnModel in columns)
			{
				SetAnnotatedColumn(columnModel);
			}
		}

		// SqlDefaultValueAttribute
		private static void SetAnnotatedColumn(ColumnModel column)
		{
			AnnotationValues values;
			if (column.Annotations.TryGetValue("SqlDefaultValue", out values))
			{
				column.DefaultValueSql = (string)values.NewValue;
			}
		}
	}
}
