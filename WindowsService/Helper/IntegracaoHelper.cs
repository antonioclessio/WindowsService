using Passaredo.Integracao.ServiceHelper;
using Passaredo.Integracao.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Passaredo.Integracao.Helper
{
    /// <summary>
    /// Classe responsável por realizar a integração
    /// </summary>
    public class IntegracaoHelper
    {
        readonly double Conversao_LBS_KGS;
        readonly string SourceName;
        public EventLog Log = null;

        public IntegracaoHelper()
        {
            // Variavel para multiplicação (conversão de lbs para kgs).
            Conversao_LBS_KGS = 0.453592;

            SourceName = "IntegracaoSoap";

            Log = new EventLog();

            if (!EventLog.SourceExists(SourceName)) EventLog.CreateEventSource(SourceName, "Integracao.SOAP");
            Log.Source = SourceName;
            Log.Log = "Integracao.SOAP";

            Log.WriteEntry(Settings.Instance.LogServicoIniciado, EventLogEntryType.Information);
        }

        /// <summary>
        /// Execução macro da integração.
        /// </summary>
        public void Execute()
        {
            // Passo 1: Recuperar os dados do serviço.
            var data = GetData();

            // Passo 2: Salvar os dados no banco de dados.
            saveInSql(data);

            // Passo 3: Salvar o arquivo na pasta previamente definida.
            GenerateCSV<IntegracaoSoap>(data);
        }

        /// <summary>
        /// Recupera os dados do serviço proveniente.
        /// </summary>
        /// <returns></returns>
        private List<IntegracaoSoap> GetData()
        {
            using (var client = new EfbServiceSoapClient())
            {
                var sessionId = Settings.Instance.GetSessionID(client);

                try
                {
                    var dataInicial = new DateTime(2019, 1, 23); //DateTime.Now.AddDays(-1);
                    var dataFinal = new DateTime(2019, 1, 24);  //DateTime.Now;
                    var dateChangedAfter = new DateTime(2019, 1, 23); ; // DateTime.Now.AddDays(-1);

                    var flights = client.GetFlightListSearch(sessionId, dataInicial, dataFinal, dateChangedAfter, null, false, null, null, null, null);

                    var result = new List<IntegracaoSoap>();
                    foreach (var item in flights.Items)
                    {
                        var flight = client.GetFlight(sessionId, item.ID, false, false, false, false);

                        var atcData = new StringBuilder();
                        atcData.AppendLine(flight.ATCData.ATCAcco);
                        atcData.AppendLine(flight.ATCData.ATCAlt1);
                        atcData.AppendLine(flight.ATCData.ATCAlt2);
                        atcData.AppendLine(flight.ATCData.ATCCap);
                        atcData.AppendLine(flight.ATCData.ATCColo);
                        atcData.AppendLine(flight.ATCData.ATCCover);
                        atcData.AppendLine(flight.ATCData.ATCCtot);
                        atcData.AppendLine(flight.ATCData.ATCDep);
                        atcData.AppendLine(flight.ATCData.ATCDest);
                        atcData.AppendLine(flight.ATCData.ATCDing);
                        atcData.AppendLine(flight.ATCData.ATCEET);
                        atcData.AppendLine(flight.ATCData.ATCEndu);
                        atcData.AppendLine(flight.ATCData.ATCEqui);
                        atcData.AppendLine(flight.ATCData.ATCFL);
                        atcData.AppendLine(flight.ATCData.ATCID);
                        atcData.AppendLine(flight.ATCData.ATCInfo);
                        atcData.AppendLine(flight.ATCData.ATCJack);
                        atcData.AppendLine(flight.ATCData.ATCNum);
                        atcData.AppendLine(flight.ATCData.ATCPers);
                        atcData.AppendLine(flight.ATCData.ATCPIC);
                        atcData.AppendLine(flight.ATCData.ATCRadi);
                        atcData.AppendLine(flight.ATCData.ATCRem);
                        atcData.AppendLine(flight.ATCData.ATCRoute);
                        atcData.AppendLine(flight.ATCData.ATCRule);
                        atcData.AppendLine(flight.ATCData.ATCSpeed);
                        atcData.AppendLine(flight.ATCData.ATCSSR);
                        atcData.AppendLine(flight.ATCData.ATCSurv);
                        atcData.AppendLine(flight.ATCData.ATCTime);
                        atcData.AppendLine(flight.ATCData.ATCTOA);
                        atcData.AppendLine(flight.ATCData.ATCType);
                        atcData.AppendLine(flight.ATCData.ATCWake);

                        var routeString = new StringBuilder();
                        routeString.AppendLine(flight.RouteStrings.ToAlt1);
                        routeString.AppendLine(flight.RouteStrings.ToAlt2);
                        routeString.AppendLine(flight.RouteStrings.ToDest);

                        var sidAlternatives = new StringBuilder();
                        if (flight.SIDAlternatives != null)
                        {
                            foreach (var sidItem in flight.SIDAlternatives)
                            {
                                sidAlternatives.AppendLine(sidItem.Distance + " / " + sidItem.ProcedureName + " / " + sidItem.RunwayName);
                            }
                        }

                        var freeTexts = new StringBuilder();
                        if (flight.FreeTextItems != null)
                        {
                            foreach (var textItem in flight.FreeTextItems)
                            {
                                freeTexts.AppendLine(textItem.Numbering + " | " + textItem.Value);
                            }
                        }

                        var messages = new StringBuilder();
                        if (flight.Messages != null)
                        {
                            foreach (var message in flight.Messages)
                            {
                                messages.AppendLine("From: " + message.SentFrom);

                                List<string> recipients = new List<string>();
                                foreach (var recipient in message.Recipients)
                                {
                                    recipients.Add(recipient.Recipient + "(" + recipient.RecipientType + ")");
                                }

                                messages.AppendLine("Recipients: " + string.Join(",", recipients.ToArray()));
                                messages.AppendLine("Subject: " + message.Subject);
                                messages.AppendLine("Message: " + message.Text);
                                messages.AppendLine("---------------------------------------------------------------------");
                            }
                        }

                        var crews = new List<string>();
                        if (flight.Crews != null)
                        {
                            foreach (var crew in flight.Crews)
                            {
                                crews.Add(crew.CrewName + "(" + crew.CrewType + ")");
                            }
                        }

                        var melItens = new StringBuilder();
                        if (flight.MelItems != null)
                        {
                            foreach (var melItem in flight.MelItems)
                            {
                                melItens.AppendLine("Identifier: " + melItem.Identifier);
                                melItens.AppendLine("Limitations: " + melItem.Limitations);
                                melItens.AppendLine("Remark: " + melItem.Remark);
                            } 
                        }

                        result.Add(new IntegracaoSoap
                        {
                            Toa = flight.TOA,
                            ACFTAIL = flight.ACFTAIL,
                            FlightLogID = flight.FlightLogID,
                            Disp = flight.DISP,
                            Dep = flight.DEP,
                            Dest = flight.DEST,
                            Alt1 = flight.ALT1,
                            Alt2 = flight.ALT2,
                            Toalt = flight.TOALT,
                            STD = flight.STD,
                            ETA = flight.ETA,
                            PAX = flight.PAX,
                            TrafficLoad = (double.Parse(flight.TrafficLoad) * Conversao_LBS_KGS),
                            EmptyWeight = double.Parse(flight.EmptyWeight) * Conversao_LBS_KGS,
                            ZFM = (double.Parse(flight.ZFM) * Conversao_LBS_KGS),
                            TripFuel = flight.TripFuel * Conversao_LBS_KGS,
                            DestTime = flight.DestTime,
                            AltFuel = flight.AltFuel * Conversao_LBS_KGS,
                            AltTime = flight.AltTime,
                            Alt2Fuel = flight.Alt2Fuel * Conversao_LBS_KGS,
                            Alt2Time = flight.Alt2Time,
                            HoldFuel = flight.HoldFuel * Conversao_LBS_KGS,
                            HoldTime = flight.HoldTime,
                            FuelMin = double.Parse(flight.FUELMIN) * Conversao_LBS_KGS,
                            TimeMin = flight.TIMEMIN,
                            FuelExtra = double.Parse(flight.FUELEXTRA) * Conversao_LBS_KGS,
                            TimeExtra = flight.TIMEEXTRA,
                            FuelTaxi = flight.FUELTAXI,
                            Fuel = flight.FUEL * Conversao_LBS_KGS,
                            TotalDistance = flight.TotalDistance,
                            ActTOW = flight.ActTOW * Conversao_LBS_KGS,
                            MaxTOM = flight.MaxTOM * Conversao_LBS_KGS,
                            Elw = flight.Elw * Conversao_LBS_KGS,
                            MaxLM = flight.MaxLM * Conversao_LBS_KGS,
                            FL = flight.Fl,
                            GCD = flight.GCD,
                            ESAD = flight.ESAD,
                            WindComponent = flight.WindComponent,
                            Climb = flight.Climb,
                            Descend = flight.Descend,
                            AddFuel = flight.AddFuel * Conversao_LBS_KGS,
                            FinalReserveFuel = flight.FinalReserveFuel * Conversao_LBS_KGS,
                            Crew = string.Join(",", crews.ToArray()),
                            DESTSTDALT = flight.DESTSTDALT,
                            TIMETAXI = flight.TIMETAXI,
                            FUELLDG = double.Parse(flight.FUELLDG) * Conversao_LBS_KGS,
                            TIMELDG = flight.TIMELDG,
                            FUELBIAS = flight.FUELBIAS,
                            SCHBLOCKTIME = flight.SCHBLOCKTIME,
                            LastEditDate = flight.LastEditDate,
                            RouteName = flight.RouteName,
                            AltDist = flight.AltDist,
                            FuelPolicy = flight.FuelPolicy,
                            MaxZFM = double.Parse(flight.MaxZFM) * Conversao_LBS_KGS,
                            FuelPL = flight.FuelPL,
                            StepClimbProfile = flight.StepClimbProfile,
                            ATCData = atcData.ToString(),
                            RouteStrings = routeString.ToString(),
                            SIDPlanned = flight.SIDPlanned == null ? null : flight.SIDPlanned.Distance + " / " + flight.SIDPlanned.ProcedureName + " / " + flight.SIDPlanned.RunwayName,
                            SIDAlternatives = sidAlternatives.ToString(),
                            FinalReserveMinutes = flight.FinalReserveMinutes,
                            AddFuelMinutes = flight.AddFuelMinutes,
                            FreeTextItems = freeTexts.ToString(),
                            SidAndStarProcedures = flight.SidAndStarProcedures == null ? null : flight.SidAndStarProcedures.Sid.Name + " | " + flight.SidAndStarProcedures.Star.Name,
                            Messages = messages.ToString(),
                            FuelAltDef = flight.FuelAltDef,
                            WeatherObsTime = flight.WeatherObsTime,
                            WeatherPlanTime = flight.WeatherPlanTime,
                            MelItems = melItens.ToString()
                        });
                    }

                    Log.WriteEntry(Settings.Instance.PrefixSuccessGetData + ": Os dados foram recuperados do serviço com sucesso. Concluido em " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), EventLogEntryType.Information);
                    return result;
                }
                catch (Exception ex)
                {
                    Log.WriteEntry(Settings.Instance.PrefixErrorMessageGetData + ": " + ex.Message, EventLogEntryType.Error);
                    throw;
                }
            }
        }

        /// <summary>
        /// Armazena os dados no banco de dados devidamenteo configurado no App.Config.
        /// </summary>
        /// <param name="flightList"></param>
        private void saveInSql(List<IntegracaoSoap> dataSource)
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    foreach (var dataItem in dataSource)
                    {
                        dataItem.DataHoraCadastro = DateTime.Now;
                        context.Set<IntegracaoSoap>().Add(dataItem);
                    }

                    context.SaveChanges();
                    Log.WriteEntry(Settings.Instance.PrefixSuccessSaveData + ": Os dados foram salvos com sucesso. Concluido em " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                Log.WriteEntry(Settings.Instance.PrefixErrorMessageSaveData + ": " + ex.Message, EventLogEntryType.Error);
                throw;
            }
        }

        /// <summary>
        /// Exportação para CSV
        /// </summary>
        /// <typeparam name="T">Tipo do objeto que deve utilizado como base para gerar a estrutura do arquivo.</typeparam>
        /// <param name="fileName">Nome do arquivo que será gerado</param>
        /// <param name="data">Dados que serão utilizados para gerar o arquivo</param>
        /// <param name="context">Contexto do Request</param>
        /// <returns></returns>
        private void GenerateCSV<T>(List<IntegracaoSoap> data) where T : class
        {
            try
            {
                Type typeResult = typeof(T);
                List<PropertyInfo> fields = typeResult.GetProperties().ToList();

                string delimiter = ";";

                StringBuilder csvFile = new StringBuilder();

                // Gerando o cabeçalho
                string line = "";
                foreach (var field in fields) line = line + field.Name + delimiter;
                csvFile.AppendLine(line.Substring(0, line.Length - 1));

                foreach (var dataItem in data)
                {
                    line = "";
                    foreach (var field in fields) line = line + dataItem.GetType().GetProperty(field.Name).GetValue(dataItem) + delimiter;
                    csvFile.AppendLine(line.Substring(0, line.Length - 1).Replace(Environment.NewLine, string.Empty).Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty));
                }

                if (!Directory.Exists(Settings.Instance.PathFile))
                {
                    Directory.CreateDirectory(Settings.Instance.PathFile);
                }

                string fileNameDate = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss").Replace("-", "");
                string fileName = Settings.Instance.PathFile + @"\" + Settings.Instance.FileName + "." + fileNameDate + ".csv";
                if (File.Exists(fileName)) File.Delete(fileName);

                using (var fs = File.Create(fileName))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(csvFile.ToString());
                    fs.Write(info, 0, info.Length);
                }

                Log.WriteEntry(Settings.Instance.PrefixSuccessSaveFile + ": O arquivo foi gerado com sucesso. Concluido em " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                Log.WriteEntry(Settings.Instance.PrefixErrorMessageSaveFile + ": " + ex.Message, EventLogEntryType.Error);
                throw;
            }
        }
    }
}
