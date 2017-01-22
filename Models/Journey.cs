using System;

namespace TaxiCab.Models
{
    public class Journey     //Represents a taxi journey in the calculator. 3 parameters are required to define a journey.
    {
        public DateTime Origin { get; set; }  //The date/time the journey began at.

        public decimal Slow { get; set; }    //The distance travelled at below 6mph, in 0.2mi increments.

        public int Fast { get; set; } //The time spent travelling at above 6mph, in minutes.

        public decimal Cost { //The cost is a function of these 3 parameters with the 2 computed properties for charges, based on the specification.
             get{
                  decimal c = 3.5m; //$3 base fare + $0.50 NY State tax. Decimal is used for accuracy with currency where we only work to 2dp.
                  c = c + 0.35m*((5m*Slow)+Fast); //Calculate the number of $0.35 units are in the fare.
                  if(NightCharge){ //If either surcharge is applied, add the appropriate amount.
                       c=c+0.5m;
                  }
                  if(WeekdayCharge){
                       c=c+1m;
                  }
                  return c; //This is the total cost of the journey.
             }
        }

        public bool NightCharge{ //Decides if the nighttime charge is applied.
             get{
                  if(Origin.Hour>=20 || Origin.Hour<6){
                       return true;
                  }else{
                       return false;
                  }
             }
        }

        public bool WeekdayCharge { //Decides if the peak weekday charge is applied.
             get{
                  if(!(Origin.DayOfWeek==DayOfWeek.Saturday||Origin.DayOfWeek==DayOfWeek.Sunday)&&(Origin.Hour>=16 && Origin.Hour<20)){
                       return true;
                  }else{
                       return false;
                  }
             }
        }
    }
}
