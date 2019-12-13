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

    public partial class Animal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Animal()
        {
            this.DiseaseHistories = new HashSet<DiseaseHistory>();
        }
        [Display(Name = "Id zwierz�cia")]
        public int id { get; set; }
        [Display(Name = "Wiek")]
        [RegularExpression("[0-9]*", ErrorMessage = "Prosz� poda� liczb� dodatni�")]
        [Required(ErrorMessage = "Wpisz wiek")]
        public int age { get; set; }
        [Display(Name = "Pochodzenie")]
        [Required(ErrorMessage = "Wpisz pochodzenie")]
        public string origin { get; set; }
        [Display(Name = "P�e�")]
        [Required(ErrorMessage = "Wybierz p�e�")]
        public string sex { get; set; }
        [Display(Name = "W Zoo od:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Wybierz dat�")]
        public System.DateTime inZooSince { get; set; }
        [Display(Name = "Imi�")]
        [RegularExpression("[^0-9]*", ErrorMessage = "Imi� nie mo�e zawiera� cyfr")]
        [Required(ErrorMessage = "Wpisz imi�")]
        public string name { get; set; }
        [Required(ErrorMessage = "Wybierz gatunek")]
        [Display(Name = "Gatunek")]
        [RegularExpression("[^0-9]*", ErrorMessage = "Gatunek nie mo�e zawiera� cyfr")]
        public int spiece { get; set; }
        [Display(Name = "Wybieg")]
        [Required(ErrorMessage = "Wybierz wybieg")]
        public int runwayID { get; set; }
    
        public virtual Runway Runway { get; set; }
        public virtual Spiece Spiece1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiseaseHistory> DiseaseHistories { get; set; }
    }
    public enum Gender
    {
        Samiec,
        Samica
    }
}
