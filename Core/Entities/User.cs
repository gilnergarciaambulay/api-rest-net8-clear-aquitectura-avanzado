using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User
    {
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }  
        public string HashClave { get; set; }
        public string CorreoElectronico { get; set; }

        // Relación opcional: un usuario puede tener varios contactos
        public ICollection<Contact> Contactos { get; set; } = new List<Contact>();
    }
}
