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
        var existingReview = await reviewRepository.GetAsync(
            x => x.UserId == command.UserId && x.CourseId == command.CourseId);
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

        // Calculate and update course rating
        var reviews = await reviewRepository.QueryAsync(
            x => x.CourseId == command.CourseId && x.IsPublished);
        if (reviews.Any())
        {
            course.AverageRating = Math.Round(reviews.Average(r => r.Rating), 2);
            course.TotalReviews = reviews.Count();
            await courseRepository.ReplaceAsync(course);
        }

        return review.Adapt<ReviewDto>();
    }
}
