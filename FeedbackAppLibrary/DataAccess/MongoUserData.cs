

namespace FeedbackAppLibrary.DataAccess;
public class MongoUserData : IUserData {
  private readonly IMongoCollection<UserModel> _users;

  public MongoUserData(IDbConection db) {
    _users = db.UserCollection;
  }

  public async Task<List<UserModel>> GetUsersAsync() {
    var results = await _users.FindAsync(_ => true);
    //return everything
    return results.ToList();
  }

  public async Task<UserModel> GetUserAsync(string id) {
    var results = await _users.FindAsync(u => u.Id == id);
    return results.FirstOrDefault();
  }
  public async Task<UserModel> GetUserFromAuthenticationAsync(string objectId) {
    var results = await _users.FindAsync(u => u.ObjectIdentifier == objectId);
    //ObjectIdentifier is the azure active directory b2c has given user object, we will look up user based upon ObjectIdentifier
    //when a user loged in,  look up this user by identifier to get all their infomation from system.

    return results.FirstOrDefault();
  }
  public Task CreateUserAsync(UserModel user) {

    return _users.InsertOneAsync(user);
  }
  public Task UpdateUserAsync(UserModel user) {
    var filter = Builders<UserModel>.Filter.Eq("Id", user.Id);

    return _users.ReplaceOneAsync(filter, user, new ReplaceOptions { IsUpsert = true });
    //if can't find user, insert a new one, 
  }
}
