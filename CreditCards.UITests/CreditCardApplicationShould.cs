using System;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;

namespace CreditCards.UITests
{
    [Trait("Category","Aplications")]
    public class CreditCardApplicationShould
    {
        private const string HomeUrl = "http://localhost:44108/";
        private const string AboutUrl = "http://localhost:44108/Home/About";
        private const string ApplyUrl = "http://localhost:44108/Home/Apply";
        private const string EasyApplyUrl = "http://localhost:44108/Apply";

        [Fact]
        public void BeInitiatedFromHomePage_NewLowRate()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                IWebElement applyLink = driver.FindElement(By.Name("ApplyLowRate"));
                applyLink.Click();

                DemoHelper.Pause();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(EasyApplyUrl, driver.Url);
           };
        }

        [Fact]
        public void BeInitiatedFromHomePage_EasyApplication()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                IWebElement carouselNext = 
                    driver.FindElement(By.CssSelector("[data-slide='next']"));
                carouselNext.Click();
                DemoHelper.Pause(1000);// Allow carousel to scroll

                IWebElement applyLink = driver.FindElement(By.LinkText("Easy: Apply Now!"));
                applyLink.Click();

                DemoHelper.Pause();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(EasyApplyUrl, driver.Url);

            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_CustomerService()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                IWebElement carouselNext =
                    driver.FindElement(By.CssSelector("[data-slide='next']"));
                carouselNext.Click();
                DemoHelper.Pause(1000);// allow carousel time to scroll
                carouselNext.Click();
                DemoHelper.Pause(1000);// allow carousel time to scroll

                IWebElement AppylLink = driver.FindElement(By.ClassName("customer-service-apply-now"));
                AppylLink.Click();
                DemoHelper.Pause();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(EasyApplyUrl, driver.Url);
            }
        }
    }
}
