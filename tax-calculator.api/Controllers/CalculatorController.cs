using congestion.calculator.Enums;
using Microsoft.AspNetCore.Mvc;

namespace tax_calculator.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly CongestionTaxCalculator _congestionTaxCalculator;
        public CalculatorController()
        {
            _congestionTaxCalculator = new CongestionTaxCalculator();
        }

        //Breaking the REST rules...Should use a GET but GET shouldn't have a body. Dirty solution...
        [HttpPost]
        public async Task<IActionResult> GetTax([FromBody] string[] dateIntervals, VehicleType vehicleType)
        {         
            var dates = dateIntervals.Select(date => DateTime.Parse(date)).ToList();

            var totalFee = _congestionTaxCalculator.GetTax(vehicleType, dates.ToArray());
            return Ok(totalFee);
        }
    }
}
