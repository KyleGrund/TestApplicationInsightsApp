using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TestApplicationInsightsApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Handler called when the application is starting.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        void AppStartup(object sender, StartupEventArgs e)
        {
            // start log4net logging services
            log4net.Config.XmlConfigurator.Configure();

            // start application insights telemetry services
            TelemetryConfiguration.Active.InstrumentationKey = "key guid";
        }

        /// <summary>
        /// Handler called when the application is exiting.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event aruments.</param>
        void AppExit(object sender, ExitEventArgs e)
        {
            // desktop apps should call flush
            new TelemetryClient().Flush();

            // allow time for flushing:
            System.Threading.Thread.Sleep(1000);
        }
    }
}
