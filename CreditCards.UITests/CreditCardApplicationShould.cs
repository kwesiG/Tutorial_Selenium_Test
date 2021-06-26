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

        [Fact]
        public void BeSubmittedWhenValid()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl(EasyApplyUrl);

                //IWebElement firstNameField = driver.FindElement(By.Id("FirstName"));
                //firstNameField.Text = "Kwesi"; NOTE: Text on a textBox property cannot be set this way
                //firstNameField.SendKeys("Sarah");

                driver.FindElement(By.Id("FirstName")).SendKeys("Sarah"); //When used like this the returned value is void so does not need to be assigned
                DemoHelper.Pause();
                driver.FindElement(By.Id("LastName")).SendKeys("Smith");
                DemoHelper.Pause();
                driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys("123456-A");
                DemoHelper.Pause();
                driver.FindElement(By.Id("Age")).SendKeys("18");
                DemoHelper.Pause();
                driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys("50000");
                DemoHelper.Pause();
                //IWebElement singleRadioButton = driver.FindElement(By.Id("Single"));
                //singleRadioButton.Click();
                driver.FindElement(By.Id("Single")).Click();// Simplified method for clicking
                DemoHelper.Pause();
                IWebElement businessSourceSelectElement =
                    driver.FindElement(By.Id("BusinessSource"));
                SelectElement businessSource = new SelectElement(businessSourceSelectElement);

                // Check default selected option is correct
                Assert.Equal("I'd Rather Not Say", businessSource.SelectedOption.Text);
                // Get all the available options
                foreach (IWebElement option in businessSource.Options)
                {
                    output.WriteLine($"Value: {option.GetAttribute("value")} Text: {option.Text}"); // This is essentially a dictionary key value pair
                    // where the Attribute  is the Key and the value is the text
                }
                Assert.Equal(5, businessSource.Options.Count);

                // Select an option

                businessSource.SelectByValue("Email"); // By value or 'Key'
                DemoHelper.Pause();
                businessSource.SelectByText("Internet Search"); // By value shown to the user or 'value'
                DemoHelper.Pause();
                businessSource.SelectByIndex(4); // By index of the collection
                DemoHelper.Pause();

                driver.FindElement(By.Id("TermsAccepted")).Click();

                // 2 Ways to submit a form
                //driver.FindElement(By.Id("SubmitApplication")).Click(); // 1.
                driver.FindElement(By.Id("Single")).Submit(); // 2.
                DemoHelper.Pause();

                Assert.StartsWith("Application Complete", driver.Title);
                Assert.Equal("ReferredToHuman", driver.FindElement(By.Id("Decision")).Text);
                Assert.NotEmpty(driver.FindElement(By.Id("ReferenceNumber")).Text);
                Assert.Equal("Sarah Smith", driver.FindElement(By.Id("FullName")).Text);
                Assert.Equal("18", driver.FindElement(By.Id("Age")).Text);
                Assert.Equal("50000", driver.FindElement(By.Id("Income")).Text);
                Assert.Equal("Single", driver.FindElement(By.Id("RelationshipStatus")).Text);
                Assert.Equal("TV", driver.FindElement(By.Id("BusinessSource")).Text);
            }
        }

        [Fact]
        public void BeSubmittedWhenValidationErrorIsCorrected()
        {
            const string firstName = "Sarah";
            const string invalidAge = "17";
            const string validAge = "18";
            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl(EasyApplyUrl);

                driver.FindElement(By.Id("FirstName")).SendKeys(firstName); //When used like this the returned value is void so does not need to be assigned
                DemoHelper.Pause();
                // Do not enter a last name
                driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys("123456-A");
                DemoHelper.Pause();
                driver.FindElement(By.Id("Age")).SendKeys(invalidAge);
                DemoHelper.Pause();
                driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys("50000");
                DemoHelper.Pause();
                //IWebElement singleRadioButton = driver.FindElement(By.Id("Single"));
                //singleRadioButton.Click();
                driver.FindElement(By.Id("Single")).Click();// Simplified method for clicking
                DemoHelper.Pause();
                IWebElement businessSourceSelectElement =
                    driver.FindElement(By.Id("BusinessSource"));
                SelectElement businessSource = new SelectElement(businessSourceSelectElement);

                // Check default selected option is correct
                Assert.Equal("I'd Rather Not Say", businessSource.SelectedOption.Text);
                // Get all the available options
                foreach (IWebElement option in businessSource.Options)
                {
                    output.WriteLine($"Value: {option.GetAttribute("value")} Text: {option.Text}"); // This is essentially a dictionary key value pair
                    // where the Attribute  is the Key and the value is the text
                }
                Assert.Equal(5, businessSource.Options.Count);

                businessSource.SelectByValue("Email"); // By value or 'Key'
                DemoHelper.Pause();
                businessSource.SelectByText("Internet Search"); // By value shown to the user or 'value'
                DemoHelper.Pause();
                businessSource.SelectByIndex(4); // By index of the collection
                DemoHelper.Pause();
                driver.FindElement(By.Id("TermsAccepted")).Click();
                driver.FindElement(By.Id("Single")).Click();
                driver.FindElement(By.Id("SubmitApplication")).Click();

                // Assert that validation Failed
                var validationErrors =
                    driver.FindElements(By.CssSelector(".validation-summary-errors > ul > li"));
                Assert.Equal(2, validationErrors.Count);
                Assert.Equal("Please provide a last name", validationErrors[0].Text);
                Assert.Equal("You must be at least 18 years old", validationErrors[1].Text);

                // Fix Errors
                driver.FindElement(By.Id("LastName")).SendKeys("Smith");
                driver.FindElement(By.Id("Age")).Clear();
                driver.FindElement(By.Id("Age")).SendKeys(validAge);

                // Resudmit the form
                driver.FindElement(By.Id("SubmitApplication")).Click();
                DemoHelper.Pause();

                Assert.StartsWith("Application Complete", driver.Title);
                Assert.Equal("ReferredToHuman", driver.FindElement(By.Id("Decision")).Text);
                Assert.NotEmpty(driver.FindElement(By.Id("ReferenceNumber")).Text);
                Assert.Equal("Sarah Smith", driver.FindElement(By.Id("FullName")).Text);
                Assert.Equal("18", driver.FindElement(By.Id("Age")).Text);
                Assert.Equal("50000", driver.FindElement(By.Id("Income")).Text);
                Assert.Equal("Single", driver.FindElement(By.Id("RelationshipStatus")).Text);
                Assert.Equal("TV", driver.FindElement(By.Id("BusinessSource")).Text);
            }
        }
    }
}
