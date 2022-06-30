using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBN_COVID.DataObject;
using FBN_COVID.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FBN_COVID.Controllers
{
    [Route("api/covid")]
    [ApiController]
    public class CovidMonitorController : ControllerBase
    {
        private readonly IFbnCovidData _fbnCovidData;

        public CovidMonitorController(IFbnCovidData fbnCovidData)
        {
            _fbnCovidData = fbnCovidData;
        }

        [HttpGet("fbn-covid-data"), ActionName("GetFbnCovidData")]
        public async Task<ActionResult> GetFbnCovidData()
        {
            var covidData = _fbnCovidData.GetCovidData();
            var covidResult = covidData.States.OrderBy(a => a.State);

            return Ok(covidResult);
        }

    }
}