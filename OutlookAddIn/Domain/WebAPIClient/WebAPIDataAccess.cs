using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OutlookAddin.Domain;
using OutlookAddIn.CustomScheduler.Model;

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
            //HttpClient client1 = new HttpClient();
            //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/accounts/3?viewFormat=EXTPUB");

            //// Add token to the Authorization header and make the request
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer");
            //HttpResponseMessage response = await client1.SendAsync(request);

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
                                StartTime = Utils.ConvertLongToDateTime(entity.facilityBooking.requestedStartDate.Value),
                                EndTime = Utils.ConvertLongToDateTime(entity.facilityBooking.requestedEndDate.Value),
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
    }
}
