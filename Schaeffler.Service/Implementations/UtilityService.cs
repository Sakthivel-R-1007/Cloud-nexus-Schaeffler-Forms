using Schaeffler.Service.Interfaces;
using System;
using System.Configuration;
using System.Net.Mail;

namespace Schaeffler.Service.Implementations
{
    public class UtilityService : IUtilityService
    {
        public string SendEmail(string Subject, string Content, string To, bool ISBCC, string FromEmail, string CCEmail = "", string Country = "")
        {
            try
            {
                MailMessage mailmessage = new MailMessage();
                mailmessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mailmessage.IsBodyHtml = true;
                mailmessage.Subject = Subject;
                mailmessage.Body = Content;
                if (!string.IsNullOrEmpty(FromEmail))
                    mailmessage.From = new MailAddress(FromEmail);
                try
                {
                    if (Country == "Japan")
                    {
                        string ToMail = ConfigurationManager.AppSettings["AdminJapan"] != null ? ConfigurationManager.AppSettings["AdminJapan"].ToString() : string.Empty;
                        mailmessage.To.Add(new MailAddress(ToMail));
                    }
                    else if (Country == "Korea")
                    {
                        string ToMail = ConfigurationManager.AppSettings["AdminKorea"] != null ? ConfigurationManager.AppSettings["AdminKorea"].ToString() : string.Empty;
                        mailmessage.To.Add(new MailAddress(ToMail));

                    }
                    else if (Country == "Australia")
                    {
                        string ToMail = ConfigurationManager.AppSettings["AdminAustralia"] != null ? ConfigurationManager.AppSettings["AdminAustralia"].ToString() : string.Empty;
                        mailmessage.To.Add(new MailAddress(ToMail));

                    }

                    if (ISBCC)
                    {
                        string BccList = ConfigurationManager.AppSettings["Conf_Main_Bcc_List"] != null ? ConfigurationManager.AppSettings["Conf_Main_Bcc_List"].ToString() : string.Empty;
                        if (!string.IsNullOrEmpty(BccList))
                        {
                            foreach (var item in BccList.Split(';'))
                            {
                                mailmessage.Bcc.Add(new MailAddress(item));
                            }
                        }
                    }
                    if (!String.IsNullOrEmpty(CCEmail))
                    {
                        foreach (var item in CCEmail.Split(';'))
                        {
                            mailmessage.CC.Add(new MailAddress(item));
                        }
                    }
                }
                catch { }
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.EnableSsl = false;
                smtpClient.Port = 587;

                smtpClient.Send(mailmessage);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "success";
        }
    }
}
