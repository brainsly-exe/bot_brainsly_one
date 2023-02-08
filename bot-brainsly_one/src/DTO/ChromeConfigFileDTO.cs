using OpenQA.Selenium.DevTools.V106.Profiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot_brainsly_one.src.DTO
{
    public class ChromeConfigFileDTO
    {
        public ProfileConfig profile {get; set;}
    }

    public class ProfileConfig
    {
        public string name { get; set; }
    }
}
