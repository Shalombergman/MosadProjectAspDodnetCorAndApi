using System;
using System.ComponentModel.DataAnnotations;

namespace MosadApiServer.Models
{
    public class Coordinates
    {
        [Key]
        public int id { get; set; }
        [Range(0,999)]
       
        public  int x { get; set; }
        [Range(0,999)]
       
        public  int y { get; set; }
       
    }
}
