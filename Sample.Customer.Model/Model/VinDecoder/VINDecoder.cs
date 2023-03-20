using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sample.Customer.Model.VinDecoder
{
   
    public class Decode
    {
        public string label { get; set; }
        public dynamic value { get; set; }
    }

  

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Balance
    {
        //[JsonProperty("API Decode")]
        public int APIDecode { get; set; }
    }


    public class VINDecoderResponseVM
    {
        public int price { get; set; }
        public string price_currency { get; set; }
        public Balance balance { get; set; }
        public Decode[] decode { get; set; }
        [NotMapped]
        public VehicleInfo VehicleInfoData { get; set; }
    }

    public class VehicleInfo
    {
        public string Make { get; set; }
        public string Manufacturer { get; set; }
        public string PlantCountry { get; set; }
        public string ProductType { get; set; }
        public string ManufacturerAddress { get; set; }
        public string CheckDigitValidity { get; set; }
        public string SequentialNumber { get; set; }
        public string Body { get; set; }
        public string EngineCylinders { get; set; }
        public string EngineDisplacement { get; set; }
        public string NumberofDoors { get; set; }
        public string EngineModel { get; set; }
        public string EnginePower { get; set; }
        public string FuelType { get; set; }
        public string Model { get; set; }
        public string ModelYear { get; set; }
        public string PlantCity { get; set; }
        public string Series { get; set; }
        public string Transmission { get; set; }
        public string Engine { get; set; }
        public string NumberofSeats { get; set; }
        public string EngineCylindersPosition { get; set; }
        public string Length { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string FuelSystem { get; set; }
        public string EnginePosition { get; set; }
        public string FuelCapacity { get; set; }
        public string FrontBreaks { get; set; }
        public string RearBreaks { get; set; }
        public string ValvesperCylinder { get; set; }
        public string MaxSpeed { get; set; }
        public string ABS { get; set; }
        public string WheelSize { get; set; }
        public string EngineStroke { get; set; }
        public string MinimumvolumeofLuggage { get; set; }
        public string MaximumvolumeofLuggage { get; set; }
        public string NumberofGears { get; set; }
        public string PowerSteering { get; set; }
    }
}
