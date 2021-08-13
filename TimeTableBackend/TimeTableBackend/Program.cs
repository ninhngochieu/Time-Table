using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Edge.SeleniumTools;
using Microsoft.Extensions.Hosting;
using TimeTableBackend.Models;

namespace TimeTableBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            RunSelenium();

        }

        private static void RunSelenium()
        {
            Context context = new Context();
            var options = new EdgeOptions();
            options.UseChromium = true;
            var driver = new EdgeDriver(options);
            driver.Url = "http://thongtindaotao.sgu.edu.vn/";
            driver.Navigate();

            var IsLogin = driver.FindElementById("ctl00_Header1_Logout1_lbtnLogOut").Text.Contains("Đăng Nhập");
            Console.WriteLine(IsLogin);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
