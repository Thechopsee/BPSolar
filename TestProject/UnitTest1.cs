using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;

namespace TestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            IWebDriver driver = new FirefoxDriver();
            //driver.Manage().Window.Maximize();
            //var element=driver.FindElement(By.Id("categoriesContent")).FindElement(By.PartialLinkText("Hardware"));
            // var element = driver.FindElement(By.Id("categoriesContent"));

            driver.Navigate().GoToUrl("https://localhost:44321");
            //var element = driver.FindElement(By.Id("categoriesContent")).FindElement(By.PartialLinkText("Hardware"));
            //element.Click();
            var el=driver.FindElement(By.Id("smf"));
            el.Click();
            var el2 = driver.FindElement(By.Id("locationh"));
            var title = driver.Title;
            Assert.Equals(" - BakalarskaPrace1", title);
        }
        
    }
}