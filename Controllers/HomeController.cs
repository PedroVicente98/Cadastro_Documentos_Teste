using Cadastro_Documentos_Teste.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cadastro_Documentos_Teste.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CriarBaseDeDados() 
        {
            var comandobase = "CREATE DATABASE CadastroDocumentos;";
            var comandotabela = "CREATE TABLE Documentos (Codigo int NOT NULL, Titulo VARCHAR(255) NOT NULL, Processo VARCHAR(255) NOT NULL,Categoria VARCHAR(255) NOT NULL,NomeArquivo VARCHAR(255) NOT NULL, ExtensaoArquivo VARCHAR(255) NOT NULL, ConteudoArquivo MEDIUMBLOB NOT NULL,PRIMARY KEY (Codigo));";

            var conexao = new MySqlConnection("server = localhost;user = root; port = 3306; password = mysql123;");
            var comandoBase = new MySqlCommand(comandobase, conexao);

            var msg = "Base de Dados Criada";
            var notificationType = NotificationType.SUCCESS;

            try
            {
                conexao.Open();
                comandoBase.ExecuteNonQuery();

                
            }
            catch (MySqlException e)
            {
                msg = e.ErrorCode + ": " + e.Message;
                notificationType = NotificationType.ERROR;

            }
            finally
            {
                this.AddNotification(msg, notificationType);
                conexao.Close();
            }

            conexao = new MySqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            comandoBase = new MySqlCommand(comandotabela, conexao);

            try
            {
                conexao.Open();
                comandoBase.ExecuteNonQuery();

                msg = "Tabela Criada";
            }
            catch (MySqlException e)
            {
                msg = e.ErrorCode + ": " + e.Message;
                notificationType = NotificationType.ERROR;
            }
            finally
            {
                this.AddNotification(msg, notificationType);
                conexao.Close();
            }


            return RedirectToAction("Index");
        }
    }
}