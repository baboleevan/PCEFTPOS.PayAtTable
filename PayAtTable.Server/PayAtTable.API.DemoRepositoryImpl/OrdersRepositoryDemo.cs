﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayAtTable.Server.Data;
using PayAtTable.Server.Models;
using PayAtTable.API.Helpers;


namespace PayAtTable.Server.DemoRepository
{
    public class OrdersRepositoryDemo: IOrdersRepository
    {
        protected static readonly Common.Logging.ILog log = 
                Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void TestLog()
        {
            log.InfoEx("Log test...");
        }

        public IEnumerable<Models.Order> GetOrdersFromTable(string tableId)
        {
            var orders = from o in SampleData.Current.Orders
                         where o.TableId.Equals(tableId)
                         select o;
            return orders;
        }

        public Models.Order GetOrder(string orderId)
        {
            var o = SampleData.Current.Orders.Find((order) => order.Id.Equals(orderId));
            if (o == null)
                throw new ResourceNotFoundException(String.Format("Order id {0} not found", orderId));

            return o;
        }

        public Models.Receipt GetCustomerReceiptFromOrderId(string orderId, string receiptOptionId)
        {
            // Find the order
            var o = SampleData.Current.Orders.Find((order) => order.Id.Equals(orderId));
            if (o == null)
                throw new ResourceNotFoundException(String.Format("Order id {0} not found", orderId));

            // Find the table for this order
            var t = SampleData.Current.Tables.Find(table => table.Id.Equals(o.TableId));

            var lines = new List<string>();
            lines.Add("------------------------");
            lines.Add(String.Format("{0, 24}", (t != null) ? t.DisplayName : "UNKNOWN"));
            lines.Add(String.Format("ORDER#{0, 18}", o.Id));
            lines.Add(String.Format("{0, 24}", o.DisplayName));
            lines.Add(String.Format("OWING{0,19:C2}", o.AmountOwing));
            lines.Add("------------------------");

            return new Receipt { Lines = lines };
        }



    }
}
