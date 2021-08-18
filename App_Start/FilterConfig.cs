using System.Web;
using System.Web.Mvc;

namespace Cadastro_Documentos_Teste
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
