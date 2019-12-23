using ProjektBazyDanych.Logic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjektBazyDanych.Logic
{
    public class SettlementLogic : ISettlementLogic
    {
        private connectionString db = new connectionString();
        public SelectList CreateShipmentList()
        {
            var shipmentList = new SelectList(db.Shipments, "id", "id");
            var newShipmentList = new List<SelectListItem>();

            foreach (var element in shipmentList)
            {
                var shipment = db.Shipments.Where(x => x.id.ToString() == element.Value).SingleOrDefault();
                element.Text = shipment.Supplier.name + ";" + shipment.shipmentDate.ToShortDateString();
                //element.Text = shipment.Supplier.name + ";" + shipment.Foods.SingleOrDefault().name + ";" + shipment.shipmentDate.ToShortDateString();
                newShipmentList.Add(element);
            }
            return new SelectList(newShipmentList, "Value", "Text");
        }
    }
}