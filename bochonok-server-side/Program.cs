using bochonok_server_side.database;
using bochonok_server_side.interfaces;
using bochonok_server_side.model;
using bochonok_server_side.Model.Image;
using bochonok_server_side.services;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IEntityService<Category>, EntityService<Category>>();
builder.Services.AddScoped<IEntityService<CatalogItem>, EntityService<CatalogItem>>();

// cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_allowed",
        policy  =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:5173");
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var test = new QRTimingPattern(9);
// var test = new QRAlignmentPattern();
test.Build();
var bytes = test.GetRgba32Bytes();
// Console.WriteLine(bytes.Length);
using (var image = Image.LoadPixelData<Rgba32>(bytes, 5, 45))
{ 
   image.Mutate((x) => x.Rotate(90));
   image.Save("timing.png", new PngEncoder());
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("_allowed");

app.MapControllers();

app.Run();

