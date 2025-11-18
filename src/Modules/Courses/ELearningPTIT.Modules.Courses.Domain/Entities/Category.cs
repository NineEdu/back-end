using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Core.Attributes;

namespace ELearningPTIT.Modules.Courses.Domain.Entities;

/// <summary>
/// Represents a course category for organizing courses
/// </summary>
[CollectionName("categories")]
public class Category : EntityBase
{
    public Category()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    [BsonElement("name")]
    [BsonRequired]
    public string Name { get; set; } = string.Empty;

    [BsonElement("slug")]
    [BsonRequired]
    public string Slug { get; set; } = string.Empty;

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("icon")]
    public string? Icon { get; set; }

    [BsonElement("parentCategoryId")]
    public string? ParentCategoryId { get; set; }

    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;

    [BsonElement("displayOrder")]
    public int DisplayOrder { get; set; } = 0;

    [BsonElement("courseCount")]
    public int CourseCount { get; set; } = 0;
}
