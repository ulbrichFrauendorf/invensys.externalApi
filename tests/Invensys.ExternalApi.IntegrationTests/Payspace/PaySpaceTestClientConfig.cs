namespace Invensys.ExternalApi.IntegrationTests.Payspace;

public class PaySpaceTestClient
{
   public string? ReportName { get; set; }
   public string? ClientId { get; set; }
   public string? ClientSecret { get; set; }
   public string? Scope { get; set; }
   public string? CompanyId { get; set; }
   public string? CompanyDescription { get; set; }
   public string? FrequencyValue { get; set; }
   public string? PayrunValue { get; set; }
   public List<string> PayrunDescriptions { get; set; } = [];
}

public class PaySpaceTestClientsConfig
{
   public List<PaySpaceTestClient>? PaySpaceTestClients { get; set; }
}
