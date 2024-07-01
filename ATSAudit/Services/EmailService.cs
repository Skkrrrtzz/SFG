using System.Net.Mail;
using System.Net;

namespace ATSSFG.Services
{
    public class EmailService
    {
        private readonly SmtpClient smtpClient = new("smtp.gmail.com");

        public async Task<bool> SendingEmail(string RecipientName, string Email, string Subject, string Body, string attachmentFilePath)
        {
            try
            {
                // Send the email via SMTP
                using (smtpClient) // Set the SMTP server
                {
                    // Set up the SMTP client
                    smtpClient.Credentials = new NetworkCredential("atsauditportal@gmail.com", "/GnyHV!r[t<j6MSxW68PD"); // Set the SMTP credentials
                    smtpClient.EnableSsl = true; // Enable SSL
                    smtpClient.Port = 587; // Set the SMTP port

                    MailAddress fromAddress = new MailAddress("atsauditportal@gmail.com", "ATS Audit Portal"); // Set the email sender
                    MailMessage mailMessage = new MailMessage(fromAddress, new MailAddress(Email, RecipientName))
                    {
                        Subject = Subject, // Set the email subject
                        Body = Body, // Set the email body
                        IsBodyHtml = true // Set the email body to be in HTML format
                    }; // Set the email sender and recipient

                    // Check if there is an attachment
                    if (!string.IsNullOrEmpty(attachmentFilePath) && File.Exists(attachmentFilePath))
                    {
                        // Attach the file to the email
                        Attachment attachment = new Attachment(attachmentFilePath);
                        mailMessage.Attachments.Add(attachment);
                    }

                    // Send the email
                    smtpClient.SendAsync(mailMessage, () => {
                        Console.WriteLine("Meow");
                    });
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
