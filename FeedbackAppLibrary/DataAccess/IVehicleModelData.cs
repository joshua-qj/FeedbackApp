namespace FeedbackAppLibrary.DataAccess;

public interface IVehicleModelData {
  Task CreateVehicleModel(VehicleModel vehicleModel);
  Task<List<VehicleModel>> GetAllVehicleModelsAsync();
}