namespace PRAAPIWEB.Models
{
    public class HelpLocationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string? Phone { get; set; }
        public string Region { get; set; } = "";
        public string Type { get; set; } = ""; // "clinic" или "migration"
        public string? WorkingHours { get; set; } // для Migrationcenter
        public bool? IsFree { get; set; } // для Medicalclinic
        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }

}
