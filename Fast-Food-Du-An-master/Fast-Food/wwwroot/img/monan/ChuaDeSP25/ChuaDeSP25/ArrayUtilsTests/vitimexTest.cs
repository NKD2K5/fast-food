using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace VitimexTest
{
    public class VitimexTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {

            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void TestRegisterAndLogin()
        {
            driver.Navigate().GoToUrl("https://vitimex.com.vn/");


            wait.Until(d => d.FindElement(By.LinkText("Đăng ký"))).Click();


            wait.Until(d => d.FindElement(By.Id("firstName"))).SendKeys("Nguyen");
            driver.FindElement(By.Id("lastName")).SendKeys("Trang");
            string email = "test" + DateTime.Now.Ticks + "@gmail.com";
            driver.FindElement(By.Id("email")).SendKeys(email);
            driver.FindElement(By.Id("password")).SendKeys("Trang@1234");
            driver.FindElement(By.Id("confirmPassword")).SendKeys("Trang@1234");


            driver.FindElement(By.Id("registerButton")).Click();


            wait.Until(d => d.Url.Contains("account"));
            Assert.IsTrue(driver.Url.Contains("account"));


            wait.Until(d => d.FindElement(By.LinkText("Đăng xuất"))).Click();


            wait.Until(d => d.FindElement(By.LinkText("Đăng nhập"))).Click();


            driver.FindElement(By.Id("email")).SendKeys(email);
            driver.FindElement(By.Id("password")).SendKeys("Trang@1234");
            driver.FindElement(By.Id("loginButton")).Click();


            wait.Until(d => d.Url.Contains("account"));
            Assert.IsTrue(driver.Url.Contains("account"));
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}
