using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;  // Added for the Semaphore class

namespace ParkingSystem
{
    // Represents a buffer that holds multiple cells for storing orders
    public class MultiCellBuffer
    {
        // Internal list to hold orders
        private readonly List<OrderClass?> cells = new();

        // Semaphore to control access to cells
        private readonly Semaphore semaphore;

        // Locks for individual cells to ensure thread-safety
        private readonly object[] cellLocks;

        // Initializes a new instance of MultiCellBuffer with the specified number of cells
        public MultiCellBuffer(int cellCount)
        {
            // Initialize the semaphore with the total number of cells
            semaphore = new Semaphore(cellCount, cellCount);

            // Initialize the lock objects for each cell
            cellLocks = new object[cellCount];

            // Populate the cells list and cell locks
            for (int i = 0; i < cellCount; i++)
            {
                cells.Add(null);  // Initialize each cell to null
                cellLocks[i] = new object();  // Initialize a new lock object for each cell
            }
        }

        // Stores an order in the first available cell
        public void SetOneCell(OrderClass order)
        {
            // Acquire semaphore lock
            semaphore.WaitOne();

            // Loop to find an empty cell and set the order
            for (int i = 0; i < cells.Count; i++)
            {
                lock (cellLocks[i])  // Acquire cell-specific lock
                {
                    if (cells[i] == null)  // Check if cell is empty
                    {
                        cells[i] = order;  // Store the order
                        break;  // Exit the loop
                    }
                }
            }

            // Release semaphore lock
            semaphore.Release();
        }

        // Retrieves an order from the first filled cell and empties the cell
        public OrderClass? GetOneCell()
        {
            // Acquire semaphore lock
            semaphore.WaitOne();

            OrderClass? order = null;  // Initialize variable to hold the retrieved order

            // Loop to find a filled cell and retrieve the order
            for (int i = 0; i < cells.Count; i++)
            {
                lock (cellLocks[i])  // Acquire cell-specific lock
                {
                    if (cells[i] != null)  // Check if cell is filled
                    {
                        order = cells[i];  // Retrieve the order
                        cells[i] = null;  // Empty the cell
                        break;  // Exit the loop
                    }
                }
            }

            // Release semaphore lock
            semaphore.Release();

            return order;  // Return the retrieved order, or null if no filled cell was found
        }
    }
}
