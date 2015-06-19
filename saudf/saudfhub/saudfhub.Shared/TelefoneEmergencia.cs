using System;
using System.Collections.Generic;
using System.Text;

namespace saudfhub
{
    class TelefoneEmergencia
    {
        public string Nome { get; set; }
        public string Numero { get; set; }

        public string CaminhoFoto { get; set; }
        public TelefoneEmergencia(string Nome, string Numero, string CaminhoFoto)
        {
            this.Nome = Nome;
            this.Numero = Numero;
            this.CaminhoFoto = CaminhoFoto;
        }
    }
}
