﻿namespace Techies.EventManagement
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int NumberOfSeats { get; set; }
        public DateTime EventDate { get; set; }
    }
}