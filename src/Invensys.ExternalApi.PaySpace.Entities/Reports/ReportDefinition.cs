namespace Invensys.ExternalApi.PaySpace.Entities.Reports;

public class ReportDefinition
{
   public string Id { get; set; } = string.Empty;
   public string Name { get; set; } = string.Empty;
   public string? Country { get; set; }
   public string? Format { get; set; }
}
