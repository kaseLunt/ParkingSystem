using System;
using System.Collections.Generic;
using System.Threading;

namespace ParkingSystem
{
    // Delegate to handle the price cut event
    public delegate void PriceCutEvent(decimal newPrice);

    // Class representing a parking structure in the simulation
    public class ParkingStructure
    {
        private decimal currentPrice;  // The current parking price
        private int priceCutCounter = 0;  // Counter for the number of price cuts
        private readonly int t = 20;  // Maximum number of price cuts
        // Event to be triggered when a price cut occurs
        public event PriceCutEvent OnPriceCut = delegate { };

        // Entry point for the parking structure's behavior
        public void Start(MultiCellBuffer multiCellBuffer)
        {
            currentPrice = 35;  // Initial parking price
            // Loop runs until the maximum number of price cuts is reached
            while (priceCutCounter < t)
            {
                // Fetch an order from the buffer
                OrderClass? order = multiCellBuffer.GetOneCell();
                if (order != null)
                {
                    // Process received order
                    ProcessOrder(order);
                }

                // Calculate new price based on pricing model
                decimal newPrice = PricingModel();
                if (newPrice < currentPrice)
                {
                    // Increment price cut counter and trigger price cut event
                    priceCutCounter++;
                    OnPriceCut?.Invoke(newPrice);

                    // Calculate and display the percentage price drop
                    decimal priceDropPercent = ((currentPrice - newPrice) / currentPrice) * 100;
                    Console.WriteLine($"Price Cut Event - New Price: {newPrice}");
                }

                // Terminate the loop if max price cuts reached
                if (priceCutCounter >= t)
                {
                    return;
                }

                // Update current price for the next iteration
                currentPrice = newPrice;
                Thread.Sleep(1000);  // Pause for a second before next iteration
            }
        }

        // Static method to generate a random price based on a model
        private static decimal PricingModel()
        {
            Random rnd = new();
            return (decimal)rnd.Next(10, 41);
        }

        // Method to process and validate an order
        public void ProcessOrder(OrderClass order)
        {
            Thread orderProcessingThread = new(() =>
            {
                OrderProcessing orderProcessing = new();
                // Validate the credit card in the order
                bool isValid = OrderProcessing.ValidateCard(order.GetCardNo());
                if (isValid)
                {
                    // Calculate and confirm the total cost of the order
                    decimal total = orderProcessing.CalculateTotal(order);
                    SendConfirmation($"Order from {order.GetSenderID()} confirmed. Total amount: {total}");
                }
                else
                {
                    // Handle invalid card scenario
                    HandleInvalidCard($"Order could not be processed. Invalid card number: {order.GetCardNo()}");
                }
            });
            // Start the order processing in a separate thread
            orderProcessingThread.Start();
        }

        // Static method to send order confirmation
        public static void SendConfirmation(string message)
        {
            Console.WriteLine(message);
        }

        // Static method to handle invalid credit card
        public static void HandleInvalidCard(string message)
        {
            Console.WriteLine(message);
        }
    }
}
