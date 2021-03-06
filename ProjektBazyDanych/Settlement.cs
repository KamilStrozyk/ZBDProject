//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjektBazyDanych
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Settlement
    {
        [Display(Name = "Id rozliczenia")]
        public string id { get; set; }
        
        public int shipmentId { get; set; }
        [Display(Name = "Data utworzenia")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Wybierz dat�")]
        public System.DateTime creationDate { get; set; }

        [Display(Name = "Data modyfikacji")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Wybierz dat�")]
        public System.DateTime modificationDate { get; set; }
        [Display(Name = "Rok")]
        [Required(ErrorMessage = "Wpisz rok")]
        [Range(1970, 2020, ErrorMessage = "Prosz� poda� odpowiedni rok")]
        public int year { get; set; }
        [Display(Name = "Miesi�c")]
        [Required(ErrorMessage = "Wpisz rok")]
        [Range(1, 12, ErrorMessage = "Prosz� poda� odpowiedni miesi�c")]
        public int month { get; set; }
        [Display(Name = "Suma w z�")]
        [Required(ErrorMessage = "Wpisz sum�")]
        [RegularExpression("[0-9]*", ErrorMessage = "Prosz� poda� liczb� dodatni�")]
        public int sum { get; set; }
        [Display(Name = "Zatwierdzone")]
        public bool approved { get; set; }
    
        public virtual Shipment Shipment { get; set; }
    }
}
