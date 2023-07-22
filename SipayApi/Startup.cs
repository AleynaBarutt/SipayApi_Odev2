using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SipayApi.Data;
using SipayApi.Middleware;
using SipayApi.Services;
using System.Net;

namespace SipayApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public object Log { get; private set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Veritabaný baðlantý dizesini alýyoruz
        var connectionString = Configuration.GetConnectionString("DefaultConnection");
        // DbContext'i ekliyoruz
        services.AddDbContext<StudentDbContext>(options => options.UseSqlServer(connectionString));
        // IStudentService ile StudentService arasýndaki DI baðlantýsýný ekliyoruz.
        // Dependency Injection için StudentService'yi ekliyoruz.
        services.AddScoped<IStudentService, StudentService>(); 
        // ...
        // Diðer servisler...
        services.AddControllers()
          .AddNewtonsoftJson();
        services.AddControllers();
        //swagger baðlantýsý
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sipay Api Collection", Version = "v1" });
        });
        // Logger yapýlandýrmasý
        services.AddLogging(builder =>
        {
            builder.AddConsole(); // Loglarý konsola yazdýrma
            builder.AddFile("app.log"); // Loglarý "app.log" dosyasýna yazdýrma
        });
       
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sipay v1"));
        }

        app.UseMiddleware<GlobalExceptionMiddleware>();


        // Global loglama için Serilog kullanýmý
        // Serilog için loglama middleware'i
        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            ExceptionHandler = async context =>
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    Message = "Internal Server Error"
                }));
            }
        });
        // Global exception middleware
        app.UseMiddleware<LogMiddleware>();
        
        app.UseHttpsRedirection();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
        });

        app.UseRouting();
        // Fake kullanýcý giriþi kontrolü için attribute eklendi
        app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments("/api") && !context.Request.Path.StartsWithSegments("/api/authenticate"))
            {
                // Burada custom bir attribute veya JWT kullanarak gerçek bir kimlik doðrulama iþlemi yapabilirsiniz.
                // Örnek olarak fake bir kimlik doðrulama yapalým.
                if (!context.Request.Headers.ContainsKey("Authorization") || context.Request.Headers["Authorization"] != "FakeToken")
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }
            }

            await next();
        });


        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
