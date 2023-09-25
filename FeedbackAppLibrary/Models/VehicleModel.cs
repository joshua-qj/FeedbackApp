namespace FeedbackAppLibrary.Models;
public class VehicleModel
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string Id { get; set; }
  public string VehicleModelName { get; set; }
  public string? VehicleModelVariant { get; set; }
  public bool ForSale { get; set; }=false;
}
