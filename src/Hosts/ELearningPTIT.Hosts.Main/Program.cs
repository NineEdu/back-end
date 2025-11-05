using ELearningPTIT.Modules.Users.Api;
using ELearningPTIT.Modules.Users.Application;
using ELearningPTIT.Modules.Users.Infrastructure;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers for Users module
builder.Services.AddControllers()
    .AddApplicationPart(typeof(ELearningPTIT.Modules.Users.Api.DependencyInjection).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "E-Learning PTIT API",
        Version = "v1"
    });
});

// Configure MongoDB
var mongoConnectionString = builder.Configuration.GetValue<string>("MongoDB:ConnectionString")
    ?? throw new InvalidOperationException("MongoDB connection string is not configured");
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));

// Wire up Users module
builder.Services.AddUsersApplication();
builder.Services.AddUsersInfrastructure(builder.Configuration);
builder.Services.AddUsersApi(builder.Configuration);

// Wire up your modules here (example module)
// builder.Services.AddYourFeatureModule(
//     new CoreSetupOptions
//     {
//         DatabaseSetupOptions = new DatabaseSetupOptions
//         {
//             ConnectionString = builder.Configuration.GetValue<string>("MongoDB:ConnectionString"),
//             DatabaseName = builder.Configuration.GetValue<string>("MongoDB:DatabaseName"),
//         },
//     }
// );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Learning PTIT API v1");
    });
}

app.UseHttpsRedirection();

// Add Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers for Users module
app.MapControllers();

app.Run();
