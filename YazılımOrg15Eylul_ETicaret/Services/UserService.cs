using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using YazılımOrg15Eylul_ETicaret.Data;
using YazılımOrg15Eylul_ETicaret.Models;

namespace YazılımOrg15Eylul_ETicaret.Services;

public class UserService
{

    Yazilima15EylulETicaretContext context = new Yazilima15EylulETicaretContext();

    public async Task<User?> loginControl(User user)
    {
        string value = MD5Sifrele(user.Password!);

        User? usr = await context
            .Users
            .FirstOrDefaultAsync(u =>
              u.Email == user.Email &&
              u.Password == value &&
              u.IsAdmin == true &&
              u.Active == true);


        return usr;
    }



    public static User? GetUserInfo(string Email)
    {
        using (Yazilima15EylulETicaretContext context = new Yazilima15EylulETicaretContext())
        {
            User? user = context.Users.FirstOrDefault(u => u.Email == Email);
            return user;
        }

    }


    public static string AddUser(User user)
    {
        using (Yazilima15EylulETicaretContext context = new Yazilima15EylulETicaretContext())
        {
            try
            {
                User? existUser = context.Users.FirstOrDefault(u => u.Email == user.Email);
                if (existUser != null)
                {
                    //bu email daha önceden kayıtlı
                    return "Email zaten var";
                }
                else
                {
                    user.Active = true;
                    user.IsAdmin = false;
                    user.Password = MD5Sifrele(user.Password!);
                    context.Users.Add(user);
                    context.SaveChanges();
                    return "Başarılı";

                }
            }
            catch (Exception)
            {

                return "Başarısız";
            }
        }
    }




    public static string MD5Sifrele(string value)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] btr = Encoding.UTF8.GetBytes(value);
            btr = md5.ComputeHash(btr);

            StringBuilder sb = new StringBuilder();
            foreach (byte item in btr)
            {
                sb.Append(item.ToString("x2").ToLower());
            }
            return sb.ToString();
        }
    }



    public static string SHA256Sifrele(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }

    }


    public static string UserControl(User user)
    {
        using (Yazilima15EylulETicaretContext context = new Yazilima15EylulETicaretContext())
        {
            string answer = "";

            try
            {
                string md5sifrele = MD5Sifrele(user.Password!);

                User? existsUser = context.Users
                    .FirstOrDefault(u =>
                    u.Email == user.Email &&
                    u.Password == md5sifrele &&
                    u.Active
                    );

                if (existsUser == null)
                {
                    //ya email ya şifre yanlış (yada her ikisi yanlış veya  aktif değil)

                    answer = "yanlış";

                }
                else
                {
                    //Email ve şifre doğru ve o kişi aktif var yani
                    //admin mi yoksa normal bir kullanıcı mı ona kacaz
                    if (existsUser.IsAdmin)
                    {
                        answer = "admin";
                    }
                    else
                    {
                        answer = existsUser.Email!;

                    }

                }
            }
            catch (Exception)
            {

                answer = "HATA";
            }


            return answer;
        }
    }

    public static string Send_Sms(string OrderGroupGUID)
    {
        Order? order;
        User? user;
        string content;
        string telefon;


        using (Yazilima15EylulETicaretContext context = new())
        {

            order = context.Orders.FirstOrDefault(o => o.OrderGroupGUID == OrderGroupGUID);

            user = context.Users.FirstOrDefault(u => u.UserID == order!.UserID);
            //Sayın Mazhar Kartal 28.02.2025 tarihinde  28022025214950 nolu siparişiniz alınmıştır
            content = $"Sayın {user?.NameSurname} , {order?.OrderDate}  tarihinde  {OrderGroupGUID} nolu siparişiniz alınmıştır";

            telefon = user?.Telephone!;

            string ss = "";
            ss += "<?xml version='1.0' encoding='UTF-8'?>";
            ss += "<mainbody>";
            ss += "<header>";
            ss += "<company dil='TR'>Netgsm</company>";
            ss += "<usercode>8503096835</usercode>";
            ss += "<password>5795$5E</password>";
            ss += "<type>1:n</type>";
            ss += "<msgheader>8503096835</msgheader>";
            ss += "</header>";
            ss += "<body>";
            ss += "<msg>";
            ss += $"<![CDATA[{content}]]>";
            ss += "</msg>";
            ss += $"<no>{telefon}</no>";
            ss += "</body>  ";
            ss += "</mainbody>";



            return XMLPOST("https://api.netgsm.com.tr/sms/send/xml", ss);

        }
    }

    static string XMLPOST(string PostAddress, string xmlData)
    {
        try
        {
            WebClient wUpload = new WebClient();
            HttpWebRequest request = WebRequest.Create(PostAddress) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] bPostArray = Encoding.UTF8.GetBytes(xmlData);
            byte[] bResponse = wUpload.UploadData(PostAddress, "POST", bPostArray);
            char[] sReturnChars = Encoding.UTF8.GetChars(bResponse);
            string sWebPage = new string(sReturnChars);
            return sWebPage;
        }
        catch
        {
            return "-1";

        }

    }


    public static void Send_Email(string OrderGroupGUID)
    {
        using (Yazilima15EylulETicaretContext context = new())
        {

            Order? order = context.Orders.FirstOrDefault(o => o.OrderGroupGUID == OrderGroupGUID);

            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress("no-reply@septembersoftware.com.tr", "Eticaret Bilgi");

            string subject = "Siparişiniz Hakkında";



            User? user = context.Users.FirstOrDefault(u => u.UserID == order!.UserID);

            mailMessage.To.Add(user.Email!);
            string content = $"Sayın {user.NameSurname}, {order!.OrderDate},tarihinde {OrderGroupGUID} nolu <b>siparişiniz</b> alınmıştır.";

            mailMessage.IsBodyHtml = true;
            mailMessage.Body = content;
            mailMessage.Subject = subject;

            SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential("no-reply@septembersoftware.com.tr", "6Vp-w1TS:oAs21_=");
            smtp.Port = 587;
            smtp.Host = "mail.kurumsaleposta.com";

            try
            {
                smtp.Send(mailMessage);
            }
            catch (Exception)
            {

                //email gönderilemedi. ilgili personeli bilgilendir
            }


        }
    }



    public static void TopluMailGonder(string mail)
    {
        using (Yazilima15EylulETicaretContext context = new())
        {

            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress("bulten@septembersoftware.com.tr", "Eticaret Bülten mazhar");

            string subject = "Siparişiniz Hakkında";



           

            mailMessage.To.Add(mail);
            string content = $"Elektronik ürünlerimizde fırsat yağmuru devam ediyor! bekleriz";

            mailMessage.IsBodyHtml = true;
            mailMessage.Body = content;
            mailMessage.Subject = subject;

            SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential("bulten@septembersoftware.com.tr", ":c6OQr:1H.-4W0oj");
            smtp.Port = 587;
            smtp.Host = "mail.kurumsaleposta.com";

            try
            {
                smtp.Send(mailMessage);
            }
            catch (Exception)
            {

                //email gönderilemedi. ilgili personeli bilgilendir
            }


        }


    }




}
