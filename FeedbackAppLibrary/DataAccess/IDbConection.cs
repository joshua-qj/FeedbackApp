using MongoDB.Driver;

namespace FeedbackAppLibrary.DataAccess;
public interface IDbConection {
  MongoClient Client { get; }
  string DbName { get; }
  IMongoCollection<FeedbackModel> FeedbackCollection { get; }
  string FeedbackCollectionName { get; }
  IMongoCollection<VehicleModel> VehicleModelCollection { get; }
  string VehicleModelCollectionName { get; }
  IMongoCollection<SalespersonModel> SalesPersonCollection { get; }
  string SalesPersonCollectionName { get; }
  IMongoCollection<UserModel> UserCollection { get; }
  string UserCollectionName { get; }
}