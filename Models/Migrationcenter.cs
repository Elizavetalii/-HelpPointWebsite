﻿namespace PRAAPIWEB.Models
{
    public class Migrationcenter
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string? Phone { get; set; }

        public string? WorkingHours { get; set; }

        public string Region { get; set; } = null!;
    }
}
