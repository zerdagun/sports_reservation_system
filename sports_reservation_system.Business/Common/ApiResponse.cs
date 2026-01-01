namespace sports_reservation_system.Business.Common;

/// <summary>
/// Tüm API yanıtları için standart format.
/// Her endpoint bu formatı kullanarak tutarlı yanıtlar döndürür.
/// </summary>
/// <typeparam name="T">Yanıt verisinin tipi (DTO, liste, vb.)</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// İşlemin başarılı olup olmadığını belirtir
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Kullanıcıya gösterilecek mesaj (başarı veya hata mesajı)
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// İşlem sonucu verisi (başarılıysa dolu, hata varsa null)
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Başarılı yanıt oluşturur
    /// </summary>
    public static ApiResponse<T> SuccessResponse(T data, string message = "İşlem başarılı")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// Hata yanıtı oluşturur
    /// </summary>
    public static ApiResponse<T> ErrorResponse(string message, T? data = default)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = data
        };
    }
}

