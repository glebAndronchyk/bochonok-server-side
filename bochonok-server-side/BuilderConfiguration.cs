using bochonok_server_side.database;
using bochonok_server_side.mapper;
using Microsoft.EntityFrameworkCore;

namespace bochonok_server_side;

public class BuilderConfiguration
{
    private readonly WebApplicationBuilder _builder;
    private WebApplication _app;
    
    public BuilderConfiguration(WebApplicationBuilder builder)
    {
        _builder = builder;
    }

    public WebApplication Configure()
    {
        ConfigureServices();
        ConfigureApp();

        return _app;
    }

    private void ConfigureServices()
    {
        _builder.Services.AddControllers();
        _builder.Services.AddEndpointsApiExplorer();
        _builder.Services.AddSwaggerGen();
        _builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(_builder.Configuration.GetConnectionString("Default")));

        MappingInjector.InjectMappings(_builder.Services);

        _builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "_allowed",
                policy  =>
                {
                    policy.WithOrigins("http://localhost:3000", "http://localhost:5173");
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });
        });

        _app = _builder.Build();
    }

    private void ConfigureApp()
    {
        if (_app.Environment.IsDevelopment())
        {
            _app.UseSwagger();
            _app.UseSwaggerUI();
        }

        _app.UseHttpsRedirection();

        _app.UseAuthorization();

        _app.UseCors("_allowed");

        _app.MapControllers();
    }
}