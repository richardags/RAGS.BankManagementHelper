using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagementHelper
{
    internal class Order
    {
        public decimal amount;
        public decimal profit;

        public decimal GetSoros
        {
            get
            {
                return amount + profit;
            }
        }

        public Order(decimal amount, decimal profit)
        {
            this.amount = amount;
            this.profit = profit;
        }
    }
}