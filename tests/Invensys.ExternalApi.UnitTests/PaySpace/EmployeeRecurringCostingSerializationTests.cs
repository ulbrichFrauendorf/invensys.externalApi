using System.Text.Json;
using FluentAssertions;
using Invensys.ExternalApi.Odata;
using Invensys.ExternalApi.PaySpace.Entities.FixedInfo;
using NUnit.Framework;

namespace Invensys.ExternalApi.UnitTests.PaySpace;

[TestFixture]
public class EmployeeRecurringCostingSerializationTests
{
   [Test]
   public void RecurringCostingResponse_DeserializesHeaderAndNestedDetails()
   {
      const string json = """
         {
            "value": [
               {
                  "RecurringCostingSplitHeaderId": 2147483648,
                  "EmployeeNumber": "E001",
                  "FullName": "Example Employee",
                  "EffectiveDate": "2024-01-01",
                  "BasedOnSplitOption": "Example option",
                  "RecurringCostingSplitDetails": [
                     {
                        "RecurringCostingSplitDetailId": 2147483649,
                        "OrganizationGroup": "FIN",
                        "OrganizationGroupDescription": "Finance",
                        "ProjectCode": "PROJECT01",
                        "ProjectActivityCode": "ACTIVITY01",
                        "Percentage": 75.25
                     },
                     {
                        "RecurringCostingSplitDetailId": 2147483650,
                        "OrganizationGroup": null,
                        "OrganizationGroupDescription": null,
                        "ProjectCode": null,
                        "ProjectActivityCode": null,
                        "Percentage": 24.75
                     }
                  ]
               }
            ]
         }
         """;

      var response = JsonSerializer.Deserialize<ListResponse<EmployeeRecurringCosting>>(json);

      response.Should().NotBeNull();
      response!.Value.Should().BeEquivalentTo(new[]
      {
         new EmployeeRecurringCosting
         {
            RecurringCostingSplitHeaderId = 2147483648L,
            EmployeeNumber = "E001",
            FullName = "Example Employee",
            EffectiveDate = new DateTime(2024, 1, 1),
            BasedOnSplitOption = "Example option",
            RecurringCostingSplitDetails = new List<RecurringCostingSplitDetail>
            {
               new()
               {
                  RecurringCostingSplitDetailId = 2147483649L,
                  OrganizationGroup = "FIN",
                  OrganizationGroupDescription = "Finance",
                  ProjectCode = "PROJECT01",
                  ProjectActivityCode = "ACTIVITY01",
                  Percentage = 75.25m
               },
               new() { RecurringCostingSplitDetailId = 2147483650L, Percentage = 24.75m }
            }
         }
      });
   }

   [Test]
   public void RecurringCostingSplitResponse_DeserializesSplitFieldsAndDateTimeOffset()
   {
      const string json = """
         {
            "value": [
               {
                  "RecurringCostingSplitDetailId": 2147483649,
                  "OrganizationGroup": "FIN",
                  "OrganizationGroupDescription": "Finance",
                  "EmployeeNumber": "E001",
                  "Percentage": 75.25,
                  "EffectiveDate": "2019-01-01T00:00:00+02:00"
               }
            ]
         }
         """;

      var response = JsonSerializer.Deserialize<ListResponse<EmployeeRecurringCostingSplit>>(json);

      response.Should().NotBeNull();
      var split = response!.Value.Should().ContainSingle().Subject;
      split.RecurringCostingSplitDetailId.Should().Be(2147483649L);
      split.OrganizationGroup.Should().Be("FIN");
      split.OrganizationGroupDescription.Should().Be("Finance");
      split.EmployeeNumber.Should().Be("E001");
      split.Percentage.Should().Be(75.25m);
      split.EffectiveDate.Should().NotBeNull();
      split.EffectiveDate!.Value.ToUniversalTime().Should().Be(
         new DateTime(2018, 12, 31, 22, 0, 0, DateTimeKind.Utc));
   }
}
