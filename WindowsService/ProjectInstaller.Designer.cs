namespace Passaredo.Integracao
{
    partial class PassaredoIntegracaoInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.IntegracaoServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.IntegracaoServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // IntegracaoServiceProcessInstaller
            // 
            this.IntegracaoServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.IntegracaoServiceProcessInstaller.Password = null;
            this.IntegracaoServiceProcessInstaller.Username = null;
            // 
            // IntegracaoServiceInstaller
            // 
            this.IntegracaoServiceInstaller.Description = "Serviço de integração entre Passaredo e Air Support. ";
            this.IntegracaoServiceInstaller.DisplayName = "Integração Air Support";
            this.IntegracaoServiceInstaller.ServiceName = "Integração Passaredo";
            // 
            // PassaredoIntegracaoInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.IntegracaoServiceProcessInstaller,
            this.IntegracaoServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller IntegracaoServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller IntegracaoServiceInstaller;
    }
}