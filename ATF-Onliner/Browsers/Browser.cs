using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Reflection;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;
using AlertAction = ATF_Onliner.Browsers.AlertAction;
using Browser = ATF_Onliner.Browsers.Browser;

namespace ATF_Onliner.Browsers
{
    public class Browser
    {
        private TimeSpan implicitWaitTimeout;
        private TimeSpan pageLoadTimeout;

        private readonly IBrowserProfile browserProfile;
        private readonly IConditionalWait conditionalWait;
        
        public Browser(IWebDriver webDriver)
        {
            Driver = webDriver;
            // Logger = AqualityServices.LocalizedLogger;
            // LocalizationManager = AqualityServices.Get<ILocalizationManager>();
            // browserProfile = AqualityServices.Get<IBrowserProfile>();
            // conditionalWait = AqualityServices.ConditionalWait;
            // var timeoutConfiguration = AqualityServices.Get<ITimeoutConfiguration>();
            SetImplicitWaitTimeout(timeoutConfiguration.Implicit);
            SetPageLoadTimeout(timeoutConfiguration.PageLoad);
            SetScriptTimeout(timeoutConfiguration.Script);
        }

        private ILocalizedLogger Logger { get; }

        private ILocalizationManager LocalizationManager { get; }
        
        public IWebDriver Driver { get; }
        
        public BrowserName BrowserName => browserProfile.BrowserName;
        
        public void SetImplicitWaitTimeout(TimeSpan timeout)
        {
            if (timeout.Equals(implicitWaitTimeout)) return;
            
            Driver.Manage().Timeouts().ImplicitWait = timeout;
            implicitWaitTimeout = timeout;
        }
        
        public void SetPageLoadTimeout(TimeSpan timeout)
        {
            pageLoadTimeout = timeout;
            
            if (!BrowserName.Equals(BrowserName.Safari))
            {
                Driver.Manage().Timeouts().PageLoad = timeout;
            }
        }

        public string CurrentUrl
        {
            get
            {
                Logger.Info("loc.browser.getUrl");
                var url = Driver.Url;
                Logger.Info("loc.browser.url.value", url);
                return url;
            }
        }
        
        public bool IsStarted => Driver?.SessionId != null;
        
        public void Quit()
        {
            Logger.Info("loc.browser.driver.quit");
            Driver?.Quit();
        }
        
        public void GoTo(string url)
        {
            Navigate().GoToUrl(url);
        }
        
        public void GoBack()
        {
            Navigate().Back();
        }
        
        public void GoForward()
        {
            Navigate().Forward();
        }
        
        public void Refresh()
        {
            Navigate().Refresh();
        }

        private INavigation Navigate()
        {
            return new BrowserNavigation(Driver);
        }
        
        public IBrowserTabNavigation Tabs()
        {
            return new BrowserTabNavigation(Driver);
        }
        
        public void Maximize()
        {
            Logger.Info("loc.browser.maximize");
            Driver.Manage().Window.Maximize();
        }
        
        public void WaitForPageToLoad()
        {
            Logger.Info("loc.browser.page.wait");
            var errorMessage = LocalizationManager.GetLocalizedMessage("loc.browser.page.timeout");
            conditionalWait.WaitForTrue(() => ExecuteScript<bool>(JavaScript.IsPageLoaded), pageLoadTimeout,
                message: errorMessage);
        }
        
        public ReadOnlyCollection<LogEntry> GetLogs(string logKind)
        {
            return Driver.Manage().Logs.GetLog(logKind);
        }

        public void ScrollWindowBy(int x, int y)
        {
            ExecuteScript(JavaScript.ScrollWindowBy, x, y);
        }
        
        public void SetWindowSize(int width, int height)
        {
            Driver.Manage().Window.Size = new Size(width, height);
        }
    }
}