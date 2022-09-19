using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core;
using WebApiHiringItm.CORE.Helpers;
using WebApiHiringItm.CORE.Interface;
using WebApiHiringItm.MODEL.Mapper;
using WebApiHiringItm.MODEL.Models;
using WebApiRifa.CORE.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUserCore, UserCore>();
builder.Services.AddScoped<IHiringDataCore, HiringDataCore>();
builder.Services.AddScoped<IFilesCore, FilesCore>();
builder.Services.AddScoped<IContractorCore, ContractorCore>();
builder.Services.AddScoped<IProjectFolder, ProjectFolderCore>();

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => { builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod(); 
    
    });
});
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new Automaping());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<Hiring_V1Context>(options =>
      options
      .UseLazyLoadingProxies()
      .UseSqlServer(builder.Configuration.GetConnectionString("HiringDatabase"))
      );


builder.Services.AddScoped<IHiring_V1Context>(provider => provider.GetService<Hiring_V1Context>());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(x => x
  .AllowAnyOrigin()
  .AllowAnyMethod()
  .AllowAnyHeader());
}
app.UseMiddleware<JwtMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors();
app.Run();
