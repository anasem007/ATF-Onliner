using System;
using OpenQA.Selenium;

namespace ATF_Onliner.Services
{
    public class BrowserService
    {
        private IWebDriver _webDriver;

        public IWebDriver WebDriver
        {
            get => _webDriver;
            set => _webDriver = value;
        }

        public BrowserService()
        {
            WebDriver = Configurator.BrowserType.ToLower() switch
            {
                "chrome" => new DriverFactory().GetChromeDriver(),
                "firefox" => new DriverFactory().GetFirefoxDriver(),
                _ => WebDriver
            };
            
            WebDriver.Manage().Window.Maximize();
            WebDriver.Manage().Cookies.DeleteAllCookies();
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
        }
    }
}