using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PierresSweetAndSavoryTreats.Models
{
  public class Treat
  {
    public int TreatId { get; set; }
    [Required(ErrorMessage = "* The treat's Type cannot be empty!")]
    public string Type { get; set; }

    public List<TreatFlavor> JoinEntities { get; set; }
  }
}