using congestion.calculator;
using congestion.calculator.Enums;
using SharpTestsEx;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace tax_calculator.test
{

    public class TaxTests
    {
        public ICongestionTaxCalculator _testclass;
        public List<DateTime> _timeSlots = new List<DateTime>();

        public TaxTests()
        {
            _testclass = new CongestionTaxCalculator();

            var stringDates = new List<string>() {
                "2013-01-14 21:00:00",
                "2013-01-15 21:00:00",
                "2013-02-07 06:23:27",
                "2013-02-07 15:27:00",
                "2013-02-08 06:27:00",
                "2013-02-08 06:20:27",
                "2013-02-08 14:35:00",
                "2013-02-08 15:29:00",
                "2013-02-08 15:47:00",
                "2013-02-08 16:01:00",
                "2013-02-08 16:48:00",
                "2013-02-08 17:49:00",
                "2013-02-08 18:29:00",
                "2013-02-08 18:35:00",
                "2013-03-26 14:25:00",
                "2013-03-28 14:07:27"
            };
            _timeSlots = stringDates.Select(date => DateTime.Parse(date)).ToList();          
        }


        [Fact]
        public void TestCar()
        {      
            var taxFee = _testclass.GetTax(VehicleType.Car, _timeSlots.ToArray());

            taxFee.Should().Not.Be(0);
            taxFee.Should().Be.EqualTo(60);
        }

        [Fact]
        public void TestTractor()
        {
            var taxFee = _testclass.GetTax(VehicleType.Tractor, _timeSlots.ToArray());

            taxFee.Should().Not.Be(0);
            taxFee.Should().Be.EqualTo(60);
        }


        [Theory]
        [InlineData(VehicleType.Motorcycle)]
        [InlineData(VehicleType.Diplomat)]
        [InlineData(VehicleType.Emergency)]
        [InlineData(VehicleType.Foreign)]
        [InlineData(VehicleType.Military)]
        [InlineData(VehicleType.Bus)]
        public void TestTollFreeVehicle(VehicleType type)
        {
            var taxFee = _testclass.GetTax(type, _timeSlots.ToArray());

            taxFee.Should().Be(0);
        }

        [Theory]
        [InlineData(VehicleType.Car, "2013-02-07 06:23:27")]
  
        public void TestTollFreeVehicleFailure(VehicleType type, string timeSlot)
        {
            var date = DateTime.Parse(timeSlot);
            var taxFee = _testclass.GetTax(type, new DateTime[] { date });

            taxFee.Should().Not.Be(0);
            taxFee.Should().Be.EqualTo(8);
        }


        [Theory]
        [InlineData(VehicleType.Car, "2013-02-09 06:23:27")]
        [InlineData(VehicleType.Car, "2013-02-10 06:23:27")]
        [InlineData(VehicleType.Car, "2013-02-10 11:59:27")]

        public void WeekendTest(VehicleType type, string timeSlot)
        {
            var date = DateTime.Parse(timeSlot);
            var taxFee = _testclass.GetTax(type, new DateTime[] { date });

            taxFee.Should().Be(0);         
        }

        [Theory]
        [InlineData(VehicleType.Car, "2013-07-01 06:23:27")]
        [InlineData(VehicleType.Car, "2013-07-31 06:23:27")]

        public void JulyTest(VehicleType type, string timeSlot)
        {
            var date = DateTime.Parse(timeSlot);
            var taxFee = _testclass.GetTax(type, new DateTime[] { date });

            taxFee.Should().Be(0);
        }

        [Theory]
        [InlineData(VehicleType.Car, "2013-02-07 06:35:27")]

        public void ShouldBe13Sek(VehicleType type, string timeSlot)
        {
            var date = DateTime.Parse(timeSlot);
            var taxFee = _testclass.GetTax(type, new DateTime[] { date });

            taxFee.Should().Not.Be(0);
            taxFee.Should().Be.EqualTo(13);
        }


        [Theory]
        [InlineData(VehicleType.Car, "2013-02-07 07:35:27")]

        public void ShouldBe18Sek(VehicleType type, string timeSlot)
        {
            var date = DateTime.Parse(timeSlot);
            var taxFee = _testclass.GetTax(type, new DateTime[] { date });

            taxFee.Should().Not.Be(0);
            taxFee.Should().Be.EqualTo(18);
        }

        [Theory]
        [InlineData(VehicleType.Car, new string[] { "2013-02-07 06:35:27", "2013-02-07 06:40:50", "2013-02-07 06:50:30", "2013-02-07 07:25:24" })]

        public void SingleChargeRuleDifferentTimeSlotHighestChargeSameTimeSlo(VehicleType type, string[] timeSlot)
        {
            var dates = timeSlot.Select(date => DateTime.Parse(date)).ToList();
            var taxFee = _testclass.GetTax(type, dates.ToArray());

            taxFee.Should().Not.Be(0);
            taxFee.Should().Be.EqualTo(18);
        }

        [Theory]
        [InlineData(VehicleType.Car, new string[] { "2013-02-07 06:35:27", "2013-02-07 06:40:50", "2013-02-07 06:50:30", "2013-02-07 06:59:24" })]

        public void SingleChargeRuleSameTimeSlot(VehicleType type, string[] timeSlot)
        {
            var dates = timeSlot.Select(date => DateTime.Parse(date)).ToList();
            var taxFee = _testclass.GetTax(type, dates.ToArray());

            taxFee.Should().Not.Be(0);
            taxFee.Should().Be.EqualTo(13);
        }

        [Theory]
        [InlineData(VehicleType.Car, new string[] { "2013-02-07 06:35:27", "2013-02-07 06:40:50", "2013-02-07 06:50:30", "2013-02-07 07:59:24" })]

        public void SingleChargeRuleOutsideHour(VehicleType type, string[] timeSlot)
        {
            var dates = timeSlot.Select(date => DateTime.Parse(date)).ToList();
            var taxFee = _testclass.GetTax(type, dates.ToArray());

            taxFee.Should().Not.Be(0);
            taxFee.Should().Be.EqualTo(31);
        }


    }
}
