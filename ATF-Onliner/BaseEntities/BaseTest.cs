using ATF_Onliner.Services;
using NUnit.Framework;
using OpenQA.Selenium;

namespace ATF_Onliner.BaseEntities
{
    public class BaseTest
    {
        public static readonly string BaseUrl = Configurator.BaseUrl;
        public IWebDriver Driver;
        protected WaitService WaitService;

        [SetUp]
        public void Setup()
        {
            Driver = new BrowserService().WebDriver;
            WaitService = new WaitService(Driver);
        }

        [TearDown]
        public void TearDown()
        {
            Driver.Quit();
        }
    }
}
