using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OutlookAddin.Domain;

namespace OutlookAddIn.WebAPIClient
{
    public class WebAPIDataAccess
    {
        private static HttpClient client = null;

        public WebAPIDataAccess()
        {
            if (client == null)
            {
                InitializeAsync(GlobalConstants.WebApiBaseUri);
            }
        }

        private void InitializeAsync(string baseUri)
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(baseUri)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // using System.Net;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls
               | SecurityProtocolType.Tls11
               | SecurityProtocolType.Tls12
               | SecurityProtocolType.Ssl3;
        }

        public async Task<bool> DoLogin(LoginEventArgs loginArgs)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(loginArgs), Encoding.UTF8, "application/json");
            // HTTP POST
            var response = await client.PostAsync("api/authentications", content);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var authResult = JsonConvert.DeserializeObject<AuthenticationResult>(data);
            }
            else
            {
                return false;
            }

            return true;
        }

        public async Task<Appointments> GetBookingRecords()
        {
            try
            {
                // HTTP GET
                var response = await client.GetAsync(String.Format("api/condos/{0}/booking-details?isCurrent=false&viewFormat=EXTMAX", GlobalConstants.WebApiCondoId));

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var bookingDetailsResult = JsonConvert.DeserializeObject<GetBookingDetailsResult>(data);

                    if (bookingDetailsResult == null ||
                       bookingDetailsResult.entities == null)
                    {
                        return null;
                    }

                    var appointments = new Appointments();
                    appointments.Clear();

                    foreach (var entity in bookingDetailsResult.entities)
                    {
                        if (entity.facilityBooking != null)
                        {
                            var appointment = new Appointment
                            {
                                Subject = string.Format("{0} - {1}",
                                        entity.facilityBooking.requestNo,
                                        entity.facilityBooking.facility.name),
                                FacilityID = entity.facilityBooking.facility.id,
                                StartTime = Utils.ConvertUnixTicksToDateTime(entity.facilityBooking.requestedStartDate.Value),
                                EndTime = Utils.ConvertUnixTicksToDateTime(entity.facilityBooking.requestedEndDate.Value),
                            };
                            appointments.Add(appointment);
                        }
                    }

                    return appointments;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }

        public async Task<ObservableCollectionWrapper<Facility>> GetFacilitiesVenue()
        {
            try
            {
                // HTTP GET
                var response = await client.GetAsync(String.Format("api/condos/{0}/condo-facilities?isActive=true&viewFormat=EXTMAX", GlobalConstants.WebApiCondoId));

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var facilitiesResult = JsonConvert.DeserializeObject<GetFacilitiesResult>(data);

                    if (facilitiesResult == null ||
                       facilitiesResult.entities == null)
                    {
                        return null;
                    }

                    var facilities = new ObservableCollectionWrapper<Facility>();

                    foreach (var entity in facilitiesResult.entities)
                    {
                        facilities.Add(entity);
                    }

                    return facilities;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }

        public async Task<ObservableCollectionWrapper<TimeSlot>> GetTimeSlots(int facilityId, long fromTicks, long toTicks)
        {
            try
            {
                // First, try getting timeslots from cache
                ObjectCache cache = MemoryCache.Default;
                string cacheKey = string.Format(GlobalConstants.TimeSlotCacheKey,
                    facilityId,
                    fromTicks,
                    toTicks);

                if (cache.Contains(cacheKey))
                {
                    var result = (ObservableCollectionWrapper<TimeSlot>)cache.Get(cacheKey);

                    if (result != null && result.Count > 0)
                        return result;
                }

                // HTTP GET
                var response = await client.GetAsync(String.Format("api/condos/{0}/condo-facilities/{1}/timeslots?from={2}&to={3}&viewFormat=EXTMAX", GlobalConstants.WebApiCondoId,
                facilityId,
                fromTicks,
                toTicks));

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var timeslotsResult = JsonConvert.DeserializeObject<GetTimeslotsResult>(data);

                    if (timeslotsResult == null ||
                       timeslotsResult.entities == null)
                    {
                        return null;
                    }

                    var timeslots = new ObservableCollectionWrapper<TimeSlot>();

                    foreach (var entity in timeslotsResult.entities)
                    {
                        timeslots.Add(entity);
                    }

                    // Store data in the cache
                    CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                    cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(1.0);
                    cache.Add(cacheKey, timeslots, cacheItemPolicy);

                    return timeslots;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }

        public async Task<bool> SaveBookingRequest(SaveBookingRequestArgs args)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(args), Encoding.UTF8, "application/json");
            // HTTP POST
            var response = await client.PostAsync(string.Format("api/condos/{0}/facility-bookings", GlobalConstants.WebApiCondoId), content);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var authResult = JsonConvert.DeserializeObject<AuthenticationResult>(data);
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
