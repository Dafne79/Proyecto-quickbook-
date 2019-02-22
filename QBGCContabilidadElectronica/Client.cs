using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QB_App
{
    public class Client
    {
        // Quickbooks
        public string listId;
        public string accountListId;
        // Base
        public string rfc;
        public string nombre;
        // Domicilio
        public string calle;
        public string noExterior;
        public string noInterior;
        public string colonia;
        public string municipio;
        public string estado;
        public string pais;
        public string codigoPostal;

        public void SetDefaults()
        {
            rfc = "";
            nombre = "Sin Nombre";
            calle = "ms";
            noExterior = "SN";
            noInterior = "";
            colonia = "";
            municipio = "";
            estado = "";
            pais = "Mexico";
            codigoPostal = "";
        }
    }
}
