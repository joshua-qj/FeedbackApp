using Microsoft.Extensions.Caching.Memory;

namespace FeedbackAppLibrary.DataAccess;
public class MongoVehicleData : IVehicleData {
  private readonly IMemoryCache _cache;
  private readonly IMongoCollection<VehicleModel> _vehicleModels;
  private const string CacheName = "CategoryData";

  public MongoVehicleData(IDbConection db, IMemoryCache cache) {
    _cache = cache;
    _vehicleModels = db.VehicleModelCollection;
  }
  public async Task<List<VehicleModel>> GetForSaleVehiclesAsync() {
    var output =await GetAllVehiclesAsync();
    return output.Where(v => v.ForSale == true).ToList();
  }

  public async Task<List<VehicleModel>> GetAllVehiclesAsync() {
    var output = _cache.Get<List<VehicleModel>>(CacheName);
    if (output is null || output.Count == 0) {
      var results = await _vehicleModels.FindAsync(_ => true);
      output = results.ToList();
      _cache.Set(CacheName, output, TimeSpan.FromSeconds(5));
      //if 5 users access  GetAllVehicleModelsAsync() at the same exact time,
      //it will _cache.Set(cacheName, output, TimeSpan.FromDays(1)); overwite cache 5 times.
    }
    return output;
  }
  public Task CreateVehicle(VehicleModel vehicleModel) {
    return _vehicleModels.InsertOneAsync(vehicleModel);
  }

  public async Task DeleteVehicle(VehicleModel vehicleModel) {
    vehicleModel.ForSale = true;
    await _vehicleModels.ReplaceOneAsync(v => v.Id == vehicleModel.Id, vehicleModel);
    _cache.Remove(CacheName);
  }

  public async Task<VehicleModel> GetVehicle(string id) {
    throw new NotImplementedException();
  }

  public async Task UpdateVehicle(VehicleModel vehicleModel) {
    await _vehicleModels.ReplaceOneAsync(v => v.Id == vehicleModel.Id, vehicleModel);
    _cache.Remove(CacheName);
  }
}
