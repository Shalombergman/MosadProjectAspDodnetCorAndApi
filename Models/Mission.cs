using System.ComponentModel.DataAnnotations;
using MosadApiServer.Enums;


namespace MosadApiServer.Models
{
    public class Mission
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; } = null!;
        public double? timeLeft { get; set; }
        public double? ActualExecutionTime { get; set; }
        public MissionStatuses? status { get; set; }
        public int? agentId { get; set; }
        public int? targetId { get; set; }
        public Agent? agent { get; set; } = null!;     
        public Target? target { get; set; } = null!;
       
    }
}
