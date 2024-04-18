﻿using System.Net.Mail;
using System.Net;

namespace SFG.Services
{
    public class Emailing
    {
        public void SendingEmail(string approverName, string approverEmail, string subject, string body)
        {
            try
            {
                // Send the email via SMTP
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    // Set up the SMTP client
                    smtpClient.Credentials = new NetworkCredential("atsbcportal@gmail.com", "beae xldk udii xeme");
                    smtpClient.EnableSsl = true;
                    smtpClient.Port = 587;

                    MailAddress fromAddress = new MailAddress("atsbcportal@gmail.com", "ATS Business Control Portal");
                    MailMessage mailMessage = new MailMessage(fromAddress, new MailAddress(approverEmail, approverName));

                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    // Send the email
                    smtpClient.Send(mailMessage);
                }

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Error sending email to {approverEmail}: {ex.Message}");
            }
        }
    }
}
