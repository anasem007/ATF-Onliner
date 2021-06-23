using OpenQA.Selenium;

namespace ATF_Onliner.Core.Wrappers
{
    public class Input
    {
        private readonly UiElement _uiElement;
        
        public Input(IWebDriver driver, By @by)
        {
            _uiElement = new UiElement(driver, @by);
        }
        
        public Input(IWebDriver driver, IWebElement webElement)
        {
            _uiElement = new UiElement(driver, webElement);
        }

        public void SendKeys(string text)
        {
            _uiElement.SendKeys(text);
        }
        
        public void Clear()
        {
            _uiElement.Clear();
        }
    }
}