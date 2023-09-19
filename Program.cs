using System;
using System.Collections.Generic;
using System.Threading;

namespace ParkingSystem
{
    class Program
    {
        // Entry point of the application
        static void Main(string[] args)
        {
            // Number of ParkingStructures in the simulation
            int K = 1;
            // Number of ParkingAgents in the simulation
            int N = 5;
            // Number of cells in the MultiCellBuffer
            int cellCount = 3;

            // Initialize arrays to hold MultiCellBuffer, ParkingStructure, and their respective threads
            MultiCellBuffer[] multiCellBuffers = new MultiCellBuffer[K];
            ParkingStructure[] parkingStructures = new ParkingStructure[K];
            Thread[] parkingStructureThreads = new Thread[K];

            // Create and start the ParkingStructure objects and their threads
            for (int k = 0; k < K; k++)
            {
                int capturedK = k; // Capture loop variable for closure
                // Create a MultiCellBuffer for each ParkingStructure
                multiCellBuffers[capturedK] = new MultiCellBuffer(cellCount);
                // Initialize ParkingStructure
                parkingStructures[capturedK] = new ParkingStructure();
                // Create and start thread for each ParkingStructure
                parkingStructureThreads[capturedK] = new Thread(new ThreadStart(() => parkingStructures[capturedK].Start(multiCellBuffers[capturedK])));
                parkingStructureThreads[capturedK].Start();
            }

            // Initialize arrays to hold ParkingAgent objects and their threads
            ParkingAgent[] parkingAgents = new ParkingAgent[N];
            Thread[] parkingAgentThreads = new Thread[N];

            // Create and start ParkingAgent objects and their threads
            for (int i = 0; i < N; i++)
            {
                // Associate each ParkingAgent with the first ParkingStructure for demonstration purposes
                parkingAgents[i] = new ParkingAgent($"Agent{i}", parkingStructures[0], multiCellBuffers[0]);
                // Create and start thread for each ParkingAgent
                parkingAgentThreads[i] = new Thread(new ThreadStart(parkingAgents[i].Start));
                parkingAgentThreads[i].Start();
            }

            // Wait for all ParkingStructure threads to complete
            foreach (Thread structureThread in parkingStructureThreads)
            {
                structureThread.Join();
            }
            // Wait for all ParkingAgent threads to complete
            foreach (Thread agentThread in parkingAgentThreads)
            {
                agentThread.Join();
            }
        }
    }
}
