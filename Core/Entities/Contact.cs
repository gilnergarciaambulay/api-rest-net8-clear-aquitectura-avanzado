using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Contact
    {
        public int Id { get; set; } = default!;
        public string Nombre { get; set; } = default!;
        public string Celular { get; set; } = default!;
        public string Telefono { get; set; } = default!;
        public string Email { get; set; } = default!;

        public string MostrarInfo()
        {
            return $"{Nombre} - {Celular} - {Telefono} - {Email}";
        }
    }
}
