using Microsoft.Extensions.Caching.Memory;

namespace FeedbackAppLibrary.DataAccess;
public class MongoSalespersonData : ISalespersonData {
	private readonly IMemoryCache _cache;
	private readonly IMongoCollection<SalespersonModel> _salesPeople;
	private const string CacheName = "SalesPersonData";

	public MongoSalespersonData(IDbConection db, IMemoryCache cache) {
		_cache = cache;
    _salesPeople = db.SalesPersonCollection;
	}
	public async Task<List<SalespersonModel>> GetSalespeople() {
		var output = _cache.Get<List<SalespersonModel>>(CacheName);
		if (output is null||output.Count==0) {
			var results = await _salesPeople.FindAsync(_ => true);
			output = results.ToList();
			_cache.Set(CacheName, output, TimeSpan.FromSeconds(5));

		}
		return output;
	}
  public async Task<List<SalespersonModel>> GetActiveSalespeople() {
    var output = await GetSalespeople();
    return output.Where(f => f.IsResigned==false).ToList();
  }

  public Task CreateSalesperson(SalespersonModel salesPerson) {
		return _salesPeople.InsertOneAsync(salesPerson);
	}

  public Task DeleteSalesperson(string id) {
    throw new NotImplementedException();
  }

  public Task<SalespersonModel> GetSalesperson(string id) {
    throw new NotImplementedException();
  }

  public Task UpdateSalesperson(SalespersonModel salesPerson) {
    throw new NotImplementedException();
  }
}
