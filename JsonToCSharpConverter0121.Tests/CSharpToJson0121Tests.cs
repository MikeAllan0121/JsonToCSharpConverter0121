using JsonToCSharpConverter0121;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;


using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.Formatting;


namespace JsonToCSharpConverter0121.Tests;


public class CSharpToJson0121Tests
{
    [Fact]
    public void Execute_ValidJson_Success()
    {
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

        var csharp = JsonToCSharpConverter.CreateFullText(json);
        Assert.NotNull(csharp);
    }


    [Fact]
    public void Execute_NullAllProperties_Success()
    {
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

        var csharp = JsonToCSharpConverter.CreateFullText(json, opt => opt.Nullable = Nullable.AllProperties);
        Assert.NotNull(csharp);
    }


    [Fact]
    public void Execute_NeverOnNewLine_Success()
    {
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

        var csharp = JsonToCSharpConverter.CreateFullText(json, opt => opt.HavePropertiesOnNewLine = HavePropertiesOnNewLine.No());
        Assert.NotNull(csharp);
    }

    [Fact]
    public void Execute_ValidLargeJson_Success()
    {
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

        var csharp = JsonToCSharpConverter.CreateFullText(json);
        Assert.NotNull(csharp);
    }

    [Fact]
    public void Execute_ReadOnlyCollection_Success()
    {
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

        var csharp = JsonToCSharpConverter.CreateFullText(json, opt => opt.ReadOnlyCollections = CollectionType.ReadOnlyCollection);
        Assert.NotNull(csharp);
    }

    [Fact]
    public void Execute_NestedPositionName_Success()
    {
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

        var csharp = JsonToCSharpConverter.CreateFullText(json, opt => opt.TypeNamingConvention = TypeNamingConvention.NestedPositionName);
        Assert.NotNull(csharp);
    }

    [Fact]
    public void Execute_IllegalName_Success()
    {
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

        var csharp = JsonToCSharpConverter.CreateFullText(json, opt => opt.TypeNamingConvention = TypeNamingConvention.NestedPositionName);
        Assert.NotNull(csharp);
    }
}

