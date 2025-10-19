using Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWPF.dto
{
    public class CreateProductDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Price { get; set; }
        public bool IsActive { get; set; } = true;
        public ProductUnit ProductUnit { get; set; } = null!;
    }
}
