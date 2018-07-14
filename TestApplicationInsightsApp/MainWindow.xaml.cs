using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestApplicationInsightsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        /// <summary>
        /// Logger instance.
        /// </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Timer used to generate exceptions.
        /// </summary>
        private Timer exceptionTimer;

        /// <summary>
        /// Timer used to create warn level log events.
        /// </summary>
        private Timer warnTimer;

        /// <summary>
        /// Timer used to create info level log events.
        /// </summary>
        private Timer infoTimer;

        /// <summary>
        /// Timer used to create debug level log events.
        /// </summary>
        private Timer debugTimer;

        /// <summary>
        /// The number of exceptions generated.
        /// </summary>
        private long ExceptionsGeneratedStore = 0;

        /// <summary>
        /// The number of warning events generated.
        /// </summary>
        private long WarnEventsGeneratedStore = 0;

        /// <summary>
        /// The number of infomational events genreated.
        /// </summary>
        private long InfoEventsGeneratedStore = 0;

        /// <summary>
        /// The number of debug events generated.
        /// </summary>
        private long DebugEventsGeneratedStore = 0;

        /// <summary>
        /// A TimerCallback that takes a parameterless Action instance and invokes it.
        /// </summary>
        private TimerCallback timerCallback = (arg) =>
        {
            ((Action)arg)();
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Gets or sets the number of exceptions which have been generated.
        /// </summary>
        private long ExceptionsGenerated
        {
            get
            {
                return this.ExceptionsGeneratedStore;
            }

            set
            {
                this.ExceptionsGeneratedStore = value;
                this.ExceptionCountLabel.Dispatcher.Invoke(() => this.ExceptionCountLabel.Content = this.ExceptionsGeneratedStore);
            }
        }

        /// <summary>
        /// Gets or sets the number of warn events which have been generated.
        /// </summary>
        private long WarnEventsGenerated
        {
            get
            {
                return this.WarnEventsGeneratedStore;
            }

            set
            {
                this.WarnEventsGeneratedStore = value;
                this.WarnCountLabel.Dispatcher.Invoke(() => this.WarnCountLabel.Content = this.WarnEventsGeneratedStore);
            }
        }

        /// <summary>
        /// Gets or sets the number of info events which have been generated.
        /// </summary>
        private long InfoEventsGenerated
        {
            get
            {
                return this.InfoEventsGeneratedStore;
            }

            set
            {
                this.InfoEventsGeneratedStore = value;
                this.InfoCountLabel.Dispatcher.Invoke(() => this.InfoCountLabel.Content = this.InfoEventsGeneratedStore);
            }
        }

        /// <summary>
        /// Gets or sets the number of debug events which have been generated.
        /// </summary>
        private long DebugEventsGenerated
        {
            get
            {
                return this.DebugEventsGeneratedStore;
            }

            set
            {
                this.DebugEventsGeneratedStore = value;
                this.DebugCountLabel.Dispatcher.Invoke(() => this.DebugCountLabel.Content = this.DebugEventsGeneratedStore);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // create a timer that generates exceptions
            // first the exception is generated, then caught, logged, then re-thrown and caught
            // this demonstrates logging of the exception when it is initially thrown and then when logged using log4net but not when it is re-thrown
            this.exceptionTimer = new Timer(
                timerCallback,
                new Action(() =>
                {
                    try
                    {
                        try
                        {
                            this.ExceptionsGenerated++;

                            // this throw is caught by the first chance logging
                            throw new Exception("Test exception.");
                        }
                        catch (Exception ex)
                        {
                            // this logs with the log4net appender
                            log.Error("Exception caught.", ex);

                            // this re-throw does not generate a log entry
                            throw;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }),
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(15));

            // create a timer that generates warn level log events
            this.warnTimer = new Timer(
                timerCallback,
                new Action(() =>
                {
                    this.WarnEventsGenerated++;

                    // generates log event using log4net
                    log.Warn("Warn event at " + DateTime.Now.ToString(CultureInfo.InvariantCulture));
                }),
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5));

            // create a timer that generates info level log events
            this.infoTimer = new Timer(
                timerCallback,
                new Action(() =>
                {
                    this.InfoEventsGenerated++;

                    // generates log event using log4net
                    log.Info("Info event at " + DateTime.Now.ToString(CultureInfo.InvariantCulture));
                }),
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(1));

            // create a timer that generates debug level log events
            this.debugTimer = new Timer(
                timerCallback,
                new Action(() =>
                {
                    this.DebugEventsGenerated++;

                    // generates log event using log4net
                    log.Debug("Debug logged event at " + DateTime.Now.ToString(CultureInfo.InvariantCulture));
                }),
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(0.1));
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Disposes this class.
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether managed resources should be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose all timers
                    if (this.debugTimer != null)
                    {
                        this.debugTimer.Dispose();
                        this.debugTimer = null;
                    }

                    if (this.infoTimer != null)
                    {
                        this.infoTimer.Dispose();
                        this.infoTimer = null;
                    }

                    if (this.warnTimer != null)
                    {
                        this.warnTimer.Dispose();
                        this.warnTimer = null;
                    }

                    if (this.exceptionTimer != null)
                    {
                        this.exceptionTimer.Dispose();
                        this.exceptionTimer = null;
                    }
                }

                // no unmanaged elements to dispose

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MainWindow() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
