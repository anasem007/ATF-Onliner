using OpenQA.Selenium;

namespace ATF_Onliner.Core.Wrappers
{
    public class Teaser
    {
        private readonly UiElement _uiElement;
        
        public Teaser(IWebDriver driver, By @by)
        {
            _uiElement = new UiElement(driver, @by);
        }
        
        public Teaser(IWebDriver driver, IWebElement webElement)
        {
            _uiElement = new UiElement(driver, webElement);
        }
    }
}