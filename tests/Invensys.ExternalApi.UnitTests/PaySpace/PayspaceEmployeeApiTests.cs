using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Apis;
using Invensys.ExternalApi.PaySpace.Entities.FixedInfo;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invensys.ExternalApi.UnitTests.PaySpace
{
    [TestFixture]
    public class PayspaceEmployeeApiTests
    {
        private Mock<IPaySpaceApiClient> _paySpaceApiClientMock;
        private PaySpaceEmployeeApi _paySpaceEmployeeApi;
        private JwtAccessTokenRequest _accessTokenRequest;

        [SetUp]
        public void SetUp()
        {
            _paySpaceApiClientMock = new Mock<IPaySpaceApiClient>();
            _paySpaceEmployeeApi = new PaySpaceEmployeeApi(_paySpaceApiClientMock.Object);
            _accessTokenRequest = new JwtAccessTokenRequest("clientId", "clientSecret", "scope");
        }

        [Test]
        public async Task EmployeeAsync_ShouldReturnEmployee()
        {
            var companyId = 1L;
            var employeeId = 1L;
            var expectedEmployee = new Employee { EmployeeId = employeeId };

            _paySpaceApiClientMock
                .Setup(client => client.GetAsync<Employee>(_accessTokenRequest, $"{companyId}/Employee/{employeeId}"))
                .ReturnsAsync(expectedEmployee);

            var result = await _paySpaceEmployeeApi.EmployeeAsync(_accessTokenRequest, companyId, employeeId);

            result.Should().BeEquivalentTo(expectedEmployee);
        }

        [Test]
        public async Task EmployeeListAsync_ShouldReturnEmployees()
        {
            var companyId = 1L;
            var expectedEmployees = new List<Employee> { new Employee { EmployeeId = 1L } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<Employee>(_accessTokenRequest, $"{companyId}/Employee"))
                .ReturnsAsync(expectedEmployees);

            var result = await _paySpaceEmployeeApi.EmployeeListAsync(_accessTokenRequest, companyId);

            result.Should().BeEquivalentTo(expectedEmployees);
        }

        [Test]
        public async Task EmployeeListAsync_WithEffectiveDate_ShouldReturnEmployees()
        {
            var companyId = 1L;
            var effectiveDate = new DateTime(2023, 1, 1);
            var expectedEmployees = new List<Employee> { new Employee { EmployeeId = 1L } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<Employee>(_accessTokenRequest, $"{companyId}/Employee/effective/{effectiveDate:yyyy-MM-dd}"))
                .ReturnsAsync(expectedEmployees);

            var result = await _paySpaceEmployeeApi.EmployeeListAsync(_accessTokenRequest, companyId, effectiveDate);

            result.Should().BeEquivalentTo(expectedEmployees);
        }

        [Test]
        public async Task EmployeeListAsync_WithEffectiveDateAndEmployeeNumbers_ShouldReturnEmployees()
        {
            var companyId = 1L;
            var effectiveDate = new DateTime(2023, 1, 1);
            var employeeNumbers = new List<string> { "E001" };
            var expectedEmployees = new List<Employee> { new Employee { EmployeeId = 1L } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsyncWithListFilter<Employee>(_accessTokenRequest, $"{companyId}/Employee/effective/{effectiveDate:yyyy-MM-dd}", "EmployeeNumber", employeeNumbers))
                .ReturnsAsync(expectedEmployees);

            var result = await _paySpaceEmployeeApi.EmployeeListAsync(_accessTokenRequest, companyId, effectiveDate, employeeNumbers);

            result.Should().BeEquivalentTo(expectedEmployees);
        }

        [Test]
        public async Task EmployeePositionsAsync_ShouldReturnEmployeePositions()
        {
            var companyId = 1L;
            var effectiveDate = new DateTime(2023, 1, 1);
            var expectedPositions = new List<EmployeePosition> { new EmployeePosition { EmployeePositionId = 1 } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<EmployeePosition>(_accessTokenRequest, $"{companyId}/EmployeePosition/effective/{effectiveDate:yyyy-MM-dd}"))
                .ReturnsAsync(expectedPositions);

            var result = await _paySpaceEmployeeApi.EmployeePositionsAsync(_accessTokenRequest, companyId, effectiveDate);

            result.Should().BeEquivalentTo(expectedPositions);
        }

        [Test]
        public async Task EmployeeEmploymentStatusAsync_ShouldReturnEmploymentStatuses()
        {
            var companyId = 1L;
            var effectiveDate = new DateTime(2023, 1, 1);
            var expectedStatuses = new List<EmploymentStatus> { new EmploymentStatus { EmployeeNumber = "E001" } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<EmploymentStatus>(_accessTokenRequest, $"{companyId}/EmployeeEmploymentStatus/effective/{effectiveDate:yyyy-MM-dd}"))
                .ReturnsAsync(expectedStatuses);

            var result = await _paySpaceEmployeeApi.EmployeeEmploymentStatusAsync(_accessTokenRequest, companyId, effectiveDate);

            result.Should().BeEquivalentTo(expectedStatuses);
        }

        [Test]
        public async Task EmployeeBankDetailAsync_ShouldReturnEmployeeBankDetails()
        {
            var companyId = 1L;
            var expectedBankDetails = new List<EmployeeBankDetail> { new EmployeeBankDetail { BankDetailId = 1 } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<EmployeeBankDetail>(_accessTokenRequest, $"{companyId}/EmployeeBankDetail"))
                .ReturnsAsync(expectedBankDetails);

            var result = await _paySpaceEmployeeApi.EmployeeBankDetailAsync(_accessTokenRequest, companyId);

            result.Should().BeEquivalentTo(expectedBankDetails);
        }

        [Test]
        public async Task EmployeeAddressAsync_ShouldReturnEmployeeAddresses()
        {
            var companyId = 1L;
            var employeeNumber = "E001";
            var expectedAddresses = new List<Address> { new Address { AddressId = 1 } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<Address>(_accessTokenRequest, $"{companyId}/EmployeeAddress/{employeeNumber}"))
                .ReturnsAsync(expectedAddresses);

            var result = await _paySpaceEmployeeApi.EmployeeAddressAsync(_accessTokenRequest, companyId, employeeNumber);

            result.Should().BeEquivalentTo(expectedAddresses);
        }
    }
}
