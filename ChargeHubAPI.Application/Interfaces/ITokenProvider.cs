using ChargeHubAPI.Domain.Entities;

namespace ChargeHubAPI.Application.Interfaces;

public interface ITokenProvider
{
    string GenerateToken(User user);
}




