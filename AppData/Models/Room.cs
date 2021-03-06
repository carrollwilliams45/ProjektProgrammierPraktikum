﻿using System.ComponentModel.DataAnnotations;

namespace AppData.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Room : Ressource
    {

        [Required]
        [Display(Name = "Anzahl Sitzplätze")]
        public int NumberOfSeats { get; set; }

        [Display(Name = "Beamer verfügbar")]
        public bool BeamerIsAvailable { get; set; }

        [Display(Name = "Lautsprecher verfügbar")]
        public bool SpeakerIsAvailable { get; set; }
    }
}
