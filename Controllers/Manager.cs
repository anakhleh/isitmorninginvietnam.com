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

            var currentUTC = DateTime.UtcNow.AddHours(12);
            var currentIndochinaTime = currentUTC.AddHours(7);


            Console.WriteLine("AAA");
            Console.WriteLine(currentIndochinaTime.Date.ToString("yyyy-MM-dd"));

            //get the sunrise time for the current day in vietnam
            var responseJSON = JsonConvert.DeserializeObject<SunriseApiResponse>(
                WebGetSync("https://api.sunrise-sunset.org/json?lat=10.8231&lng=106.6297&formatted=0&date="
                            + currentIndochinaTime.Date.ToString("yyyy-MM-dd"))
            );

            Console.WriteLine("https://api.sunrise-sunset.org/json?lat=10.8231&lng=106.6297&formatted=0&date="
                            + currentIndochinaTime.Date.ToString("yyyy-MM-dd"));

            var noonUTC = currentUTC.Date + new TimeSpan(12,0,0);

            var isMorning = IsMorning(
                responseJSON.results.sunrise,
                noonUTC, 
                currentUTC
            );

            Console.WriteLine(isMorning);


            Console.WriteLine(currentUTC);
            Console.WriteLine(responseJSON.results.sunrise);

            //if it is morning
            if(isMorning){
                Console.WriteLine("Itsmorning");
                return new IsItMorningInVietnamResponse
                {
                    IsMorning = true,
                    TimeToNextSunriseText = ""
                };
            }
            //If the current utc date is before the one in vietnam
            //or its the same day, but morning has not come yet
            else if(currentUTC.Date < currentIndochinaTime.Date
                    || (currentUTC < responseJSON.results.sunrise) )
            {
                Console.WriteLine("NotYetMorning. Its tomorrow or later today");
                TimeSpan timeToNextSunrise = responseJSON.results.sunrise.Subtract(currentUTC);
                timeToNextSunrise = timeToNextSunrise.Add(
                    TimeSpan.FromMinutes(5-((int)timeToNextSunrise.TotalMinutes % 5)) );
                
                return new IsItMorningInVietnamResponse
                {
                    IsMorning = false,
                    TimeToNextSunriseText = ((int) Math.Floor((double)(int) timeToNextSunrise.TotalHours)).ToString()
                                            + " hours and "
                                            + ((int)(timeToNextSunrise.TotalMinutes) % 60).ToString()
                                             + " minutes"
                };
            }
            //if it's the same day that it is in vietnam, and morning has already passed
            else
            {
                Console.WriteLine("Morning Happned, its again tomorrow.");
                
                 var responseJSONForTomorrow = JsonConvert.DeserializeObject<SunriseApiResponse>(
                    WebGetSync("https://api.sunrise-sunset.org/json?lat=10.8231&lng=106.6297&formatted=0&date="
                    + currentIndochinaTime.AddDays(1).Date.ToString("yyyy-MM-dd"))
                );

                TimeSpan timeToNextSunrise = responseJSONForTomorrow.results.sunrise.Subtract(currentUTC);
                timeToNextSunrise = timeToNextSunrise.Add(
                    TimeSpan.FromMinutes(5-((int)timeToNextSunrise.TotalMinutes % 5)) );

                return new IsItMorningInVietnamResponse
                {
                    IsMorning = false,
                    TimeToNextSunriseText = ((int) Math.Floor((double)(int) timeToNextSunrise.TotalHours)).ToString()
                                            + " hours and "
                                            + ((int)(timeToNextSunrise.TotalMinutes) % 60).ToString()
                                             + " minutes"
                };
            }


            // var currentUTC = DateTime.UtcNow;
            // var todayNoonUTC = DateTime.UtcNow;

            // todayNoonUTC = todayNoonUTC.AddMilliseconds(
            //     -todayNoonUTC.Millisecond
            // ).AddSeconds(
            //     -todayNoonUTC.Second
            // ).AddMinutes(
            //     -todayNoonUTC.Minute
            // );

            // if(todayNoonUTC.Hour > 12)
            // {
            //     todayNoonUTC.AddHours(-(todayNoonUTC.Hour - 12));
            // }
            // else
            // {
            //     todayNoonUTC.AddHours(12 - todayNoonUTC.Hour);
            // }

            // Console.WriteLine(responseJSON.results.sunrise);
            // Console.WriteLine(todayNoonUTC);
            // Console.WriteLine(currentUTC);

            // bool isMorning = IsItMorning(
            //     responseJSON.results.sunrise,
            //     todayNoonUTC,
            //     currentUTC
            // );

            
            // if(!isMorning)
            // {
            //     if(currentUTC.Date > responseJSON.results.sunrise.Date)
            //     {
            //                         Console.WriteLine("A");
            //         Console.WriteLine(responseJSON.results.sunrise.ToString());
            //         Console.WriteLine( currentUTC.AddHours(7).ToString() );
            //         string tomorrowDate = DateTime.Now.AddDays(1).Date
            //                         .ToString("u",CultureInfo.CreateSpecificCulture("de-DE"))
            //                         .Substring(0,10);
            //          var nextMorningResponseJSON = JsonConvert.DeserializeObject<SunriseApiResponse>(
            //         WebGetSync("https://api.sunrise-sunset.org/json?lat=10.8231&lng=106.6297&formatted=0&date=" + tomorrowDate)
            //     );
            //      //ceiling to next 5 minutes
            //     var timeToNextSunrise = nextMorningResponseJSON.results.sunrise.Add(
            //         TimeSpan.FromMinutes((int)5-(nextMorningResponseJSON.results.sunrise.Minute % 5))
            //     );

            //      return new IsItMorningInVietnamResponse
            //         {
            //             IsMorning = isMorning,
            //             TimeToNextSunriseText = ((int) Math.Floor((double)(int) timeToNextSunrise.Hour)).ToString()
            //                                     + " hours and "
            //                                     + ((int)(timeToNextSunrise.Minute % 60)).ToString()
            //                                     + " minutes"
            //         };
               
            //     }
            //     else if (currentUTC.Hour > responseJSON.results.solar_noon.Hour)
            //     {
            //         Console.WriteLine("B1");
            //         Console.WriteLine(responseJSON.results.sunrise.ToString());
            //         Console.WriteLine( currentUTC.AddHours(7).ToString() );
            //         TimeSpan timeToTodaySunrise = (responseJSON.results.sunrise).Subtract(currentUTC.AddHours(7));
            //         
            //     );

            //         return new IsItMorningInVietnamResponse
            //         {
            //             IsMorning = isMorning,
            //             TimeToNextSunriseText = ((int) Math.Floor((double)(int) timeToTodaySunrise.TotalHours)).ToString()
            //                                     + " hours and "
            //                                     + ((int)(timeToTodaySunrise.TotalMinutes) % 60).ToString()
            //                                     + " minutes"
            //         };
            //     }
            //     else
            //     {
            //         Console.WriteLine("B2");

            //         TimeSpan timeToTodaySunrise = (currentUTC.AddHours(7)).Subtract(responseJSON.results.sunrise);
            //         timeToTodaySunrise = timeToTodaySunrise.Add(
            //         TimeSpan.FromMinutes(5-((int)timeToTodaySunrise.TotalMinutes % 5))
            //     );

            //         return new IsItMorningInVietnamResponse
            //         {
            //             IsMorning = isMorning,
            //             TimeToNextSunriseText = ((int) Math.Floor((double)(int) timeToTodaySunrise.TotalHours)).ToString()
            //                                     + " hours and "
            //                                     + ((int)(timeToTodaySunrise.TotalMinutes) % 60).ToString()
            //                                     + " minutes"
            //         };
            //     }

               

                

               

            // }
        }

        public bool IsMorning(DateTime sunriseUTC, DateTime noonUTC, DateTime givenUTC)
        {
            return (sunriseUTC <= givenUTC) && (givenUTC < noonUTC);
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

    }
}