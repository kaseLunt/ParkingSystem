using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSystem
{
    // Class representing an order in the parking system simulation
    public class OrderClass
    {
        // Fields to hold various attributes of an order
        private string senderID = string.Empty;   // ID of the agent sending the order
        private int cardNo;                       // Credit card number for the transaction
        private string receiverID = string.Empty; // ID of the parking structure receiving the order
        private int quantity;                     // Number of parking slots ordered
        private decimal unitPrice;                // Price per parking slot

        // Object used for thread synchronization
        private readonly object syncLock = new();

        // Methods to set the attributes of an order, with thread-safety
        public void SetSenderID(string senderID)
        {
            lock (syncLock)
            {
                this.senderID = senderID ?? string.Empty;
            }
        }

        public void SetCardNo(int cardNo)
        {
            lock (syncLock)
            {
                this.cardNo = cardNo;
            }
        }

        public void SetReceiverID(string receiverID)
        {
            lock (syncLock)
            {
                this.receiverID = receiverID ?? string.Empty;
            }
        }

        public void SetQuantity(int quantity)
        {
            lock (syncLock)
            {
                this.quantity = quantity;
            }
        }

        public void SetUnitPrice(decimal unitPrice)
        {
            lock (syncLock)
            {
                this.unitPrice = unitPrice;
            }
        }

        // Methods to retrieve the attributes of an order, with thread-safety
        public string GetSenderID()
        {
            lock (syncLock)
            {
                return senderID;
            }
        }

        public int GetCardNo()
        {
            lock (syncLock)
            {
                return cardNo;
            }
        }

        public string GetReceiverID()
        {
            lock (syncLock)
            {
                return receiverID;
            }
        }

        public int GetQuantity()
        {
            lock (syncLock)
            {
                return quantity;
            }
        }

        public decimal GetUnitPrice()
        {
            lock (syncLock)
            {
                return unitPrice;
            }
        }
    }
}
