namespace Skeleton.Core.Infrastructure.Communications.Email.Templates;

public static class WorkAssignedTemplate
{
    private const string LogoUrl = "https://your-logo-url-here/logo.png";

    public static string Render(
        string documentTitle,
        string heading,
        string subheading,
        string bodyHtml,
        string actionUrl,
        string actionText)
    {
        return
            $$"""
              <!DOCTYPE html>
              <html lang="en">
              <head>
                  <meta charset="UTF-8">
                  <meta name="viewport" content="width=device-width, initial-scale=1.0">
                  <meta name="color-scheme" content="light dark">
                  <meta name="supported-color-schemes" content="light dark">
                  <title>{{documentTitle}}</title>

                  <style>
                      :root {
                          color-scheme: light dark;
                          supported-color-schemes: light dark;
                      }

                      @media (prefers-color-scheme: dark) {
                          .email-body {
                              background-color: #1f1f1f !important;
                          }

                          .email-card {
                              background-color: #2b2b2b !important;
                              border-color: #555555 !important;
                          }

                          .email-title,
                          .email-text {
                              color: #f2f2f2 !important;
                          }

                          .email-subtitle {
                              color: #7db9ff !important;
                          }

                          .email-footer {
                              color: #b5b5b5 !important;
                          }

                          .email-divider {
                              border-color: #555555 !important;
                          }
                      }
                  </style>
              </head>

              <body class="email-body" style="font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; color: #333333;">
                  <table role="presentation" width="100%" cellspacing="0" cellpadding="0" border="0" style="background-color: #f4f4f4;" class="email-body">
                      <tr>
                          <td align="center" style="padding: 24px 12px;">
                              <table role="presentation"
                                     width="100%"
                                     cellspacing="0"
                                     cellpadding="0"
                                     border="0"
                                     class="email-card"
                                     style="max-width: 600px; background-color: #ffffff; border: 1px solid #dddddd; border-radius: 10px; overflow: hidden;">

                                  <tr>
                                      <td align="center" style="padding: 32px 28px 16px 28px;">
                                          <img src="{{LogoUrl}}"
                                               alt="Logo"
                                               width="300"
                                               style="display: block; width: 300px; max-width: 70%; height: auto; border: 0;">
                                      </td>
                                  </tr>

                                  <tr>
                                      <td style="padding: 0 28px;">
                                          <h1 class="email-title" style="font-size: 24px; line-height: 1.3; color: #333333; text-align: center; margin: 10px 0 4px 0; font-weight: bold;">
                                              {{heading}}
                                          </h1>

                                          <h2 class="email-subtitle" style="font-size: 17px; line-height: 1.4; color: #005baa; text-align: center; margin: 0 0 24px 0; font-weight: normal;">
                                              {{subheading}}
                                          </h2>

                                          <table role="presentation" width="100%" cellspacing="0" cellpadding="0" border="0">
                                              <tr>
                                                  <td class="email-divider" style="border-top: 1px solid #e3e3e3; padding-top: 24px;"></td>
                                              </tr>
                                          </table>

                                          {{bodyHtml}}

                                          <table role="presentation" cellspacing="0" cellpadding="0" border="0" align="center" style="margin: 30px auto;">
                                              <tr>
                                                  <td align="center" bgcolor="#005baa" style="border-radius: 6px;">
                                                      <a href="{{actionUrl}}"
                                                         style="display: inline-block; padding: 13px 22px; font-size: 16px; line-height: 1.2; font-family: Arial, sans-serif; color: #ffffff; text-decoration: none; font-weight: bold; background-color: #005baa; border: 1px solid #005baa; border-radius: 6px;">
                                                          {{actionText}}
                                                      </a>
                                                  </td>
                                              </tr>
                                          </table>

                                          <p class="email-text" style="font-size: 16px; line-height: 1.5; color: #333333; margin: 0 0 16px 0;">
                                              Best regards,
                                          </p>

                                          <p class="email-text" style="font-size: 16px; line-height: 1.5; color: #333333; margin: 0 0 28px 0;">
                                              The Team
                                          </p>
                                      </td>
                                  </tr>

                                  <tr>
                                      <td align="center" style="padding: 18px 28px 28px 28px;">
                                          <p class="email-footer" style="font-size: 13px; line-height: 1.4; color: #888888; margin: 0;">
                                              &copy; 2026 Your Company. All rights reserved.
                                          </p>
                                      </td>
                                  </tr>

                              </table>
                          </td>
                      </tr>
                  </table>
              </body>
              </html>
              """;
    }

    public static string Paragraph(string html, bool isLast = false)
    {
        var marginBottom = isLast ? "24px" : "16px";

        return
            $$"""
              <p class="email-text" style="font-size: 16px; line-height: 1.5; color: #333333; margin: 0 0 {{marginBottom}} 0;">
                  {{html}}
              </p>
              """;
    }
}
