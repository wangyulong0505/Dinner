using System;

namespace Dinner.Dapper
{
    public class BaseModel : IEntity<Guid>
    {
        public Guid Id { get; set; }
    }
}
