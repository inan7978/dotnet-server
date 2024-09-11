using CourseAPI.Dtos;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GetCoursesDto> courses = [
        new (
            1,
            "Node Backend Development",
            "This is a demo course",
            20,
            "1"
        ),
        new (
            2,
            "React Development",
            "This is a Full course",
            10,
            "2"
        ),
        new (
            3,
            "Java with OOP Internship Bootcamp",
            "This is a Full course",
            20,
            "2"
        )];


    string HeavyCpuUtilization()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        double result = 0;
    
        
            // Perform some CPU-intensive work
            for (int i = 0; i < 10000000; i++)
            {
                result += Math.Sqrt(i);
            }
        

        stopwatch.Stop();
        Console.WriteLine($"Completed heavy CPU task in {stopwatch.Elapsed} seconds.");
    return $"Completed heavy CPU task in {stopwatch.Elapsed} seconds. Ended at result {result}.";
    }



app.MapGet("/", () =>
{
    Console.WriteLine("Entry endpoint called. Means the server is running.");
    return "dotnet-server running. You can close this tab and continue in Postman.";
});

app.MapGet("/courses", () =>
{
    Console.WriteLine("get /courses called...");
    return courses;
});

app.MapGet("/courses/wait", () =>
{
    Console.WriteLine("get /courses/wait called...");
    System.Threading.Thread.Sleep(5000);
    return courses;
});
app.MapGet("/courses/heavy", () =>
{
    Console.WriteLine("get /courses/heavy called...");
    return HeavyCpuUtilization();
    
});

app.MapGet("courses/{id}", (int id) =>
{
    Console.WriteLine($"Get course with ID: {id} called...");
    return courses.Find(course => course.Id == id);
}).WithName("GetCourse");

app.MapPost("courses", (CreateCourseDto newCourse) => // newCourse is the input parameter that represents the new course to be created
{
    Console.WriteLine("POST courses called...");
    int id = courses.Count + 1;

    // creates a new course
    GetCoursesDto course = new(id, newCourse.Name, newCourse.Description, newCourse.NoOfChapters, newCourse.InstructorId);

    // adds the course created above to your in memory courses list
    courses.Add(course);

    // provides a 201 status code, includes location header, and returns the newely created course
    return Results.CreatedAtRoute("GetCourse", new { id = id }, course);
});

app.MapPut("courses/{id}", (int id, CreateCourseDto updatedCourse) =>
{

    Console.WriteLine($"PUT courses with ID: {id} called...");
    GetCoursesDto? currCourse = courses.Find(course => course.Id == id);
    if (currCourse == null)
    {
        return Results.NotFound();
    }
    GetCoursesDto newCourse = new(
        id,
        updatedCourse.Name,
        updatedCourse.Description,
        updatedCourse.NoOfChapters,
        updatedCourse.InstructorId

    );
    courses[id - 1] = newCourse;
    return Results.Ok();
});
app.MapDelete("courses/{id}", (int id) =>
{
    Console.WriteLine($"DELETE course with ID: {id} called...");
    int courseId = courses.FindIndex(course => course.Id == id);
    if (courseId == -1)
    {
        return Results.NoContent();
    }
    courses.RemoveAt(id - 1);
    return Results.NoContent();
});

app.Run();
