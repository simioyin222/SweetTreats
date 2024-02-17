using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PierresSweetAndSavoryTreats.Models
{
  public class TreatFlavor
  {
    public int TreatFlavorId { get; set; }

    public int FlavorId { get; set; }
    public int TreatId { get; set; }

    public Flavor Flavor { get; set; }
    public Treat Treat { get; set; }
  }
}