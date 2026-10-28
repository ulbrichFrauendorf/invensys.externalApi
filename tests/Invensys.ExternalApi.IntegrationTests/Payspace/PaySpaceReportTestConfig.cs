namespace Invensys.ExternalApi.IntegrationTests.Payspace;

public class PaySpaceReportTestConfig
{
   public string? ReportId { get; set; }
   public string ParametersJson { get; set; } = "{}";
   public string? Frequency { get; set; }
   public bool DisableBookmarks { get; set; }
   public string? OutputDirectory { get; set; }
   public int TimeoutSeconds { get; set; } = 300;
}
