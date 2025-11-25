namespace ChargeHubAPI.Application.Templates;

public static class EmailTemplateBuilder
{
    public static string BuildVerificationTemplate(string recipientName, string code, string purpose)
{
    return $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"" />
    <title>S-Charge Verification</title>
    <style>
        body {{ font-family: Arial, sans-serif; background-color: #f5f5f5; padding: 24px; }}
        .card {{ max-width: 480px; margin: auto; background: #ffffff; border-radius: 12px; padding: 32px; box-shadow: 0 10px 40px rgba(0,0,0,0.1); }}
        .logo {{ text-align: center; margin-bottom: 24px; }}
        .title {{ color: #f59e0b; /* sun color from logo */ font-size: 22px; margin-bottom: 16px; }}
        .code {{ font-size: 36px; letter-spacing: 8px; color: #1e40af; /* blue from solar panel */ margin: 24px 0; }}
        .footer {{ font-size: 13px; color: #6b7280; margin-top: 32px; text-align: center; }}
    </style>
</head>
<body>
    <div class=""card"">
      
        <div class=""title"">S-Charge Verification</div>
        <p>Hi {recipientName},</p>
        <p>Use the verification code below to {purpose}.</p>
        <div class=""code"">{code}</div>
        <p style=""color:#374151;"">This code expires in 15 minutes. If you did not request it, please ignore this email.</p>
        <div class=""footer"">&copy; {DateTime.UtcNow.Year} ChargeHub Wireless Charging • Secure Energy On Demand</div>
    </div>
</body>
</html>";
}

}

