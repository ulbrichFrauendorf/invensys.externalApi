namespace Invensys.ExternalApi.PaySpace.Entities.Reports;

public class ReportGenerationResponse
{
   public string RequestId { get; set; } = string.Empty;
   public string? Message { get; set; }
   public string? Link { get; set; }
}
