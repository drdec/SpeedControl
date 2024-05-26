namespace SpeedControl.Models
{
    public class VehicleRecordRequest
    {
        public string Number { get; set; }
        public DateTime Timestamp { get; set; }
        public float Speed { get; set; }
    }
}
