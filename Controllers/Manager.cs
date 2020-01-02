using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;

namespace isitmorninginvietnam.com.Controllers
{
    public class Manager
    {
        public IsItMorningInVietnamResponse IsItMorningInVietnam(){
            //All time calculations are done in UTC time

            //Get current time in UTC
            

            //Hit the Sunrise Sunset API to get sunrise time. Save as string for manual processing later.
            //do this because parsing the response into an object containing DateTime objects with
            //Newtonsoft.JSON does not work properly. Hours from response are not maintained.
            string ApiResponseAsString = WebGetSync(
                "https://api.sunrise-sunset.org/json?lat=10.8231&lng=106.6297&formatted=0"
            );

            //Get sunrise time as yyyy-MM-ddTHH-mm-ssZ
            string ApiResponseSunriseDateTimeString = ApiResponseAsString.Substring(23,19) + "Z";

            //Parse the api response sunrise time string into a datetime manually.
            var sunriseUTC = 
                new DateTime(
                    Int32.Parse(ApiResponseSunriseDateTimeString.Substring(0,4)), //year
                    Int32.Parse(ApiResponseSunriseDateTimeString.Substring(5,2)), //month
                    Int32.Parse(ApiResponseSunriseDateTimeString.Substring(8,2))  //day
                ).Date //use the date to make sure we take none of the time portion
                + new TimeSpan(
                    Int32.Parse(ApiResponseSunriseDateTimeString.Substring(11,2)), //hours
                    Int32.Parse(ApiResponseSunriseDateTimeString.Substring(14,2)), //minutes
                    Int32.Parse(ApiResponseSunriseDateTimeString.Substring(17,2))  //seconds
                );
            
            //Get current UTC onto the same date as sunrise date
            var systemTime = DateTime.UtcNow;
            var currentUTC = systemTime;


            //Indochina (Vietnam) time is UTC+7 or 7 hours ahead of UTC.
            //Therefore, when it is noon in Vietnam, it 5 AM in UTC Time. (5 + 7 = 12)
            var noonUTC = currentUTC.Date + new TimeSpan(5,0,0);

            var isMorning = IsMorning(
                sunriseUTC,
                noonUTC, 
                currentUTC
            );
            // Console.WriteLine("sunriseUTC: ", sunriseUTC);
            // Console.WriteLine("noonUTC: ", noonUTC);
            // Console.WriteLine("currentUTC: ", currentUTC);
            
            if(isMorning)
            {
                Console.WriteLine("isMorning");
                //If it is morning
                //Note, the representation of current time is in UTC,
                //which is 7 hours behind Vietnam (Indochina) time,
                //we have to add 7 hours to UTC time to get local Indochina time
                return new IsItMorningInVietnamResponse{
                    IsMorning = true,
                    TimeToNextSunriseText = "",
                    CurrentTimeText = currentUTC.AddHours(7).ToShortTimeString() //Get time as HH:mm AMorPM
                };
            }
            else if((currentUTC < sunriseUTC))
            {
                Console.WriteLine("current < sunrise");
                //if it is not morning, but the time is not yet sunrise

                //Get a string to represent how much time till sunrise today like:
                //HH hours and mm minutes
                // or 
                //H hours and m minutes
                //depending on the amount of hours and minutes
                //Ceiling this time to the nearest 5 minutes

                TimeSpan timeSpanToNextSunrise = sunriseUTC.Subtract(currentUTC);

                return new IsItMorningInVietnamResponse{
                    IsMorning = false,
                    TimeToNextSunriseText = GetTimeToNextSunriseTextFromHoursAndMinutes(
                        timeSpanToNextSunrise.Hours,
                        timeSpanToNextSunrise.Minutes
                    ),
                    CurrentTimeText = ""
                };
            }
            else
            {
                Console.WriteLine("else");
                //if it is not morning, but the time is past noon,
                //get tomorrow's sunrise time, and then give the time till next sunrise 

                string ApiResponseForNextDayAsString = WebGetSync(
                    "https://api.sunrise-sunset.org/json?lat=10.8231&lng=106.6297&formatted=0&date="
                    + currentUTC.Date.AddDays(1).ToString("yyyy-MM-dd")
                );

                //Get sunrise time as yyyy-MM-ddTHH-mm-ssZ
                string ApiResponseNextSunriseDateTimeString = ApiResponseForNextDayAsString.Substring(23,19) + "Z";

                //Parse the api response sunrise time string into a datetime manually.
                var nextSunriseUTC = 
                    new DateTime(
                        Int32.Parse(ApiResponseNextSunriseDateTimeString.Substring(0,4)), //year
                        Int32.Parse(ApiResponseNextSunriseDateTimeString.Substring(5,2)), //month
                        Int32.Parse(ApiResponseNextSunriseDateTimeString.Substring(8,2))  //day
                    ).Date //use the date to make sure we take none of the time portion
                  + new TimeSpan(
                        Int32.Parse(ApiResponseNextSunriseDateTimeString.Substring(11,2)), //hours
                        Int32.Parse(ApiResponseNextSunriseDateTimeString.Substring(14,2)), //minutes
                        Int32.Parse(ApiResponseNextSunriseDateTimeString.Substring(17,2))  //seconds
                    );

                

                TimeSpan timeSpanToNextSunrise = nextSunriseUTC.Subtract(currentUTC);
                Console.WriteLine(nextSunriseUTC);
                Console.WriteLine(timeSpanToNextSunrise);

                return new IsItMorningInVietnamResponse{
                    IsMorning = false,
                    TimeToNextSunriseText = GetTimeToNextSunriseTextFromHoursAndMinutes(
                        timeSpanToNextSunrise.Hours,
                        timeSpanToNextSunrise.Minutes
                    ),
                    CurrentTimeText = ""
                };
            }
        }

        public bool IsMorning(DateTime sunriseUTC, DateTime noonUTC, DateTime givenUTC)
        {
            Console.WriteLine(sunriseUTC);
            Console.WriteLine(noonUTC);
            Console.WriteLine(givenUTC);
            Console.WriteLine((sunriseUTC <= givenUTC));
            Console.WriteLine((givenUTC < noonUTC));
            return (sunriseUTC <= givenUTC.AddDays(-1)) && (givenUTC.AddDays(-1) < noonUTC);
        }
       
        public string WebGetSync(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using(HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using(Stream stream = response.GetResponseStream())
            using(StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public string GetTimeToNextSunriseTextFromHoursAndMinutes(int hours, int minutes)
        {
            if(hours == 0){
                return (minutes + (5 - (minutes % 5))).ToString() + " minutes";
            }
            else if (minutes > 55)
            {
                //if there are more than 55 minutes in the timespan, round to the next hour
                return (hours + 1).ToString() + " hours";
            }
            else
            {
                //if there are not more than 55 minutes in the timespan
                return hours.ToString()
                    + " hours and "
                    //ceiling minutes to nearest 5 minutes
                    + (minutes + (5 - (minutes % 5))).ToString() 
                    + " minutes";
            }   
        }

    }
}