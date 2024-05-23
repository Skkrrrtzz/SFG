using System.Net.Mail;
using System.Net;

namespace ATSSFG.Services
{
    public class Emailing
    {
        public async Task<bool> SendingEmail(string RecipientName, string Email, string Subject, string Body, string attachmentFilePath)
        {
            try
            {
                // Send the email via SMTP
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")) // Set the SMTP server
                {
                    // Set up the SMTP client
                    smtpClient.Credentials = new NetworkCredential("atsbcportal@gmail.com", "beae xldk udii xeme"); // Set the SMTP credentials
                    smtpClient.EnableSsl = true; // Enable SSL
                    smtpClient.Port = 587; // Set the SMTP port

                    MailAddress fromAddress = new MailAddress("atsbcportal@gmail.com", "ATS Business Control Portal"); // Set the email sender
                    MailMessage mailMessage = new MailMessage(fromAddress, new MailAddress(Email, RecipientName)); // Set the email sender and recipient

                    mailMessage.Subject = Subject; // Set the email subject
                    mailMessage.Body = Body; // Set the email body
                    mailMessage.IsBodyHtml = true; // Set the email body to be in HTML format

                    // Check if there is an attachment
                    if (!string.IsNullOrEmpty(attachmentFilePath) && File.Exists(attachmentFilePath))
                    {
                        // Attach the file to the email
                        Attachment attachment = new Attachment(attachmentFilePath);
                        mailMessage.Attachments.Add(attachment);
                    }

                    // Send the email
                    smtpClient.Send(mailMessage);
                }
                return true; // Email sent successfully
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Error sending email to {Email}: {ex.Message}"); // Print the error message
                return false; // Email sending failed
            }
        }
    }
}