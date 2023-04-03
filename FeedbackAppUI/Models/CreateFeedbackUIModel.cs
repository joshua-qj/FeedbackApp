using System.ComponentModel.DataAnnotations;

namespace FeedbackAppUI.Models;

public class CreateFeedbackUIModel {
  [Required]
  [MaxLength(100)]
  public string Feedback { get; set; }
  [Required]
  [MinLength(1)]
  [Display(Name ="Vehicle Model")]
  public string VehicleModelId { get; set; } //radio option, so type is string
  [MaxLength(500)]
  public string Description { get; set; }
}
