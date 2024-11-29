using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
namespace Task
{

    public class Plane
    {
        public int planeId { get; set; }
        public int maxCapacity { get; set; }
        private List<Order> orders;
        public Plane()
        {
            orders = new List<Order>();
        }
        public void AddOrder(Order order)
        {
            orders.Add(order);
        }
        public List<Order> GetOrders() => orders;
    }
    public class Flight
    {
        public int flightId { get; set; }
        public string flightCode { get; set; }
        public string departure { get; set; }
        public string destination { get; set; }
        public string day { get; set; }
        public Plane plane;
    }
    public class FlightsList
    {
        List<Flight> flights;
        int flightItr = 1;
        public FlightsList()
        {
            flights = new List<Flight>();
        }
        public Flight FindFlight(string destination)
        {
            return flights.Find(x => x.destination == destination);
        }
        public void ScheduleFlight(string dep,string dest,Plane plane,string day)
        {
            Flight flight = new Flight()
            {
                flightId = flightItr,
                flightCode = "Flight " + flightItr,
                day = day,
                departure = dep,
                destination = dest,
                plane = plane
            };
            flightItr++;
            flights.Add(flight);
        }
        public List<Flight> GetFlights() => flights;

        public void PrintAllFlights()
        {
            foreach (Flight flight in flights)
            {
                Console.WriteLine(flight.flightCode + ",departure:" + flight.departure + ",arrival:" + flight.destination + ",day:" + flight.day);
            }

        }

    }
    public class Order
    {
        public string orderId { get; set; }
        public string destination { get; set; }
        public List<Flight> flights { get; set; }

        public Order()
        {
            flights = new List<Flight>();
        }
        
    }
  
    public class Orders
    {
        List<Order> _orders;
        
        public Orders()
        {
            _orders = new List<Order>();
            string filePath = @"/Users/ayadhalal/Downloads/archive/coding-assigment-orders.json";

         
            string jsonContent = File.ReadAllText(filePath);
            Dictionary<string,Order> raw_orders = JsonConvert.DeserializeObject<Dictionary<string, Order>>(jsonContent);
            foreach (KeyValuePair<string,Order> pair in raw_orders)
            {
                pair.Value.orderId = pair.Key;
                _orders.Add(pair.Value);
            }
        }
       
        
        public void LoadOrdersInFlights(List<Flight> flights)//for the list of scheduled flights ,load the order into each plane(flight)
        {
            foreach (Order order in _orders)
            {
                foreach (Flight flight in flights)
                {
                    if (flight.destination == order.destination)
                    {
                        flight.plane.AddOrder(order);
                        order.flights.Add(flight);
                    }
                }
               
            }
        }
        
        public void PrintNonScheduled()//print orders that dont have flights associated with it
        {
            List<Order> non_scheduled = _orders.FindAll(x =>x.flights.Count==0);
            non_scheduled.ForEach((x) =>
            {
                //order: order-X, flightNumber: not scheduled
                Console.WriteLine("order:" + x.orderId + "flightNumber: not scheduled");
            });
        }
        public void PrintScheduled()//print orders that have flights associated with it
        {
            List<Order> scheduled = _orders.FindAll(x => x.flights.Count > 0);
            scheduled.ForEach((x) =>
            {
                //order: order-001, flightNumber: 1, departure: <departure_city>, arrival: <arrival_city>, day: x
                //order: order-X, flightNumber: not scheduled
                x.flights.ForEach((c) =>
                {
                    Console.WriteLine("order:" + x.orderId + ",flightNumber:" + c.flightId + ",departure:" + c.departure + ",arrival:" + c.destination + ",day:" + c.day);

                });
            
            });
        }

    }
    class MainClass
    {

        static FlightsList flightsList = new FlightsList();
        static Orders orders = new Orders();
        public static void Main(string[] args)
        {
            //first we get the planes
            //then we schedule flight for each of the planes on the selective daa
            //then we load the orders into those planes(flights)
            List<Plane> planes=GetPlanes();
            flightsList.ScheduleFlight("YUL", "YYZ", planes[0], "1");
            flightsList.ScheduleFlight("YUL", "YYC", planes[1], "1");
            flightsList.ScheduleFlight("YUL", "YVR", planes[2], "1");
            //
            flightsList.ScheduleFlight("YUL", "YYZ", planes[0], "2");
            flightsList.ScheduleFlight("YUL", "YYC", planes[1], "2");
            flightsList.ScheduleFlight("YUL", "YVR", planes[2], "2");
            orders.LoadOrdersInFlights(flightsList.GetFlights());
  
        
            //
            
         

            while (true)
            {
                string command = Console.ReadLine();
                
                switch(command) //simple command console utility 
                {
                    case "flights":/// type flights to get print list of all available flights
                         flightsList.PrintAllFlights(); //
                          break;
                    case "schd":

                        orders.PrintScheduled();
                        break;
                    case "non-schd":
                        orders.PrintNonScheduled();
                       // orders.PrintNonScheduledOrders();
                        break;
                }

            }
           
        }
        
        
        static List<Plane> GetPlanes()//there is 3 planes with specific max capacity 
        {
            List<Plane> planes = new List<Plane>()
            {
                new Plane()
                {
                    planeId = 1,
                    maxCapacity = 20
                },
                new Plane()
                {
                    planeId=2,
                    maxCapacity=20
                },
                new Plane()
                {
                    planeId=3,
                    maxCapacity=20
                }

            };

            return planes;
        }
    }
}
