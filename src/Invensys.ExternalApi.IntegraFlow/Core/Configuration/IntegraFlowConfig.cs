using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invensys.ExternalApi.IntegraFlow.Core.Configuration
{
   public class IntegraFlowConfig
   {
      public required string AuthorizationUrl { get; set; }
      public required string ApiBaseUrl { get; set; }
   }

}
