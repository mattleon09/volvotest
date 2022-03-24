using congestion.calculator.Enums;

namespace congestion.calculator
{
    public class Car : IVehicle
    {
        public int GetVehicleType()
        {
            return (int)VehicleType.Car;
        }
    }
}