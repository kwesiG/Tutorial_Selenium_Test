
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CreditCards.UITests.PageObjectModels
{
    class HomePage : Page
    {

        protected override string PageUrl => "http://localhost:44108/";
        protected override string PageTitle => "Home Page - Credit Cards";


        public string HomUrl
        {
            get
            {
               return PageUrl ;
            } 
        }

        public HomePage(IWebDriver driver)
        {
            Driver = driver;
        }

        public ReadOnlyCollection<(string name, string interestRate)> Products
        { 
            get
            {
                var products = new List<(string name, string interestRate)>();

                var productCells = Driver.FindElements(By.TagName("td"));

                for (int i = 0; i < productCells.Count -1; i += 2)
                {
                    string name = productCells[i].Text;
                    string interestRate = productCells[i + 1].Text;
                    products.Add((name, interestRate));
                }
                return products.AsReadOnly();
            }
        }

        public string GenerationToken => Driver.FindElement(By.Id("GenerationToken")).Text;
        public void ClickContacFooterLink() => Driver.FindElement(By.Id("ContactFooter")).Click();
        public void ClickLiveChatFooterLink() => Driver.FindElement(By.Id("LiveChat")).Click();
        public void ClickLearnAboutUsLink() => Driver.FindElement(By.Id("LearnAboutUs")).Click();
        public ApplicationPage ClickApplyLowRateLink()
        {
            Driver.FindElement(By.Name("ApplyLowRate")).Click();
            return new ApplicationPage(Driver);
        }
        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl(PageUrl);
            EnsurePageLoaded();
        }

        public void EnsurePageLoaded()
        {
            bool pageHasLoaded = ((Driver.Url == PageUrl) && (Driver.Title == PageTitle));

            if(!pageHasLoaded)
            {
                throw new Exception($"Failed to load page. Page URL = '{Driver.Url}' Page Source : \r\n {Driver.PageSource}");
            }
        }
    }
}
