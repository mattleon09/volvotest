using congestion.calculator;
using congestion.calculator.Enums;
using Microsoft.AspNetCore.Mvc;

namespace tax_calculator.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly ICongestionTaxCalculator _congestionTaxCalculator;
        public CalculatorController(ICongestionTaxCalculator congestionTaxCalculator)
        {
            _congestionTaxCalculator = congestionTaxCalculator;
        }

        //Breaking the REST rules...Should use a GET but GET shouldn't have a body. Dirty solution...
        [HttpPost]
        public async Task<IActionResult> GetTax([FromBody] string[] dateIntervals, VehicleType vehicleType)
        {         
            _ = dateIntervals ?? throw new ArgumentNullException(nameof(dateIntervals));

            var dates = Array.Empty<DateTime>();
            try
            {
                dates = dateIntervals.Select(date => DateTime.Parse(date)).ToArray();
            } catch(FormatException fe)
            {
                BadRequest("One or more dates could not be parsed"); 
            }
            
            var totalFee = _congestionTaxCalculator.GetTax(vehicleType, dates);
            return Ok(totalFee);
        }
    }
}
