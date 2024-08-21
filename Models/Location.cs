using System;
using System.ComponentModel.DataAnnotations;

namespace MosadApiServer.Models
{
    public class Location
    {
        public int Id { get; set; }
        [Range(1,1000)]
        public  int x { get; set; }
        [Range(1,1000)]
        public  int y { get; set; }
       
    }
}
