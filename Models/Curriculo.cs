//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Faps.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Curriculo
    {
        public int codigo_curriculo { get; set; }
        public int codigo_user { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set; }


        public string Usuario { get; set; }
        public string Senha { get; set; }



        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Genero { get; set; }
        public string DataNascimento { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string CEP { get; set; }
        public string Pais { get; set; }
        public string Curso { get; set; }
        public string Curso_status { get; set; }
        public string TituloCargo { get; set; }
        public string Empresa { get; set; }
        public string Data_inicio { get; set; }
        public string DataTermino { get; set; }
        public string DescricaoAtividades { get; set; }
    
        public virtual Usuarios Usuarios { get; set; }

    }
}
