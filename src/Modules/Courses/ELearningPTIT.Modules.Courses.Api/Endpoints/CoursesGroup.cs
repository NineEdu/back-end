using FastEndpoints;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints;

public sealed class CoursesGroup : Group
{
    public CoursesGroup()
    {
        Configure("courses", options =>
        {
            options.Tags("Courses");
        });
    }
}
