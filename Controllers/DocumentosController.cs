
using System.Data;
using System.Configuration;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cadastro_Documentos_Teste.Models.ViewModels;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;
using System.Web;
using System.IO;
using Cadastro_Documentos_Teste.Extensions;
using System.Linq;

namespace Cadastro_Documentos_Teste.Controllers
{
    public class DocumentosController : Controller
    {
        
        public ActionResult CadastrarDocumentos()
        {  
            return View(new DocumentosViewModel());
        }

        [HttpPost]
        public ActionResult CadastrarDocumentos(DocumentosViewModel model) 
        {
            if (ArquivoValido(model.Arquivo))
            {
                var conexao = new MySqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
                var comando = "INSERT INTO documentos(Codigo, Titulo, Processo, Categoria, NomeArquivo, ExtensaoArquivo, ConteudoArquivo) VALUES (@Codigo, @Titulo, @Processo, @Categoria, @NomeArquivo, @ExtensaoArquivo, @ConteudoArquivo)";
                var cmd = new MySqlCommand(comando, conexao);

                var ExtensaoArquivo = model.Arquivo.ContentType;
                var NomeArquivo = model.Arquivo.FileName;

                var ms = new MemoryStream();
                model.Arquivo.InputStream.CopyTo(ms);

                var ConteudoArquivo = ms.ToArray();

                var msg = "Cadastro Feito";
                var notificationType = NotificationType.SUCCESS;



                try
                {
                    conexao.Open();
                    cmd.Parameters.AddWithValue("@Codigo", model.Codigo);
                    cmd.Parameters.AddWithValue("@Titulo", model.Titulo);
                    cmd.Parameters.AddWithValue("@Processo", model.Processo);
                    cmd.Parameters.AddWithValue("@Categoria", model.Categoria);
                    cmd.Parameters.AddWithValue("@NomeArquivo", NomeArquivo);
                    cmd.Parameters.AddWithValue("@ExtensaoArquivo", ExtensaoArquivo);
                    cmd.Parameters.AddWithValue("@ConteudoArquivo", ConteudoArquivo);


                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    msg = e.ErrorCode + ": " + e.Message;
                    notificationType = NotificationType.ERROR;
                }
                finally
                {
                    conexao.Close();
                    this.AddNotification(msg, notificationType);
                }

                return View(model);

            }
            else 
            {
                this.AddNotification("Extensão de Arquivo não suportada", NotificationType.ERROR);
                return View(model);
            }

        }

        private bool ArquivoValido(HttpPostedFileBase file)
        {
            //PDF, DOC, XLS, DOCX e XLSX. 
            var formats = new string[] {"doc", "docx", "pdf", "xls", "xlsx" }; // add more if u like...
          
            // linq from Henrik Stenbæk
            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        public ActionResult ConsultarDocumentos()
        {
            var conexao = new MySqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            var cmd = new MySqlCommand("SELECT * FROM Documentos ORDER BY Titulo;", conexao);
            var ListaDocumentos = new List<DocumentosViewModel>();

            try
            {
                conexao.Open();

                var tabDados = new DataTable();

                tabDados.Load(cmd.ExecuteReader());

                foreach (DataRow dr in tabDados.Rows)
                {
                    var documento = new DocumentosViewModel
                    {
                        Codigo = Convert.ToInt32(dr["Codigo"]),  
                        Titulo = dr["Titulo"].ToString(),
                        Processo = dr["Processo"].ToString(),
                        Categoria = dr["Categoria"].ToString(),
                        NomeArquivo = dr["NomeArquivo"].ToString()                       
                    };

                    ListaDocumentos.Add(documento);
                }

            }
            catch (MySqlException e) 
            {
                var error = e.ErrorCode + ": " + e.Message;
                this.AddNotification(error, NotificationType.ERROR);
            }
            finally
            {
                conexao.Close();
            }

            return View(ListaDocumentos);
        }

        public ActionResult BaixarArquivo(string NomeArquivo) 
        {
            var conexao = new MySqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            var comandoArquivo = "SELECT ConteudoArquivo, ExtensaoArquivo FROM documentos WHERE NomeArquivo = @NomeArquivo;";
            var cmd = new MySqlCommand(comandoArquivo, conexao);
            string ExtensaoArquivo = null;
            byte[] ConteudoArquivo = null;
            try 
            {
                conexao.Open();
                cmd.Parameters.AddWithValue("@NomeArquivo", NomeArquivo);
                var leitor = cmd.ExecuteReader();
                leitor.Read();
                ConteudoArquivo = (byte[])leitor.GetValue(leitor.GetOrdinal("ConteudoArquivo"));
                ExtensaoArquivo = (string)leitor.GetValue(leitor.GetOrdinal("ExtensaoArquivo"));
            }
            catch(MySqlException e) 
            {
                var error = e.ErrorCode + ": " + e.Message;
                this.AddNotification(error, NotificationType.ERROR);
            }
            finally 
            {
                conexao.Close();
            }


            return File(ConteudoArquivo,ExtensaoArquivo);
        }


    }
}