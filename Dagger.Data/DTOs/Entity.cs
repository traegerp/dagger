using Dapper.Contrib.Extensions;

namespace Dagger.Data.DTOs
{
    public class Entity
    {
        [Key]
        public int id {get; set;}
    }
}