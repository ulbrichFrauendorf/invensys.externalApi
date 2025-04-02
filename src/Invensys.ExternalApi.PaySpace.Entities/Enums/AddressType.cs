using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum AddressType
{
   [Description("Physical")]
   Physical = 1,

   [Description("PO Box")]
   POBox = 2,

   [Description("Private Bag")]
   PrivateBag = 3,

   [Description("Street")]
   Street = 4
}
