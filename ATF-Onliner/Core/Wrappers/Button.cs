using OpenQA.Selenium;

namespace ATF_Onliner.Core.Wrappers
{
    public class Button
    {
        private readonly UiElement _uiElement;
        
        public Button(IWebDriver driver, By @by)
        {
            _uiElement = new UiElement(driver, @by);
        }
        
        public Button(IWebDriver driver, IWebElement webElement)
        {
            _uiElement = new UiElement(driver, webElement);
        }

        public void Click()
        {
            _uiElement.Click();
        }
    }
}