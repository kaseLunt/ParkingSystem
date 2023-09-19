using System;
using System.Collections.Generic;
using System.Threading;

namespace ParkingSystem
{
    public class ParkingAgent
    {
        private readonly string agentID;
        private decimal currentPrice;
        private decimal previousPrice;  // New member to keep track of the previous price
        private readonly ParkingStructure associatedStructure;
        private readonly MultiCellBuffer multiCellBuffer;

        public ParkingAgent(string agentID, ParkingStructure associatedStructure, MultiCellBuffer multiCellBuffer)
        {
            this.agentID = agentID;
            this.associatedStructure = associatedStructure;
            this.multiCellBuffer = multiCellBuffer;
            this.currentPrice = 0;
            this.previousPrice = 0;  // Initialize to 0
        }
        public void OnPriceCutReceived(decimal newPrice)
        {
            this.previousPrice = this.currentPrice;  // Store the current price before updating it
            this.currentPrice = newPrice;
            EvaluateAndPlaceOrder();
        }
        public void EvaluateAndPlaceOrder()
        {
            Random random = new();
            double randNum = random.NextDouble();  // Random number between 0 and 1

            // Calculate the percentage price cut
            double priceCutPercent = (double)(previousPrice == 0 ? 0 : ((previousPrice - currentPrice) / previousPrice) * 100);

            // Check conditions
            if (randNum > 0.7 || priceCutPercent >= 35)
            {
                GenerateOrder();
            }
        }
        public void GenerateOrder()
        {
            OrderClass order = new OrderClass();
            // Generate a random card number between 2000 and 6000
            Random rand = new Random();
            int cardNumber = rand.Next(2000, 6001);
            order.SetCardNo(cardNumber);

            // Populate other order attributes
            order.SetSenderID(agentID);  // Set the agentID as the Sender ID
            order.SetReceiverID("ParkingStructure1");  // Set the Receiver ID as the ParkingStructure's ID
            order.SetUnitPrice(currentPrice);  // Set the current price as the Unit Price
            int quantity = rand.Next(1, 10);  // Randomly generate the quantity of parking spaces to book
            order.SetQuantity(quantity);  // Set the Quantity

            // Send the populated order to the MultiCellBuffer
            multiCellBuffer.SetOneCell(order);

            // Print order status
            Console.WriteLine($"{order.GetSenderID()}: sent order.");

        }


        public void Start()
        {
            this.associatedStructure.OnPriceCut += OnPriceCutReceived;
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"{agentID}: Checking parking structure's current price.");
            }
        }
    }
}
