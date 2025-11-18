using ELearningPTIT.Modules.Courses.Domain.Entities;
using YourCompany.YourProject.Shared.Core.Abstractions;

namespace ELearningPTIT.Modules.Courses.Domain.Repositories;

/// <summary>
/// Repository interface for Question entity (Q&A)
/// </summary>
public interface IQuestionRepository : IRepository<Question>
{
    Task<IEnumerable<Question>> GetByCourseIdAsync(string courseId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Question>> GetByLectureIdAsync(string lectureId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Question>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Question>> GetUnansweredQuestionsAsync(string courseId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Question>> GetFlaggedQuestionsAsync(CancellationToken cancellationToken = default);
}
