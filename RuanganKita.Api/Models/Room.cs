namespace RuanganKita.Api.Models;

public class Room
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public Building? Building { get; set; }
    public int BuildingId { get; set; }
    public int Capacity { get; set; }
    public TimeOnly AvailableFrom { get; set; }
    public TimeOnly AvailableTo { get; set; }
}
