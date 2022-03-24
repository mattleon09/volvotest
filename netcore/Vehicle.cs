using congestion.calculator.Enums;
using System;

namespace congestion.calculator
{
    public class Vehicle
    {
        public static object GetVehicle(VehicleType vehicleType)
        {
            var ns = typeof(IVehicle).Namespace;
            var typeName = ns + "." + vehicleType.ToString();

            return Activator.CreateInstance(Type.GetType(typeName));
        }      
    }
}
