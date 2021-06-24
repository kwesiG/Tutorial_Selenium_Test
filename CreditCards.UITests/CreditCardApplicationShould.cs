using System;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

namespace CreditCards.UITests
{
    [Trait("Category", "Aplications")]
    public class CreditCardApplicationShould
    {
        private const string HomeUrl = "http://localhost:44108/";
        private const string AboutUrl = "http://localhost:44108/Home/About";
        private const string ApplyUrl = "http://localhost:44108/Home/Apply";
        private const string EasyApplyUrl = "http://localhost:44108/Apply";

        private readonly ITestOutputHelper output;

        public CreditCardApplicationShould(ITestOutputHelper output)
        {
            this.output = output;
        }

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

                //IWebElement carouselNext =
                //    driver.FindElement(By.CssSelector("[data-slide='next']"));
                //carouselNext.Click();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(11));
                IWebElement applyLink =
                    wait.Until((d) => d.FindElement(By.LinkText("Easy: Apply Now!")));
                applyLink.Click(); 

                //IWebElement applyLink = driver.FindElement(By.LinkText("Easy: Apply Now!"));
                //applyLink.Click();

                DemoHelper.Pause();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(EasyApplyUrl, driver.Url);

            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_EasyApplication_Prebuilt_Condiotions()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(11));
                IWebElement applyLink =
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.PartialLinkText("- Apply Now!")));
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

        [Fact]
        public void BeInitiatedFromHomePage_CustomerService_ImplicitWait_Fails()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Setting implicit wait");
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(12);

                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Navigating to '{HomeUrl}'");
                driver.Navigate().GoToUrl(HomeUrl);

                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Finding Element");
                IWebElement AppylLink = driver.FindElement(By.ClassName("customer-service-apply-now"));

                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Found element Displayed={AppylLink.Displayed} Enabled={AppylLink.Enabled}");
                AppylLink.Click();
                DemoHelper.Pause();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(EasyApplyUrl, driver.Url);
                //TODO: Fix later
            }
        }

        [Fact]
        public void BeInitiatedFromHomepage()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                IWebElement randomGreetingApplyLink =
                    driver.FindElement(By.PartialLinkText("- Apply Now!"));
                randomGreetingApplyLink.Click();

                DemoHelper.Pause();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(EasyApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomepage_RandomGreeting_Using_XPATH()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                IWebElement randomGreetingApplyLink =
                    driver.FindElement(By.XPath("//a[text()[contains(.,'- Apply Now!')]]"));
                randomGreetingApplyLink.Click();
                DemoHelper.Pause();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(EasyApplyUrl, driver.Url);
            }
        }
    }
}
