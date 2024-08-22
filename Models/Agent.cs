using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using MosadApiServer.Enums;

namespace MosadApiServer.Models
{
    public class Agent
    {

           [Key]
          public int id { get; set; }
          public string nickname { get; set; }
          public string? photo_url { get; set; }
          public AgentStatuses? status { get; set; }
          public Location? location { get; set; }
          public List<Mission>? missions { get; } = [];
          public List<Target>? targets { get; } = [];
                          
    }
}

