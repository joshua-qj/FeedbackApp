using MongoDB.Driver;

namespace FeedbackAppLibrary.DataAccess;
public interface IDbConection {
  MongoClient Client { get; }
  string DbName { get; }
  IMongoCollection<FeedbackModel> FeedbackCollection { get; }
  string FeedbackCollectionName { get; }
  IMongoCollection<VehicleModel> ModelCollection { get; }
  string ModelCollectionName { get; }
  IMongoCollection<StatusModel> StatusCollection { get; }
  string StatusCollectionName { get; }
  IMongoCollection<UserModel> UserCollection { get; }
  string UserCollectionName { get; }
}