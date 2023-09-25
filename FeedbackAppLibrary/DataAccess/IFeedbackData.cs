namespace FeedbackAppLibrary.DataAccess;

public interface IFeedbackData {
  Task CreateFeedback(FeedbackModel feedback);
  Task<List<FeedbackModel>> GetAllConfirmedFeedbacks();
  Task<List<FeedbackModel>> GetAllFeedbacks();

  Task<List<FeedbackModel>> GetAllFeedbacksWaitingForConfirmed();
  Task<FeedbackModel> GetFeedback(string id);
Task<List<FeedbackModel>> SearchFeedbacks(string? search,DateTime? dateFrom,DateTime? dateTo,VehicleModel? vehicleModel, SalespersonModel? salesperson,bool? confirmed);
  Task UpdateFeedback(FeedbackModel feedback);
  //Task UpvoteFeedback(string feedbackId, string userId);
}