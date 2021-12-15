using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(builder => builder.AddSeq());
builder.Services.AddOpenTelemetryTracing(builder => builder
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Upstream"))
    .AddAspNetCoreInstrumentation()
    .AddHttpClientInstrumentation()
    .AddJaegerExporter()
    .AddZipkinExporter()
);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.Use((context, next) =>
{
    var traceId = Activity.Current?.TraceId.ToString() ?? string.Empty;
    context.Response.Headers.Add("X-Trace-Id", traceId);
    return next(context);
});

app.UseAuthorization();

app.MapControllers();

app.Run();
