using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSystem
{
    // Represents the order processing logic, including card validation and total cost calculation
    public class OrderProcessing
    {
        // Constants for tax and location charge ranges
        private readonly double taxMin = 0.08;  // Minimum tax percentage
        private readonly double taxMax = 0.12;  // Maximum tax percentage
        private readonly double locationMin = 2.0;  // Minimum location charge
        private readonly double locationMax = 8.0;  // Maximum location charge

        // Validates the card number based on predefined rules
        public static bool ValidateCard(int cardNo)
        {
            // Card number should be between 2000 and 6000 (inclusive) for validation
            if (cardNo >= 2000 && cardNo <= 6000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Calculates the total cost for an order including tax and location charge
        public decimal CalculateTotal(OrderClass order)
        {
            Random rnd = new();
            // Calculate random tax rate within the range [taxMin, taxMax]
            double tax = rnd.NextDouble() * (taxMax - taxMin) + taxMin;

            // Calculate random location charge within the range [locationMin, locationMax]
            double locationCharge = rnd.NextDouble() * (locationMax - locationMin) + locationMin;

            // Calculate the base total
            decimal total = order.GetUnitPrice() * order.GetQuantity();

            // Add tax
            total += total * (decimal)tax;

            // Add location charge
            total += (decimal)locationCharge;

            return total;
        }
    }
}
