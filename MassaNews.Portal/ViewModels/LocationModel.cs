﻿using Web.Models;

namespace Web.ViewModels
{
  public class LocationModel : BaseModel
  {
    public string Status { get; set; }
    public string Country { get; set; }
    public string CountryCode { get; set; }
    public string Region { get; set; }
    public string RegionName { get; set; }
    public string City { get; set; }
    public string Zip { get; set; }
    public string Lat { get; set; }
    public string Lon { get; set; }
    public string TimeZone { get; set; }
    public string ISP { get; set; }
    public string ORG { get; set; }
    public string AS { get; set; }
    public string Query { get; set; }
  }
}