using System.Text.Json.Serialization;
using FluentAssertions;
using Invensys.ExternalApi.Common.Http.Extensions;
using NUnit.Framework;

namespace Invensys.ExternalApi.UnitTests.Common
{
   public class FormDataExtensionsTests
   {
      private class TestObject
      {
         [JsonPropertyName("custom_name")]
         public string? PropertyWithCustomName { get; set; }
         public string? RegularProperty { get; set; }
         public string? NullProperty { get; set; }
      }

      [Test]
      public void ToFormData_ShouldConvertObjectToFormUrlEncodedContent()
      {
         // Arrange
         var testObject = new TestObject
         {
            PropertyWithCustomName = "value1",
            RegularProperty = "value2",
            NullProperty = null
         };

         // Act
         var formData = testObject.ToFormData();

         // Assert
         var formDataDictionary = formData.ReadAsStringAsync().Result.Split('&');
         formDataDictionary.Should().Contain("custom_name=value1");
         formDataDictionary.Should().Contain("RegularProperty=value2");
         formDataDictionary.Should().NotContain("NullProperty=");
      }

      [Test]
      public void ToFormData_ShouldHandleEmptyObject()
      {
         // Arrange
         var emptyObject = new { };

         // Act
         var formData = emptyObject.ToFormData();

         // Assert
         var formDataDictionary = formData.ReadAsStringAsync().Result;
         formDataDictionary.Should().BeEmpty();
      }

      [Test]
      public void ToFormData_ShouldHandleObjectWithAllNullProperties()
      {
         // Arrange
         var testObject = new TestObject
         {
            PropertyWithCustomName = null,
            RegularProperty = null,
            NullProperty = null
         };

         // Act
         var formData = testObject.ToFormData();

         // Assert
         var formDataDictionary = formData.ReadAsStringAsync().Result;
         formDataDictionary.Should().BeEmpty();
      }
   }
}
