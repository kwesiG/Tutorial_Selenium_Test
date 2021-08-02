using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CreditCards.UITests.PageObjectModels
{
    class ApplicationPage : Page
    {
        protected override string PageUrl => "http://localhost:44108/Apply";
        protected override string PageTitle => "Credit Card Application - Credit Cards";

        public ApplicationPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public ReadOnlyCollection<string> ValidationErrorMessages
        {
            get
            {
                return Driver.FindElements
                    (By.CssSelector(".validation-summary-errors > ul > li"))
                    .Select(x => x.Text)
                    .ToList().AsReadOnly();
            }
        }

        public void ClearAge() => Driver.FindElement(By.Id("Age")).Clear();
        public void EnterFirstName(string firstName) => Driver.FindElement(By.Id("FirstName")).SendKeys(firstName);

        public void EnterLastName(string lastName) => Driver.FindElement(By.Id("LastName")).SendKeys(lastName);

        public void EnterFrequentFlyerNumber(string number) => Driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys(number);

        public void EnterAge(string age) => Driver.FindElement(By.Id("Age")).SendKeys(age);

        public void EnterGrossAnnualIncome(string income) => Driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys(income);

        public void ChooseMaritalStatusSingle() => Driver.FindElement(By.Id("Single")).Click();
        public void AcceptTermsAndConditions() => Driver.FindElement(By.Id("TermsAccepted")).Click();

        public void ChooseBusinessSourceTV()
        { 
            IWebElement businessSourceSelectElement = Driver.FindElement(By.Id("BusinessSource"));
            SelectElement businessSource = new SelectElement(businessSourceSelectElement);
            businessSource.SelectByValue("TV");
        }

        public ApplicationCompletePage SubmitApplication()
        {
            Driver.FindElement(By.Id("SubmitApplication")).Click();
            return new ApplicationCompletePage(Driver);
        }

        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl(PageUrl);
            EnsurePageLoaded();
        }

        public void EnsurePageLoaded()
        {
            bool pageHasLoaded = ((Driver.Url == PageUrl) && (Driver.Title == PageTitle));

            if (!pageHasLoaded)
            {
                throw new Exception($"Failed to load page. Page URL = '{Driver.Url}' Page Source : \r\n {Driver.PageSource}");
            }
        }
    }
}
