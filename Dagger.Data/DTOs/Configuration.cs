using Dapper.Contrib.Extensions;

namespace Dagger.Data.DTOs
{
    [Table("dbo.Configuration")]
    public class ConfigurationDTO:Entity
    {
        public string queue_system {get; set;}
    }
}