using static System.Net.Mime.MediaTypeNames;

namespace Domain.Entities;

public class Card
{
    
   public string Id { get; set; } = "";

   public string SetId { get; set; } = "";

   public string Name { get; set; } = "";

   public string LocalId { get; set; } = "";

   public string? Image { get; set; }

}
