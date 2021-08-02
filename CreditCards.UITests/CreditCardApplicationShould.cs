using System;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;
using CreditCards.UITests.PageObjectModels;

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
                var homePage = new HomePage(driver);
                homePage.NavigateTo();


                ApplicationPage applicationPage = homePage.ClickApplyLowRateLink();

                applicationPage.EnsurePageLoaded();
            };
        }

        [Fact]
        public void BeInitiatedFromHomePage_EasyApplication()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                // driver.Manage().Window.Minimize(); Demonstrates some browser automation brittleness. This line would make the test fail.
                driver.Navigate().GoToUrl(HomeUrl);
                driver.Manage().Window.Minimize(); //  By adding this the browser is minimised from view so the mouse pointer hovering no longer affects the carousel
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
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2000);

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

        [Fact]
        public void BeSubmittedWhenValid()
        {
            const string FirstName = "Sarah";
            const string LastName = "Smith";
            const string FrequentFlyerNumber = "123456-A";
            const string Age = "18";
            const string Income = "50000";

            using (IWebDriver driver = new FirefoxDriver())
            {
                var applicationPage = new ApplicationPage(driver);
                applicationPage.NavigateTo();

                applicationPage.EnterFirstName(FirstName);
                applicationPage.EnterLastName(LastName);
                applicationPage.EnterFrequentFlyerNumber(FrequentFlyerNumber);
                applicationPage.EnterAge(Age);
                applicationPage.EnterGrossAnnualIncome(Income);
                applicationPage.ChooseMaritalStatusSingle();
                applicationPage.ChooseBusinessSourceTV();
                applicationPage.AcceptTermsAndConditions();
                ApplicationCompletePage applicationCompletePage =
                    applicationPage.SubmitApplication();

                applicationCompletePage.EnsurePageLoaded();


                Assert.Equal("ReferredToHuman", applicationCompletePage.Decision);
                Assert.NotEmpty(applicationCompletePage.ReferenceNumber);
                Assert.Equal($"{FirstName} {LastName}", applicationCompletePage.FullName);
                Assert.Equal(Age, applicationCompletePage.Age);
                Assert.Equal(Income, applicationCompletePage.Income);
                Assert.Equal("Single", applicationCompletePage.RelationshipStatus);
                Assert.Equal("TV", applicationCompletePage.BusinessSource);
            }
        }

        [Fact]
        public void BeSubmittedWhenValidationErrorIsCorrected()
        {
            const string firstName = "Sarah";
            const string lastName = "Smith";
            const string invalidAge = "17";
            const string validAge = "18";

            using (IWebDriver driver = new FirefoxDriver())
            {
                var applicationPage = new ApplicationPage(driver);
                applicationPage.NavigateTo();

                applicationPage.EnterFirstName(firstName);
                // Do not enter lastname
                applicationPage.EnterFrequentFlyerNumber("123456-A");
                applicationPage.EnterAge(invalidAge);
                applicationPage.EnterGrossAnnualIncome("50000");
                applicationPage.ChooseMaritalStatusSingle();
                applicationPage.ChooseBusinessSourceTV();
                applicationPage.AcceptTermsAndConditions();
                applicationPage.SubmitApplication();

                driver.FindElement(By.Id("TermsAccepted")).Click();
                driver.FindElement(By.Id("Single")).Click();
                driver.FindElement(By.Id("SubmitApplication")).Click();

                // Assert that validation Failed
                Assert.Equal(3, applicationPage.ValidationErrorMessages.Count);
                Assert.Equal("Please provide a last name", applicationPage.ValidationErrorMessages[0]);
                Assert.Equal("You must be at least 18 years old", applicationPage.ValidationErrorMessages[1]);

                // Fix Errors
                applicationPage.EnterLastName(lastName);
                applicationPage.ClearAge();
                applicationPage.EnterAge(validAge);
                driver.FindElement(By.Id("TermsAccepted")).Click();

                // Resudmit the form
                ApplicationCompletePage applicationCompletePage = applicationPage.SubmitApplication();

                // Check form submitted
                applicationCompletePage.EnsurePageLoaded();
            }
        }
    }
}
