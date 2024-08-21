using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using MosadApiServer.Enums;

namespace MosadApiServer.Models
{
    public class Target
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }      
        public Location Location { get; set; }
        public TargetStatuses Status { get; set; }
        
    }
}

