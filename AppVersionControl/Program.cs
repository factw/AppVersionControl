using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using AppVersionControl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 1. add httpcliente
builder.Services.AddHttpClient();

//2. Add app versioning control

builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(1, 0);
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;
});

//3. Add configurataion to document verison

builder.Services.AddVersionedApiExplorer(setup =>
{
    //Aca ponemos como queremos las verisonas
    setup.GroupNameFormat = "'v'VVV";// 1.0.0
    setup.SubstituteApiVersionInUrl = true;
});




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 4. Configure options

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();

//5. Congfirute Endpoints for swagger DOCS for each of the version of the api
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
//6. Configure Swagger DOCS
    app.UseSwaggerUI(options =>
    {
        foreach(var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant()
                );
            // /swagger/v2/
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
