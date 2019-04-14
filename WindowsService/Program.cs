using Passaredo.Integracao.Helper;
using System.ServiceProcess;

namespace Passaredo.Integracao
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            IntegracaoHelper helper = new IntegracaoHelper();
            helper.Execute();

            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new IntegracaoService()
            //};
            //ServiceBase.Run(ServicesToRun);
        }
    }
}
