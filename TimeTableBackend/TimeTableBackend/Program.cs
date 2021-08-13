using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Edge.SeleniumTools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TimeTableBackend.Models;

namespace TimeTableBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RunSelenium();
            CreateHostBuilder(args).Build().Run();

        }

        private static void RunSelenium()
        {
            Context context = new Context();
            var options = new EdgeOptions();
            options.UseChromium = true;
            var driver = new EdgeDriver(options);
            driver.Url = "http://thongtindaotao.sgu.edu.vn/";
            driver.Navigate();

            var IsLogin = checkLogin(driver);
            if (!IsLogin)
            {
                DoLogin(driver);
            }

            Console.WriteLine(getNienKhoa(driver, context));
        }

        private static NienKhoa getNienKhoa(EdgeDriver driver, Context context)
        {
            var xemTkbInput = driver.FindElementById("ctl00_menu_lblThoiKhoaBieu");
            xemTkbInput.Click();
            var nienKhoaInput = driver.FindElementByXPath("//*[@id=\"ctl00_ContentPlaceHolder1_ctl00_ddlChonNHHK\"]/option[1]");
            var hocKy = Regex.Match(nienKhoaInput.Text, @"\d+").Value;
            var nienKhoa = Regex.Match(nienKhoaInput.Text, @"20\d\d-20\d\d").Value;

            if(hocKy is null || nienKhoa is null)
            {
                throw new NullReferenceException("Khong lay duoc thong tin TKB");
            }

            NienKhoa nienKhoaDb = context.NienKhoas.Where(h=>h.HocKy == hocKy).Where(n=>n.NamHoc == nienKhoa).FirstOrDefault();
            if (nienKhoaDb is null)
            {
                nienKhoaDb = createNewNienKhoa(context,hocKy,nienKhoa);   
            }

            return nienKhoaDb;
        }

        private static NienKhoa createNewNienKhoa(Context context, string hocKy, string nienKhoa)
        {
            NienKhoa nienKhoaDb = new NienKhoa { };
            if(int.Parse(hocKy) == 3)
            {
                string[] nam = nienKhoa.Split("-");
                string namBatDau = (int.Parse(nam[0]) + 1).ToString();
                string namKetThuc = (int.Parse(nam[1]) + 1).ToString();
                string niemKhoaMoi = namBatDau + "-" + namKetThuc;

                nienKhoaDb.HocKy = "1";
                nienKhoaDb.NamHoc = niemKhoaMoi;
            }
            else
            {
                string hocKyMoi = (int.Parse(hocKy) + 1).ToString();
                nienKhoaDb.HocKy = hocKyMoi;
                nienKhoaDb.NamHoc = nienKhoa;
            }
            context.NienKhoas.Add(nienKhoaDb);
            if(context.SaveChanges() == 0)
            {
                throw new DbUpdateConcurrencyException("Co loi khi luu doi tuong xuong database");
            }
            return nienKhoaDb;
        }

        private static bool checkLogin(EdgeDriver driver)
        {
            return !driver.FindElementById("ctl00_Header1_Logout1_lbtnLogOut").Text.Contains("Đăng Nhập");
        }

        private static bool DoLogin(EdgeDriver driver)
        {
            var usernameInput = driver.FindElementById("ctl00_ContentPlaceHolder1_ctl00_ucDangNhap_txtTaiKhoa");
            usernameInput.SendKeys("3118410087");
            var passwordInput = driver.FindElementById("ctl00_ContentPlaceHolder1_ctl00_ucDangNhap_txtMatKhau");
            passwordInput.SendKeys("Anhducjav123");
            var button = driver.FindElementById("ctl00_ContentPlaceHolder1_ctl00_ucDangNhap_btnDangNhap");
            button.Click();
            if (!checkLogin(driver))
            {
                throw new UnauthorizedAccessException("Login that bai");
            }
            return true;

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
