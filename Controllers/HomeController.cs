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
            string comandobase = "CREATE DATABASE CadastroDocumentos;";
            string comandotabela = "CREATE TABLE Documentos (Codigo int NOT NULL, Titulo VARCHAR(255) NOT NULL, Processo VARCHAR(255) NOT NULL,Categoria VARCHAR(255) NOT NULL,NomeArquivo VARCHAR(255) NOT NULL, ExtensaoArquivo VARCHAR(255) NOT NULL, ConteudoArquivo MEDIUMBLOB NOT NULL,PRIMARY KEY (Codigo));";

            MySqlConnection conexao = new MySqlConnection("server = localhost;user = root; port = 3306; password = mysql123;");
            MySqlCommand comandoBase = new MySqlCommand(comandobase, conexao);
            
            try
            {
                conexao.Open();
                comandoBase.ExecuteNonQuery();

                TempData["success"] = "Base de Dados Criada";
            }
            catch (MySqlException e)
            {
                var error = "Error" + e.ErrorCode + ": " + e.Message;
                TempData["error"] = error;
            }
            finally
            {
                conexao.Close();
            }

            conexao = new MySqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            comandoBase = new MySqlCommand(comandotabela, conexao);

            try
            {
                conexao.Open();
                comandoBase.ExecuteNonQuery();

                TempData["success"] = "Base de Dados Criada";
            }
            catch (MySqlException e)
            {
                var error = "Error" + e.ErrorCode + ": " + e.Message;
                TempData["error"] = error;
            }
            finally
            {
                conexao.Close();
            }


            return RedirectToAction("Index");
        }
    }
}