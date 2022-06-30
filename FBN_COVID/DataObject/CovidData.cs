using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBN_COVID.DataObject
{
    public class CovidResponse
    {
        public object Data { get; set; }
    }

    public class CovidSummary
    {
        public string TotalSamplesTested { get; set; }
        public double TotalConfirmedCases { get; set; }
        public double TotalActiveCases { get; set; }
        public double Discharged { get; set; }
        public double Death { get; set; }
        public IList<CovidStatesData> States { get; set; } = new List<CovidStatesData>();
    }

    public class CovidStatesData
    {
        public string State { get; set; }
        public string _id { get; set; }
        public double ConfirmedCases { get; set; }
        public double CasesOnAdmission { get; set; }
        public double Discharged { get; set; }
        public double Death { get; set; }
    }

}
