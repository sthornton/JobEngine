using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Models
{
    public class AssemblyJobParameter
    {
        [Key]
        public int AssemblyJobParameterId { get; set; }

        public int AssemblyJobId { get; set; }

        public string Name { get; set; }

        public DataType DataType { get; set; }

        public bool IsRequired { get; set; }

        public bool IsEncrypted { get; set; }

        public string InputValidationRegExPattern { get; set; }
    }
}
