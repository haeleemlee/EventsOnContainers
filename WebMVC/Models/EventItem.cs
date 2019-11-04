using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models
{
    public class EventItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public int EPrice { get; set; }
        public string PictureUrl { get; set; }
        public int EventTypeId { get; set; }
        public string EventType { get; set; }
        public int EventPriceId { get; set; }
        //public decimal EventPrice { get; set; }
        public string EventPrice { get; set; }
        public int EventLocationId { get; set; }
        public string EventLocation { get; set; }
    }
}