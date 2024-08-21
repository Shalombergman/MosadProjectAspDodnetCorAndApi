namespace MosadApiServer.Models
{
    public class Mission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Agent Agent { get; set; }
        public Target Target { get; set; }
    }
}
