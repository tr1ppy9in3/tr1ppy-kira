using System.Text.Json.Nodes;

namespace Kira.Bfff.Middlewares;

public sealed class SwaggerTransformMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value;
        
        if (path != null && path.StartsWith("/swagger/") && path.EndsWith("swagger.json"))
        {
            var originalBodyStream = context.Response.Body;
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await next(context);

            if (context.Response.StatusCode == StatusCodes.Status200OK)
            {
                await TransformSwaggerJson(context, memoryStream, originalBodyStream, path);
            }
            else
            {
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }
        else
        {
            await next(context);
        }
    }

    private static async Task TransformSwaggerJson(HttpContext context, MemoryStream memoryStream, Stream originalStream, string path)
    {
        memoryStream.Position = 0;
        using var reader = new StreamReader(memoryStream);
        var responseBody = await reader.ReadToEndAsync();

        try
        {
            var jsonNode = JsonNode.Parse(responseBody);
            var pathsObj = jsonNode?["paths"]?.AsObject();
            
            if (pathsObj != null)
            {
                var newPaths = new JsonObject();
                var segment = path.Split('/')[2];

                foreach (var property in pathsObj.ToArray())
                {
                    var newPathKey = property.Key.Replace("/api/v1/", $"/api/v1/{segment}/");
                    newPaths.Add(newPathKey, property.Value?.DeepClone());
                }

                jsonNode!["paths"] = newPaths;
                var modifiedJson = jsonNode.ToJsonString();
                var modifiedBytes = System.Text.Encoding.UTF8.GetBytes(modifiedJson);

                context.Response.ContentLength = modifiedBytes.Length;
                await originalStream.WriteAsync(modifiedBytes);
            }
        }
        catch
        {
            memoryStream.Position = 0;
            await memoryStream.CopyToAsync(originalStream);
        }
    }
}