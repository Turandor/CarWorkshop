using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWorkshopLibrary
{
    public enum PartStatus
    {
        Available,
        Ordered,
        Unavailable
    }
    public class PartObject
    {
        public string partName;
        public PartStatus partStatus;
        public int partAmount;

        
        public PartObject(string partName, PartStatus partStatus, int partAmount)
        {
            this.partName = partName;
            this.partStatus = partStatus;
            this.partAmount = partAmount;
        }
        
    }
}
