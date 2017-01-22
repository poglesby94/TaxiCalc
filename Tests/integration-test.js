//Integration tests of our whole application.
//Run using protractor.js.
//Requires the dotnet app to be run as well as a selenium web service.
//First run ">dotnet run" and ">webdriver-manager start" in terminals.
//Setup using http://www.protractortest.org/#/tutorial.
//Then run the test file using ">protractor protractor.conf.js".

describe('Taxi Journey Calculator App', function() {
     beforeEach(function(){
          browser.get('http://localhost:5000/#/input'); //Access the application
     })

     it('should not allow you to click the button if the form is not completed', function() {
          expect(element(by.id('calculate')).isEnabled()).toEqual(false);

          //Input first field, button should still be disabled.
          element(by.model('dateOrigin')).sendKeys("08-10-2010");
          expect(element(by.id('calculate')).isEnabled()).toEqual(false);

          //Input second field, button should still be disabled.
          element(by.model('timeOrigin')).sendKeys("17:30");
          expect(element(by.id('calculate')).isEnabled()).toEqual(false);

          //Input third field, button should still be disabled.
          element(by.model('slow')).sendKeys("2");
          expect(element(by.id('calculate')).isEnabled()).toEqual(false);

          //Input final field, button should become enabled.
          element(by.model('fast')).sendKeys("5");
          expect(element(by.id('calculate')).isEnabled()).toEqual(true);
     });

     it('should correctly process the spec example', function() {

              //Fill input fields with data.
              element(by.model('dateOrigin')).sendKeys("10-08-2010");
              element(by.model('timeOrigin')).sendKeys("17:30");
              element(by.model('slow')).sendKeys("2");
              element(by.model('fast')).sendKeys("5");

              //Ensure the button is now able to be clicked.
              expect(element(by.id('calculate')).isEnabled()).toEqual(true);

              //Click the button to execute the call to the .net service and wait for content to load.
              element(by.id('calculate')).click();
              browser.sleep(2000);

              //Ensure all result fields contain the correct value.
              expect(element(by.id('dateValue')).getText()).toEqual("Tuesday, August 10, 2010 at 5:30 PM");
              expect(element(by.id('baseValue')).getText()).toEqual("$3.00");
              expect(element(by.id('taxValue')).getText()).toEqual("$0.50");
              expect(element(by.id('slowValue')).getText()).toEqual("$3.50");
              expect(element(by.id('fastValue')).getText()).toEqual("$1.75");
              expect(element(by.id('weekdayValue')).getText()).toEqual("$1.00");
              expect(element(by.id('nightValue')).isPresent()).toEqual(false);
              expect(element(by.id('costValue')).getText()).toEqual("$9.75");
      });

     it('should allow you to process a second calculation once you have completed the first', function(){
           //Fill input fields with data.
           element(by.model('dateOrigin')).sendKeys("08-10-2010");
           element(by.model('timeOrigin')).sendKeys("17:30");
           element(by.model('slow')).sendKeys("2");
           element(by.model('fast')).sendKeys("5");

           //Trigger the calculation.
           element(by.id('calculate')).click();

           //Input new data into fields.
           element(by.model('dateOrigin')).sendKeys("22-01-2017");
           element(by.model('timeOrigin')).sendKeys("03:30");
           element(by.model('slow')).clear();
           element(by.model('slow')).sendKeys("1.4");
           element(by.model('fast')).clear();
           element(by.model('fast')).sendKeys("9");

           //Trigger the calculation.
           element(by.id('calculate')).click();
           browser.sleep(2000);

           //Ensure the results are correct wrt the second form.
           expect(element(by.id('baseValue')).getText()).toEqual("$3.00");
           expect(element(by.id('taxValue')).getText()).toEqual("$0.50");
           expect(element(by.id('slowValue')).getText()).toEqual("$2.45");
           expect(element(by.id('fastValue')).getText()).toEqual("$3.15");
           expect(element(by.id('weekdayValue')).isPresent()).toEqual(false);
           expect(element(by.id('nightValue')).getText()).toEqual("$0.50");
           expect(element(by.id('costValue')).getText()).toEqual("$9.60");



     });

     it('should always redirect you to http://localhost/#/input if you access http://localhost/#/***', function(){
          browser.get('http://localhost:5000/#/testing1'); //Access the application with a random string.
          browser.sleep(2000);
          expect(browser.getLocationAbsUrl()).toEqual('/input');

          browser.get('http://localhost:5000/#/'); //Access the application with empty string.
          browser.sleep(2000);
          expect(browser.getLocationAbsUrl()).toEqual('/input');

     });

     it('should return the correct result cost for example input',function(){
          var dateData = ["04-12-2012","22-01-2017","14-09-2016"];
          var timeData = ["03:30","13:17","21:42"];
          var slowData = ["3.2","0.4","10"];
          var fastData = ["2","24","11"];
          var costData = ["$10.30","$12.60","$25.35"];

          for(var i=0;i<dateData.length;i++){
               //Submit input into fields.
               element(by.model('dateOrigin')).sendKeys(dateData[i]);
               element(by.model('timeOrigin')).sendKeys(timeData[i]);
               element(by.model('slow')).sendKeys(slowData[i]);
               element(by.model('fast')).sendKeys(fastData[i]);

               //Trigger the calculation.
               element(by.id('calculate')).click();
               browser.sleep(2000);

               //Check the result cost is correct.
               expect(element(by.id('costValue')).getText()).toEqual(costData[i]);

               //Reload the browser page
               browser.get('http://localhost:5000/#/input');
          }
     });

});
