

using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Text;
using Npgsql;

namespace MakesEasy.Services
{
    public class EmailService
    {
        public async Task<int> SendEmail(string email, string subject, string resetLink)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("cse.210840131054@gmail.com", "rbwiaxmdusnfaspp") // Use App Password
                };

                string plainTextBody = $"Hello,\n\nYou requested to reset your Makes Easy password.\n\n" +
                                       $"Click the link to reset your password: {resetLink}\n\n" +
                                       $"This link will expire in 30 minutes.\n\n" +
                                       $"If you didn't request this, you can safely ignore this email.\n\n" +
                                       $"Best regards,\nThe Makes Easy Team\n© {DateTime.UtcNow.Year} Makes Easy.";

                string htmlBody = $@" <!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""UTF-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <title>Password Reset - Makes Easy</title>
    <style>
      body {{
        margin: 0;
        padding: 0;
        background-color: #f8fafc;
        font-family: Arial, sans-serif;
        line-height: 1.6;
        color: #334155;
      }}
      .container {{
        width: 100%;
        background-color: #f8fafc;
        padding: 20px 0;
      }}
      .email-wrapper {{
        max-width: 600px;
        width: 100%;
        background-color: #ffffff;
        margin: 0 auto;
        border-radius: 16px;
        box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
        overflow: hidden;
      }}
      .header {{
        background: black;
        padding: 40px 30px;
        text-align: center;
      }}
      .logo {{
        display: inline-flex;
        align-items: center;
        gap: 12px;
        justify-content: center;
      }}
      .logo-icon {{
        width: 40px;
        height: 40px;
        background-color: rgba(255, 255, 255, 0.2);
        border-radius: 8px;
        display: inline-flex;
        align-items: center;
        justify-content: center;
        font-weight: bold;
        color: #ffffff;
        font-size: 18px;
      }}
      .logo-text {{
        color: #ffffff;
        font-size: 28px;
        font-weight: 600;
      }}
      .tagline {{
        color: rgba(255, 255, 255, 0.9);
        font-size: 16px;
        margin-top: 12px;
      }}
      .content {{
        padding: 50px 40px;
      }}
      .icon-wrapper {{
        text-align: center;
        margin-bottom: 30px;
      }}
      .icon {{
        width: 80px;
        height: 80px;
        background: linear-gradient(135deg, #f0f9ff 0%, #e0f2fe 100%);
        border: 3px solid #0ea5e9;
        border-radius: 50%;
        display: inline-flex;
        align-items: center;
        justify-content: center;
        font-size: 32px;
        margin: 0 auto;
      }}
      .title {{
        text-align: center;
        color: #0f172a;
        font-size: 32px;
        font-weight: 700;
        margin: 20px 0;
      }}
      .subtitle {{
        text-align: center;
        color: #64748b;
        font-size: 18px;
        margin-bottom: 30px;
      }}
      .message-box {{
        background: #f8fafc;
        border-left: 4px solid #3b82f6;
        padding: 24px;
        margin: 30px 0;
        border-radius: 0 8px 8px 0;
      }}
      .button-wrapper {{
        text-align: center;
        margin: 40px 0;
      }}
      .reset-button {{
        display: inline-block;
        background: linear-gradient(135deg, #3b82f6 0%, #8b5cf6 100%);
        color: #ffffff;
        text-decoration: none;
        padding: 18px 40px;
        border-radius: 12px;
        font-size: 18px;
        font-weight: 600;
      }}
      .warning-box {{
        background: #fef3c7;
        border: 1px solid #f59e0b;
        border-radius: 12px;
        padding: 20px;
        margin: 30px 0;
      }}
      .link-box {{
        background: #f1f5f9;
        border-radius: 12px;
        padding: 24px;
        margin: 30px 0;
      }}
      .link-container {{
        background: #ffffff;
        border: 2px dashed #cbd5e1;
        border-radius: 8px;
        padding: 16px;
        word-break: break-word;
      }}
      .footer {{
        background: #f8fafc;
        padding: 30px;
        text-align: center;
        border-top: 1px solid #e2e8f0;
      }}
      .signature {{
        margin-top: 50px;
        padding-top: 30px;
        border-top: 2px solid #f1f5f9;
      }}
    </style>
  </head>
  <body>
    <div class=""container"">
      <table
        class=""email-wrapper""
        cellpadding=""0""
        cellspacing=""0""
        border=""0""
        style=""
          max-width: 600px;
          width: 100%;
          background-color: #ffffff;
          margin: 0 auto;
          border-radius: 16px;
          box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
          overflow: hidden;
        ""
      >
        <tr>
          <td style=""background: black; padding: 40px 30px; text-align: center"">
            <div style=""  display: flex; justify-content: center; align-items: center;"">
              <div
                style=""
                  display: inline-block;
                  width: 40px;
                  height: 40px;
                  border-radius: 8px;
                  line-height: 40px;
                  text-align: center;
                  margin-right: 12px;
                ""
              >
                <svg
                  xmlns=""http://www.w3.org/2000/svg""
                  viewBox=""0 0 24 24""
                  width=""36""
                  height=""36""
                  style=""vertical-align: middle;""
                  fill=""none""
                >
                  <path
                    d=""M5.14286 14C4.41735 12.8082 4 11.4118 4 9.91886C4 5.54539 7.58172 2 12 2C16.4183 2 20 5.54539 20 9.91886C20 11.4118 19.5827 12.8082 18.8571 14""
                    stroke=""white""
                    stroke-width=""1.5""
                    stroke-linecap=""round""
                  />
                  <path
                    d=""M7.38287 17.0982C7.291 16.8216 7.24507 16.6833 7.25042 16.5713C7.26174 16.3343 7.41114 16.1262 7.63157 16.0405C7.73579 16 7.88105 16 8.17157 16H15.8284C16.119 16 16.2642 16 16.3684 16.0405C16.5889 16.1262 16.7383 16.3343 16.7496 16.5713C16.7549 16.6833 16.709 16.8216 16.6171 17.0982C16.4473 17.6094 16.3624 17.8651 16.2315 18.072C15.9572 18.5056 15.5272 18.8167 15.0306 18.9408C14.7935 19 14.525 19 13.9881 19H10.0119C9.47495 19 9.2065 19 8.96944 18.9408C8.47283 18.8167 8.04281 18.5056 7.7685 18.072C7.63755 17.8651 7.55266 17.6094 7.38287 17.0982Z""
                    stroke=""white""
                    stroke-width=""1.5""
                  />
                  <path
                    d=""M15 19L14.8707 19.6466C14.7293 20.3537 14.6586 20.7072 14.5001 20.9866C14.2552 21.4185 13.8582 21.7439 13.3866 21.8994C13.0816 22 12.7211 22 12 22C11.2789 22 10.9184 22 10.6134 21.8994C10.1418 21.7439 9.74484 21.4185 9.49987 20.9866C9.34144 20.7072 9.27073 20.3537 9.12932 19.6466L9 19""
                    stroke=""white""
                    stroke-width=""1.5""
                  />
                  <path
                    d=""M8.25 9.75L10.5 12L10.5 16M8.25 10.5C8.66421 10.5 9 10.1642 9 9.75C9 9.33579 8.66421 9 8.25 9C7.83579 9 7.5 9.33579 7.5 9.75C7.5 10.1642 7.83579 10.5 8.25 10.5Z""
                    stroke=""white""
                    stroke-width=""1.5""
                    stroke-linecap=""round""
                    stroke-linejoin=""round""
                  />
                  <path
                    d=""M15.75 9.75L13.5 12L13.5 16M15.75 10.5C15.3358 10.5 15 10.1642 15 9.75C15 9.33579 15.3358 9 15.75 9C16.1642 9 16.5 9.33579 16.5 9.75C16.5 10.1642 16.1642 10.5 15.75 10.5Z""
                    stroke=""white""
                    stroke-width=""1.5""
                    stroke-linecap=""round""
                    stroke-linejoin=""round""
                  />
                </svg>
              </div>

              <span
                style=""
                  color: #ffffff;
                  font-size: 28px;
                  font-weight: 600;
                  vertical-align: middle;
                ""
                >Makes Easy</span
              >
            </div>
            <p
              style=""
                color: rgba(255, 255, 255, 0.9);
                font-size: 16px;
                margin-top: 12px;
                margin-bottom: 0;
              ""
            >
              Your trusted productivity partner
            </p>
          </td>
        </tr>
        <tr>
          <td style=""padding: 50px 40px"">
            <div style=""text-align: center; margin-bottom: 30px"">
              <div
                style=""
                  width: 80px;
                  height: 80px;
                  background: linear-gradient(135deg, #f0f9ff 0%, #e0f2fe 100%);
                  border: 3px solid black;
                  border-radius: 50%;
                  display: inline-block;
                  line-height: 74px;
                  font-size: 32px;
                ""
              >
                🔐
              </div>
            </div>

            <h1
              style=""
                text-align: center;
                color: #0f172a;
                font-size: 32px;
                font-weight: 700;
                margin: 20px 0;
              ""
            >
              Reset Your Password
            </h1>
            <p
              style=""
                text-align: center;
                color: #64748b;
                font-size: 18px;
                margin-bottom: 30px;
              ""
            >
              We received a request to reset your account password
            </p>

            <div
              style=""
                background: #f8fafc;
                border-left: 4px solid black;
                padding: 24px;
                margin: 30px 0;
                border-radius: 0 8px 8px 0;
              ""
            >
              <p style=""margin: 0; font-size: 16px; color: #475569"">
                <strong>Hello,</strong><br /><br />Someone requested a password
                reset for your <strong>Makes Easy</strong> account. If this was
                you, click the button below to create a new password.
              </p>
            </div>

            <div style=""text-align: center; margin: 40px 0"">
              <a
                href=""{resetLink}""
                style=""
                  display: inline-block;
                  background: black;
                  color: #ffffff;
                  text-decoration: none;
                  padding: 18px 40px;
                  border-radius: 12px;
                  font-size: 18px;
                  font-weight: 600;
                ""
                >🔓 Reset My Password</a
              >
            </div>

            <div
              style=""
                background: #fef3c7;
                border: 1px solid #f59e0b;
                border-radius: 12px;
                padding: 20px;
                margin: 30px 0;
              ""
            >
              <p style=""margin: 0; color: #92400e; font-size: 14px"">
                ⚠️ This link will expire in <strong>30 minutes</strong> for your
                security.
              </p>
            </div>

            <div
              style=""
                background: #f1f5f9;
                border-radius: 12px;
                padding: 24px;
                margin: 30px 0;
              ""
            >
              <p
                style=""
                  margin: 0 0 12px 0;
                  font-size: 14px;
                  color: #64748b;
                  font-weight: 600;
                ""
              >
                Button not working? Copy and paste this link:
              </p>
              <div
                style=""
                  background: #ffffff;
                  border: 2px dashed #cbd5e1;
                  border-radius: 8px;
                  padding: 16px;
                  word-break: break-word;
                ""
              >
                <a
                  href=""{resetLink}""
                  style=""color: #3b82f6; font-size: 14px; text-decoration: none""
                  >{resetLink}</a
                >
              </div>
            </div>

            <p style=""text-align: center; color: #64748b; font-size: 15px"">
              Didn't request this password reset? You can safely ignore this
              email. Your password will remain unchanged.
            </p>

            <div
              style=""
                margin-top: 50px;
                padding-top: 30px;
                border-top: 2px solid #f1f5f9;
              ""
            >
              <p style=""color: #475569; font-size: 16px; margin-bottom: 5px"">
                Best regards,
              </p>
              <p
                style=""
                  color: #0f172a;
                  font-size: 18px;
                  font-weight: 600;
                  margin-top: 0;
                ""
              >
                The Makes Easy Team 🚀
              </p>
            </div>
          </td>
        </tr>
        <tr>
          <td
            style=""
              background: #f8fafc;
              padding: 30px;
              text-align: center;
              border-top: 1px solid #e2e8f0;
            ""
          >
            <p style=""color: #94a3b8; font-size: 12px; margin: 5px 0"">
              &copy; {{DateTime.UtcNow.Year}} Makes Easy. All rights reserved.
            </p>
            <p style=""color: #cbd5e1; font-size: 11px; margin: 5px 0"">
              This email was sent because a password reset was requested for
              your account.
            </p>
            <p style=""margin: 12px 0 5px 0; color: #94a3b8; font-size: 12px"">
              Need help? Contact us at
              <a href=""mailto:support@makeseasy.in"" style=""color: #3b82f6""
                >support@makeseasy.in</a
              >
            </p>
          </td>
        </tr>
      </table>
    </div>
  </body>
</html>
";

                var message = new MailMessage
                {
                    From = new MailAddress("cse.210840131054@gmail.com", "Makes Easy Support"),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = htmlBody,
                    BodyEncoding = Encoding.UTF8
                };

                message.To.Add(email);
                message.ReplyToList.Add(new MailAddress("support@makeseasy.in"));

                var plainView = AlternateView.CreateAlternateViewFromString(plainTextBody, Encoding.UTF8, "text/plain");
                message.AlternateViews.Add(plainView);

                var htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, "text/html");
                message.AlternateViews.Add(htmlView);

                await smtpClient.SendMailAsync(message);
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email send error: " + ex.Message);
                return 0;
            }
        }



        public async Task SendOTPAsync(string email, string otp)
        {

            try
            {

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("cse.210840131054@gmail.com", "rbwiaxmdusnfaspp") // Use App Password
                };

                string plainTextBody = $"Hello,\n\n" +
                                $"You requested an OTP to verify your identity in Makes Easy.\n\n" +
                                $"Your OTP is: {otp}\n\n" +
                                $"This OTP will expire in 30 minutes.\n\n" +
                                $"If you did not request this, you can safely ignore this email.\n\n" +
                                $"Best regards,\nThe Makes Easy Team\n© {DateTime.UtcNow.Year} Makes Easy.";


                string htmlBody = $@"<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""UTF-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <title>OTP Verification - Makes Easy</title>
    <style>
      body {{
        margin: 0;
        padding: 0;
        background-color: #f8fafc;
        font-family: Arial, sans-serif;
        line-height: 1.6;
        color: #334155;
      }}
      .container {{
        width: 100%;
        background-color: #f8fafc;
        padding: 20px 0;
      }}
      .email-wrapper {{
        max-width: 600px;
        width: 100%;
        background-color: #ffffff;
        margin: 0 auto;
        border-radius: 16px;
        box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
        overflow: hidden;
      }}
      .header {{
        background: black;
        padding: 40px 30px;
        text-align: center;
      }}
      .logo {{
        display: inline-flex;
        align-items: center;
        gap: 12px;
        justify-content: center;
      }}
      .logo-icon {{
        width: 40px;
        height: 40px;
        background-color: rgba(255, 255, 255, 0.2);
        border-radius: 8px;
        display: inline-flex;
        align-items: center;
        justify-content: center;
        font-weight: bold;
        color: #ffffff;
        font-size: 18px;
      }}
      .logo-text {{
        color: #ffffff;
        font-size: 28px;
        font-weight: 600;
      }}
      .tagline {{
        color: rgba(255, 255, 255, 0.9);
        font-size: 16px;
        margin-top: 12px;
      }}
      .content {{
        padding: 50px 40px;
      }}
      .icon-wrapper {{
        text-align: center;
        margin-bottom: 30px;
      }}
      .icon {{
        width: 80px;
        height: 80px;
        background: linear-gradient(135deg, #f0f9ff 0%, #e0f2fe 100%);
        border: 3px solid #0ea5e9;
        border-radius: 50%;
        display: inline-flex;
        align-items: center;
        justify-content: center;
        font-size: 32px;
        margin: 0 auto;
      }}
      .title {{
        text-align: center;
        color: #0f172a;
        font-size: 32px;
        font-weight: 700;
        margin: 20px 0;
      }}
      .subtitle {{
        text-align: center;
        color: #64748b;
        font-size: 18px;
        margin-bottom: 30px;
      }}
      .message-box {{
        background: #f8fafc;
        border-left: 4px solid #3b82f6;
        padding: 24px;
        margin: 30px 0;
        border-radius: 0 8px 8px 0;
      }}
      .otp-container {{
        text-align: center;
        margin: 40px 0;
        padding: 30px;
        background: linear-gradient(135deg, #f8faff 0%, #f0f9ff 100%);
        border-radius: 16px;
        border: 2px solid #e0f2fe;
      }}
      .otp-code {{
        display: inline-block;
        font-size: 36px;
        font-weight: 700;
        color: #1e293b;
        background: #ffffff;
        padding: 20px 40px;
        border-radius: 12px;
        letter-spacing: 8px;
        border: 3px solid black;
        font-family: 'Courier New', monospace;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
      }}
      .otp-label {{
        display: block;
        font-size: 14px;
        color: #64748b;
        margin-bottom: 15px;
        font-weight: 600;
        text-transform: uppercase;
        letter-spacing: 1px;
      }}
      .warning-box {{
        background: #fef3c7;
        border: 1px solid #f59e0b;
        border-radius: 12px;
        padding: 20px;
        margin: 30px 0;
      }}
      .info-box {{
        background: #f1f5f9;
        border-radius: 12px;
        padding: 24px;
        margin: 30px 0;
        border-left: 4px solid black;
      }}
      .footer {{
        background: #f8fafc;
        padding: 30px;
        text-align: center;
        border-top: 1px solid #e2e8f0;
      }}
      .signature {{
        margin-top: 50px;
        padding-top: 30px;
        border-top: 2px solid #f1f5f9;
      }}
    </style>
  </head>
  <body>
    <div class=""container"">
      <table
        class=""email-wrapper""
        cellpadding=""0""
        cellspacing=""0""
        border=""0""
        style=""
          max-width: 600px;
          width: 100%;
          background-color: #ffffff;
          margin: 0 auto;
          border-radius: 16px;
          box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
          overflow: hidden;
        ""
      >
        <tr>
          <td style=""background: black; padding: 40px 30px; text-align: center"">
            <div style=""display: flex; justify-content: center; align-items: center;"">
              <div
                style=""
                  display: inline-block;
                  width: 40px;
                  height: 40px;
                  border-radius: 8px;
                  line-height: 40px;
                  text-align: center;
                  margin-right: 12px;
                ""
              >
        


              </div>

              <span
                style=""
                  color: #ffffff;
                  font-size: 28px;
                  font-weight: 600;
                  vertical-align: middle;
                ""
                >Makes Easy</span
              >
            </div>
            <p
              style=""
                color: rgba(255, 255, 255, 0.9);
                font-size: 16px;
                margin-top: 12px;
                margin-bottom: 0;
              ""
            >
              Your trusted productivity partner
            </p>
          </td>
        </tr>
        <tr>
          <td style=""padding: 50px 40px"">
            <div style=""text-align: center; margin-bottom: 30px"">
              <div
                style=""
                  width: 80px;
                  height: 80px;
                  background: linear-gradient(135deg, #f0f9ff 0%, #e0f2fe 100%);
                  border: 3px solid black;
                  border-radius: 50%;
                  display: inline-block;
                  line-height: 74px;
                  font-size: 32px;
                ""
              >
                🔐
              </div>
            </div>

            <h1
              style=""
                text-align: center;
                color: #0f172a;
                font-size: 32px;
                font-weight: 700;
                margin: 20px 0;
              ""
            >
              Verify Your Account
            </h1>
            <p
              style=""
                text-align: center;
                color: #64748b;
                font-size: 18px;
                margin-bottom: 30px;
              ""
            >
              Enter this verification code to complete your authentication
            </p>

            <div
              style=""
                background: #f8fafc;
                border-left: 4px solid black;
                padding: 24px;
                margin: 30px 0;
                border-radius: 0 8px 8px 0;
              ""
            >
              <p style=""margin: 0; font-size: 16px; color: #475569"">
                <strong>Hello,</strong><br /><br />We've generated a One-Time Password (OTP) for your <strong>Makes Easy</strong> account verification. Please use the code below to complete your authentication.
              </p>
            </div>

            <div
              style=""
                text-align: center;
                margin: 40px 0;
                padding: 30px;
                background: linear-gradient(135deg, #f8faff 0%, #f0f9ff 100%);
                border-radius: 16px;
                border: 2px solid #e0f2fe;
              ""
            >
              <span
                style=""
                  display: block;
                  font-size: 14px;
                  color: #64748b;
                  margin-bottom: 15px;
                  font-weight: 600;
                  text-transform: uppercase;
                  letter-spacing: 1px;
                ""
                >Your OTP Code</span
              >
              <div
                style=""
                  display: inline-block;
                  font-size: 36px;
                  font-weight: 700;
                  color: #1e293b;
                  background: #ffffff;
                  padding: 20px 40px;
                  border-radius: 12px;
                  letter-spacing: 8px;
                  border: 3px solid black;
                  font-family: 'Courier New', monospace;
                  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
                ""
              >
                {otp}
              </div>
            </div>

            <div
              style=""
                background: #fef3c7;
                border: 1px solid #f59e0b;
                border-radius: 12px;
                padding: 20px;
                margin: 30px 0;
              ""
            >
              <p style=""margin: 0; color: #92400e; font-size: 14px"">
                ⚠️ This OTP will expire in <strong>10 minutes</strong> for your security.
              </p>
            </div>

            <div
              style=""
                background: #f1f5f9;
                border-radius: 12px;
                padding: 24px;
                margin: 30px 0;
                border-left: 4px solid black;
              ""
            >
              <p
                style=""
                  margin: 0 0 8px 0;
                  font-size: 14px;
                  color: #475569;
                  font-weight: 600;
                ""
              >
                📋 Instructions:
              </p>
              <ul style=""margin: 8px 0; padding-left: 20px; color: #64748b; font-size: 14px;"">
                <li>Enter this 6-digit code in the verification field</li>
                <li>Do not share this code with anyone</li>
                <li>If you didn't request this, please contact support immediately</li>
              </ul>
            </div>

            <p style=""text-align: center; color: #64748b; font-size: 15px"">
              Didn't request this verification code? You can safely ignore this email or contact our support team if you have concerns.
            </p>

            <div
              style=""
                margin-top: 50px;
                padding-top: 30px;
                border-top: 2px solid #f1f5f9;
              ""
            >
              <p style=""color: #475569; font-size: 16px; margin-bottom: 5px"">
                Best regards,
              </p>
              <p
                style=""
                  color: #0f172a;
                  font-size: 18px;
                  font-weight: 600;
                  margin-top: 0;
                ""
              >
                The Makes Easy Team 🚀
              </p>
            </div>
          </td>
        </tr>
        <tr>
          <td
            style=""
              background: #f8fafc;
              padding: 30px;
              text-align: center;
              border-top: 1px solid #e2e8f0;
            ""
          >
            <p style=""color: #94a3b8; font-size: 12px; margin: 5px 0"">
              &copy; {DateTime.UtcNow.Year} Makes Easy. All rights reserved.
            </p>
            <p style=""color: #cbd5e1; font-size: 11px; margin: 5px 0"">
              This email was sent because an OTP verification was requested for your account.
            </p>
            <p style=""margin: 12px 0 5px 0; color: #94a3b8; font-size: 12px"">
              Need help? Contact us at
              <a href=""mailto:support@makeseasy.in"" style=""color: #3b82f6""
                >support@makeseasy.in</a
              >
            </p>
          </td>
        </tr>
      </table>
    </div>
  </body>
</html>";

                var message = new MailMessage
                {
                    From = new MailAddress("cse.210840131054@gmail.com", "Makes Easy Support"),
                    Subject = "Verify Otp",
                    IsBodyHtml = true,
                    Body = htmlBody,
                    BodyEncoding = Encoding.UTF8
                };

                message.To.Add(email);
                message.ReplyToList.Add(new MailAddress("support@makeseasy.in"));

                var plainView = AlternateView.CreateAlternateViewFromString(plainTextBody, Encoding.UTF8, "text/plain");
                message.AlternateViews.Add(plainView);

                var htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, "text/html");
                message.AlternateViews.Add(htmlView);

                await smtpClient.SendMailAsync(message);

            }
            catch (System.Exception)
            {

                throw;
            }

        }
    }
}

