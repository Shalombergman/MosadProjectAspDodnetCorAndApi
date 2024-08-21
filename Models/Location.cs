using System;
using System.ComponentModel.DataAnnotations;

namespace MosadApiServer.Models
{
    public class Location
    {
        public int id { get; set; }
        [Range(0,1000)]
        public  int x { get; set; }
        [Range(0,1000)]
        public  int y { get; set; }
       
    }
}
