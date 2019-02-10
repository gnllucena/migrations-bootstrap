using System;
using FluentMigrator;
using Microsoft.Extensions.Options;

namespace Migrations.Versions
{

    [Migration(201901012359)]
    public class Bootstrap : Migration
    {
        public override void Up()
        {
            Create.Table("Process")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Status").AsInt32();
        }

        public override void Down()
        {
            Delete.Table("Process");
        }
    }
}
