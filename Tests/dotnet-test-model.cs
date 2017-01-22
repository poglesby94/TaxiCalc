//Unit test for the .net code.
//The controller object simply creates an object given a request or returns a view.
//These are standard functions of the .net mvc framework so we do not unit test them.
//Tested using XUnit, with the command ">dotnet test".

using Xunit;
using System;
using TaxiCab.Models;

//In the project specification, it states "All examples/problems will have distance in units of 1/5 of a mile and in minutes - there will be no fractional units)."
//This code will be accessed through the controller, which is only to be accessed through the Angular JS Front end.
//Therefore this code will only be accessed with correct data types, so will only test with normal and extreme data, not exceptional data.

namespace TaxiCab.ModelTests
{
        public class NightChargeTesting //Tests the calculated field value NightCharge. Should be true if the trip occurs after 8pm or before 6am, any day of the week.
        {
              //Based off example in specification (Ride takes place at 17:30 so should not have Night surcharge).
              [Fact]
              public void ExampleTest()
              {
                   var j = new Journey{Origin=DateTime.Parse("2010-10-08T17:30:00"),Slow=2,Fast=5};
                   Assert.Equal(j.NightCharge, false);
              }

              //Tests cases with normal times where the Night charge is applied.
              [Theory]
              [InlineData("2017-01-20T20:30:00")]
              [InlineData("2015-05-22T22:15:22")]
              [InlineData("1980-10-22T23:33:22")]
              [InlineData("2017-06-15T00:21:54")]
              [InlineData("2015-03-21T01:12:33")]
              [InlineData("1800-11-05T03:04:17")]
              [InlineData("100-07-01T05:21:54")]
              public void NormalDataTrueTest(String input)
              {
                   var j = new Journey{Origin=DateTime.Parse(input)}; //Has no dependency on the Fast or Slow fields of the model.
                   Assert.Equal(j.NightCharge, true); //Night charge should be applied.
              }

              //Tests cases with normal times where the Night charge is not applied.
              [Theory]
              [InlineData("2017-04-11T08:20:00")]
              [InlineData("2013-01-01T11:30:01")]
              [InlineData("2012-05-31T12:40:59")]
              [InlineData("0001-01-01T14:33:16")]
              [InlineData("320-06-11T16:12:42")]
              [InlineData("1800-11-05T17:39:21")]
              public void NormalDataFalseTest(String input)
              {
                   var j = new Journey{Origin=DateTime.Parse(input)}; //Has no dependency on the Fast or Slow fields of the model.
                   Assert.Equal(j.NightCharge, false); //Night charge should not be applied.
              }

              //Tests cases with edge case times where the Night charge is applied.
              [Theory]
              [InlineData("2017-01-20T20:00:00")]
              [InlineData("2015-05-22T20:00:01")]
              [InlineData("1980-10-22T00:00:00")]
              [InlineData("2017-06-15T23:59:59")]
              [InlineData("2015-03-21T05:59:59")]
              public void ExtremeDataTrueTest(String input)
              {
                   var j = new Journey{Origin=DateTime.Parse(input)}; //Has no dependency on the Fast or Slow fields of the model.
                   Assert.Equal(j.NightCharge, true); //Night charge should be applied.
              }

              //Tests cases with edge case times where the Night charge is not applied.
              [Theory]
              [InlineData("2017-04-11T06:00:00")]
              [InlineData("2013-01-01T06:00:01")]
              [InlineData("2012-05-31T19:59:59")]
              [InlineData("320-06-11T19:59:59")]
              [InlineData("1800-11-05T06:00:01")]
              [InlineData("100-07-01T06:00:00")]
              [InlineData("2019-04-22T19:59:58")]
              public void ExtremeDataFalseTest(String input)
              {
                   var j = new Journey{Origin=DateTime.Parse(input)}; //Has no dependency on the Fast or Slow fields of the model
                   Assert.Equal(j.NightCharge, false); //Night charge should not be applied.
              }

       };

       public class WeekdayChargeTesting
       {
            //Based off example in specification (Ride takes place at 17:30 so should have weekday surcharge).
            [Fact]
            public void ExampleTest()
            {
                 var j = new Journey{Origin=DateTime.Parse("2010-10-08T17:30:00"),Slow=2,Fast=5};
                 Assert.Equal(j.WeekdayCharge, true);
            }

            //Tests cases with normal times where the Night charge is applied.
            [Theory]
            [InlineData("2017-01-18T16:30:00")]
            [InlineData("2005-09-06T17:33:22")]
            [InlineData("2009-10-27T17:21:54")]
            [InlineData("2016-04-12T18:12:33")]
            [InlineData("1800-11-05T18:04:17")]
            [InlineData("2016-11-29T19:21:54")]
            public void NormalDataTrueTest(String input)
            {
                 var j = new Journey{Origin=DateTime.Parse(input)}; //Has no dependency on the Fast or Slow fields of the model.
                 Assert.Equal(j.WeekdayCharge, true); //Night charge should be applied.
            }

            //Tests cases with normal times where the Night charge is not applied.
            [Theory]
            [InlineData("2017-04-11T14:20:00")]
            [InlineData("2013-01-01T11:30:01")]
            [InlineData("2012-05-31T21:40:59")]
            [InlineData("2015-05-22T22:15:22")]
            [InlineData("1980-10-25T17:33:22")]
            [InlineData("2017-06-17T19:21:54")]
            [InlineData("2015-03-21T19:12:33")]
            [InlineData("2016-11-05T16:39:21")]
            public void NormalDataFalseTest(String input)
            {
                 var j = new Journey{Origin=DateTime.Parse(input)}; //Has no dependency on the Fast or Slow fields of the model
                 Assert.Equal(j.WeekdayCharge, false); //Night charge should not be applied.
            }

            //Tests cases with edge case times where the Night charge is applied.
            [Theory]
            [InlineData("2017-01-18T16:00:00")] //Wednesday at 4pm
            [InlineData("2015-05-19T19:59:59")] //Tuesday at 7:59:59pm
            [InlineData("1980-10-20T16:00:00")] //Monday at 4pm
            [InlineData("2017-06-16T19:59:59")] //Friday at 7:59:59pm
            public void ExtremeDataTrueTest(String input)
            {
                 var j = new Journey{Origin=DateTime.Parse(input)}; //Has no dependency on the Fast or Slow fields of the model.
                 Assert.Equal(j.WeekdayCharge, true); //Night charge should be applied.
            }

            //Tests cases with edge case times where the Night charge is not applied.
            [Theory]
            [InlineData("2017-04-11T15:59:59")] //Tuesday at 15:59:59pm
            [InlineData("2013-01-04T20:00:00")] //Friday at 8pm
            [InlineData("2012-05-28T15:59:59")] //Monday at 15:59:59pm
            [InlineData("2012-05-28T06:00:01")] //Monday at 8pm
            [InlineData("2017-07-15T16:00:00")]  //Saturday at 4pm
            [InlineData("2019-09-22T19:59:59")] //Sunday at 7:59:59pm
            public void ExtremeDataFalseTest(String input)
            {
                 var j = new Journey{Origin=DateTime.Parse(input)}; //Has no dependency on the Fast or Slow fields of the model.
                 Assert.Equal(j.WeekdayCharge, false); //Night charge should be applied.
            }
      };

      public class CostTesting //Tests the calculated field value Cost. Values calculated manually to compare to.
      {
           //Based off example in specification, total cost should be $9.75.
           [Fact]
           public void ExampleTest(){
                var j = new Journey{Origin = DateTime.Parse("2010-10-08T17:30:00"),Slow=2,Fast=5};
                Assert.Equal(j.Cost,9.75m);
           }

           //Test case with normal type data, Night/weekday charge's be applied(will note in those cases).
           [Theory]
           [InlineData("2017-01-20T18:30:00", 1.4,4,8.35)] //Friday at 6:30pm, 1.4mi slow and 8 minutes fast, Weekday Charge applied.
           [InlineData("2016-06-13T11:00:37",0.6,9,7.7)] //Monday at 11am, 0.6mi slow and 9 minutes fast.
           [InlineData("2014-10-25T19:14:22",0.2,14,8.75)] //Saturday at 7.14pm, 0.2mi slow and 14 minutes fast.
           [InlineData("2015-12-14T23:49:41",2.6,28,18.35)] //Wednesday at 11:49pm, 2.6mi slow and 28 minutes fast, Night Charge Applied.
           [InlineData("2021-03-09T04:14:28",1,3,6.8)] //Tuesday at 4:14am, 1mi slow and 3 minutes fast, Night charge applied.
           [InlineData("2017-06-01T19:45:07",5.4,2,14.65)]//Thursday at 7:45pm, 5.4mi slow and 2 minutes fast. //Weekday charge apploed.
           public void normalTest(string o,decimal s,int f, decimal r){ //Checks if the journey with parameters origin o, slow dist s and fast time f, has result cost r.
                var j = new Journey{Origin = DateTime.Parse(o),Slow = s,Fast = f};
                Assert.Equal(j.Cost,r);
           }

           //Test case with extreme type data, Night/weekday charge's be applied(will note in those cases).
           [Theory]
           [InlineData("2017-01-20T16:00:00", 1.4,4,8.35)] //Friday at 4pm, 1.4mi slow and 8 minutes fast, Weekday Charge applied.
           [InlineData("2017-01-20T15:59:59", 1.4,4,7.35)]//Friday at 3:59:59pm, 1.4mi slow and 8 minutes fast, Weekday Charge not applied.
           [InlineData("2017-01-20T19:59:59", 1.4,4,8.35)] //Friday at 7:59:59pm, 1.4mi slow and 8 minutes fast, Weekday Charge applied.
           [InlineData("2017-01-20T20:00:00", 1.4,4,7.85)]//Friday at 8:00:00pm, 1.4mi slow and 8 minutes fast, Weekday Charge not applied, Night Charge Applied.
           [InlineData("2015-12-14T05:59:59",2.6,28,18.35)] //Wednesday at 5:59:59am, Night Charge Applied
           [InlineData("2015-12-14T06:00:00",2.6,28,17.85)]  //Wednesday at 6:00am, No charge applied
           [InlineData("2017-06-01T23:59:59",5.4,2,14.15)] //Thursday at 11:59:59pm, Night charge applied.
           [InlineData("2017-06-02T00:00:00",5.4,2,14.15)] //Friday at 12:00:00am, Night charge applied.
           [InlineData("2015-12-14T13:49:41",0,0,3.5)] //Wednesday at 13:49pm, 0mi slow and 0 minutes fast, no charges applied.
           [InlineData("2015-12-14T17:49:41",0,0,4.5)] //Wednesday at 5:49pm, 0mi slow and 0 minutes fast, weekday charge applied.
           [InlineData("2015-12-14T21:49:41",0,0,4)]   //Wednesday at 9:49pm, 0mi slow and 0 minutes fast, Night charge applied.
           public void extremeTest(string o,decimal s,int f, decimal r){ //Checks if the journey with parameters origin o, slow dist s and fast time f, has result cost r.
                var j = new Journey{Origin = DateTime.Parse(o),Slow = s,Fast = f};
                Assert.Equal(j.Cost,r);
           }
      }

}
