using Passaredo.Integracao.Helper;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;

namespace Passaredo.Integracao
{
    public partial class IntegracaoService : ServiceBase
    {
        readonly string SourceName;
        private EventLog Log { get; set; }

        public IntegracaoService()
        {
            SourceName = "Passaredo";

            InitializeComponent();
            Log = new EventLog();
        }

        #region Eventos do serviço
        protected override void OnStart(string[] args)
        {
            if (!EventLog.SourceExists(SourceName)) EventLog.CreateEventSource(SourceName, "Integracao.AirSupport");
            Log.Source = SourceName;
            Log.Log = "Integracao.AirSupport";

            Log.WriteEntry(Settings.Instance.LogServicoIniciado, EventLogEntryType.Information);

            Timer timer = new Timer(10 * 60 * 6000); // A cada 60 minutos
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            Log.WriteEntry("Integração Air Support iniciado com sucesso.", EventLogEntryType.Information);
        }

        protected override void OnStop()
        {
            Log.WriteEntry(Settings.Instance.LogServicoParado, EventLogEntryType.Warning);
        }

        protected override void OnPause()
        {
            Log.WriteEntry(Settings.Instance.LogServicoPausa, EventLogEntryType.Warning);
        }

        protected override void OnContinue()
        {
            Log.WriteEntry(Settings.Instance.LogServicoReinicio, EventLogEntryType.Information);
        }
        #endregion

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Log.WriteEntry("Integração Air Support: Verificando se é hora de realizar integração. A hora é configurada no App.Config do projeto.", EventLogEntryType.Information);
            if (e.SignalTime.Hour != Settings.Instance.HoraExecucao) return;

            IntegracaoHelper helper = new IntegracaoHelper();
            helper.Execute();
        }
    }
}
