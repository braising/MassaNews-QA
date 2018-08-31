namespace MassaNews.Portal.Models.Newsletter
{
  public class NewsletterUpdate
  {
    public string Hash { get; set; }
    public string Name { get; set; }
    public string CellPhone { get; set; }
    public int Period { get; set; }
    public int City { get; set; }
    public int[] Preferences { get; set; }
  }
}