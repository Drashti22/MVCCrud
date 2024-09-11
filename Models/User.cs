//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcPractical.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public partial class User
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Please enter a valid email address.")]
        // [UniqueEmail(ErrorMessage = "This email is already in use")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile No is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile No must be exactly 10 digits")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "State is required")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        public string KnownLanguage { get; set; }

        [Required(ErrorMessage = "Photo is required")]
        public string PhotoUrl { get; set; }

        public virtual Country Country { get; set; }
        public virtual State State { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
