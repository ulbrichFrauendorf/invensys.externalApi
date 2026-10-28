using System.Globalization;
using System.Security.Cryptography;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Apis.V2_0;
using Invensys.ExternalApi.PaySpace.Core.Attributes;
using Invensys.ExternalApi.PaySpace.Entities.FixedInfo;
using Invensys.ExternalApi.PaySpace.Entities.Lookups;
using Invensys.ExternalApi.PaySpace.Interfaces.V2_0;
using static Invensys.ExternalApi.IntegrationTests.Testing;

namespace Invensys.ExternalApi.IntegrationTests.PaySpace;

[TestFixture]
[Explicit("Creates synthetic employee records in the configured South African PaySpace staging company.")]
public class PaySpaceEmployeeOnboardingApiTest : BaseTestFixture
{
   [Test]
   public async Task InspectOnboardingLookupsWithoutWriting()
   {
      var setup = await ResolveOnboardingLookupsAsync();

      setup.Nationality.Value.Should().Be("SA");

      setup.Citizenship.Description.Should().Be("South Africa");

      setup.IdentityType.Value.Should().Be("Identity");

      setup.AddressCountry.Value.Should().Be("ZA");

      setup.Province.Value.Should().Be("Gauteng");
   }

   [Test]
   public async Task CreatesEmployeeAndConfiguredOnboardingRecords()
   {
      var (
         credentials,
         companyId,
         frequency,
         employmentDate,
         title,
         race,
         nationality,
         citizenship,
         taxStatus,
         natureOfPerson,
         identityType,
         organizationPosition,
         organizationGroup,
         generalLedger,
         addressCountry,
         province
      ) = await ResolveOnboardingLookupsAsync();

      var suffix = Guid.NewGuid().ToString("N")[..12].ToUpperInvariant();

      var employeeNumber = $"T{suffix}";

      const char southAfricanCitizenDigit = '0';

      var identityNumber = WithCheckDigit(
         $"900101{RandomNumberGenerator.GetInt32(5000, 10000):D4}{southAfricanCitizenDigit}8"
      );

      identityNumber[10].Should().Be(southAfricanCitizenDigit);

      var taxReference = WithCheckDigit($"1{RandomNumberGenerator.GetInt32(10000000, 100000000):D8}"); // 1023425158

      var api = IPaySpaceEmployeeApi();

      TestContext.Progress.WriteLine($"Creating staging test employee {employeeNumber} in company {companyId}.");

      var employee = await api.CreateEmployeeAsync(
         credentials,
         companyId,
         new CreateEmployeeRequest
         {
            EmployeeNumber = employeeNumber,
            CompanyFrequency = frequency.Value,
            FirstName = "Integration",
            LastName = "TestEmployee",
            PreferredName = "Integration Test",
            Initials = "IT",
            Title = title.Value,
            Birthday = new DateOnly(1990, 1, 1),
            Gender = "Male",
            Race = race.Value,
            Nationality = nationality.Value,
            Citizenship = citizenship.Value,
            Email = $"payspace-{suffix.ToLowerInvariant()}@example.invalid",
            WorkNumber = "0115550100",
            Address =
            [
               new CreateEmployeeAddressRequest
               {
                  AddressType = "Physical",
                  StreetNumber = "123",
                  AddressLine1 = "Integration Test Street",
                  AddressLine2 = "Sandton",
                  AddressLine3 = "Johannesburg",
                  AddressCode = "2196",
                  AddressCountry = addressCountry.Value,
                  Province = province.Value
               }
            ],
            ForeignNational = false,
            InitiateWorkFlow = false,
            EtiExempt = true
         }
      );

      employee.EmployeeId.Should().BeGreaterThan(0);

      employee.FirstName.Should().Be("Integration");

      employee.Citizenship.Should().Be(citizenship.Value);

      employee.EmployeeNumber.Should().NotBeNullOrWhiteSpace();

      employee.WorkNumber.Should().Be("0115550100");

      employee.Address.Should().NotBeNull();

      employee.Address.Should().Contain(address => address.AddressType == "Physical");

      var createdEmployeeNumber = employee.EmployeeNumber!;

      TestContext.Progress.WriteLine(
         $"Created employee {createdEmployeeNumber}, ID {employee.EmployeeId}. Records remain in staging after this test."
      );

      var status = await api.CreateEmploymentStatusAsync(
         credentials,
         companyId,
         new CreateEmploymentStatusRequest
         {
            EmployeeNumber = createdEmployeeNumber,
            GroupJoinDate = employmentDate,
            EmploymentDate = employmentDate,
            EmploymentAction = "New",
            TaxStatus = taxStatus.Value,
            NatureOfPerson = natureOfPerson.Value,
            IdentityType = identityType.Value,
            IdNumber = identityNumber,
            TaxReferenceNumber = taxReference
         }
      );

      status.EmploymentStatusId.Should().BeGreaterThan(0);

      status.EmployeeNumber.Should().Be(createdEmployeeNumber);

      status.EmploymentDate.Should().Be(employmentDate);

      var position = await api.CreatePositionAsync(
         credentials,
         companyId,
         new CreatePositionRequest
         {
            EmployeeNumber = createdEmployeeNumber,
            EffectiveDate = employmentDate,
            PositionEffectiveDate = employmentDate,
            OrganizationPositionWithCode = string.IsNullOrWhiteSpace(organizationPosition.UploadCode)
               ? organizationPosition.Value
               : organizationPosition.UploadCode,
            OrganizationGroup = organizationGroup.Value,
            GeneralLedger = generalLedger.Value,
            Comments = "Synthetic API integration test employee"
         }
      );

      position.EmployeePositionId.Should().BeGreaterThan(0);

      position.EmployeeNumber.Should().Be(createdEmployeeNumber);

      position.EffectiveDate.Should().Be(employmentDate);

      position.GeneralLedger.Should().Be(generalLedger.Value);

      var bank = await api.CreateBankDetailAsync(
         credentials,
         companyId,
         new CreateBankDetailRequest
         {
            EmployeeNumber = createdEmployeeNumber,
            PaymentMethod = "Cash",
            SkipValidation = false,
            Comments = "Synthetic API integration test: cash payment"
         }
      );

      bank.BankDetailId.Should().BeGreaterThan(0);

      bank.EmployeeNumber.Should().Be(createdEmployeeNumber);

      bank.PaymentMethod.Should().Be("Cash");

      var storedEmployee = await IPaySpaceEmployeeApi()
         .EmployeeAsync(credentials, companyId, employee.EmployeeId!.Value);

      storedEmployee.EmployeeNumber.Should().Be(createdEmployeeNumber);

      storedEmployee.LastName.Should().Be("TestEmployee");
   }

   private static async Task<OnboardingSetup> ResolveOnboardingLookupsAsync()
   {
      var client = GetPaySpaceTestClientConfig().PaySpaceTestClients!.First();

      var credentials = GetPaySpaceJwtAccessTokenRequest(client);

      var companyId = long.Parse(client.CompanyId!, CultureInfo.InvariantCulture);

      var lookupApi = PaySpaceApiClient();

      async Task<OnboardingLookup> Lookup(string name, params string[] preferred)
      {
         var items = await lookupApi.GetListAsync<OnboardingLookup>(
            credentials,
            PaySpaceApiAttribute.BuildPath(typeof(PaySpaceLookupApi), $"{companyId}/Lookup/{name}")
         );

         var candidates = items.Where(item => !string.IsNullOrWhiteSpace(item.Value)).ToList();

         var selected = preferred
            .Select(value =>
               candidates.FirstOrDefault(item =>
                  string.Equals(item.Value, value, StringComparison.OrdinalIgnoreCase)
                  || string.Equals(item.Description, value, StringComparison.OrdinalIgnoreCase)
               )
            )
            .FirstOrDefault(item => item != null);

         if (preferred.Length == 0)
         {
            selected = candidates.FirstOrDefault();
         }

         selected
            .Should()
            .NotBeNull(
               $"the staging company's {name} lookup must contain a valid value for this South African fixture"
            );

         TestContext.Progress.WriteLine(
            $"{name}: selected {selected!.Value} | {selected.Description} | UploadCode={selected.UploadCode} ({items.Count} available)"
         );

         return selected;
      }

      var frequencies = await IPaySpaceCompanyApi().GetCompanyFrequenciesAsync(credentials, companyId);

      var frequency =
         frequencies.FirstOrDefault(item =>
            !string.IsNullOrWhiteSpace(item.Value) && item.Value == client.FrequencyValue
         ) ?? frequencies.FirstOrDefault(item => item.Value == "Monthly");

      frequency.Should().NotBeNull("the staging company must have a payroll frequency");

      var runs = await lookupApi.GetListAsync<CompanyRun>(
         credentials,
         PaySpaceApiAttribute.BuildPath(
            typeof(PaySpaceLookupApi),
            $"{companyId}/Lookup/CompanyRun?frequency={Uri.EscapeDataString(frequency!.Value!)}"
         )
      );

      var run = runs.Where(item =>
            item.PeriodStartDate.HasValue && string.Equals(item.RunStatus, "Open", StringComparison.OrdinalIgnoreCase)
         )
         .OrderByDescending(item => item.PeriodStartDate)
         .FirstOrDefault();

      run.Should().NotBeNull("the staging company must have an open payroll run before onboarding");

      var employmentDate = DateOnly.FromDateTime(run!.PeriodStartDate!.Value);

      TestContext.Progress.WriteLine(
         $"Frequency: {frequency.Value}; open run: {run.Value}; employment date: {employmentDate:yyyy-MM-dd}"
      );

      var title = await Lookup("Title", "Mr");

      var race = await Lookup("Race", "African");

      var nationality = await Lookup("Nationality", "SA");

      var citizenship = await Lookup("Citizenship", "South Africa");

      var taxStatus = await Lookup("TaxStatus", "Standard Employment (Normal)");

      var natureOfPerson = await Lookup("NatureOfPerson", "A - Individual with ID number or Passport no");

      var identityType = await Lookup("IdentityType", "Identity");

      var organizationPosition = await Lookup("OrganizationPositionWithCode", "Consultant");

      var organizationGroup = await Lookup("OrganizationGroup", "12001");

      var generalLedger = await Lookup($"GeneralLedger?frequency={Uri.EscapeDataString(frequency.Value!)}");

      var addressCountry = await Lookup("AddressCountry", "South Africa", "SA", "ZAF", "ZA");

      var province = await Lookup($"AddressCountry/{Uri.EscapeDataString(addressCountry.Value!)}", "Gauteng");

      return new OnboardingSetup(
         credentials,
         companyId,
         frequency,
         employmentDate,
         title,
         race,
         nationality,
         citizenship,
         taxStatus,
         natureOfPerson,
         identityType,
         organizationPosition,
         organizationGroup,
         generalLedger,
         addressCountry,
         province
      );
   }

   private sealed record OnboardingSetup(
      JwtAccessTokenRequest Credentials,
      long CompanyId,
      CompanyFrequency Frequency,
      DateOnly EmploymentDate,
      OnboardingLookup Title,
      OnboardingLookup Race,
      OnboardingLookup Nationality,
      OnboardingLookup Citizenship,
      OnboardingLookup TaxStatus,
      OnboardingLookup NatureOfPerson,
      OnboardingLookup IdentityType,
      OnboardingLookup OrganizationPosition,
      OnboardingLookup OrganizationGroup,
      OnboardingLookup GeneralLedger,
      OnboardingLookup AddressCountry,
      OnboardingLookup Province
   );

   private static string WithCheckDigit(string digits)
   {
      var sum = 0;

      for (var index = digits.Length - 1; index >= 0; index--)
      {
         var value = digits[index] - '0';

         if ((digits.Length - index) % 2 == 1)
         {
            value *= 2;
         }

         sum += value > 9 ? value - 9 : value;
      }

      return digits + ((10 - sum % 10) % 10).ToString(CultureInfo.InvariantCulture);
   }

   public sealed class OnboardingLookup : GenLookupValue
   {
      public string? UploadCode { get; set; }
   }
}
