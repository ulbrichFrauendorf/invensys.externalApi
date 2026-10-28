namespace Invensys.ExternalApi.PaySpace.Core.Configuration;

public class ReportPollingOptions
{
   public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);
   public TimeSpan InitialInterval { get; set; } = TimeSpan.FromSeconds(2);
   public TimeSpan MaximumInterval { get; set; } = TimeSpan.FromSeconds(15);
}
