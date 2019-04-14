using Passaredo.Integracao.ServiceHelper;
using System;
using System.Configuration;
using System.Diagnostics;

namespace Passaredo.Integracao.Helper
{
    public class Settings
    {
        private EventLog Log { get; set; }

        #region Construtor
        public Settings()
        {
            try
            {
                Log = new EventLog();
                Log.Source = "Windows Log";
                Log.Log = "Application";

                PartnerUser = ConfigurationManager.AppSettings["PartnerUser"];
                PartnerPassword = ConfigurationManager.AppSettings["PartnerPassword"];
                CustomerUser = ConfigurationManager.AppSettings["CustomerUser"];
                CustomerPassword = ConfigurationManager.AppSettings["CustomerPassword"];

                PathFile = ConfigurationManager.AppSettings["PathFile"];
                FileName = ConfigurationManager.AppSettings["FileName"];

                LogServicoIniciado = ConfigurationManager.AppSettings["LogServicoIniciado"];
                LogServicoParado = ConfigurationManager.AppSettings["LogServicoParado"];
                LogServicoPausa = ConfigurationManager.AppSettings["LogServicoPausa"];
                LogServicoReinicio = ConfigurationManager.AppSettings["LogServicoReinicio"];

                PrefixErrorMessageGetData = ConfigurationManager.AppSettings["PrefixErrorMessageGetData"];
                PrefixErrorMessageSaveData = ConfigurationManager.AppSettings["PrefixErrorMessageSaveData"];
                PrefixErrorMessageSaveFile = ConfigurationManager.AppSettings["PrefixErrorMessageSaveFile"];
                PrefixErrorLoadSettings = ConfigurationManager.AppSettings["PrefixErrorLoadSettings"];

                HoraExecucao = int.Parse(ConfigurationManager.AppSettings["HoraExecucao"]);
            }
            catch (Exception ex)
            {
                Log.WriteEntry(PrefixErrorLoadSettings + ": " + ex.Message, EventLogEntryType.Error);
                throw;
            }
        }
        #endregion

        #region Padrão Singleton
        private static Settings _Instance = null;
        private static readonly object padlock = new object();

        public static Settings Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_Instance == null)
                    {
                        _Instance = new Settings();
                    }
                    return _Instance;
                }
            }
        }
        #endregion

        #region Propriedades
        /// <summary>
        /// Configuração de acesso ao serviço.
        /// </summary>
        public string PartnerUser { get; set; }

        /// <summary>
        /// Configuração de acesso ao serviço.
        /// </summary>
        public string PartnerPassword { get; set; }

        /// <summary>
        /// Configuração de acesso ao serviço.
        /// </summary>
        public string CustomerUser { get; set; }

        /// <summary>
        /// Configuração de acesso ao serviço.
        /// </summary>
        public string CustomerPassword { get; set; }

        /// <summary>
        /// Local onde o arquivo CSV será salvo
        /// </summary>
        public string PathFile { get; set; }

        /// <summary>
        /// Nome do arquivo que será criado. A extensão é "CSV" por padrão.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Mensagem de log para quando o serviço for iniciado.
        /// </summary>
        public string LogServicoIniciado { get; set; }

        /// <summary>
        /// Mensagem de log para quando o serviço for parado.
        /// </summary>
        public string LogServicoParado { get; set; }

        /// <summary>
        /// Mensagem de log para quando o serviço for pausado.
        /// </summary>
        public string LogServicoPausa { get; set; }

        /// <summary>
        /// Mensagem de log para quando o serviço for reiniciado.
        /// </summary>
        public string LogServicoReinicio { get; set; }

        /// <summary>
        /// Em caso de erro no consumo do serviço, o erro será exibido com o prefixo configurado aqui
        /// </summary>
        public string PrefixErrorMessageGetData { get; set; }

        /// <summary>
        /// Em caso de erro ao salvar os dados no banco sql, o erro será exibido com o prefixo configurado aqui
        /// </summary>
        public string PrefixErrorMessageSaveData { get; set; }

        /// <summary>
        /// Em caso de erro ao criar o arquivo csv, o erro será exibido com o prefixo configurado aqui
        /// </summary>
        public string PrefixErrorMessageSaveFile { get; set; }

        /// <summary>
        /// Erro na leitura das configurações
        /// </summary>
        public string PrefixErrorLoadSettings { get; set; }

        /// <summary>
        /// Em caso de sucesso ao recuperar os dados do serviço 
        /// </summary>
        public string PrefixSuccessGetData { get; set; }

        /// <summary>
        /// Em caso de sucesso ao salvar as informações no banco de dados
        /// </summary>
        public string PrefixSuccessSaveData { get; set; }

        /// <summary>
        /// Em caso de sucesso ao gerar o arquivo XMLS
        /// </summary>
        public string PrefixSuccessSaveFile { get; set; }

        /// <summary>
        /// Define a hora em que o serviço será executado. Isto não significa que será no horário exato, mas qualquer minuto dentro do horário especificado
        /// </summary>
        public int HoraExecucao { get; set; }
        #endregion

        #region Métodos
        /// <summary>
        /// Retorna a SessionID necessária para que a chamada ao serviço seja realizada.
        /// Consome as configurações previamente definidas.
        /// </summary>
        /// <param name="client">Instância do serviço a ser criada pelo método consumidor</param>
        /// <returns>SessionID gerada pelo serviço.</returns>
        public string GetSessionID(EfbServiceSoapClient client)
        {
            return client.GetSessionID(Instance.PartnerUser, Instance.PartnerPassword, Instance.CustomerUser, Instance.CustomerPassword);
        }
        #endregion
    }
}
