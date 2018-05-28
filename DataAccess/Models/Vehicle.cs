namespace DataAccess.Models
{
    using System;

    public class Vehicle
    {
        #region Properties
        public int Id { get; set; }
        public string LicencePlate { get; set; }
        public int Displacement { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public int Payment { get; set; }
        public int State { get; set; }
        public string VehicleType { get; set; }
        #endregion

        
    }
}
