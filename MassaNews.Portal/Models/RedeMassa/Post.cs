using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MassaNews.Portal.Models.Roteiros
{
  public class Post
  {
    #region Properties

    [BsonIgnoreIfDefault()]
    public int NId { get; set; }

    public string Id { get; set; }
    public DateTime Registered { get; set; }
    public bool IsDeleted { get; set; }
    #endregion

    #region Properties
    public string Title { get; set; }
    public string Call { get; set; }
    public string Content { get; set; }
    public string Slug { get; set; }
    public DateTime Published { get; set; }
    public bool IsFullContent { get; set; }
    public string VideoUrl { get; set; }
    public IEnumerable<Image> Images { get; set; }
    public bool Status { get; set; }
    public bool IsLive { get; set; }

    //Aux
    [BsonIgnore]
    public string SlugHash => $"{Slug}-{NId}";
    [BsonIgnoreIfNull]
    public double? Score { get; set; }
    #endregion

    #region References
    [BsonIgnoreIfNull]
    public IEnumerable<string> Macroregions { get; set; }
    public string ShowId { get; set; }
    #endregion
  }
}
