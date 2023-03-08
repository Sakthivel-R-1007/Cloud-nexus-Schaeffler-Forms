namespace Schaeffler.Service.Interfaces
{
    public interface IUtilityService
    {
        string SendEmail(string Subject, string Content, string To, bool ISBCC, string FromEmail, string CCEmail = "",string Country="");
    }
}
