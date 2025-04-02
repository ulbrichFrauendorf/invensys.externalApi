using Invensys.ExternalApi.Common.Authentication;
using Invensys.ExternalApi.PaySpace.Entities.Company;

namespace Invensys.ExternalApi.PaySpace.Interfaces;

public interface IPaySpaceAuthenticationProvider : IAuthenticationProvider<GroupCompany[]>;
