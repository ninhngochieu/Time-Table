using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Edge.SeleniumTools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TimeTableBackend.Models;

namespace TimeTableBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //RunSelenium();
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

            NienKhoa nienKhoaDb = getNienKhoa(driver, context);
            getDanhSachMonHoc(nienKhoaDb, context,driver);
        }

        private static void getDanhSachMonHoc(NienKhoa nienKhoaDb, Context context, EdgeDriver driver)
        {
            var dkmhSpan = driver.FindElementById("ctl00_menu_lblDangKyMonHoc");
            dkmhSpan.Click();
            var chonKhoaParent = driver.FindElementById("divDanhSachDieuKienLoc");
            var IsVisible = chonKhoaParent.Displayed;
            while (!IsVisible)
            {
                IsVisible = chonKhoaParent.Displayed;
            }
            var chonKhoa = driver.FindElementByXPath("//*[@id=\"selectDKLoc\"]/option[2]");
            chonKhoa.Click();
            var chonDanhSachKhoa = driver.FindElementById("selectKhoa");
            ReadOnlyCollection<IWebElement> webElements = chonDanhSachKhoa.FindElements(By.TagName("option"));
            foreach(IWebElement element in webElements)
            {
                element.Click();
                IWebElement selectedElement = driver.FindElementByXPath("//*[@id=\"divTDK\"]");
                Thread.Sleep(500);
                ReadOnlyCollection <IWebElement> monHocs = selectedElement.FindElements(By.TagName("tr"));
                //Console.WriteLine(element.Text + " - " + monHocs.Count);
                foreach(var mh in monHocs)
                {
                    ReadOnlyCollection<IWebElement> tds = mh.FindElements(By.TagName("td"));
                    string maMH = tds[1].Text;
                    string tenMH = tds[2].Text;
                    int soChi = int.Parse(tds[5].Text); 
                    MonHoc monHoc = context.MonHocs.Where(m => m.MaMonHoc.Contains(maMH)).FirstOrDefault();
                    if(monHoc is null)
                    {
                        monHoc = new MonHoc
                        {
                            Ten = tenMH,
                            MaMonHoc = maMH,
                            SoTinChi = soChi,
                        };
                        List<MonHoc> listMonHoc = new List<MonHoc>()
                        {
                            monHoc
                        };
                        if(nienKhoaDb.MonHocs is null)
                        {
                            nienKhoaDb.MonHocs = listMonHoc;
                        }
                        else
                        {
                            nienKhoaDb.MonHocs.AddRange(listMonHoc);
                        }
                        context.SaveChanges();
                    }
                }
            }
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

            NienKhoa nienKhoaCreated = createNewNienKhoa(context, hocKy, nienKhoa);
            NienKhoa nienKhoaDb = context.NienKhoas
                .Where(h => h.HocKy.Contains(nienKhoaCreated.HocKy))
                .Where(n => n.NamHoc.Contains(nienKhoaCreated.NamHoc)).FirstOrDefault();
            if(nienKhoaDb is null)
            {
                context.NienKhoas.Add(nienKhoaCreated);
                context.SaveChanges();
                return nienKhoaCreated;
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
