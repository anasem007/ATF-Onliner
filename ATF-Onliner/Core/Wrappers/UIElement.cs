using System;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace ATF_Onliner.Core.Wrappers
{
    public class UiElement : IWebElement
    {
        private readonly IWebElement _webElementImplementation;
        private readonly IWebDriver _driver;
        private By _by;
        private readonly Actions _actions;
        private readonly IJavaScriptExecutor _javaScriptExecutor;

        public UiElement(IWebDriver webDriver, By @by)
        {
            _driver = webDriver;
            _actions = new Actions(webDriver);
            _javaScriptExecutor = (IJavaScriptExecutor) _driver;
            _by = by;
            _webElementImplementation = webDriver.FindElement(@by);
        }

        public UiElement(IWebDriver webDriver, IWebElement webElement)
        {
            _webElementImplementation = webElement;
            _driver = webDriver;
            _actions = new Actions(webDriver);
            _webElementImplementation = webElement;
        }

        public void Hover()
        {
            _actions.MoveToElement(_webElementImplementation).Build().Perform();
        }
        
        public IWebElement FindElement(By @by)
        {
            return _webElementImplementation.FindElement(@by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By @by)
        {
            return _webElementImplementation.FindElements(@by);
        }

        public void Clear()
        {
            _webElementImplementation.Clear();
        }

        public void SendKeys(string text)
        {
            _webElementImplementation.SendKeys(text);
        }

        public void Submit()
        {
            _webElementImplementation.Submit();
        }

        public void Click()
        {
            try
            {
                _webElementImplementation.Click();
            }
            catch (Exception e)
            {
                try
                {
                    _actions
                        .MoveToElement(_webElementImplementation)
                        .Click()
                        .Build()
                        .Perform();
                }
                catch (Exception exception)
                {
                    _javaScriptExecutor.ExecuteScript("arguments[0].click();", _webElementImplementation);
                }
            }
        }

        public string GetAttribute(string attributeName)
        {
            return _webElementImplementation.GetAttribute(attributeName);
        }

        public string GetProperty(string propertyName)
        {
            return _webElementImplementation.GetProperty(propertyName);
        }

        public string GetCssValue(string propertyName)
        {
            return _webElementImplementation.GetCssValue(propertyName);
        }

        public string TagName => _webElementImplementation.TagName;

        public string Text => _webElementImplementation.Text;

        public bool Enabled => _webElementImplementation.Enabled;

        public bool Selected => _webElementImplementation.Selected;

        public Point Location => _webElementImplementation.Location;

        public Size Size => _webElementImplementation.Size;

        public bool Displayed => _webElementImplementation.Displayed;
    }
}