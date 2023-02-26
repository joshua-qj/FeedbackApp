namespace FeedbackAppLibrary.Models;
public class FeedbackModel {
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string Id { get; set; }
  public string Feedback { get; set; }
  public string Description  { get; set; }
  public DateTime DateCreated { get; set; } = DateTime.UtcNow;

  public VehicleModel Vehicle { get; set; }
  public BasicUserModel Author { get; set; }
  public HashSet<string> UserVotes { get; set; }
  // use HashSet no duplicated user
  public StatusModel FeedbackStatus { get; set; }
  public string OwnerNotes { get; set; }
  public bool ConfirmedForRelease { get; set; } = false;
  public bool Archived { get; set; } = false;

  public bool Rejected { get; set; } = false;

}
