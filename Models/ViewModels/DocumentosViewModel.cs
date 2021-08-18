using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cadastro_Documentos_Teste.Models.ViewModels
{
    public class DocumentosViewModel
    {
        public DocumentosViewModel() 
        {
        
        }

        public DocumentosViewModel(int codigo, string titulo, string processo, string categoria, HttpPostedFileBase arquivo)
        {
            Codigo = codigo;
            Titulo = titulo;
            Processo = processo;
            Categoria = categoria;
            Arquivo = arquivo;
        }

        public int Codigo { get; set; }
        public string Titulo { get; set; }

        public string Processo { get; set; }

        public string Categoria { get; set; }

        public string NomeArquivo { get; set; }

        public HttpPostedFileBase Arquivo { get; set; }

    }
}