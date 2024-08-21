using System.Drawing;
using MosadApiServer.Enums;

namespace MosadApiServer.Models
{
    public class Agent
    {
       
            public int Id { get; set; }
            public string Nickname { get; set; }
            public Location Location { get; set; }
            public AgentStatuses Status { get; set; }
            
           

    }
}

