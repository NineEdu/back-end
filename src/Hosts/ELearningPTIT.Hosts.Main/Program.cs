using ELearningPTIT.Modules.Users.Api;
using ELearningPTIT.Modules.Courses.Api;
using ELearningPTIT.Modules.Media.Api;
using FastEndpoints;
using FastEndpoints.Swagger;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add FastEndpoints
builder.Services.AddFastEndpoints();

// Add Swagger for FastEndpoints
builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.Title = "E-Learning PTIT API";
        s.Version = "v1";
    };
});

// Configure MongoDB
var mongoConnectionString = builder.Configuration.GetValue<string>("MongoDB:ConnectionString")
    ?? throw new InvalidOperationException("MongoDB connection string is not configured");
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));

// Wire up Users module (3-layer DI pattern)
builder.Services.AddUsersModule(builder.Configuration);

// Wire up Courses module
builder.Services.AddCoursesModule(builder.Configuration);

// Wire up Media module
builder.Services.AddMediaModule(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.UseHttpsRedirection();

// Add Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map FastEndpoints
app.UseFastEndpoints();

app.Run();
