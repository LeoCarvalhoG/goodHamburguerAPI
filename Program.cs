using GoodHamburgerAPI.Data;
using GoodHamburgerAPI.Models;
using GoodHamburgerAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Configure in-memory database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("GoodHamburgerDb"));

// Register the OrderService
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Sandwiches.Any())
    {
        db.Sandwiches.AddRange(new[]
        {
            new Sandwich { Name = "X Burger", Price = 5.00m },
            new Sandwich { Name = "X Egg", Price = 4.50m },
            new Sandwich { Name = "X Bacon", Price = 7.00m }
        });
    }

    if (!db.Extras.Any())
    {
        db.Extras.AddRange(new[]
        {
            new Extra { Name = "Fries", Price = 2.00m },
            new Extra { Name = "Soft drink", Price = 2.50m }
        });
    }

    db.SaveChanges();
}

app.Run();
