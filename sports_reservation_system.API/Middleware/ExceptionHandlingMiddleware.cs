using System.Net;
using System.Text.Json;
using sports_reservation_system.Business.Common;

namespace sports_reservation_system.API.Middleware;

/// <summary>
/// Global Exception Handling Middleware
/// Tüm uygulamada oluşan hataları yakalar ve standart formatta döndürür.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Bir sonraki middleware'e geç (eğer hata yoksa)
            await _next(context);
        }
        catch (Exception ex)
        {
            // Hata oluştuğunda buraya gelir
            _logger.LogError(ex, "Bir hata oluştu: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // HTTP Status Code'u belirle
        var statusCode = exception switch
        {
            ArgumentNullException => HttpStatusCode.BadRequest, // 400
            ArgumentException => HttpStatusCode.BadRequest, // 400
            KeyNotFoundException => HttpStatusCode.NotFound, // 404
            InvalidOperationException => HttpStatusCode.Conflict, // 409
            UnauthorizedAccessException => HttpStatusCode.Unauthorized, // 401
            _ => HttpStatusCode.InternalServerError // 500
        };

        // Response tipini JSON olarak ayarla
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        // Standart API Response formatında hata mesajı oluştur
        var response = ApiResponse<object>.ErrorResponse(
            message: exception.Message,
            data: null
        );

        // JSON'a çevir ve gönder
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}

