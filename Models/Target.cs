using System.Drawing;
using MosadApiServer.Enums;

namespace MosadApiServer.Models
{
    public class Target
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public Location Location { get; set; }
        public TargetStatuses Status { get; set; }
        
    }
}

