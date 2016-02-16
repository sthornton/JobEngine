using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Models
{
    public class ClientInstallFile
    {
        public int ClientInstallFileId { get; set; }

        public string Name { get; set; }

        public byte[] File { get; set; }

        public string Version { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateModified { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public string CreatedBy { get; set; }
    }
}
