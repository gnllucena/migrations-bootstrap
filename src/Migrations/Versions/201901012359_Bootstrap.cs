using FluentMigrator;

namespace Migration.Versions
{

    [Migration(201901012359)]
    public class Bootstrap : Migration
    {
        public override void Up()
        {
            if (!IfDatabase("Oracle").Schema.Schema("ESTABCORE").Table("TARIFAS").Column("DSTARIFA").Exists())
            {
                Create.Column("DSTARIFA").OnTable("TARIFAS").InSchema("ESTABCORE").AsString();

                Create.Table("Status")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("Description").AsString();
            }
        }

        public override void Down()
        {
        }
    }
}
