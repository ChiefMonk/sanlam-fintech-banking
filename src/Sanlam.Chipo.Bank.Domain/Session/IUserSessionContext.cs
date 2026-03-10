namespace Sanlam.Chipo.Bank.Domain.Session;

public interface IUserSessionContext
{
    long UserId { get; set; }
    string UserName { get; set; }

    Guid SessionKey { get; set; }
}

public class UserSessionContext : IUserSessionContext
{
    public long UserId { get; set; } = 100;
    public string UserName { get; set; } = "chipoh";

    public Guid SessionKey { get; set; } = Guid.NewGuid();
}