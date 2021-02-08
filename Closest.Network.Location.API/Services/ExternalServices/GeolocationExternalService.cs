using Closest.Network.Location.API.Services.Dtos;
using Closest.Network.Location.API.Services.ExternalServices.Contracts;
using Closest.Network.Location.API.Settings.Contracts;
using Messages.Core;
using Messages.Core.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Closest.Network.Location.API.Services.ExternalServices
{
    public class GeolocationExternalService : IGeolocationExternalService
    {
        IHttpClientFactory ClientFactory { get; }

        IClosestNetworkLocationSettings Settings { get; }

        public GeolocationExternalService(IHttpClientFactory clientFactory, IClosestNetworkLocationSettings settings)
        {
            ClientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<Response<LocationDto>> GetGeolocationAsync(string address)
        {
            var response = Response<LocationDto>.Create();

            if (string.IsNullOrEmpty(address))
                return response.WithBusinessError("Endereço inválido");

            var client = ClientFactory.CreateClient(Settings.GeocoddingSettings.GeocodeApiBaseSetting);

            var url = $"?address={address.Replace(" ", "+").Replace(",", string.Empty)}&components=country:BR&language=pt-BR&key={Settings.GeocoddingSettings.GeocodeApiKey}";

            var geocodingResponse = await client.GetAsync(url);

            var deserializedResponse =  await DeserializeResponseAsync(geocodingResponse);

            if (deserializedResponse.HasError)
                return response.WithMessages(deserializedResponse.Messages);

            return deserializedResponse.Data.Value.Results[0].Geometry.Location;
        }

        static async Task<Response<GeoLocationResponseDto>> DeserializeResponseAsync(HttpResponseMessage geocodingResponse)
        {
            var response = Response<GeoLocationResponseDto>.Create();

            if (!geocodingResponse.IsSuccessStatusCode)
                return response.WithCriticalError($"StatusCode: {geocodingResponse.StatusCode}; ReasonPhase: {geocodingResponse.ReasonPhrase}; RequestUri: {geocodingResponse.RequestMessage.RequestUri}");

            try
            {
                var content = geocodingResponse.Content.ReadAsStringAsync().Result;

                var responseBody = await geocodingResponse.Content.ReadAsAsync<ExpandoObject>() as IDictionary<string, object>;

                if (!responseBody.ContainsKey("status"))
                    return response.WithCriticalError(content);

                else if (responseBody["status"].ToString().Equals("ZERO_RESULTS"))
                    return response.WithBusinessError("Endereço não foi encontrado");

                else if (!responseBody["status"].ToString().Equals("OK"))
                    return response.WithCriticalError($"Falha na consulta ao endereço: {responseBody["status"]}");

                return response.SetValue(JsonConvert.DeserializeObject<GeoLocationResponseDto>(content));
            }
            catch (Exception ex)
            {
                return response.WithCriticalError(ex.ToString());
            }
        }
    }
}
