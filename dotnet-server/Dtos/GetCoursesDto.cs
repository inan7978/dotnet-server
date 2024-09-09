//A DTO is a class specifically designed to hold the data we want to transfer between different parts of our
//application, or even between our application and external systems (like an API).
namespace CourseAPI.Dtos;
public record class GetCoursesDto(
    int Id,
    string Name,
    string Description,
    int NoOfChapters,
    string InstructorId
);