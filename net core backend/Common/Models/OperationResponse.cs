using net_core_backend.Models;
using System.Collections.Generic;

namespace Common.Models
{
    public class OperationResponse
    {
        public double IncomeSumm { get; set; }
        public double ExpanseSumm { get; set; }
        public List<OperationModel> Operations { get; set; }

        public OperationResponse()
        {
            IncomeSumm = 0;
            ExpanseSumm = 0;
            Operations = new List<OperationModel>();
        }
    }
}
