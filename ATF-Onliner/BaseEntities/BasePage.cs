using System;
using NUnit.Framework;
using OpenQA.Selenium;

namespace ATF_Onliner.BaseEntities
{
    public abstract class BasePage
    {
        [ThreadStatic] protected static IWebDriver Driver;

        protected abstract void OpenPage();
        public abstract bool IsPageOpened();

        public BasePage(IWebDriver driver, bool openPageByUrl)
        {
            Driver = driver;

            if (openPageByUrl)
            {
                OpenPage();
            }

            WaitForOpen();
        }

        protected void WaitForOpen()
        {
            if (!IsPageOpened())
            {
                throw new AssertionException("Page was not opened.");
            }
        }
    }
}