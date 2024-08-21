using System.ComponentModel.DataAnnotations;
using MosadApiServer.Enums;


namespace MosadApiServer.Models
{
    public class Mission
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Agent? Agent { get; set; }
        public Target? Target { get; set; }
        public double timeLeft { get; set; }
        public double ActualExecutionTime { get; set; }
        public MissionStatuses status { get; set; }
    }
}
