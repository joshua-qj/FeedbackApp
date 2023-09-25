namespace FeedbackAppLibrary.DataAccess;

public interface ISalespersonData {
  Task CreateSalesperson(SalespersonModel salesPerson);
  Task DeleteSalesperson(string id);
  Task<SalespersonModel> GetSalesperson(string id);
  Task UpdateSalesperson(SalespersonModel salesPerson);
  Task<List<SalespersonModel>> GetSalespeople();
  Task<List<SalespersonModel>> GetActiveSalespeople();
}