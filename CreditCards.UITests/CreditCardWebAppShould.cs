using System;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Support.UI;
using ApprovalTests.Reporters.Windows;
using ApprovalTests.Reporters;
using System.IO;
using ApprovalTests;
using CreditCards.UITests.PageObjectModels;

namespace CreditCards.UITests
{
    public class CreditCardWebAppShould
    {
        private const string AboutUrl = "http://localhost:44108/Home/About";
        private const string HomeUrl = "http://localhost:44108/";
        private const string HomeTitle = "Home Page - Credit Cards";
        private const string ContactUrl = "http://localhost:44108/Home/Contact";

        [Fact]
        [Trait("Category", "Smoke")]
        public void LoadApplicationPage()
        {

            using(IWebDriver driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                Assert.Equal(HomeTitle, driver.Title);
                Assert.Equal(HomeUrl, driver.Url);
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        public void LoadHomePage()
        {

            using (IWebDriver driver = new FirefoxDriver())
            {
                var homePage = new HomePage(driver);
                homePage.NavigateTo();
            }
        }

        [Fact]
        [Trait("Category","Smoke")]
        public void ReloadHomePageOnBack()
        {

            using (IWebDriver driver = new FirefoxDriver())
            {
                var homePage = new HomePage(driver);
                homePage.NavigateTo();

                string initialToken = homePage.GenerationToken;

                driver.Navigate().GoToUrl(AboutUrl);
                driver.Navigate().Back();

                homePage.EnsurePageLoaded();

                string reloadedToken = homePage.GenerationToken;

                Assert.NotEqual(initialToken, reloadedToken);
            }
        }

        [Fact]
        [Trait("Category","Smoke")]
        public void ReloadHomePageOnForward()
        {
            using(IWebDriver driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl(AboutUrl);
                DemoHelper.Pause();

                driver.Navigate().GoToUrl(HomeUrl);
                IWebElement generationTokenElement =
                    driver.FindElement((By.Id("GenerationToken")));
                string initialToken = generationTokenElement.Text;
                DemoHelper.Pause();

                driver.Navigate().Back();
                DemoHelper.Pause();

                driver.Navigate().Forward();
                DemoHelper.Pause();

                Assert.Equal(HomeTitle, driver.Title);
                Assert.Equal(HomeUrl, driver.Url);

                string reloadedToken = driver.FindElement(By.Id("GenerationToken")).Text;

                Assert.NotEqual(initialToken, reloadedToken);
            }
        }

        [Fact]
        public void DisplayProductsAndRates()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                var homePage = new HomePage(driver);
                homePage.NavigateTo();

                DemoHelper.Pause();

                IWebElement firstTableCell = driver.FindElement(By.TagName("td"));
                string firstProduct = firstTableCell.Text;

                Assert.Equal("Easy Credit Card", firstProduct);
                

                //TODO: Check the rest of the tables
            }
        }

        [Fact]
        public void DisplayProductsAndRates_MultipleResults()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                var homePage = new HomePage(driver);
                homePage.NavigateTo();

                DemoHelper.Pause();


                Assert.Equal("Easy Credit Card", homePage.Products[0].name);
                Assert.Equal("20% APR", homePage.Products[0].interestRate);

                Assert.Equal("Silver Credit Card", homePage.Products[1].name);
                Assert.Equal("18% APR", homePage.Products[1].interestRate);

                Assert.Equal("Gold Credit Card", homePage.Products[2].name);
                Assert.Equal("17% APR", homePage.Products[2].interestRate);
            }
        }

        [Fact]
        public void OpenContactFooterLinkInNewTab()
        {
            using(IWebDriver driver = new FirefoxDriver())
            {
                var homePage = new HomePage(driver);
                homePage.NavigateTo();

                homePage.ClickContacFooterLink();


                ReadOnlyCollection<string> allTabs = driver.WindowHandles;
                string homePageTab = allTabs[0];
                string contactTab = allTabs[1];

                driver.SwitchTo().Window(contactTab);

                Assert.Equal(ContactUrl, driver.Url);
            }
        }

        [Fact]
        public void AlertIfLiveChatClosed()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                var homePage = new HomePage(driver);
                homePage.NavigateTo();

                homePage.ClickLiveChatFooterLink();

                WebDriverWait wait = 
                    new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());

                Assert.Equal("Live chat is currently closed.", alert.Text);

                alert.Accept();
                DemoHelper.Pause();
            }
        }

        [Fact]
        public void NotNavigateToAboutUsWhenCancelledClicked()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                var homePage = new HomePage(driver);
                homePage.NavigateTo();

                homePage.ClickLearnAboutUsLink();

                WebDriverWait wait =
                    new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());

                alert.Dismiss();

                homePage.EnsurePageLoaded();
            }
        }

        [Fact]
        public void NotNavigateToAboutUsWhenOkClicked()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                var homePage = new HomePage(driver);
                homePage.NavigateTo();

                homePage.ClickLearnAboutUsLink();

                WebDriverWait wait =
                    new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());

                alert.Accept();

                Assert.EndsWith("Home/About", driver.Url);
            }
        }

        [Fact]
        public void NotDisplayCookieUseMessage()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                var homePage = new HomePage(driver);
                homePage.NavigateTo();

                driver.Manage().Cookies.AddCookie(new Cookie("acceptedCookies", "true"));
                driver.Navigate().Refresh();

                ReadOnlyCollection<IWebElement> message = 
                    driver.FindElements(By.Id("CookiesBeingUsed"));

                Assert.Empty(message);// This is because the FindElements method will not throw an exception if it finds no elements it will just return an empty collection

                Cookie cookieValue = driver.Manage().Cookies.GetCookieNamed("acceptedCookies");
                Assert.Equal("true", cookieValue.Value);

                driver.Manage().Cookies.DeleteCookieNamed("acceptedCookies");
                driver.Navigate().Refresh();
                DemoHelper.Pause(3);
                Assert.NotNull(driver.FindElement(By.Id("CookiesBeingUsed")));
            }
        }

        [Fact]
        [UseReporter(typeof(BeyondCompareReporter))]
        public void RenderAboutPage()
        {
            using(IWebDriver driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl(AboutUrl);

                ITakesScreenshot screenShotDriver = (ITakesScreenshot)driver;

                Screenshot screenshot = screenShotDriver.GetScreenshot();

                screenshot.SaveAsFile("aboutpage.bmp", ScreenshotImageFormat.Bmp);

                FileInfo file = new FileInfo("aboutpage.bmp");

                Approvals.Verify(file);
            }
        }
    }
}
