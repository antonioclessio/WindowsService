CREATE TABLE [dbo].[IntegracaoSOAP] ( IdIntegracaoSOAP     INT IDENTITY(1, 1) NOT NULL
                                    , DataHoraCadastro     DATETIME           NOT NULL
									, Toa                  VARCHAR(50)            NULL
									, ACFTAIL              VARCHAR(50)            NULL
									, FlightLogID          VARCHAR(50)            NULL
									, Disp                 VARCHAR(50)            NULL
									, Dep                  VARCHAR(50)            NULL
									, Dest                 VARCHAR(50)            NULL
									, Alt1                 VARCHAR(50)            NULL
									, Alt2                 VARCHAR(50)            NULL
									, Toalt                VARCHAR(50)            NULL
									, [STD]                DATETIME               NULL
									, ETA                  DATETIME               NULL
									, PAX                  INT                    NULL
									, TrafficLoad          DECIMAL(16,6)          NULL
									, EmptyWeight          INT                    NULL
									, ZFM                  DECIMAL(16,6)          NULL
									, TripFuel             DECIMAL(16,6)          NULL
									, DestTime             INT                    NULL
									, AltFuel              DECIMAL(16,6)          NULL
									, AltTime              INT                    NULL
									, Alt2Fuel             DECIMAL(16,6)          NULL
									, Alt2Time             INT                    NULL
									, HoldFuel             DECIMAL(16,6)          NULL
									, HoldTime             INT                    NULL
									, FuelMin              DECIMAL(16,6)          NULL
									, TimeMin              VARCHAR(50)            NULL
									, FuelExtra            DECIMAL(16,6)          NULL
									, TimeExtra            VARCHAR(50)            NULL
									, FuelTaxi             VARCHAR(50)            NULL
									, Fuel                 DECIMAL(16,6)          NULL
									, TotalDistance        INT                    NULL
									, ActTOW               DECIMAL(16,6)          NULL
									, MaxTOM               DECIMAL(16,6)          NULL
									, Elw                  DECIMAL(16,6)          NULL
									, MaxLM                DECIMAL(16,6)          NULL
									, MelItems             VARCHAR(50)            NULL
									, FL                   INT                    NULL
									, GCD                  VARCHAR(50)            NULL
									, ESAD                 VARCHAR(50)            NULL
									, WindComponent        VARCHAR(50)            NULL
									, Climb                VARCHAR(50)            NULL
									, Descend              VARCHAR(50)            NULL
									, AddFuel              DECIMAL(16,6)          NULL
									, FinalReserveFuel     DECIMAL(16,6)          NULL
									, DESTSTDALT           VARCHAR(50)            NULL
									, TIMETAXI             VARCHAR(50)            NULL
									, FUELLDG              VARCHAR(50)            NULL
									, TIMELDG              VARCHAR(50)            NULL
									, Crew                 varchar(1000)          NULL
									, FUELBIAS             VARCHAR(50)            NULL
									, SCHBLOCKTIME         INT                    NULL
									, LastEditDate         DATETIME               NULL
									, RouteName            VARCHAR(50)            NULL
									, AltDist              INT                    NULL
									, FuelPolicy           VARCHAR(50)            NULL
									, MaxZFM               DECIMAL(16,6)          NULL
									, FuelPL               VARCHAR(50)            NULL
									, StepClimbProfile     VARCHAR(50)            NULL
									, ATCData              TEXT                   NULL
									, RouteStrings         TEXT                   NULL
									, SIDAlternatives      TEXT                   NULL
									, FreeTextItems        TEXT                   NULL
									, SidAndStarProcedures VARCHAR(50)            NULL
									, FinalReserveMinutes  INT                    NULL
									, AddFuelMinutes       INT                    NULL
									, FuelAltDef           VARCHAR(50)            NULL
									, WeatherObsTime       DATETIME               NULL
									, WeatherPlanTime      DATETIME               NULL
									, SIDPlanned           VARCHAR(1000)          NULL
									, Messages             TEXT                   NULL
                                    , CONSTRAINT PK_IntegracaoSOAP PRIMARY KEY CLUSTERED (IdIntegracaoSOAP)
									);
GO

ALTER TABLE [dbo].[IntegracaoSOAP]
  ADD CONSTRAINT DF_IntegracaoSOAP_DataHoraCadastro DEFAULT GETDATE() FOR DataHoraCadastro;
GO