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
    if (output is  null) {
      var results = await _feedbacks.FindAsync(f => f.Archived == false);
      output = results.ToList();
      _cache.Set(CacheName, output, TimeSpan.FromMinutes(1));
    }
    return output;
  }

  public async Task<List<FeedbackModel>> GetUsersFeedbacks(string userId) {
    var output = _cache.Get<List<FeedbackModel>>(userId);
    if (output is null) {
      var results = await _feedbacks.FindAsync(f => f.Author.Id == userId);
      output = results.ToList();
      _cache.Set(userId, output, TimeSpan.FromMinutes(1));
    }
    return output;
  }
  public async Task<List<FeedbackModel>> GetAllConfirmedFeedbacks() {
    var output = await GetAllFeedbacks();
    return output.Where(f => f.ConfirmedForRelease).ToList();
  }
  public async Task<FeedbackModel> GetFeedback(string id) {
    var results = await _feedbacks.FindAsync(f => f.Id == id);
    return results.FirstOrDefault();
  }
  public async Task<List<FeedbackModel>> GetAllFeedbacksWaitingForConfirmed() {
    var output = await GetAllFeedbacks();
    return output.Where(f => f.ConfirmedForRelease == false && f.Rejected == false).ToList();
  }
  public async Task UpdateFeedback(FeedbackModel feedback) {
    await _feedbacks.ReplaceOneAsync(f => f.Id == feedback.Id, feedback);
    _cache.Remove(CacheName);
  }
  public async Task UpvoteFeedback(string feedbackId, string userId) {
    var client = _db.Client;
    using var session = await client.StartSessionAsync();
    session.StartTransaction();
    try {
      var db = client.GetDatabase(_db.DbName);
      var feedbackInTransaction = db.GetCollection<FeedbackModel>(_db.FeedbackCollectionName);
      var feedback = (await feedbackInTransaction.FindAsync(f => f.Id == feedbackId)).First();
      //don't use firstordefault, firstordefault won't trigger exception
      bool isUpvote = feedback.UserVotes.Add(userId);
      if (isUpvote == false) {
        feedback.UserVotes.Remove(userId);
      }
      await feedbackInTransaction.ReplaceOneAsync(f => f.Id == feedbackId, feedback);
      var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
      var user = await _userData.GetUserAsync(feedback.Author.Id);
      if (isUpvote) {
        user.VotedOnFeedbacks.Add(new BasicFeedbackModel(feedback));
      }
      else {
        var feedbackToRemove = user.VotedOnFeedbacks.Where(f => f.Id == feedbackId).First();
        user.VotedOnFeedbacks.Remove(feedbackToRemove);
      }
      await usersInTransaction.ReplaceOneAsync(u => u.Id == userId, user);
      await session.CommitTransactionAsync();
      _cache.Remove(CacheName);
    }
    catch (Exception) {
      await session.AbortTransactionAsync();
      throw;
    }
  }

  public async Task CreateFeedback(FeedbackModel feedback) {
    var client = _db.Client;
    using var session = await client.StartSessionAsync();
    session.StartTransaction();
    try {
      var db = client.GetDatabase(_db.DbName);
      var feedbackInTransaction = db.GetCollection<FeedbackModel>(_db.FeedbackCollectionName);
      await feedbackInTransaction.InsertOneAsync(feedback);

      var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
      var user = await _userData.GetUserAsync(feedback.Author.Id);
      user.AuthoredFeedbacks.Add(new BasicFeedbackModel(feedback));
      await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);

      await session.CommitTransactionAsync();
    }
    catch (Exception ex) {
      await session.AbortTransactionAsync();
      throw;
    }
  }



}
