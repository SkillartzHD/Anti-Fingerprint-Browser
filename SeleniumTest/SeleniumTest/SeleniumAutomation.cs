using OpenQA.Selenium.Chrome;
using static SeleniumTest.api_data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDSE.ScreenshotMaker;
using WDSE.Decorators;
using WDSE;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium;
using System.Threading;
using System.IO;
using OpenQA.Selenium.DevTools.V87.Emulation;
using OpenQA.Selenium.DevTools;

namespace SeleniumTest
{
    internal class SeleniumAutomation
    {

        public static void RunTest(int index)
        {
            if (Serverdown == true)
                RunTest1(index);
            else
                Console.WriteLine("Test fail " + index);
            Console.ReadLine();
        }

        private static void RunTest1(int index)
        {
            try
            {
                ChromeOptions chromeOptions = new ChromeOptions();
                ChromeDriverService service = ChromeDriverService.CreateDefaultService(@"driver", "chromedriver.exe");

                string dir_name = ProfilName[index]
                    .Replace("--user-data-dir=", null)
                    .Replace("\n", null)
                    .Replace("\"", null);

                chromeOptions.AddArgument(FontList[index]);
                chromeOptions.AddArgument(FingerprintList[index]);
                chromeOptions.AddArgument(UserAgentList[index]);
                chromeOptions.AddArgument(@"Profile\" + ProfilName[index]);

                chromeOptions.AddArgument("--disable-extensions");
                chromeOptions.AddArgument("--disable-gpu");
                chromeOptions.AddArgument("--disable-notifications");
                chromeOptions.AddAdditionalChromeOption("useAutomationExtension", false);
                chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.geolocation", 1);

                ChromeDriver driver = new ChromeDriver(service, chromeOptions);

                try
                {
                    int count = 0;

                    if (!Directory.Exists(dir_name))
                    {
                        Directory.CreateDirectory(dir_name);
                    }

                    foreach (var test in Test)
                    {
                        driver.Navigate().GoToUrl(test);

                        if (test.Contains("openwpm"))
                        {
                            List<IWebElement> click_bttn = new List<IWebElement>();

                            click_bttn.AddRange(driver.FindElements(By.Id("fp_button")));
                            if (click_bttn.Count > 0)
                            {
                                click_bttn[0].Click();
                            }
                        }
                        if (test.Contains("/demo/"))
                            Thread.Sleep(10000);
                        else
                            Thread.Sleep(4000);

                        VerticalCombineDecorator vcd = new VerticalCombineDecorator(new ScreenshotMaker());
                        driver.TakeScreenshot(vcd).ToMagickImage().Write(dir_name + "\\test_" + count + ".png", ImageMagick.MagickFormat.Png);
                        count++;
                    }
                    driver.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    driver.Dispose();
                    RunTest(index);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
