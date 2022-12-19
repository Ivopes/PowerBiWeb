using Microsoft.PowerBI.Api;
using Microsoft.Rest;

namespace PowerBiWeb.Server.Utilities.PowerBI
{
    public static class PowerBiUtility
    {
        public static PowerBIClient GetPowerBIClient(AadService service)
        {
            var tokenCredentials = new TokenCredentials(service.GetAccessToken(), "Bearer");
            return new PowerBIClient(new Uri(service.PowerBiUrl), tokenCredentials);
        }
    }
}
