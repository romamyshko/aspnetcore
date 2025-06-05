using ModelValidationsExample.CustomModelBinders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options =>
{
});

builder.Services.AddControllers().AddXmlSerializerFormatters();
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
