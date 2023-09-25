namespace FeedbackAppLibrary.Models;
public class FeedbackModel {
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string Id { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string EmailAddress { get; set; }
  public string Feedback { get; set; }
  public SalespersonModel SalesPerson { get; set; }
  public Rating Rating { get; set; }
  public DateTime DateCreated { get; set; } = DateTime.UtcNow;

  public VehicleModel VehicleModel { get; set; }

  public string? ManagerNote { get; set; }
  public bool Confirmed { get; set; } = false;

  public bool IsContactable { get; set; } = false;

}
