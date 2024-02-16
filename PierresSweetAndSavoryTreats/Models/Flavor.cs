using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PierresSweetAndSavoryTreats.Models
{
    public class Flavor
    {
        public Flavor()
        {
            this.Treats = new HashSet<FlavorTreat>();
        }

        public int FlavorId { get; set; }
        public string Name { get; set; }
        // Other properties as needed

        public virtual ICollection<FlavorTreat> Treats { get; set; }
    }
}