using System.Reflection;
using Ardalis.GuardClauses;

namespace Invensys.ExternalApi.PaySpace.Core.Attributes;

/// <summary>Declares an API collection's version and subdirectory relative to the configured domain.</summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PaySpaceApiAttribute : Attribute
{
   public PaySpaceApiAttribute(string version, string subdirectory = "")
   {
      Guard.Against.NullOrWhiteSpace(version, nameof(version));

      Version = version.Trim('/');

      Subdirectory = subdirectory.Trim('/');
   }

   public string Version { get; }
   public string Subdirectory { get; }

   /// <summary>Resolves a collection-relative path using the attribute declared on its type.</summary>
   public static string BuildPath(Type type, string path)
   {
      Guard.Against.Null(type, nameof(type));

      Guard.Against.NullOrWhiteSpace(path, nameof(path));

      var paySpaceApiAttribute =
         type.GetCustomAttribute<PaySpaceApiAttribute>()
         ?? throw new InvalidOperationException($"Declare PaySpaceApiAttribute on {type.Name}.");

      Guard.Against.InvalidInput(
         path,
         nameof(path),
         value => !Uri.TryCreate(value, UriKind.Absolute, out _),
         "Collection paths must be relative to the API domain."
      );

      return string.Join(
         '/',
         new[] { paySpaceApiAttribute.Subdirectory, paySpaceApiAttribute.Version, path.TrimStart('/') }.Where(segment =>
            !string.IsNullOrEmpty(segment)
         )
      );
   }
}
