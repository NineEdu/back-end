using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Domain.Entities;
using ELearningPTIT.Modules.Courses.Domain.Exceptions;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class CreateReviewCommandHandler(
    IReviewRepository reviewRepository,
    ICourseRepository courseRepository
) : ICommandHandler<CreateReviewCommand, ReviewDto>
{
    public async Task<ReviewDto> HandleAsync(CreateReviewCommand command)
    {
        // Verify course exists
        var course = await courseRepository.GetAsync(command.CourseId);
        if (course == null)
        {
            throw new CourseNotFoundException(command.CourseId);
        }

        // Check if user already reviewed this course
        var existingReview = await reviewRepository.GetByUserAndCourseAsync(command.UserId, command.CourseId);
        if (existingReview != null)
        {
            throw new DuplicateReviewException(command.UserId, command.CourseId);
        }

        var review = new Review
        {
            CourseId = command.CourseId,
            UserId = command.UserId,
            Rating = command.Rating,
            Comment = command.Comment,
            IsPublished = true
        };

        await reviewRepository.CreateAsync(review);

        // Update course rating
        var stats = await reviewRepository.GetCourseRatingStatsAsync(command.CourseId);
        await courseRepository.UpdateRatingAsync(command.CourseId, stats.AverageRating, stats.TotalReviews);

        return review.Adapt<ReviewDto>();
    }
}
