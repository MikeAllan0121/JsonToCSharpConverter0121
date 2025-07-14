using Extensions0121.Result0121;

namespace JsonToCSharpConverter0121.Tests;


public class CSharpToJson0121Tests
{
    [Fact]
    public void Execute_ValidJson_Success()
    {
        // arrange
        string json = """
{
    "name": "Alice",
    "age": 25,
    "height": 5.6,
    "isStudent": false,
    "hobbies": ["reading", "gaming", "swimming"],
    "address": {
        "street": "123 Main St",
        "city": "London",
        "postalCode": "SW1A 1AA",
        "residents": ["Alice", "Bob", "Charlie"]
    },
    "grades": [85, 90, 78],
    "metadata": null
}
""";
        // act
        var csharp = JsonToCSharpConverter.CreateFullText(json);
        // assert
        var output = """
            public record Root(
                string Name,
                double Age,
                double Height,
                bool IsStudent,
                List<string> Hobbies,
                Address Address,
                List<double> Grades,
                object Metadata
            );

            public record Address(string Street, string City, string PostalCode, List<string> Residents);
            """;
        csharp.AssertOutput(output);
    }

    [Fact]
    public void Execute_NullAllProperties_Success()
    {
        // arrange
        string json = """
{
    "name": "Alice",
    "age": 25,
    "height": 5.6,
    "isStudent": false,
    "hobbies": ["reading", "gaming", "swimming"],
    "address": {
        "street": "123 Main St",
        "city": "London",
        "postalCode": "SW1A 1AA",
        "residents": ["Alice", "Bob", "Charlie"]
    },
    "grades": [85, 90, 78],
    "metadata": null
}
""";

        // act
        var csharp = JsonToCSharpConverter.CreateFullText(json, opt => opt.Nullable = Nullable.AllProperties);

        // assert
        var output = """
            public record Root(
                string? Name,
                double? Age,
                double? Height,
                bool? IsStudent,
                List<string>? Hobbies,
                Address? Address,
                List<double>? Grades,
                object? Metadata
            );

            public record Address(string? Street, string? City, string? PostalCode, List<string>? Residents);
            """;
        csharp.AssertOutput(output);
    }


    [Fact]
    public void Execute_NeverOnNewLine_Success()
    {
        // arrange
        string json = """
{
    "name": "Alice",
    "age": 25,
    "height": 5.6,
    "isStudent": false,
    "hobbies": ["reading", "gaming", "swimming"],
    "address": {
        "street": "123 Main St",
        "city": "London",
        "postalCode": "SW1A 1AA",
        "residents": ["Alice", "Bob", "Charlie"]
    },
    "grades": [85, 90, 78],
    "metadata": null
}
""";

        // act
        var csharp = JsonToCSharpConverter.CreateFullText(json, opt => opt.PropertyCreationMethod = PropertyCreationMethod.PositionalParameters(HavePropertiesOnNewLine.No()));

        // assert
        var output = """
            public record Root(string Name, double Age, double Height, bool IsStudent, List<string> Hobbies, Address Address, List<double> Grades, object Metadata);

            public record Address(string Street, string City, string PostalCode, List<string> Residents);
            """;
        csharp.AssertOutput(output);
    }

    [Fact]
    public void Execute_ValidLargeJson_Success()
    {
        // arrange
        string json = """
{
  "users": [
    {
      "id": 1,
      "name": "Alice Johnson",
      "email": "alice.johnson@example.com",
      "address": {
        "street": "123 Main St",
        "city": "New York",
        "state": "NY",
        "postalCode": "10001"
      },
      "phoneNumbers": [
        "+1-555-1234",
        "+1-555-5678"
      ],
      "preferences": {
        "theme": "dark",
        "notifications": {
          "email": true,
          "sms": false,
          "push": true
        }
      },
      "orders": [
        {
          "orderId": 101,
          "date": "2025-06-01",
          "total": 49.99,
          "items": [
            {"productId": "A001", "name": "Laptop", "price": 999.99},
            {"productId": "A002", "name": "Mouse", "price": 29.99}
          ]
        }
      ]
    },
    {
      "id": 2,
      "name": "Bob Smith",
      "email": "bob.smith@example.com",
      "address": {
        "street": "456 Elm St",
        "city": "San Francisco",
        "state": "CA",
        "postalCode": "94103"
      },
      "phoneNumbers": [
        "+1-555-9876"
      ],
      "preferences": {
        "theme": "light",
        "notifications": {
          "email": false,
          "sms": true,
          "push": false
        }
      },
      "orders": [
        {
          "orderId": 202,
          "date": "2025-06-03",
          "total": 199.99,
          "items": [
            {"productId": "B001", "name": "Smartphone", "price": 799.99}
          ]
        }
      ]
    }
  ],
  "metadata": {
    "version": "1.0",
    "generatedAt": "2025-06-06T15:30:00Z",
    "source": "CraftSharp User API"
  }
}
""";

        // act
        var csharp = JsonToCSharpConverter.CreateFullText(json);
        // assert
        var output = """
            public record Root(List<Users> Users, Metadata Metadata);

            public record Metadata(string Version, string GeneratedAt, string Source);

            public record Users(
                double Id,
                string Name,
                string Email,
                Address Address,
                List<string> PhoneNumbers,
                Preferences Preferences,
                List<Orders> Orders
            );

            public record Orders(double OrderId, string Date, double Total, List<Items> Items);

            public record Items(string ProductId, string Name, double Price);

            public record Preferences(string Theme, Notifications Notifications);

            public record Notifications(bool Email, bool Sms, bool Push);

            public record Address(string Street, string City, string State, string PostalCode);
            """;
        csharp.AssertOutput(output);
    }

    [Fact]
    public void Execute_ReadOnlyCollection_Success()
    {
        // arrange
        string json = """
{
  "users": [
    {
      "id": 1,
      "name": "Alice Johnson",
      "email": "alice.johnson@example.com",
      "address": {
        "street": "123 Main St",
        "city": "New York",
        "state": "NY",
        "postalCode": "10001"
      }
    },
    {
      "id": 2,
      "name": "Bob Smith",
      "email": "bob.smith@example.com",
      "address": {
        "street": "456 Elm St",
        "city": "San Francisco",
        "state": "CA",
        "postalCode": "94103"
      }
    }
  ],
  "metadata": {
    "version": "1.0",
    "generatedAt": "2025-06-06T15:30:00Z",
    "source": "CraftSharp User API"
  }
}
""";

        // act
        var csharp = JsonToCSharpConverter.CreateFullText(json, opt => opt.ReadOnlyCollections = CollectionType.ReadOnlyCollection);
        // assert
        var output = """
            public record Root(ReadOnlyCollection<Users> Users, Metadata Metadata);

            public record Metadata(string Version, string GeneratedAt, string Source);

            public record Users(double Id, string Name, string Email, Address Address);

            public record Address(string Street, string City, string State, string PostalCode);
            """;
        csharp.AssertOutput(output);
    }

    [Fact]
    public void Execute_NestedPositionName_Success()
    {
        // arrange
        string json = """
{
  "users": [
    {
      "id": 1,
      "name": "Alice Johnson",
      "email": "alice.johnson@example.com",
      "address": {
        "street": "123 Main St",
        "city": "New York",
        "state": "NY",
        "postalCode": "10001"
      }
    },
    {
      "id": 2,
      "name": "Bob Smith",
      "email": "bob.smith@example.com",
      "address": {
        "street": "456 Elm St",
        "city": "San Francisco",
        "state": "CA",
        "postalCode": "94103"
      }
    }
  ],
  "metadata": {
    "version": "1.0",
    "generatedAt": "2025-06-06T15:30:00Z",
    "source": "CraftSharp User API"
  }
}
""";

        // act
        var csharp = JsonToCSharpConverter.CreateFullText(json, opt => opt.TypeNamingConvention = TypeNamingConvention.NestedPositionName);
        // assert
        var output = """
            public record Root(List<RootUsers> Users, RootMetadata Metadata);

            public record RootMetadata(string Version, string GeneratedAt, string Source);

            public record RootUsers(double Id, string Name, string Email, RootUsersAddress Address);

            public record RootUsersAddress(string Street, string City, string State, string PostalCode);
            """;
        csharp.AssertOutput(output);
    }

    [Fact]
    public void Execute_IllegalName_Success()
    {
        // arrange
        string json = """
{
    "1isStudent": false,
    "#address": {
        "str2eet": "123 Main St",
        "1city": "London",
        "postal Code": "SW1A 1AA",
        "residents ": ["Alice", "Bob", "Charlie"]
    },
    "grades": [85, 90, 78]
}
""";

        // act
        var csharp = JsonToCSharpConverter.CreateFullText(json, opt => opt.TypeNamingConvention = TypeNamingConvention.NestedPositionName);
        // assert
        var output = """
            public record Root([JsonProperty("1isStudent")] bool _1isStudent, [JsonProperty("#address")] RootAddress Address, List<double> Grades);

            public record RootAddress(string Str2eet, [JsonProperty("1city")] string _1city, [JsonProperty("postal Code")] string PostalCode, [JsonProperty("residents ")] List<string> Residents);
            """;
        csharp.AssertOutput(output);
    }

    [Fact]
    public void Execute_InitAutoProperties_Success()
    {
        // arrange
        string json = """
{
    "name": "Alice",
    "age": 25,
    "height": 5.6,
    "isStudent": false,
    "hobbies": ["reading", "gaming", "swimming"],
    "address": {
        "street": "123 Main St",
        "city": "London",
        "postalCode": "SW1A 1AA",
        "residents": ["Alice", "Bob", "Charlie"]
    },
    "grades": [85, 90, 78],
    "metadata": null
}
""";

        // act
        var csharp = JsonToCSharpConverter.CreateFullText(json, opt => opt.PropertyCreationMethod = PropertyCreationMethod.InitAutoProperties());
        // assert
        var output = """
            public record Root
            {
                public string Name { get; init; }
                public double Age { get; init; }
                public double Height { get; init; }
                public bool IsStudent { get; init; }
                public List<string> Hobbies { get; init; }
                public Address Address { get; init; }
                public List<double> Grades { get; init; }
                public object Metadata { get; init; }
            }

            public record Address
            {
                public string Street { get; init; }
                public string City { get; init; }
                public string PostalCode { get; init; }
                public List<string> Residents { get; init; }
            }
            """;
        csharp.AssertOutput(output);
    }


    [Fact]
    public void Execute_InitAutoPropertiesIllegalName_Success()
    {
        // arrange
        string json = """
{
    "isStudent": false,
    "#address": {
        "str2eet": "123 Main St",
        "1city": "London",
        "postal Code": "SW1A 1AA",
        "residents ": ["Alice", "Bob", "Charlie"]
    },
    "grades": [85, 90, 78]
}
""";

        // act
        var csharp = JsonToCSharpConverter.CreateFullText(json, opt => {
            opt.TypeNamingConvention = TypeNamingConvention.NestedPositionName;
            opt.PropertyCreationMethod = PropertyCreationMethod.InitAutoProperties();
        });
        // assert
        var output = """
            public record Root
            {
                public bool IsStudent { get; init; }

                [JsonProperty("#address")]
                public RootAddress Address { get; init; }
                public List<double> Grades { get; init; }
            }

            public record RootAddress
            {
                public string Str2eet { get; init; }

                [JsonProperty("1city")]
                public string _1city { get; init; }

                [JsonProperty("postal Code")]
                public string PostalCode { get; init; }

                [JsonProperty("residents ")]
                public List<string> Residents { get; init; }
            }
            """;
        csharp.AssertOutput(output);
    }
}

