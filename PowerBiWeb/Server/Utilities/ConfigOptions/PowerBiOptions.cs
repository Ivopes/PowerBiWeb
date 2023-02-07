namespace PowerBiWeb.Server.Utilities.ConfigOptions
{
    public class PowerBiOptions
    {
        public string WorkspaceId { get; set; } = string.Empty;
        public string ReportId { get; set; } = string.Empty;

        // Can be set to 'MasterUser' or 'ServicePrincipal'
        public string AuthenticationMode { get; set; } = string.Empty;

        // URL used for initiating authorization request
        public string AuthorityUrl { get; set; } = string.Empty;

        // Client Id (Application Id) of the AAD app
        public string ClientId { get; set; } = string.Empty;

        // Id of the Azure tenant in which AAD app is hosted. Required only for Service Principal authentication mode.
        public string TenantId { get; set; } = string.Empty;

        // ScopeBase of AAD app. Use the below configuration to use all the permissions provided in the AAD app through Azure portal.
        public string[] ScopeBase { get; set; } = Array.Empty<string>();

        // Master user email address. Required only for MasterUser authentication mode.
        public string PbiUsername { get; set; } = string.Empty;

        // Master user email password. Required only for MasterUser authentication mode.
        public string PbiPassword { get; set; } = string.Empty;

        // Client Secret (App Secret) of the AAD app. Required only for ServicePrincipal authentication mode.
        public string ClientSecret { get; set; } = string.Empty;

        public string PowerBiApiUrl { get; set; } = string.Empty;
    }
}
