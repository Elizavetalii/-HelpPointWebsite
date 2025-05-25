﻿namespace PRAAPIWEB.Models
{
    public class Medicalclinic
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string? Phone { get; set; }

        public bool? IsFree { get; set; }

        public string Region { get; set; } = null!;
    }
}
