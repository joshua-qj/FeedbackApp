using System.ComponentModel.DataAnnotations;

namespace FeedbackAppUI.Models;

public class CreateFeedbackUIModel {
  [Required]
  public string FirstName { get; set; }
  [Required]
  public string LastName { get; set; }
  [Required]
  public string EmailAddress { get; set; }
  [Required]
  [MinLength(1)]
  [Display(Name = "SalesPerson Model")]
  public string SalesPersonId { get; set; }
  [Required]
  public int Rating { get; set; }
  [Required]
  [MaxLength(1000)]
  public string Feedback { get; set; }
  [Required]
  [MinLength(1)]
  [Display(Name ="Vehicle Model")]
  public string VehicleModelId { get; set; } //radio option, so type is string

  public bool IsContactable { get; set; } = false;


}
