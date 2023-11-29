using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Reproject.Configures;
using Reproject.Models.Context;
using Reproject.Processors;

var builder = WebApplication.CreateBuilder(args);
AuthOptions authOptions = builder.Configuration.GetSection("Jwt").Get<AuthOptions>();
ServiceConfigure serviceConfigure = builder.Configuration.GetSection("ServiceConfigure").Get<ServiceConfigure>();
builder.Services.AddSingleton(authOptions);
builder.Services.AddSingleton(serviceConfigure);
builder.Services.AddSingleton<NoteDbContext>();
builder.Services.AddSingleton<ClaimsIdentityGenerator>();
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // ��������, ����� �� �������������� �������� ��� ��������� ������
                ValidateIssuer = true,
                // ������, �������������� ��������
                ValidIssuer = authOptions.Issuer,

                // ����� �� �������������� ����������� ������
                ValidateAudience = true,
                // ��������� ����������� ������
                ValidAudience = authOptions.Audience,
                // ����� �� �������������� ����� �������������
                ValidateLifetime = true,

                // ��������� ����� ������������
                IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                // ��������� ����� ������������
                ValidateIssuerSigningKey = true,
            };
        });
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
