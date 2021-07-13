using OpenQA.Selenium;

namespace ATF_Onliner.Browsers
{
    public class BrowserFactory
    {
        protected BrowserFactory(IActionRetrier actionRetrier, IBrowserProfile browserProfile,
            ITimeoutConfiguration timeoutConfiguration, ILocalizedLogger localizedLogger)
        {
            ActionRetrier = actionRetrier;
            BrowserProfile = browserProfile;
            TimeoutConfiguration = timeoutConfiguration;
            LocalizedLogger = localizedLogger;
        }

        protected IActionRetrier ActionRetrier { get; }
        protected IBrowserProfile BrowserProfile { get; }
        protected ITimeoutConfiguration TimeoutConfiguration { get; }
        protected ILocalizedLogger LocalizedLogger { get; }

        protected abstract IWebDriver Driver { get; }

        public virtual Browser Browser
        {
            get
            {
                var browser = new Browser(ActionRetrier.DoWithRetry(() => Driver, new[] {typeof(WebDriverException)}));
                LocalizedLogger.Info("loc.browser.ready", BrowserProfile.BrowserName);
                return browser;
            }
        }
    }
}