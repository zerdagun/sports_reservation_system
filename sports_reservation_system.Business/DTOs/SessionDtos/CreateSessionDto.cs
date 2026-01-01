namespace sports_reservation_system.Business.DTOs.SessionDtos;

public class CreateSessionDto
{
    public int BranchId { get; set; } // Hangi şubeye ait?
    public int SportId { get; set; } // Hangi spor?
    public DateTime StartTime { get; set; } // Ne zaman başlıyor?
    public int DurationMinutes { get; set; } // Kaç dakika sürecek?
    public int Quota { get; set; } // Kaç kişilik yer var?
    public decimal Price { get; set; } // Ücreti ne kadar?
}
