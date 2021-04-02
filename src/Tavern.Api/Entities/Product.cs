using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tavern.Api.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Category Category { get; set; }
    }
}
