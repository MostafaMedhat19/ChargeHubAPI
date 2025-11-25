namespace ChargeHubAPI.Application.Interfaces;

public interface IEmailSender
{
    Task SendVerificationCodeAsync(string email, string subject, string htmlBody, CancellationToken cancellationToken);
}


