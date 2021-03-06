﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TerminUndRaumplanung.Models.ManageViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class EnableAuthenticatorViewModel
    {

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [ReadOnly(true)]
        public string SharedKey { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string AuthenticatorUri { get; set; }
    }
}
