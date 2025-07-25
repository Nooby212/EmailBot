using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;

namespace Email
{
    class Program
    {
        const string SENDER_EMAIL = ""; //For this to work, you'll have to make your own email/password
        const string SENDER_PASSWORD = ""; //this cannot be empty
        static bool errors = false;
        
        static void Main(string[] args)
        {
            int attempts = 0;
            while (attempts < 10)
            {
                try
                {
                    Send2();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{DateTime.Now}]: FATAL: Program crashed unexpectedly. Message: {ex.Message}");
                    Thread.Sleep(1000);
                    attempts++;
                    bool errors = true;
                }
                
            }
            if (attempts >= 10)
            {
                Console.WriteLine($"[{DateTime.Now}]: FATAL: Program crashed after {attempts} attempts. Quitting...");
                Environment.Exit(1);
            }
            else if (errors = true)
            {
                Console.WriteLine($"[{DateTime.Now}]: Program finished with errors");
            }
            else
            {
                Console.WriteLine($"[{DateTime.Now}]: Program finished successfully.");
            }

        }
            

        public static void Send2()
        {
            data data = new data();
            data.sender_email = SENDER_EMAIL;
            data.password = SENDER_PASSWORD;
            data.port = 587; //Depending on what you use, you might need to change that
            data.receiver_email = input(); 
            data.message = " "; //message goes here 

            Console.WriteLine("Starting");

            if (string.IsNullOrEmpty(SENDER_EMAIL) || string.IsNullOrEmpty(SENDER_PASSWORD))
            {
                Console.WriteLine($"[{DateTime.Now}]: FATAL: string SENDER_EMAIL and string SENDER_PASSWORD are null or empty.");
                bool errors = true;
            }
            else
            {
                Console.WriteLine("Sending...");
                Send(data);
            }
        }
        public static char[] validC = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.@_-".ToCharArray(); 
        public static bool isValid(string input) 
        {
            if (string.IsNullOrEmpty(input)) return false;
            if (!input.All(c => validC.Contains(c))) return false;
            if (input.Count(c => c == '@') != 1) return false;
            int atIndex = input.IndexOf('@');
            if (!input.Substring(atIndex).Contains(".")) return false;
            return true;
        }
        public static string input()
        {
            string mail_inputed;
            while (true)
            {
                Console.Write($"[{DateTime.Now}]: receiver_email~>> ");
                mail_inputed = Console.ReadLine();
                if (isValid(mail_inputed))
                {
                    break;
                }
                else
                { 
                    Console.Write($"[{DateTime.Now}]: FATAL: Invalid email address; string did not pass the first layer of inspection; try again \n");
                    bool errors = true;
                }
                
            }
            Console.WriteLine($"[{DateTime.Now}]: sending to: {mail_inputed}");
            return mail_inputed;
        }
        public static void Send(data d)
        {
            using (var client = new SmtpClient(" ", d.port)) //inside the quotes goes your SMTP host, don't use yahoo
                
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(d.sender_email, d.password);
                

                var mail = new MailMessage();
                mail.From = new MailAddress(d.sender_email);
                mail.To.Add(d.receiver_email);
                mail.Subject = "test"; //subject
                mail.Body = d.message;
                mail.IsBodyHtml = false;
                //mail.Attachments.Add(new Attachment(" ")); //uncomment this line if you want attachments and put the file path
                
                try
                {
                    client.Send(mail);
                    Console.WriteLine($"[{DateTime.Now}]: Email sent");
                    bool errors = false;
                }
                
                catch (FormatException)
                {
                    Console.WriteLine($"[{DateTime.Now}]: FATAL: String did not pas the second layer of inspection; Program quit\n");
                    bool errors = true;
                }
                catch (SmtpException smtpEx)
                {
                    Console.WriteLine($"[{DateTime.Now}]: Working");
                    Console.WriteLine($"[{DateTime.Now}]: SMTP error: " + smtpEx.Message + ". Try checking your internet connection\n");
                    bool errors = true;
                }
                
                catch (Exception ex)
                {
                    Console.WriteLine($"[{DateTime.Now}]: Error: " + ex.Message);
                    Console.WriteLine($"[{DateTime.Now}]: FATAL: Program quit: Unexpected error");
                    bool errors = true;
                }

                //some ass error handling. might fix that later
                
            }
        }
    }
}

