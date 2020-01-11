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
        public SelectList CreateShipmentList(string id)
        {
            var shipmentList = new SelectList(db.Shipments, "id", "id");
            var newShipmentList = new List<SelectListItem>();
            SelectListItem selected=null;
            foreach (var element in shipmentList)
            {
                var shipment = db.Shipments.Where(x => x.id.ToString() == element.Value).SingleOrDefault();
                element.Text = shipment.Supplier.name + ";" + shipment.shipmentDate.ToShortDateString();
                //element.Text = shipment.Supplier.name + ";" + shipment.Foods.SingleOrDefault().name + ";" + shipment.shipmentDate.ToShortDateString();
                if (element.Value == id)
                {
                    element.Selected = true;
                    selected = element;
                }
                else
                {
                    element.Selected = false;
                   
                }
                newShipmentList.Add(element);
            }
            return new SelectList(newShipmentList, "Value", "Text",selected.Value);
        }
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