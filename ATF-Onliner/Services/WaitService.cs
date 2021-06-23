using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using static OpenQA.Selenium.Support.UI.ExpectedConditions;

namespace ATF_Onliner.Services
{
    public class WaitService
    {
        [ThreadStatic] protected static IWebDriver Driver;

        private readonly WebDriverWait _wait;
        
        public WaitService(IWebDriver driver)
        {
            Driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Configurator.Timeout));
        }

        public IWebElement GetVisibleElement(By by)
        {
            try
            {
                return _wait.Until(ElementIsVisible(by));
            }
            catch (Exception e)
            {
                throw new AssertionException(e.Message, e);
            } 
        }
    }
}