namespace Invensys.ExternalApi.IntegrationTests.Iserve;

public class IserveTestClientsConfig
{
    public IserveTestClient[]? IserveTestClients { get; set; }
}

public class IserveTestClient
{
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Scope { get; set; }
    public string? TenantId { get; set; }
}
