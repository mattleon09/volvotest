using congestion.calculator.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace congestion.calculator
{
    public interface ICongestionTaxCalculator
    {
        int GetTax(VehicleType vehicle, DateTime[] dates);
    }
}
