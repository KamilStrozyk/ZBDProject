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
    
    public partial class Settlement
    {
        public int shipmentId { get; set; }
        public System.DateTime creationDate { get; set; }
        public System.DateTime modificationDate { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int sum { get; set; }
        public bool approved { get; set; }
    
        public virtual Shipment Shipment { get; set; }
    }
}
