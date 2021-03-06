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
            //Context context = new Context();
            //context.NienKhoas.Remove(context.NienKhoas.First());
            //context.SaveChanges();
            //RunSeleniumSGU();
            //RunSeleniumBK();
        }

        private static void RunSeleniumSGU()
        {
            Context context = new Context();

            NienKhoa nienKhoa = context.NienKhoas.Where(s => s.HocKy == "Bach Khoa Co Khi").Include(s => s.MonHocs).FirstOrDefault();
            if (nienKhoa is not null)
            {
                context.NienKhoas.Remove(nienKhoa);
                context.SaveChanges();
            }

            NienKhoa nienKhoaSGU = context.NienKhoas.Where(s => s.HocKy == "SGU").Include(s => s.MonHocs).FirstOrDefault();
            if (nienKhoaSGU is not null)
            {
                context.NienKhoas.Remove(nienKhoaSGU);
                context.SaveChanges();
            }

            nienKhoaSGU = new NienKhoa
            {
                HocKy = "SGU",
                NamHoc = "2021",
                MonHocs = new List<MonHoc>()
            };
            context.NienKhoas.Add(nienKhoaSGU);
            context.SaveChanges();

            var options = new EdgeOptions();
            options.UseChromium = true;
            var driver = new EdgeDriver(options);
            driver.Url = "http://thongtindaotao.sgu.edu.vn/";
            driver.Navigate();

            var username = driver.FindElementById("ctl00_ContentPlaceHolder1_ctl00_ucDangNhap_txtTaiKhoa");
            username.SendKeys("3118410087");

            var password = driver.FindElementById("ctl00_ContentPlaceHolder1_ctl00_ucDangNhap_txtMatKhau");
            password.SendKeys("Anhducjav123");

            var buttonLogin = driver.FindElementById("ctl00_ContentPlaceHolder1_ctl00_ucDangNhap_btnDangNhap");
            buttonLogin.Click();

            var dkmh = driver.FindElementById("ctl00_menu_lblDangKyMonHoc");
            dkmh.Click();

            Thread.Sleep(500);

            var locDk = driver.FindElementById("selectDKLoc");
            locDk.Click();

            var chonKhoa = driver.FindElementByXPath("//*[@id=\"selectDKLoc\"]/option[2]");
            chonKhoa.Click();

            var elementKhoa = driver.FindElementById("selectKhoa");
            var danhSachKhoa = elementKhoa.FindElements(By.TagName("option"));

            foreach(var khoa in danhSachKhoa)
            {
                khoa.Click();
                Thread.Sleep(1000);
                var bangHocPhan = driver.FindElementById("divTDK");
                var dsHocPhan = bangHocPhan.FindElements(By.TagName("tr"));
                foreach(var hocPhan in dsHocPhan)
                {
                    Console.WriteLine(hocPhan.Text);
                    var column = hocPhan.FindElements(By.TagName("td"));
                    string maMh = column[1].Text;
                    string tenMh = column[2].Text;
                    string soTinChi = column[5].Text;
                    string nmh = column[3].Text;
                    MonHoc monHoc = context.MonHocs.Where(s => s.MaMonHoc == maMh).FirstOrDefault();
                    if(monHoc is null)
                    {
                        monHoc = new MonHoc
                        {
                            MaMonHoc = maMh,
                            Ten = tenMh,
                            SoTinChi = int.Parse(soTinChi),
                            NhomMonHoc = new List<NhomMonHoc>()
                        };
                        nienKhoaSGU.MonHocs.Add(monHoc);

                        NhomMonHoc nhomMonHoc = new NhomMonHoc
                        {
                            NMH = nmh,
                            Buois = new List<Buoi>()
                        };
                        monHoc.NhomMonHoc.Add(nhomMonHoc);

                        ReadOnlyCollection<IWebElement> dsThu= column[11].FindElements(By.TagName("div"));
                        for(int i = 0; i < dsThu.Count; i += 2)
                        {
                            int numThu = 0;
                            switch (dsThu[i].Text)
                            {
                                case "Hai":
                                    numThu = 2;
                                    break;
                                case "Ba":
                                    numThu = 3;
                                    break;
                                case "Tư":
                                    numThu = 4;
                                    break;
                                case "Năm":
                                    numThu = 5;
                                    break;
                                case "Sáu":
                                    numThu = 6;
                                    break;
                                case "Bảy":
                                    numThu = 7;
                                    break;
                                default:
                                    numThu = 0;
                                    break;
                            }
                            Buoi buoi = new Buoi
                            {
                                BatDauLuc = numThu
                            };
                            nhomMonHoc.Buois.Add(buoi);
                        }

                        ReadOnlyCollection<IWebElement> dsTietBatDau = column[12].FindElements(By.TagName("div"));
                        for (int i = 0; i < dsTietBatDau.Count; i += 2)
                        {
                            string tietBatDau = dsTietBatDau[i].Text;
                            if(i%2 == 0)
                            {
                                nhomMonHoc.Buois[i / 2].TietBatDau = int.Parse(tietBatDau);
                            }
                        }

                        ReadOnlyCollection<IWebElement> dsSoTiet = column[13].FindElements(By.TagName("div"));
                        for (int i = 0; i < dsSoTiet.Count; i += 2)
                        {
                            string soTiet = dsSoTiet[i].Text;
                            if (i % 2 == 0)
                            {
                                nhomMonHoc.Buois[i / 2].SoTiet = int.Parse(soTiet);

                            }

                        }

                        ReadOnlyCollection<IWebElement> dsPhong = column[14].FindElements(By.TagName("div"));
                        for (int i = 0; i < dsPhong.Count; i += 2)
                        {
                            string phong = dsPhong[i].Text;
                            if (i % 2 == 0)
                            {
                                nhomMonHoc.Buois[i / 2].Phong = phong;

                            }

                        }

                        ReadOnlyCollection<IWebElement> dsGiangVien = column[15].FindElements(By.TagName("div"));
                        for (int i = 0; i < dsGiangVien.Count; i += 2)
                        {
                            string giangVien = dsGiangVien[i].Text;
                            if (i % 2 == 0)
                            {
                                nhomMonHoc.Buois[i / 2].GiangVien = giangVien;

                            }

                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        NhomMonHoc nhomMonHoc = new NhomMonHoc
                        {
                            NMH = nmh,
                            Buois = new List<Buoi>()
                        };
                        monHoc.NhomMonHoc.Add(nhomMonHoc);

                        ReadOnlyCollection<IWebElement> dsThu = column[11].FindElements(By.TagName("div"));
                        for (int i = 0; i < dsThu.Count; i += 2)
                        {
                            int numThu = 0;
                            switch (dsThu[i].Text)
                            {
                                case "Hai":
                                    numThu = 2;
                                    break;
                                case "Ba":
                                    numThu = 3;
                                    break;
                                case "Tư":
                                    numThu = 4;
                                    break;
                                case "Năm":
                                    numThu = 5;
                                    break;
                                case "Sáu":
                                    numThu = 6;
                                    break;
                                case "Bảy":
                                    numThu = 7;
                                    break;
                                default:
                                    numThu = 0;
                                    break;
                            }
                            Buoi buoi = new Buoi
                            {
                                BatDauLuc = numThu
                            };
                            nhomMonHoc.Buois.Add(buoi);
                        }

                        ReadOnlyCollection<IWebElement> dsTietBatDau = column[12].FindElements(By.TagName("div"));
                        for (int i = 0; i < dsTietBatDau.Count; i += 2)
                        {
                            string tietBatDau = dsTietBatDau[i].Text;
                            if (i % 2 == 0)
                            {
                                nhomMonHoc.Buois[i / 2].TietBatDau = int.Parse(tietBatDau);
                            }
                        }

                        ReadOnlyCollection<IWebElement> dsSoTiet = column[13].FindElements(By.TagName("div"));
                        for (int i = 0; i < dsSoTiet.Count; i += 2)
                        {
                            string soTiet = dsSoTiet[i].Text;
                            if (i % 2 == 0)
                            {
                                nhomMonHoc.Buois[i / 2].SoTiet = int.Parse(soTiet);

                            }

                        }

                        ReadOnlyCollection<IWebElement> dsPhong = column[14].FindElements(By.TagName("div"));
                        for (int i = 0; i < dsPhong.Count; i += 2)
                        {
                            string phong = dsPhong[i].Text;
                            if (i % 2 == 0)
                            {
                                nhomMonHoc.Buois[i / 2].Phong = phong;

                            }

                        }

                        ReadOnlyCollection<IWebElement> dsGiangVien = column[15].FindElements(By.TagName("div"));
                        for (int i = 0; i < dsGiangVien.Count; i += 2)
                        {
                            string giangVien = dsGiangVien[i].Text;
                            if (i % 2 == 0)
                            {
                                nhomMonHoc.Buois[i / 2].GiangVien = giangVien;

                            }

                        }

                        context.SaveChanges();
                    }
                }

            }
        }

        private static void RunSeleniumBK()
        {
            var options = new EdgeOptions();
            options.UseChromium = true;
            var driver = new EdgeDriver(options);
            driver.Url = "http://dk4.dut.udn.vn/";
            driver.Navigate();

            Context context = new Context();

            NienKhoa nienKhoa = context.NienKhoas.Where(s => s.HocKy == "Bach Khoa Co Khi").Include(s=>s.MonHocs).FirstOrDefault();
            if(nienKhoa is not null)
            {
                context.NienKhoas.Remove(nienKhoa);
                context.SaveChanges();
            }
            nienKhoa = new NienKhoa
            {
                HocKy = "Bach Khoa Co Khi",
                NamHoc = "2021",
                MonHocs = new List<MonHoc>()
            };
            context.NienKhoas.Add(nienKhoa);
            context.SaveChanges();

            var lopHocPhan = driver.FindElementByXPath("//*[@id=\"NavigationMenu\"]/ul/li[3]");
            lopHocPhan.Click();

            var danhSachLop = driver.FindElementById("MainContent_BTRefresh1");
            danhSachLop.Click();

            Thread.Sleep(2000);

            var table = driver.FindElementById("MainContent_Grid1");
            ReadOnlyCollection<IWebElement> danhSachMonHoc = table.FindElements(By.TagName("tr"));
            for (int i = 1; i < danhSachMonHoc.Count - 1; i++)
            {
                ReadOnlyCollection<IWebElement> monHoc = danhSachMonHoc[i].FindElements(By.TagName("td"));
                string tenMocHoc = monHoc[2].Text;
                string soChi = monHoc[3].Text;
                MonHoc monHocCu = context.MonHocs.Where(s=>s.Ten ==tenMocHoc).Include(s=>s.NhomMonHoc).FirstOrDefault();
                if (monHoc[5].Text!=" ")
                {
                    if (monHocCu is null)
                    {
                        //Tao moi
                        monHocCu = new MonHoc
                        {
                            MaMonHoc = tenMocHoc,
                            Ten = tenMocHoc,
                            SoTinChi = int.Parse(MathF.Round(float.Parse(soChi)).ToString()),
                            NhomMonHoc = new List<NhomMonHoc>()
                        };

                        string giangVien = monHoc[4].Text;

                        nienKhoa.MonHocs.Add(monHocCu);
                        NhomMonHoc nhomMonHoc = new NhomMonHoc
                        {
                            NMH = giangVien,
                            Buois = new List<Buoi>()
                        };
                        monHocCu.NhomMonHoc.Add(nhomMonHoc);

                        Buoi buoi = new Buoi
                        {
                            BatDauLuc = int.Parse(Regex.Match(monHoc[5].Text, @"\d+").Value),
                            TietBatDau = int.Parse(Regex.Match(monHoc[5].Text, @"\d+-\d+").Value.Split("-")[0]),
                            GiangVien = giangVien,
                            Phong = "",
                            SoTiet = int.Parse(Regex.Match(monHoc[5].Text, @"\d+-\d+").Value.Split("-")[1]) - int.Parse(Regex.Match(monHoc[5].Text, @"\d+-\d+").Value.Split("-")[0]) + 1
                        };
                        nhomMonHoc.Buois.Add(buoi);

                        context.SaveChanges();
                    }
                    else
                    {
                        string giangVien = monHoc[4].Text;
                        NhomMonHoc nhomMonHoc = new NhomMonHoc
                        {
                            NMH = giangVien,
                            Buois = new List<Buoi>()
                        };
                        monHocCu.NhomMonHoc.Add(nhomMonHoc);

                        Buoi buoi = new Buoi
                        {
                            BatDauLuc = int.Parse(Regex.Match(monHoc[5].Text, @"\d+").Value),
                            TietBatDau = int.Parse(Regex.Match(monHoc[5].Text, @"\d+-\d+").Value.Split("-")[0]),
                            GiangVien = giangVien,
                            Phong = "",
                            SoTiet = int.Parse(Regex.Match(monHoc[5].Text, @"\d+-\d+").Value.Split("-")[1]) - int.Parse(Regex.Match(monHoc[5].Text, @"\d+-\d+").Value.Split("-")[0]) + 1
                        };
                        nhomMonHoc.Buois.Add(buoi);

                        context.SaveChanges();
                        //string giangVien = monHoc[4].Text;
                        ////Kiem tra xem co giao vien chua
                        //NhomMonHoc nhomMonHoc = context.NhomMonHocs.Where(s => s.MonHocId == monHocCu.Id).Where(s => s.NMH == giangVien).Include(s => s.Buois).FirstOrDefault();
                        //if (nhomMonHoc is null)
                        //{
                        //    nhomMonHoc = new NhomMonHoc
                        //    {
                        //        NMH = giangVien,
                        //        Buois = new List<Buoi>()
                        //    };
                        //    monHocCu.NhomMonHoc.Add(nhomMonHoc);

                        //    Buoi buoi = new Buoi
                        //    {
                        //        BatDauLuc = int.Parse(Regex.Match(monHoc[5].Text, @"\d+").Value),
                        //        TietBatDau = int.Parse(Regex.Match(monHoc[5].Text, @"\d+-\d+").Value.Split("-")[0]),
                        //        GiangVien = giangVien,
                        //        Phong = "",
                        //        SoTiet = int.Parse(Regex.Match(monHoc[5].Text, @"\d+-\d+").Value.Split("-")[1]) - int.Parse(Regex.Match(monHoc[5].Text, @"\d+-\d+").Value.Split("-")[0]) + 1
                        //    };
                        //    nhomMonHoc.Buois.Add(buoi);

                        //    context.SaveChanges();
                        //}
                        //else
                        //{
                        //    Buoi buoi = new Buoi
                        //    {
                        //        BatDauLuc = int.Parse(Regex.Match(monHoc[5].Text, @"\d+").Value),
                        //        TietBatDau = int.Parse(Regex.Match(monHoc[5].Text, @"\d+-\d+").Value.Split("-")[0]),
                        //        GiangVien = giangVien,
                        //        Phong = "",
                        //        SoTiet = int.Parse(Regex.Match(monHoc[5].Text, @"\d+-\d+").Value.Split("-")[1]) - int.Parse(Regex.Match(monHoc[5].Text, @"\d+-\d+").Value.Split("-")[0]) + 1
                        //    };
                        //    nhomMonHoc.Buois.Add(buoi);
                        //    context.SaveChanges();

                        //    //Console.WriteLine(Regex.Match(monHoc[5].Text, @"\d+").Value);
                        //    //Console.WriteLine(Regex.Match(monHoc[5].Text, @"\d+-\d+").Value.Split("-")[0]);
                        //}
                    }

                }
                Console.WriteLine(danhSachMonHoc[i].Text);
            }
        }

        private static void RunSelenium()
        {
            //Reset DB
            Context context = new Context();
            List<Buoi> buois = context.Buois.ToList();
            context.Buois.RemoveRange(buois);
            List<NhomMonHoc> nhomMonHocs = context.NhomMonHocs.ToList();
            context.NhomMonHocs.RemoveRange(nhomMonHocs);
            List<MonHoc> monHocs = context.MonHocs.ToList();
            context.MonHocs.RemoveRange(monHocs);
            context.SaveChanges();
            //Crawl
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
                Thread.Sleep(750);
                ReadOnlyCollection <IWebElement> monHocs = selectedElement.FindElements(By.TagName("tr"));
                //Console.WriteLine(element.Text + " - " + monHocs.Count);
                foreach(var mh in monHocs)
                {
                    ReadOnlyCollection<IWebElement> tds = mh.FindElements(By.TagName("td"));
                    string maMH = tds[1].Text;
                    string tenMH = tds[2].Text;
                    int soChi = int.Parse(tds[5].Text);
                    MonHoc monHoc = context.MonHocs
                        .Where(m => m.MaMonHoc.Contains(maMH))
                        .Include(i => i.NhomMonHoc)
                        .ThenInclude(b => b.Buois).FirstOrDefault();
                    if (monHoc is null)
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
                    string nHM = tds[3].Text;



                    NhomMonHoc nhomMonHoc = new NhomMonHoc
                    {
                        NMH = nHM
                    };

                    ReadOnlyCollection<IWebElement> thuMay = tds[11].FindElements(By.TagName("div"));
                    List<Buoi> buois = new List<Buoi>();
                    foreach (var thu in thuMay)
                    {
                        int numThu = 0;
                        switch (thu.Text)
                        {
                            case "Hai":
                                numThu = 2;
                                break;
                            case "Ba":
                                numThu = 3;
                                break;
                            case "Tư":
                                numThu = 4;
                                break;
                            case "Năm":
                                numThu = 5;
                                break;
                            case "Sáu":
                                numThu = 6;
                                break;
                            case "Bảy":
                                numThu = 7;
                                break;
                            default:
                                numThu = 0;
                                break;
                        }
                        Buoi buoi = new Buoi
                        {
                            BatDauLuc = numThu
                        };
                        buois.Add(buoi);
                    }
                    ReadOnlyCollection<IWebElement> tietBatDau = tds[12].FindElements(By.TagName("div"));
                    for (int i = 0; i < tietBatDau.Count; i+=2)
                    {
                        buois[i].TietBatDau = int.Parse(tietBatDau[i].Text);
                    }
                    ReadOnlyCollection<IWebElement> soTiet = tds[13].FindElements(By.TagName("div"));
                    for (int i = 0; i < soTiet.Count; i += 2)
                    {
                        buois[i].SoTiet = int.Parse(soTiet[i].Text);
                    }
                    ReadOnlyCollection<IWebElement> phong = tds[14].FindElements(By.TagName("div"));
                    for (int i = 0; i < phong.Count; i += 2)
                    {
                        buois[i].Phong = phong[i].Text;
                    }
                    ReadOnlyCollection<IWebElement> giangVien = tds[15].FindElements(By.TagName("div"));
                    for (int i = 0; i < giangVien.Count; i += 2)
                    {
                        buois[i].GiangVien = giangVien[i].Text;
                    }
                    List<Buoi> buoiMoi = buois.Where(c => c.BatDauLuc != 0).ToList();
                    if (nhomMonHoc.Buois is null)
                    {
                        nhomMonHoc.Buois = buoiMoi;
                    }
                    else
                    {
                        nhomMonHoc.Buois.AddRange(buoiMoi);
                    }

                    List<NhomMonHoc> listNhomMonHoc = new List<NhomMonHoc>
                    {
                        nhomMonHoc   
                    };
                    if(monHoc.NhomMonHoc is null)
                    {
                        monHoc.NhomMonHoc = listNhomMonHoc;
                    }
                    else
                    {
                        NhomMonHoc nhomMonHocCurrent = monHoc.NhomMonHoc
                            .Where(c=>c.NMH.Equals(nHM))
                            .FirstOrDefault();
                        if(nhomMonHocCurrent is null)
                        {
                            monHoc.NhomMonHoc.AddRange(listNhomMonHoc);
                        }
                    }
                    context.SaveChanges();
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
