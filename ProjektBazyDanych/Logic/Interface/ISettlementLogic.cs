using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProjektBazyDanych.Logic.Interface
{
    interface ISettlementLogic
    {
        SelectList CreateShipmentList();
        SelectList CreateShipmentList(string id);
    }
}
