using FluentAssertions;
using Invensys.Api.Common.Authentication.Exceptions;
using Invensys.Api.Common.Authentication.Models.Request;
using Invensys.Api.Common.Authentication.Validators;
using NUnit.Framework;

namespace UnitTests;

[TestFixture]
public class AccessTokenRequestValidatorTests
{
    [Test]
    public void Validate_ShouldReturnValidRequest_WhenRequestIsValid()
    {
        // Arrange
        var request = new AccessTokenRequest();

        // Act
        var result = AccessTokenRequestValidator.Validate<AccessTokenRequest>(request);

        // Assert
        result.Should().Be(request);
    }

    [Test]
    public void Validate_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Arrange
        AccessTokenRequest request = null!;

        // Act
        Action act = () => AccessTokenRequestValidator.Validate<AccessTokenRequest>(request);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'request')");
    }

    [Test]
    public void Validate_ShouldThrowAuthenticationConfigurationException_WhenRequestIsOfInvalidType()
    {
        // Arrange
        var request = new AccessTokenRequest();

        // Act
        Action act = () => AccessTokenRequestValidator.Validate<AnotherAccessTokenRequest>(request);

        // Assert
        act.Should().Throw<AuthenticationConfigurationException>().WithMessage("AnotherAccessTokenRequest object expected");
    }

    private class AnotherAccessTokenRequest : AccessTokenRequest
    {
    }
}
