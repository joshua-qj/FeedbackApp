using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace FeedbackAppLibrary.DataAccess {
  public class DbConection : IDbConection {
    private readonly IConfiguration _config;
    private readonly IMongoDatabase _db;
    //this _db just usable internally, make it private.
    private string _connectionId = "MongoDB";
    //This "MongoDB" is appsettings "MongoDB"
    public string DbName { get; private set; }
    public string ModelCollectionName { get; private set; } = "models";
    public string StatusCollectionName { get; private set; } = "statuses";
    public string UserCollectionName { get; private set; } = "users";
    public string FeedbackCollectionName { get; private set; } = "feedbacks";
    //above 4 collectionname are available externally
    public MongoClient Client { get; private set; }
    //above 4 collectionnames will be connected outside of this db connection,
    //so this Client should be public
    //where create transactional content in which case we have to connect the database
    // directly instead of using an exsiting an open connection to a collection.
    //therefore we use this Client externally .
    public IMongoCollection<VehicleModel> ModelCollection { get; private set; }
    public IMongoCollection<StatusModel> StatusCollection { get; private set; }
    public IMongoCollection<UserModel> UserCollection { get; private set; }
    public IMongoCollection<FeedbackModel> FeedbackCollection { get; private set; }

    public DbConection(IConfiguration config) {
      _config = config;
      Client = new MongoClient(_config.GetConnectionString(_connectionId));
      DbName = _config["DatabaseName"];
      //get an item based upon its name, and come back as a string value, and the string value
      //can be casted to a specific type, in here , string is fine for DbName
      //can be replaced by using extensions.configuratrion.binder nuget package, _config.get value...
      _db = Client.GetDatabase(DbName);

      ModelCollection = _db.GetCollection<VehicleModel>(ModelCollectionName);
      StatusCollection = _db.GetCollection<StatusModel>(StatusCollectionName);
      UserCollection = _db.GetCollection<UserModel>(UserCollectionName);
      FeedbackCollection = _db.GetCollection<FeedbackModel>(FeedbackCollectionName);
      //wire up collection
      /*
       In this constructor, create a new client, connection to database,grab dbname,then connect to database
      _db = Client.GetDatabase(DbName);
      Then create all four of collections.
            ModelCollection = _db.GetCollection<VehicleModel>(ModelCollectionName);
      StatusCollection = _db.GetCollection<StatusModel>(StatusCollectionName);
      UserCollection = _db.GetCollection<UserModel>(UserCollectionName);
      FeedbackCollection = _db.GetCollection<FeedbackModel>(FeedbackCollectionName);


      Put it dependecy injection ,make this singleton, everytime ask for this instance , create
      teh same instance rather than reinstantiatin this db collection. That way, only connect db once,
      only connect collections once and reuse those collections and connections.
       */
    }
  }
}