namespace Dagger.HostedServices.Application.Configuration
{
    public class Settings
    {
        public readonly string SECTION = "Settings";
        public int Polling {get; set;} = 10;
    }
}