
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace ErieGarbage.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Model.DatabaseModels.ErieGarbage.ErieGarbage>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
    } 
}