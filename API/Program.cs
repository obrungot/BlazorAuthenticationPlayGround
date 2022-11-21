using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    //options.AddPolicy("Open", builder =>
    options.AddDefaultPolicy(builder =>
    {
        builder
            .WithOrigins("https://localhost:7298")
            //.SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyHeader()
            .AllowAnyMethod()
            //.AllowCredentials();
            .AllowAnyOrigin();
    });
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
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        options.RequireHttpsMetadata = true;
        options.Audience = "BlazorAuthenticationPlayGround";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();
//app.UseCors("Open");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
