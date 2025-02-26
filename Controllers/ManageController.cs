using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web_QLNS_VTH.Models;

namespace Web_QLNS_VTH.Controllers
{
    public class ManageController : Controller
    {
        public string danger = "";
        decimal luongCoSo = 2340000;
        private Model1 db = new Model1();
        public bool CheckProperties<T>(T obj, string id)
        {
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                // Bỏ qua thuộc tính Id
                if (property.Name.Equals(id, StringComparison.OrdinalIgnoreCase))
                {
                    continue; // Bỏ qua kiểm tra cho thuộc tính Id
                }

                var value = property.GetValue(obj);
                Debug.WriteLine($"Property: {property.Name}, Value: {value}");
                danger = "Vui lòng nhập đầy đủ thông tin, không được để trống!!!";
                // Kiểm tra nếu giá trị là chuỗi
                if (value is string strValue)
                {
                    // Loại bỏ các dấu cách ở đầu và cuối chuỗi
                    strValue = strValue.Trim();

                    // Kiểm tra xem chuỗi có rỗng không
                    if (string.IsNullOrEmpty(strValue))
                    {
                        return false; // Có thuộc tính chuỗi rỗng hoặc chỉ chứa dấu cách
                    }
                }
                if (value == null)
                {
                    return false; // Có thuộc tính null
                }
            }
            return true; // Tất cả thuộc tính đều khác rỗng và không phải null (trừ Id)
        }
        public bool checkMK(string mk)
        {
            if(mk.Length < 6) {
                danger = "Mật khẩu phải từ 6 kí tự trở lên!";
                return false ;
            }
            return true;
        }
        public ActionResult profile(string id)
        {
            var nv = db.NhanViens.Find(id);
            return View(nv);
        }
        public ActionResult tangCaQT(string id)
        {
            var tangCa = db.TangCas.Where(x => x.maNV == id).ToList();
            return View(tangCa);
        }
        public ActionResult nghiPhepQT(string id)
        {
            var np = db.NghiPheps.Where(x => x.maNV == id).ToList();
            return View(np);
        }
        public ActionResult yeuCauNPQT(string id)
        {
            var yc = db.YeuCauNPs.Where(x => x.maNV == id).ToList();
            return View(yc);
        }
        public ActionResult luongQT(string id)
        {
            return View(db.Luongs.Where(x => x.maNV == id).ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult yeuCauNPQT([Bind(Include = "tuNgay,denNgay")] YeuCauNP yeuCauNP)
        {
            if (ModelState.IsValid)
            {
                ManageController x = new ManageController();
                TaiKhoan tk;
                if (Session["taikhoan"] != null)
                {
                    tk = Session["taikhoan"] as TaiKhoan;
                    if (checkTimePhuHop((DateTime)yeuCauNP.tuNgay,(DateTime)yeuCauNP.denNgay,tk.maNV)) { 
                    yeuCauNP.maNV = tk.maNV;
                    yeuCauNP.maYeuCauNP = x.tuSinhMa(db.YeuCauNPs.Max(m => m.maYeuCauNP));
                    yeuCauNP.tinhTrang = "Chưa duyệt";
                    db.YeuCauNPs.Add(yeuCauNP);
                    db.SaveChanges();
                    var yc = db.YeuCauNPs.Where(n => n.maNV == tk.maNV).ToList();
                    return View(yc);}
                    Session["danger"] = danger;
                    return RedirectToAction("showYeuCauNP", "Manage", new { id = tk.maNV });
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult doiMK(string oldMK, string newMK)
        {
            TaiKhoan tk = Session["taikhoan"] as Web_QLNS_VTH.Models.TaiKhoan;

            if (tk != null && tk.matKhau == oldMK)
            {
                if(checkMK(newMK))
                {
                    // Cập nhật mật khẩu  
                    TaiKhoan tkc = db.TaiKhoans.Find(tk.maTK);
                    db.TaiKhoans.Attach(tkc);
                    tkc.matKhau = newMK;
                    // Lưu các thay đổi vào cơ sở dữ liệu
                    db.Entry(tkc).State = EntityState.Modified;
                    db.SaveChanges();
                    // Chuyển hướng đến trang đăng nhập
                    return RedirectToAction("login", "Manage", null);
                }
                Session["danger"] = danger;
            } else Session["danger"] = "Mật khẩu không đúng!";
            return RedirectToAction("profile","Manage",new {id = tk.maNV});
        }
        // GET: Manage
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(string tk, string mk)
        {
            var typeA = db.Admins.FirstOrDefault(x => x.tenDangNhap == tk && x.matKhau == mk);
            if (typeA != null)
            {
                Session["taikhoan"] = new TaiKhoan { maTK = "0", tenDangNhap = typeA.tenDangNhap, matKhau = typeA.matKhau, maNV = "0", loaiTK = "AD", NhanVien = null };
                return RedirectToAction("Index", "NhanViens", null);
            }
            var type = db.TaiKhoans.FirstOrDefault(x => x.tenDangNhap == tk && x.matKhau == mk);
            if (type != null)
            {
                Session["taikhoan"] = type;
                if (type.loaiTK == "NV") return RedirectToAction("Index", "Client", null);
                return RedirectToAction("Index", "NhanViens", null);
            }
            Session["danger"] = 1;
            return View();
        }
        public ActionResult logout()
        {
            Session.Remove("taikhoan");
            return RedirectToAction("login");
        }
        public string tuSinhMa(string tuSinhMa)
        {
            // Kiểm tra độ dài của chuỗi đầu vào
            if (string.IsNullOrEmpty(tuSinhMa) || tuSinhMa.Length < 5)
            {
                throw new ArgumentException("Mã không hợp lệ.");
            }

            // Lấy phần trước và phần sau
            string pTrc = tuSinhMa.Substring(0, 2);
            string pSau = "";

            // Tăng giá trị số lên 1
            int n = int.Parse(tuSinhMa.Substring(2, 3)) + 1;

            // Kiểm tra xem n có lớn hơn 999 không
            if (n > 999)
            {
                throw new InvalidOperationException("Giá trị đã vượt quá giới hạn tối đa.");
            }

            // Định dạng lại giá trị số thành 3 chữ số
            pSau = n.ToString("D3");

            // Trả về chuỗi mới
            return pTrc + pSau;
        }
        public int soNgayLamThang(int year, int month)
        {
            int totalDays = DateTime.DaysInMonth(year, month);
            int weekdaysCount = 0;
            for (int day = 1; day <= totalDays; day++)
            {
                DateTime currentDate = new DateTime(year, month, day);
                // Kiểm tra nếu ngày không phải là thứ Bảy (6) hoặc Chủ Nhật (0)
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    weekdaysCount++;
                }
            }
            return weekdaysCount;
        }
        public decimal xTangCa(DateTime date, string loaiTangCa)
        {
            if (loaiTangCa == "Lễ")
            {
                return 3m;
            }
            else
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    return 2m;
                }
                else
                {
                    return 1.5m;
                }
            }
        }
        public int tinhKCNgaySinh(DateTime tuNgay, DateTime ngaySinh)
        {
            int soNam = tuNgay.Year - ngaySinh.Year;

            // Kiểm tra nếu chưa đến ngày sinh trong năm nay
            if (tuNgay < ngaySinh.AddYears(soNam))
            {
                soNam--;
            }
            return soNam;
        }
        public bool checkNgaySinh(DateTime ngaySinh)
        {
            if (tinhKCNgaySinh(DateTime.Now, ngaySinh) < 18)
            {
                Debug.WriteLine(tinhKCNgaySinh(DateTime.Now, ngaySinh).ToString());
                danger = "Vui lòng nhập nhân viên từ 18 tuổi trở lên!";
                return false;
            }
            else return true;
        }
        public bool checkTimeTrcSau(DateTime tuNgay, DateTime denNgay)
        {
            if (denNgay > tuNgay) return true;
            else
            {
                danger = "Vui lòng nhập khoảng thời gian phù hợp!";
                return false;
            }
        }

        public bool checkTimePhuHop(DateTime tuNgay, DateTime denNgay, string maNV)
        {
            NhanVien nv = db.NhanViens.Find(maNV);
            if (tinhKCNgaySinh(tuNgay, (DateTime)nv.ngaySinh) >= 18)
            {
                if (checkTimeTrcSau((DateTime)nv.ngayVaoCT, tuNgay))
                {
                    if (checkTimeTrcSau(tuNgay, denNgay)) return true;
                    else return false;
                }
                else
                {
                    danger = $"Khoảng thời gian nhập không phù hợp với ngày vào công ty của {nv.hoTen}: {nv.ngayVaoCT.Value.ToString("dd/MM/yyyy")}!";
                    return false;
                }
            }
            else
            {
                danger = $"Khoảng thời gian nhập không phù hợp với ngày sinh của {nv.hoTen}: {nv.ngaySinh.Value.ToString("dd/MM/yyyy")}!";
                return false;
            }
        }
        public bool IsDateInRange(DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            // Kiểm tra xem ngày cần kiểm tra có nằm trong khoảng không
            return dateToCheck >= startDate && dateToCheck <= endDate;
        }
        public int soNPTrongNam(int year, string maNV)
        {
            var np = db.NghiPheps.Where(x => x.maNV == maNV).ToList();
            int soNPinYear = 0;
            foreach (var x in np)
            {
                if (x.tuNgay.Value.Year == year)
                {
                    soNPinYear += (int)(x.denNgay.Value - x.tuNgay.Value).TotalDays;
                }
            }
            return soNPinYear;
        }
        public bool checkSoNP(int year, string maNV, int soNgay)
        {
            if (soNPTrongNam(year, maNV) >= 15)
            {
                danger = $"Nhân viên mã {maNV} đã nghỉ đủ 15 ngày phép!";
                return false;
            }
            else if (soNPTrongNam(year, maNV) + soNgay >= 15)
            {
                danger = $"Khoảng nghỉ phép này không hợp lệ, nhân viên mã {maNV} đã nghỉ được {soNPTrongNam(year, maNV)}!";
                return false;
            }
            return true;
        }
        public bool checkNghiPhep(DateTime tuNgay, DateTime denNgay, string maNV)
        {
            var np = db.NghiPheps.Where(x => x.maNV == maNV).ToList();
            if (checkTimePhuHop(tuNgay, denNgay, maNV))
            {
                if (checkSoNP(tuNgay.Year, maNV, (int)(denNgay - tuNgay).TotalDays))
                {
                    foreach (var x in np)
                    {
                        if (IsDateInRange(tuNgay, (DateTime)x.tuNgay, (DateTime)x.denNgay) || IsDateInRange(denNgay, (DateTime)x.tuNgay, (DateTime)x.denNgay))
                        {
                            danger = "Khoảng thời gian nghỉ phép bị trùng khoảng nghỉ phép trước đó!";
                            return false;
                        }
                    }
                    return true;
                }
                else return false;
            }
            else return false;
        }
        public bool checkTangCa(DateTime ngay, int soGio, string maNV, string loaiTangCa)
        {
            var np = db.NghiPheps.Where(x => x.maNV == maNV).ToList();
            var tc = db.TangCas.Where(x => x.maNV == maNV).ToList();
            var nv = db.NhanViens.Find(maNV);
            if (tinhKCNgaySinh(ngay, (DateTime)nv.ngaySinh) >= 18)
            {
                if (checkTimeTrcSau((DateTime)nv.ngayVaoCT, ngay))
                {
                    foreach (var i in np)
                    {
                        if (IsDateInRange(ngay, (DateTime)i.tuNgay, (DateTime)i.denNgay))
                        {
                            danger = $"Thời gian nhập trùng với khoảng nghỉ phép của {nv.hoTen}: {i.tuNgay.Value.ToString("dd/MM/yyyy")} - {i.denNgay.Value.ToString("dd/MM/yyyy")} !";
                            return false;
                        }
                    }
                    foreach (var i in tc)
                    {
                        if (ngay == i.ngayTangCa)
                        {
                            danger = $"Nhân viên {nv.hoTen} đã được ghi nhận tăng ca vào ngày: {i.ngayTangCa.Value.ToString("dd/MM/yyyy")}!";
                            return false;
                        }
                    }
                    if (loaiTangCa == "Lễ" || ngay.DayOfWeek == DayOfWeek.Sunday || ngay.DayOfWeek == DayOfWeek.Saturday)
                    {
                        if (soGio > 12)
                        {
                            danger = "Không được tăng ca quá 12 tiếng vào ngày lễ, thứ 7, CN!";
                            return false;
                        }
                        else return true;
                    }
                    else
                    {
                        if (soGio > 4)
                        {
                            danger = "Không được tăng ca quá 4 tiếng vào ngày thường!";
                            return false;
                        }
                        else return true;
                    }
                }
                else
                {
                    danger = $"Thời gian nhập không phù hợp với ngày vào công ty của {nv.hoTen}: {nv.ngayVaoCT.Value.ToString("dd/MM/yyyy")}!";
                    return false;
                }
            }
            else
            {
                danger = $"Thời gian nhập không phù hợp với ngày sinh của {nv.hoTen}: {nv.ngaySinh.Value.ToString("dd/MM/yyyy")}!";
                return false;
            }
        }
        public bool checkTK(string tk)
        {
            var check = db.TaiKhoans.FirstOrDefault(x => x.tenDangNhap == tk) != null || db.Admins.FirstOrDefault(x => x.tenDangNhap == tk) != null;
            if (check)
            {
                danger = "Tài khoản đã tồn tại. Vui lòng nhập lại tên đăng nhập khác!";
                return false;
            }
            return true;
        }
        public (decimal, decimal, int) xetBLTrinhDo(string maNV)
        {
            var nv = db.NhanViens.FirstOrDefault(x => x.maNV == maNV);
            if (nv != null)
            {
                switch (nv.trinhDo)
                {
                    case "Cao đẳng": return (3.5m, 0.33m, 8);
                    case "Đại học": return (4.4m, 0.34m, 8);
                    case "Thạc sĩ": return (5.75m, 0.35m, 6);
                    case "Tiến sĩ": return (6.2m, 0.36m, 6);
                }
            }
            danger = $"Không tìm thấy dữ liệu tương ứng với mã: {maNV}!";
            return (0, 0, 0);
        }
        public decimal xetHSPhuCap(string maNV)
        {
            var nv = db.NhanViens.FirstOrDefault(x => x.maNV == maNV);
            if (nv != null)
            {
                if (nv.ChucVu.tenChucVu == "Giám đốc") return 1.2m;
                else if (nv.ChucVu.tenChucVu == "Phó giám đốc") return 1m;
                else if (nv.ChucVu.tenChucVu.Contains("Trưởng phòng")) return 0.7m;
                else return 0.3m;
            }
            danger = $"Không tìm thấy dữ liệu tương ứng với mã: {maNV}!";
            return 0;
        }
        public bool xetThangNam(string maNV, int month, int year)
        {
            var nv = db.NhanViens.Find(maNV);
            DateTime lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            if (lastDay < nv.ngaySinh || lastDay < nv.ngayVaoCT)
            {
                danger = $"Vui lòng nhập tháng năm sau ngày sinh, ngày vào công ty của nhân viên mã {maNV}!";
                return false;
            }
            return true;
        }
        public decimal xetPCThamNien(string maNV, int month, int year)
        {
            var nv = db.NhanViens.Find(maNV);
            DateTime lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            int nam = (int)Math.Floor((lastDay - (DateTime)nv.ngayVaoCT).TotalDays / 365);
            if (nam < 4) return 0;
            else return nam * 0.01m;
        }
        public decimal layHeSoLuong(string maNV, int month, int year)
        {
            var x = xetBLTrinhDo(maNV);
            var nv = db.NhanViens.Find(maNV);
            DateTime lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            var kc = (int)(lastDay - (DateTime)nv.ngayVaoCT).TotalDays / (365 * 2);
            if (kc > x.Item3) return x.Item1 + x.Item2 * x.Item3;
            else return x.Item1 + x.Item2 * kc;
        }
        public decimal tinhPhuCapCV(string maNV)
        {
            return luongCoSo * (decimal)xetHSPhuCap(maNV);
        }
        public decimal tinhPhuCapThamNien(string maNV, int month, int year)
        {
            return luongCoSo * (decimal)(layHeSoLuong(maNV, month, year) * xetPCThamNien(maNV, month, year));
        }
        public decimal tinhPhuCap(string maNV, int month, int year)
        {
            return tinhPhuCapCV(maNV) + tinhPhuCapThamNien(maNV, month, year);
        }
        public decimal luongTrenGio(string maNV, int month, int year)
        {
            return (luongCoSo * (decimal)layHeSoLuong(maNV, month, year) + tinhPhuCap(maNV, month, year)) / soNgayLamThang(year, month);
        }
        public decimal tinhLuongTangCa(string maNV, int month, int year)
        {
            var tc = db.TangCas.Where(x => x.maNV == x.maNV).ToList();
            decimal luongTC = 0;
            foreach (var i in tc)
            {
                if (checkDayInMonth((DateTime)i.ngayTangCa, month, year))
                {
                    luongTC += luongTrenGio(maNV, month, year) * xTangCa((DateTime)i.ngayTangCa, i.loaiTangCa);
                }
            }
            return luongTC;
        }
        public bool checkDayInMonth(DateTime day, int month, int year)
        {
            // Kiểm tra xem tháng và năm có khớp với ngày đã cho không
            return day.Month == month && day.Year == year;
        }
        public int soNgayKoPhep(string maNV, int month, int year, int soNghi)
        {
            var np = db.NghiPheps.Where(x => x.maNV == maNV).ToList();
            foreach (var i in np)
            {
                if (checkDayInMonth((DateTime)i.tuNgay.Value, month, year))
                {
                    soNghi -= (int)(i.denNgay - i.tuNgay).Value.TotalDays;
                }
            }
            return soNghi;
        }
        public int soNgayCong(string maNV, int month, int year, int soNghi)
        {
            return soNgayLamThang(year, month) - soNgayKoPhep(maNV, month, year, soNghi);
        }
        public decimal mucLuong(string maNV, int month, int year)
        {
            return luongCoSo * layHeSoLuong(maNV, month, year);
        }
        public decimal tongLuong(string maNV, int month, int year, int soNghi)
        {
            return (luongCoSo * layHeSoLuong(maNV, month, year) + tinhPhuCap(maNV, month, year))
                / soNgayLamThang(year, month) * soNgayCong(maNV, month, year, soNghi) + tinhLuongTangCa(maNV, month, year);
        }
        public decimal luongNhan(string maNV, int month, int year, int soNghi, decimal ungTrc)
        {
            return Decimal.Truncate(tongLuong(maNV, month, year, soNghi) - ungTrc);
        }
        public bool checkNgayHopLe(DateTime ngay, string maNV)
        {
            var nv = db.NhanViens.Find(maNV);
            if(ngay < (DateTime)nv.ngaySinh || ngay < (DateTime)nv.ngayVaoCT)
            {
                danger = "Vui lòng nhập thời gian phù hợp";
                return false;
            }
            return true;
        }
        public bool checkLuong(string maNV, string thangNam)
        {
            if(db.Luongs.FirstOrDefault(x=>x.maNV == maNV && x.thangNam == thangNam) != null)
            {
                danger = "Lương tháng này đã tồn tại";
                return false;
            }
            return true;
        }
        public bool checkTimeLonNho(DateTime dn,DateTime dl)
        {
            if (dl > dn) return true;
            else return false;
        }
    }
}