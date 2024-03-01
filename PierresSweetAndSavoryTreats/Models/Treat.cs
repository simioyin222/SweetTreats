using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PierresSweetAndSavoryTreats.Models;

namespace PierresSweetAndSavoryTreats.Models
{
  public class Treat
  {
    public int TreatId { get; set; }
    [RegularExpression(@"^[a-zA-Z0-9. ]+$", ErrorMessage = "Please enter up to 40 alphanumeric characters.")]
    [Required(ErrorMessage = "Please enter a valid input for the treat.")]        
    public string Name { get; set; }
    public List<TreatFlavor> JoinEntities { get; }
  }
}