using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
  options.AddPolicy("Open", builder => builder.WithOrigins("https://localhost:7298"));
});

builder.Services.AddControllers()
  .AddJsonOptions(options =>
{
  options.JsonSerializerOptions.PropertyNamingPolicy = null; //to avoid defaulting to camelCase
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
  {
    c.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
    c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
      ValidAudience = builder.Configuration["Auth0:Audience"],
      ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}"
    };
  });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("Open");

app.MapControllers();

app.Run();
