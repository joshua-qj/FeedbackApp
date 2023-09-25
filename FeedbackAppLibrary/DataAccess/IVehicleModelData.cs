namespace FeedbackAppLibrary.DataAccess;

public interface IVehicleData {
  Task CreateVehicle(VehicleModel vehicleModel);
  Task DeleteVehicle(VehicleModel vehicleModel);
  Task<VehicleModel> GetVehicle(string id);
  Task UpdateVehicle(VehicleModel vehicleModel);
  Task<List<VehicleModel>> GetAllVehiclesAsync();
  Task<List<VehicleModel>> GetForSaleVehiclesAsync();
}