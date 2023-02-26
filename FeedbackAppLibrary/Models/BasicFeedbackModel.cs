namespace FeedbackAppLibrary.Models;
public class BasicFeedbackModel {
  [BsonRepresentation(BsonType.ObjectId)]
  public string Id { get; set; }
  public string Title { get; set; }
  public BasicFeedbackModel() {

  }
  public BasicFeedbackModel(FeedbackModel feedback) {
    Id = feedback.Id;
    Title = feedback.Feedback;

  }
}
