namespace RuanganKita.Api.Models;

public class Reservation
{
    public int Id { get; set; }
    public Room? Room { get; set; }
    public int RoomId { get; set; }
    public User? User { get; set; }
    public int UserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}