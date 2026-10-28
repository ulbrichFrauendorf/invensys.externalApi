namespace Invensys.ExternalApi.PaySpace.Entities.Reports;

public class ReportGenerationStatus
{
   public bool IsReady { get; set; }
   public string? Reason { get; set; }
   public string? ReportUrl { get; set; }
}
