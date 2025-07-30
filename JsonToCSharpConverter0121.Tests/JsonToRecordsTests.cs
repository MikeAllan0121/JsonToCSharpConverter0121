using JsonToCSharpConverter0121.Internal.Records;
using JsonToCSharpConverter0121.Internal.Records.Naming;
using JsonToCSharpConverter0121.Internal.Records.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JsonToCSharpConverter0121.Tests;

public class JsonToRecordsTests
{
    [Fact]
    public void Convert_SingleObjectWithPrimitives_ProducesExpectedRecord()
    {
        // arrange
        string json = """
    {
        "name": "Alice",
        "age": 30,
        "isStudent": true
    }
    """;
        var options = new RecordOptions("Root", TypeNamingConvention.PropertyName);
        using var doc = JsonDocument.Parse(json);

        // act
        var result = JsonToRecords.Convert(doc, options);

        // assert
        var record = result[new NameBuilder("Root", TypeNamingConvention.PropertyName)];
        var properties = record.Properties;

        Assert.Equal("Root", record.Name.Name); // or .CsharpName if that’s your intended display

        var simpleProps = properties.OfType<SimpleProperty>().ToList();

        // Assert for "Name"
        var nameProp = simpleProps.FirstOrDefault(p => p.Type == "string" && p.Name.CsharpName == "Name");
        Assert.NotNull(nameProp);
        Assert.Equal("string", nameProp.Type);
        Assert.Equal("Name", nameProp.Name.CsharpName);

        // Assert for "Age"
        var ageProp = simpleProps.FirstOrDefault(p => p.Type == "double" && p.Name.CsharpName == "Age");
        Assert.NotNull(ageProp);
        Assert.Equal("double", ageProp.Type);
        Assert.Equal("Age", ageProp.Name.CsharpName);

        // Assert for "IsStudent"
        var isStudentProp = simpleProps.FirstOrDefault(p => p.Type == "bool" && p.Name.CsharpName == "IsStudent");
        Assert.NotNull(isStudentProp);
        Assert.Equal("bool", isStudentProp.Type);
        Assert.Equal("IsStudent", isStudentProp.Name.CsharpName);

    }

    [Fact]
    public void Convert_NestedObjectsWithMixedTypes_ProducesExpectedRecords()
    {
        // arrange
        string json = """
    {
        "user": {
            "id": 101,
            "profile": {
                "name": "Charlie",
                "contact": {
                    "email": "charlie@example.com",
                    "phone": "123-456-7890"
                }
            }
        },
        "status": "active",
        "metadata": {
            "createdBy": "admin",
            "createdAt": "2025-07-24T19:00:00Z"
        }
    }
    """;
        var options = new RecordOptions("Root", TypeNamingConvention.PropertyName);
        using var doc = JsonDocument.Parse(json);

        // act
        var result = JsonToRecords.Convert(doc, options);

        // assert
        var root = result[new NameBuilder("Root", TypeNamingConvention.PropertyName)];
        var user = result[new NameBuilder("User", TypeNamingConvention.PropertyName)];
        var profile = result[new NameBuilder("Profile", TypeNamingConvention.PropertyName)];
        var contact = result[new NameBuilder("Contact", TypeNamingConvention.PropertyName)];
        var metadata = result[new NameBuilder("Metadata", TypeNamingConvention.PropertyName)];

        // Root properties
        var rootComplexUser = root.Properties.OfType<ComplexProperty>().FirstOrDefault(p => p.Name.CsharpName == "User");
        Assert.NotNull(rootComplexUser);
        Assert.Equal("User", rootComplexUser.Name.CsharpName);

        var rootSimpleStatus = root.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "string" && p.Name.CsharpName == "Status");
        Assert.NotNull(rootSimpleStatus);
        Assert.Equal("string", rootSimpleStatus.Type);
        Assert.Equal("Status", rootSimpleStatus.Name.CsharpName);

        var rootComplexMetadata = root.Properties.OfType<ComplexProperty>().FirstOrDefault(p => p.Name.CsharpName == "Metadata");
        Assert.NotNull(rootComplexMetadata);
        Assert.Equal("Metadata", rootComplexMetadata.Name.CsharpName);

        // User properties
        var userSimpleId = user.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "double" && p.Name.CsharpName == "Id");
        Assert.NotNull(userSimpleId);
        Assert.Equal("double", userSimpleId.Type);
        Assert.Equal("Id", userSimpleId.Name.CsharpName);

        var userComplexProfile = user.Properties.OfType<ComplexProperty>().FirstOrDefault(p => p.Name.CsharpName == "Profile");
        Assert.NotNull(userComplexProfile);
        Assert.Equal("Profile", userComplexProfile.Name.CsharpName);

        // Profile properties
        var profileSimpleName = profile.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "string" && p.Name.CsharpName == "Name");
        Assert.NotNull(profileSimpleName);
        Assert.Equal("string", profileSimpleName.Type);
        Assert.Equal("Name", profileSimpleName.Name.CsharpName);

        var profileComplexContact = profile.Properties.OfType<ComplexProperty>().FirstOrDefault(p => p.Name.CsharpName == "Contact");
        Assert.NotNull(profileComplexContact);
        Assert.Equal("Contact", profileComplexContact.Name.CsharpName);

        // Contact properties
        var contactSimpleEmail = contact.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "string" && p.Name.CsharpName == "Email");
        Assert.NotNull(contactSimpleEmail);
        Assert.Equal("string", contactSimpleEmail.Type);
        Assert.Equal("Email", contactSimpleEmail.Name.CsharpName);

        var contactSimplePhone = contact.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "string" && p.Name.CsharpName == "Phone");
        Assert.NotNull(contactSimplePhone);
        Assert.Equal("string", contactSimplePhone.Type);
        Assert.Equal("Phone", contactSimplePhone.Name.CsharpName);

        // Metadata properties
        var metadataCreatedBy = metadata.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "string" && p.Name.CsharpName == "CreatedBy");
        Assert.NotNull(metadataCreatedBy);
        Assert.Equal("string", metadataCreatedBy.Type);
        Assert.Equal("CreatedBy", metadataCreatedBy.Name.CsharpName);

        var metadataCreatedAt = metadata.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "string" && p.Name.CsharpName == "CreatedAt");
        Assert.NotNull(metadataCreatedAt);
        Assert.Equal("string", metadataCreatedAt.Type);
        Assert.Equal("CreatedAt", metadataCreatedAt.Name.CsharpName);
    }
    [Fact]
    public void Convert_NestedObjectWithArraysAndMixedTypes_ProducesExpectedRecords()
    {
        // arrange
        string json = """
    {
        "user": {
            "id": 42,
            "profile": {
                "name": "Eve",
                "email": "eve@example.com"
            },
            "roles": ["admin", "editor"]
        },
        "settings": {
            "theme": "dark",
            "notifications": {
                "email": true,
                "sms": false
            }
        },
        "tags": ["alpha", "beta", "gamma"]
    }
    """;
        var options = new RecordOptions("Root", TypeNamingConvention.PropertyName);
        using var doc = JsonDocument.Parse(json);

        // act
        var result = JsonToRecords.Convert(doc, options);

        // assert
        var root = result[new NameBuilder("Root", TypeNamingConvention.PropertyName)];
        var user = result[new NameBuilder("User", TypeNamingConvention.PropertyName)];
        var profile = result[new NameBuilder("Profile", TypeNamingConvention.PropertyName)];
        var settings = result[new NameBuilder("Settings", TypeNamingConvention.PropertyName)];
        var notifications = result[new NameBuilder("Notifications", TypeNamingConvention.PropertyName)];

        Assert.Equal("Root", root.Name.Name);

        // Root properties
        var rootUser = root.Properties.OfType<ComplexProperty>().FirstOrDefault(p => p.Name.CsharpName == "User");
        Assert.NotNull(rootUser);
        Assert.Equal("User", rootUser.Name.CsharpName);

        var rootSettings = root.Properties.OfType<ComplexProperty>().FirstOrDefault(p => p.Name.CsharpName == "Settings");
        Assert.NotNull(rootSettings);
        Assert.Equal("Settings", rootSettings.Name.CsharpName);

        var rootTags = root.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "string" && p.Name.CsharpName == "Tags");
        Assert.NotNull(rootTags);
        Assert.Equal("string", rootTags.Type);
        Assert.Equal("Tags", rootTags.Name.CsharpName);

        // User properties
        var userId = user.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "double" && p.Name.CsharpName == "Id");
        Assert.NotNull(userId);
        Assert.Equal("double", userId.Type);
        Assert.Equal("Id", userId.Name.CsharpName);

        var userProfile = user.Properties.OfType<ComplexProperty>().FirstOrDefault(p => p.Name.CsharpName == "Profile");
        Assert.NotNull(userProfile);
        Assert.Equal("Profile", userProfile.Name.CsharpName);

        var userRoles = user.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "string" && p.Name.CsharpName == "Roles");
        Assert.NotNull(userRoles);
        Assert.Equal("string", userRoles.Type);
        Assert.Equal("Roles", userRoles.Name.CsharpName);

        // Profile properties
        var profileName = profile.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "string" && p.Name.CsharpName == "Name");
        Assert.NotNull(profileName);
        Assert.Equal("string", profileName.Type);
        Assert.Equal("Name", profileName.Name.CsharpName);

        var profileEmail = profile.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "string" && p.Name.CsharpName == "Email");
        Assert.NotNull(profileEmail);
        Assert.Equal("string", profileEmail.Type);
        Assert.Equal("Email", profileEmail.Name.CsharpName);

        // Settings properties
        var settingsTheme = settings.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "string" && p.Name.CsharpName == "Theme");
        Assert.NotNull(settingsTheme);
        Assert.Equal("string", settingsTheme.Type);
        Assert.Equal("Theme", settingsTheme.Name.CsharpName);

        var settingsNotifications = settings.Properties.OfType<ComplexProperty>().FirstOrDefault(p => p.Name.CsharpName == "Notifications");
        Assert.NotNull(settingsNotifications);
        Assert.Equal("Notifications", settingsNotifications.Name.CsharpName);

        // Notifications properties
        var notificationsEmail = notifications.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "bool" && p.Name.CsharpName == "Email");
        Assert.NotNull(notificationsEmail);
        Assert.Equal("bool", notificationsEmail.Type);
        Assert.Equal("Email", notificationsEmail.Name.CsharpName);

        var notificationsSms = notifications.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "bool" && p.Name.CsharpName == "Sms");
        Assert.NotNull(notificationsSms);
        Assert.Equal("bool", notificationsSms.Type);
        Assert.Equal("Sms", notificationsSms.Name.CsharpName);
    }
    [Fact]
    public void Convert_ArrayAsRootWithObjects_ProducesMultipleRecords()
    {
        // arrange
        string json = """
    [
        {
            "id": 1,
            "name": "Alice"
        },
        {
            "id": 2,
            "name": "Bob"
        }
    ]
    """;
        var options = new RecordOptions("Person", TypeNamingConvention.PropertyName);
        using var doc = JsonDocument.Parse(json);

        // act
        var result = JsonToRecords.Convert(doc, options);

        // assert
        // Result item existence
        var personRecord = result.Items.FirstOrDefault(r => r.Name.CsharpName == "Person");
        Assert.NotNull(personRecord);
        Assert.Equal("Person", personRecord.Name.CsharpName);

        // Accessing the record via NameBuilder key
        var record = result[new NameBuilder("Person", TypeNamingConvention.PropertyName)];
        Assert.NotNull(record); // sanity check

        // Record properties
        var recordId = record.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "double" && p.Name.CsharpName == "Id");
        Assert.NotNull(recordId);
        Assert.Equal("double", recordId.Type);
        Assert.Equal("Id", recordId.Name.CsharpName);

        var recordName = record.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "string" && p.Name.CsharpName == "Name");
        Assert.NotNull(recordName);
        Assert.Equal("string", recordName.Type);
        Assert.Equal("Name", recordName.Name.CsharpName);
    }
    [Fact]
    public void Convert_ComplexArrayRootWithNestedObjectsAndArrays_ProducesExpectedRecords()
    {
        // arrange
        string json = """
    [
        {
            "id": 1,
            "name": "Alice",
            "contact": {
                "email": "alice@example.com",
                "phones": ["123-456", "789-012"]
            },
            "preferences": {
                "notifications": {
                    "email": true,
                    "sms": false
                },
                "tags": ["alpha", "beta"]
            }
        },
        {
            "id": 2,
            "name": "Bob",
            "contact": {
                "email": "bob@example.com",
                "phones": ["555-666"]
            },
            "preferences": {
                "notifications": {
                    "email": false,
                    "sms": true
                },
                "tags": ["gamma"]
            }
        }
    ]
    """;
        var options = new RecordOptions("User", TypeNamingConvention.PropertyName);
        using var doc = JsonDocument.Parse(json);

        // act
        var result = JsonToRecords.Convert(doc, options);

        // assert
        // Extract records by name
        var user = result[new NameBuilder("User", TypeNamingConvention.PropertyName)];
        var contact = result[new NameBuilder("Contact", TypeNamingConvention.PropertyName)];
        var preferences = result[new NameBuilder("Preferences", TypeNamingConvention.PropertyName)];
        var notifications = result[new NameBuilder("Notifications", TypeNamingConvention.PropertyName)];

        // User record
        Assert.NotNull(user);
        Assert.Equal("User", user.Name.CsharpName);

        var userId = user.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "double" && p.Name.CsharpName == "Id");
        Assert.NotNull(userId);
        Assert.Equal("double", userId.Type);
        Assert.Equal("Id", userId.Name.CsharpName);

        var userName = user.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "string" && p.Name.CsharpName == "Name");
        Assert.NotNull(userName);
        Assert.Equal("string", userName.Type);
        Assert.Equal("Name", userName.Name.CsharpName);

        var userContact = user.Properties.OfType<ComplexProperty>().FirstOrDefault(p => p.Name.CsharpName == "Contact");
        Assert.NotNull(userContact);
        Assert.Equal("Contact", userContact.Name.CsharpName);

        var userPreferences = user.Properties.OfType<ComplexProperty>().FirstOrDefault(p => p.Name.CsharpName == "Preferences");
        Assert.NotNull(userPreferences);
        Assert.Equal("Preferences", userPreferences.Name.CsharpName);

        // Contact record
        Assert.NotNull(contact);

        var contactEmail = contact.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "string" && p.Name.CsharpName == "Email");
        Assert.NotNull(contactEmail);
        Assert.Equal("string", contactEmail.Type);
        Assert.Equal("Email", contactEmail.Name.CsharpName);

        var contactPhones = contact.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "string" && p.Name.CsharpName == "Phones");
        Assert.NotNull(contactPhones);
        Assert.Equal("string", contactPhones.Type);
        Assert.Equal("Phones", contactPhones.Name.CsharpName);

        // Preferences record
        Assert.NotNull(preferences);

        var preferencesNotifications = preferences.Properties.OfType<ComplexProperty>().FirstOrDefault(p => p.Name.CsharpName == "Notifications");
        Assert.NotNull(preferencesNotifications);
        Assert.Equal("Notifications", preferencesNotifications.Name.CsharpName);

        var preferencesTags = preferences.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "string" && p.Name.CsharpName == "Tags");
        Assert.NotNull(preferencesTags);
        Assert.Equal("string", preferencesTags.Type);
        Assert.Equal("Tags", preferencesTags.Name.CsharpName);

        // Notifications record
        Assert.NotNull(notifications);

        var notifEmail = notifications.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "bool" && p.Name.CsharpName == "Email");
        Assert.NotNull(notifEmail);
        Assert.Equal("bool", notifEmail.Type);
        Assert.Equal("Email", notifEmail.Name.CsharpName);

        var notifSms = notifications.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Type == "bool" && p.Name.CsharpName == "Sms");
        Assert.NotNull(notifSms);
        Assert.Equal("bool", notifSms.Type);
        Assert.Equal("Sms", notifSms.Name.CsharpName);
    }
    [Fact]
    public void Convert_SmallNestedObject_NestedPositionName_ProducesExpectedRecords()
    {
        // arrange
        string json = """
        {
            "outer": {
                "inner": {
                    "value": 123
                }
            }
        }
        """;
        var options = new RecordOptions("Root", TypeNamingConvention.NestedPositionName);
        using var doc = JsonDocument.Parse(json);

        // act
        var result = JsonToRecords.Convert(doc, options);

        // assert
        // The expected names with NestedPositionName convention
        var rootName = new NameBuilder("Root", TypeNamingConvention.NestedPositionName);
        var outerName = rootName.ExtendNameBuilder("Outer");
        var innerName = outerName.ExtendNameBuilder("Inner");

        var root = result[rootName];
        var outer = result[outerName];
        var inner = result[innerName];

        Assert.Equal("Root", root.Name.CsharpName);
        Assert.Equal("RootOuter", outer.Name.FullCsharpName);
        Assert.Equal("RootOuterInner", inner.Name.FullCsharpName);

        // Root should have a complex property "Outer"
        var rootOuter = root.Properties.OfType<ComplexProperty>().FirstOrDefault(p => p.Name.CsharpName == "Outer");
        Assert.NotNull(rootOuter);
        Assert.Equal("Outer", rootOuter.Name.CsharpName);

        // Outer should have a complex property "Inner"
        var outerInner = outer.Properties.OfType<ComplexProperty>().FirstOrDefault(p => p.Name.CsharpName == "Inner");
        Assert.NotNull(outerInner);
        Assert.Equal("Inner", outerInner.Name.CsharpName);

        // Inner should have a simple property "Value"
        var innerValue = inner.Properties.OfType<SimpleProperty>().FirstOrDefault(p => p.Name.CsharpName == "Value" && p.Type == "double");
        Assert.NotNull(innerValue);
        Assert.Equal("Value", innerValue.Name.CsharpName);
        Assert.Equal("double", innerValue.Type);
    }

}
