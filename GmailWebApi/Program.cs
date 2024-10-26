var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Other service registrations
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigins", policy =>
    {
        policy//.WithOrigins("http://localhost:5081") // Add the client URL here
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Allow credentials for SignalR WebSocket connections
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAnyOrigins");
app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();