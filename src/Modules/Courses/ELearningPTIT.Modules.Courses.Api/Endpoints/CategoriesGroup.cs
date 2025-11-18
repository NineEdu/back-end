using FastEndpoints;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints;

public sealed class CategoriesGroup : Group
{
    public CategoriesGroup()
    {
        Configure("categories", options =>
        {
            options.Tags("Categories");
        });
    }
}
