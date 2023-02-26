namespace FeedbackAppLibrary.Models;
public class VehicleModel
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string Id { get; set; }
  public string VehicleName { get; set; }
  public string VehicleDescption { get; set; }
}
