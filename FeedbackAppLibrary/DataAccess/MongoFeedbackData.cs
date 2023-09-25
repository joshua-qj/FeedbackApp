using Microsoft.Extensions.Caching.Memory;

namespace FeedbackAppLibrary.DataAccess;
public class MongoFeedbackData : IFeedbackData {
  private readonly IDbConection _db;
  private readonly IUserData _userData;
  private readonly IMemoryCache _cache;
  private readonly IMongoCollection<FeedbackModel> _feedbacks;
  private const string CacheName = "FeedbackData";

  public MongoFeedbackData(IDbConection db, IUserData userData, IMemoryCache cache) {
    _db = db;
    _userData = userData;
    _cache = cache;
    _feedbacks = db.FeedbackCollection;
  }
  public async Task<List<FeedbackModel>> GetAllFeedbacks() {
    var output = _cache.Get<List<FeedbackModel>>(CacheName);
    if (output is null) {
      var results = await _feedbacks.FindAsync(_ => true);
      output = results.ToList();
      _cache.Set(CacheName, output, TimeSpan.FromMinutes(1));
    }
    return output;
  }


  public async Task<List<FeedbackModel>> GetAllConfirmedFeedbacks() {
    var output = await GetAllFeedbacks();
    return output.Where(f => f.Confirmed).ToList();
  }
  public async Task<List<FeedbackModel>> GetAllFeedbacksWaitingForConfirmed() {
    var output = await GetAllFeedbacks();
    return output.Where(f => f.Confirmed == false).ToList();
  }
  public async Task<FeedbackModel> GetFeedback(string id) {
    var results = await _feedbacks.FindAsync(f => f.Id == id);
    return results.FirstOrDefault();
  }

  public async Task UpdateFeedback(FeedbackModel feedback) {
    await _feedbacks.ReplaceOneAsync(f => f.Id == feedback.Id, feedback);
    _cache.Remove(CacheName);
  }

  public async Task CreateFeedback(FeedbackModel feedback) {
    var client = _db.Client;
    using var session = await client.StartSessionAsync();
    session.StartTransaction();
    try {
      var db = client.GetDatabase(_db.DbName);
      var feedbackInTransaction = db.GetCollection<FeedbackModel>(_db.FeedbackCollectionName);
      await feedbackInTransaction.InsertOneAsync(session, feedback);

      var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
      //var user = await _userData.GetUserAsync(feedback.Author.Id);
      //user.AuthoredFeedbacks.Add(new BasicFeedbackModel(feedback));
      //await usersInTransaction.ReplaceOneAsync(session/*,u => u.Id == user.Id, user);

      await session.CommitTransactionAsync();
    }
    catch (Exception ex) {
      await session.AbortTransactionAsync();
      throw;
    }
  }

  public async Task<List<FeedbackModel>> SearchFeedbacks(string? search,
DateTime? dateFrom,
DateTime? dateTo,
VehicleModel? vehicleModel,
SalespersonModel? salesperson,
bool? confirmed) {
    var output = await GetAllFeedbacks();
    return output.Where(f => (confirmed is null || f.Confirmed == confirmed &&
(string.IsNullOrWhiteSpace(search) || f.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase)) &&
(string.IsNullOrWhiteSpace(search) || f.LastName.Contains(search, StringComparison.OrdinalIgnoreCase)) &&
(!dateFrom.HasValue || f.DateCreated >= dateFrom.Value.Date) &&
(!dateTo.HasValue || f.DateCreated <= dateTo.Value.Date) &&
(vehicleModel.Id is null || f.VehicleModel.Id == vehicleModel.Id) &&
(salesperson.Id is null || f.SalesPerson.Id == salesperson.Id))

).ToList();
  }
}
