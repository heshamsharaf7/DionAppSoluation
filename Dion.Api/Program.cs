using Dion.Api.Data;
using Dion.Api.Repositories.Contracts;
using Dion.Api.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<DionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DionConnection"))
);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Logging.AddConsole();

var app = builder.Build();
//if (builder.Environment.IsDevelopment())
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var context = scope.ServiceProvider.GetRequiredService<DionDbContext>();
//        context.Database.Migrate(); // This will run only in Development
//    }
//}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || builder.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(policy => policy
    .AllowAnyOrigin() // Allow requests from any origin
    .AllowAnyMethod() // Allow any HTTP method
    .WithHeaders(HeaderNames.ContentType) // Specify additional headers to allow
);
app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.Run();
