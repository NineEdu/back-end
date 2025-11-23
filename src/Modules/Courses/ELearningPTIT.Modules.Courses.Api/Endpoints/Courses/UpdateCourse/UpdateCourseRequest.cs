using ELearningPTIT.Modules.Courses.Domain.ValueObjects;
using FastEndpoints;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.UpdateCourse;

public class UpdateCourseRequest
{
    public required string CourseId { get; set; }
    
    [FromBody]
    public required UpdateCourseRequestBody Body { get; set; }

    public class UpdateCourseRequestBody
    {
        public required string Title { get; set; }
        public string? Subtitle { get; set; }
        public string? Description { get; set; }
        public required string CategoryId { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? PreviewVideoUrl { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public string Language { get; set; } = "en";
        public List<string> Subtitles { get; set; } = new();
        public List<string> LearningOutcomes { get; set; } = new();
        public List<string> Requirements { get; set; } = new();
        public List<string> TargetAudience { get; set; } = new();
    }
}
