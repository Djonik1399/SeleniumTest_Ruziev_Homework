using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTests
{
public class FirstSeleniumTests
{
    
[SetUp]
public void SetUp()
{ 
    var options = new ChromeOptions();
options.AddArgument("start-maximized");
driver = new ChromeDriver(options);

}

private ChromeDriver driver;


private string urlCreateNameParrot = "https://qa-course.kontur.host/selenium-practice/"; 

private string email = "test@mail.ru"; // Корректный e-mail

private string expectedResultTextBoy = "Хорошо, мы пришлём имя для вашего мальчика на e-mail:";

private string expectedResultTextGirl = "Хорошо, мы пришлём имя для вашей девочки на e-mail:";

private By emailInputLocator = By.Name("email"); // Локатор поля ввода почты
private By buttonPickUpNameLocator = By.Id("sendMe"); // Локатор кнопки "Подобрать имя"
private By resultTextLocator = By.ClassName("result-text"); // Локатор итогового текста 
private By linkAnotherEmailLocator = By.LinkText("указать другой e-mail"); // Локатор ссылки на другой e-mail
private By expectedEmailLocator = By.ClassName("your-email"); // Локатор ожидаемого текста
private By RadioButtonGirlLocator = By.Id("girl");
private By RadioButtonBoyLocator = By.Id("boy");
private By TextForErrorLocator = By.ClassName("form-error");
private string ErrorTextLocator = "Некорректный email";

[Test]
public void CreateNameBoy_SendEmail_Success()
{
    driver.Navigate().GoToUrl(urlCreateNameParrot);
    driver.FindElement(emailInputLocator).SendKeys(email);
    driver.FindElement(buttonPickUpNameLocator).Click();
    Assert.Multiple(() =>
    {
        Assert.IsTrue(driver.FindElement(resultTextLocator).Displayed,
            "Сообщение об успехе создания заявки отображается");
        Assert.AreEqual(expectedResultTextBoy, driver.FindElement(resultTextLocator).Text,
            "Неверное сообщение об успехе создания заявки");
        Assert.IsTrue(driver.FindElement(expectedEmailLocator).Displayed,
            "Отображается почта, которую вводил пользователь");
        Assert.AreEqual(email,driver.FindElement(expectedEmailLocator).Text, 
            "Неверный email, на которую будем отвечать");
        Assert.IsTrue(driver.FindElement(linkAnotherEmailLocator).Displayed,
            "Ссылка 'Указать другой email' отображается");
        Assert.AreEqual(email,driver.FindElement(emailInputLocator).GetAttribute("value"),
            "Поле не сохранило данные");
    });
    
}

[Test]
public void CreateNameGirl_SendMail_Success()
{
    
    driver.Navigate().GoToUrl(urlCreateNameParrot);
    driver.FindElement(RadioButtonGirlLocator).Click();
    driver.FindElement(emailInputLocator).SendKeys(email);
    driver.FindElement(buttonPickUpNameLocator).Click();
    Assert.Multiple(() =>
    {
        Assert.IsTrue(driver.FindElement(resultTextLocator).Displayed,
            "Сообщение об успехе создания заявки отображается");
        Assert.AreEqual(expectedResultTextGirl, driver.FindElement(resultTextLocator).Text,
            "Неверное сообщение об успехе создания заявки");
        Assert.IsTrue(driver.FindElement(expectedEmailLocator).Displayed, "Отображается почта, которую вводил пользователь");
        Assert.AreEqual(email, driver.FindElement(expectedEmailLocator).Text,
            "Неверный email, на которую будем отвечать");
        Assert.IsTrue(driver.FindElement(linkAnotherEmailLocator).Displayed,
            "Ссылка 'Указать другой email' отображается");
        Assert.AreEqual(email,driver.FindElement(emailInputLocator).GetAttribute("value"),
            "Поле не сохранило данные");
        
        
    });
   
}

[Test]
public void CreateName_ClickLinkAnotherEmail_Success()
{
    driver.Navigate().GoToUrl(urlCreateNameParrot);
    driver.FindElement(emailInputLocator).SendKeys(email);
    driver.FindElement(buttonPickUpNameLocator).Click();
    driver.FindElement(linkAnotherEmailLocator).Click();
    Assert.Multiple(() =>
    {
        Assert.IsEmpty(driver.FindElement(emailInputLocator).Text, 
            "Ожидали что поле для ввода email очистится");
        Assert.IsTrue(driver.FindElement(buttonPickUpNameLocator).Displayed,
            "Ожидали, что кнопка 'Подобрать имя' отображается");
        Assert.AreEqual(0, driver.FindElements(linkAnotherEmailLocator).Count,
            "Ожидали, что исчезнет ссылка 'Указать другой email'");
        
    });
    

}

[Test]
public void CreateName_InputEmailRefresh_EmptyField()
{
    driver.Navigate().GoToUrl(urlCreateNameParrot);
    driver.FindElement(emailInputLocator).SendKeys(email);
    driver.FindElement(RadioButtonGirlLocator).Click();
    driver.Navigate().Refresh();
    Assert.Multiple(() =>
    {   
        Assert.IsEmpty(driver.FindElement(emailInputLocator).Text, 
            "Ожидали что поле для ввода email очистится");
        Assert.IsTrue(driver.FindElement(RadioButtonBoyLocator).Selected, 
            "Ожидали, что радиокнопка переключится на 'мальчика'");
    });
    
}

[Test]
public void CreateName_InputEmailBackForward_DataSave()
{
    driver.Navigate().GoToUrl(urlCreateNameParrot);
    driver.FindElement(RadioButtonGirlLocator).Click();
    driver.FindElement(emailInputLocator).SendKeys(email);
    driver.Navigate().Back();
    driver.Navigate().Forward();
    Assert.Multiple(() =>
    {
        Assert.AreEqual(email,driver.FindElement(emailInputLocator).GetAttribute("value"),
            "Поле не сохранило данные");
        Assert.IsTrue(driver.FindElement(RadioButtonGirlLocator).Selected, 
            "Ожидали, что радиокнопка выбрана на 'девочку'");
    });
    
    
}

[Test]
public void CreateName_EmptyInputEmail_ErrorMessage()
{
    driver.Navigate().GoToUrl(urlCreateNameParrot);
    driver.FindElement(buttonPickUpNameLocator).Click();
    Assert.Multiple(() =>
    {
        Assert.AreEqual("Введите email",driver.FindElement(TextForErrorLocator).Text, "Обязательное поле");
        Assert.AreEqual(0, driver.FindElements(linkAnotherEmailLocator).Count,
            "Ожидали, что не появится ссылка 'Указать другой email'");
    });
   
}

[Test]
public void CreateName_IncorrectEmailNotExistDomain_ErrorMessage()
{
    driver.Navigate().GoToUrl(urlCreateNameParrot);
    var incorrectemail = "test@gmail.";
    driver.FindElement(emailInputLocator).SendKeys(incorrectemail);
    driver.FindElement(buttonPickUpNameLocator).Click();
    Assert.Multiple(() =>
    {
        Assert.AreEqual(ErrorTextLocator,driver.FindElement(TextForErrorLocator).Text,
            "Не введен корректный домен");
        Assert.AreEqual(incorrectemail,driver.FindElement(emailInputLocator).GetAttribute("value"),
            "Поле не сохранило данные");
    });
}

[Test]
public void CreateName_EmailLimitExceeded_ErrorMessage()
{
    driver.Navigate().GoToUrl(urlCreateNameParrot);
    var incorrectemail = "yourchoiceisyourresponsibilityyourchoiceisyourresponsibilityyourchoiceisyourresponsibilityyourchoiceisyourresponsibilityyourchoiceisyourresponsibilityyourchoiceisyourresponsibilityyourchoiceisyourresponsibilityyourchoiceisyourresponsibilityyourchoiceis@test.com";
    driver.FindElement(emailInputLocator).SendKeys(incorrectemail);
    driver.FindElement(buttonPickUpNameLocator).Click();
    Assert.Multiple(() =>
    {
        Assert.AreEqual(ErrorTextLocator,driver.FindElement(TextForErrorLocator).Text,
            "Некорректный адрес электронной почты Длина не должна превышать 255 символов");
        Assert.AreEqual(incorrectemail,driver.FindElement(emailInputLocator).GetAttribute("value"),
            "Поле не сохранило данные");
    });
}


    
[TearDown]
public void TearDown()
{
driver.Quit();
}
}
}