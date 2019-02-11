using System;
using FluentMigrator;
using Microsoft.Extensions.Options;

namespace Migrations.Versions
{

    [Migration(201901020000)]
    public class Insert : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript(@"Initial.sql");
        }

        public override void Down()
        {
        }
    }
}
