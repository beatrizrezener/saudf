using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;

namespace saudfhub
{
    class UnidadeDAO
    {
        public List<Unidade> Listar()
        {
            return Conexao.Conn().Table<Unidade>().ToList();
        }

        public static List<Unidade> Listar(string nome)
        {
            return Conexao.Conn().Table<Unidade>().Where(x => x.Nome.Contains(nome)).ToList();
        }

        public List<Unidade> Listar(string porNome, string eBairro)
        {
            return Conexao.Conn().Table<Unidade>().Where(x => x.Nome.Contains(porNome) && x.Bairro.Contains(eBairro)).ToList();
        }

        //public List<Unidade> Listar(Expression<Func<Unidade, bool>> filtro)
        //{
        //    //para filtros mais especificos
        //    return Conexao.Conn().Table<Unidade>().Where(filtro).ToList();
        //}

        //public Unidade Buscar(int id)
        //{
        //    return Listar(x => x.IdUnidade == id).FirstOrDefault();
        //}
    }
}
