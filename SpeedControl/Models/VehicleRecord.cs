namespace SpeedControl.Models
{
    public class VehicleRecord
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Timestamp { get; set; }
        public float Speed { get; set; }
    }
}
