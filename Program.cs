using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.UseUrls("https://0.0.0.0:44301");

// Add the MCP services: the transport to use (http) and the tools to register.
builder.Services
    .AddMcpServer()
    .WithHttpTransport(options =>
    {
        // Stateless mode is recommended for servers that don't need
        // server-to-client requests like sampling or elicitation.
        // See https://csharp.sdk.modelcontextprotocol.io/concepts/transports/transports.html for details.
        options.Stateless = true;
    })
    .WithTools<RandomNumberTools>();



#if RELAY
if (true)
{
    builder.WebHost.UseAzureRelay(options =>
    {
        var conStr = Environment.GetEnvironmentVariable("relay1");
        options.UrlPrefixes.Add(conStr);
    });
}
#endif




var app = builder.Build();

app.Use(async (context, next) =>
{
    var feature =
        context.Features.Get<IHttpResponseBodyFeature>();

    if (feature != null)
    {
        context.Features.Set<IHttpResponseBodyFeature>(
            new McpServer1.RelayResponseBodyFeature(feature));
    }

    await next();
});



app.Use(async (context, next) =>
{
    Console.WriteLine(
        $"Incoming: {context.Request.Method} " +
        $"{context.Request.PathBase}{context.Request.Path}");

    Console.WriteLine(
        $"Accept: {context.Request.Headers.Accept}");

    Console.WriteLine(
        $"Content-Type: {context.Request.ContentType}");

    Console.WriteLine(
        $"MCP-Protocol-Version: " +
        $"{context.Request.Headers["MCP-Protocol-Version"]}");

    await next();

    Console.WriteLine($"Outgoing: {context.Response.StatusCode}");
});


app.MapMcp().AllowAnonymous(); ;


//app.MapMcp();
//app.UseHttpsRedirection();

app.Run();
