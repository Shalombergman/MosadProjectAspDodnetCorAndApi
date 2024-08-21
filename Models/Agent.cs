using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using MosadApiServer.Enums;

namespace MosadApiServer.Models
{
    public class Agent
    {

           [Key]
          public int Id { get; set; }
          public string Nickname { get; set; }
          public string Image { get; set; }         
          public Location Location { get; set; }
          public AgentStatuses Status { get; set; }                     
    }
}

