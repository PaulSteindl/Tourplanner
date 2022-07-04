using Tourplanner.BusinessLayer;
using Tourplanner.DataAccessLayer;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourplanner.Configuration
{
    internal class AppConfiguration : IMapQuestConfiguration, IPostgreSqlDAOConfiguration
    {
        private readonly IConfiguration configuration;

        public string DirectionUrl => configuration["mapquest:directionUrl"];

        public string MapUrl => configuration["mapquest:mapUrl"];

        public string MapQuestKey => configuration["mapquest:key"];

        public string ConnectionString => configuration["database:connectionString"];

        public AppConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}
