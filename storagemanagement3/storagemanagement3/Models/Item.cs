namespace storagemanagement3.Models
{
    public class Item
    {
        public string? Id { get; set; } // RavenDB assigns IDs automatically
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Location { get; set; }
    }

}
