using congestion.calculator.Enums;

namespace congestion.calculator
{
    public class Motorcycle : IVehicle
    {
        public int GetVehicleType()
        {
            return (int)VehicleType.Motorcycle;
        }
    }
}