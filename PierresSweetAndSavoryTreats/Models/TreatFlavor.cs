using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PierresSweetAndSavoryTreats.Models
{
    public class TreatFlavor
    {
        public int TreatFlavorId { get; set; }
        public int TreatId { get; set; }
        public int FlavorId { get; set; }
        public virtual Treat Treat { get; set; }
        public virtual Flavor Flavor { get; set; }
    }
}