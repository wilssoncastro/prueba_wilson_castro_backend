using System;

namespace prueba_wilson_castro.Models
{
    public class Paciente
    {

        public int Id { get; set; }
        public int NumeroDocumento { get; set; }
        public String Tipo_documento { get; set; }

        public String Nombres { get; set; }

        public String Apellidos { get; set; }

        public String Correo { get; set; }

        public String Telefono { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public Boolean Estado { get; set; }

      
    }
}
