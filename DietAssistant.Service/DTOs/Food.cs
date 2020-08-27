namespace DietAssistant.Services.DTOs
{
    public class Food
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal CarbohydratesAmount { get; set; }

        public decimal ProteinsAmount { get; set; }

        public decimal FatsAmount { get; set; }
    }
}
