using Microsoft.Extensions.Caching.Memory;

namespace FeedbackAppLibrary.DataAccess;
public class MongoVehicleModelData : IVehicleModelData {
	private readonly IMemoryCache _cache;
	private readonly IMongoCollection<VehicleModel> _vehicleModels;
	private const string CacheName = "CategoryData";

	public MongoVehicleModelData(IDbConection db, IMemoryCache cache) {
		_cache = cache;
		_vehicleModels = db.ModelCollection;
	}
	public async Task<List<VehicleModel>> GetAllVehicleModelsAsync() {
		var output = _cache.Get<List<VehicleModel>>(CacheName);
		if (output is null || output.Count == 0) {
			var results = await _vehicleModels.FindAsync(_ => true);
			output = results.ToList();
			_cache.Set(CacheName, output, TimeSpan.FromDays(1));
			//if 5 users access  GetAllVehicleModelsAsync() at the same exact time,
			//it will _cache.Set(cacheName, output, TimeSpan.FromDays(1)); overwite cache 5 times.
		}
		return output;
	}
	public Task CreateVehicleModel(VehicleModel vehicleModel) {
		return _vehicleModels.InsertOneAsync(vehicleModel);
	}

}
