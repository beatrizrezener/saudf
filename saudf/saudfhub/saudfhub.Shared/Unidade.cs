using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace saudfhub
{
    class Unidade
    {
        [PrimaryKey, AutoIncrement]
        public int IdUnidade { get; set; }

        public string Nome { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string Telefone { get; set; }

        public string Endereco { get; set; }

        public string Bairro { get; set; }

        public string Tipo { get; set; }
    }
}
