﻿using OpenQA.Selenium;

namespace bot_brainsly_one.src.utils
{
    public static class WebDriverExtensionsUtils
    {
        public static bool TryFindElement(this IWebDriver driver, By by, out IWebElement element)
        {
            try
            {
                element = driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                element = null;
                return false;
            }
        }

        public static bool IsElementEnabled(this IWebDriver driver, By by)
        {
            IWebElement element = null;

            if (driver.TryFindElement(by, out element))
            {
                return element.Enabled;
            }

            return false;
        }
    }
}
