namespace FeedbackAppLibrary.Models;
public class SalespersonModel {
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string Id { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string? DisplayName { get; set; }
  public bool IsResigned { get; set; }=false;
}
