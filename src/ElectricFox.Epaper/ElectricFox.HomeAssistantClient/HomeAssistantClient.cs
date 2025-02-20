using System.Net.Http.Headers;
using System.Text.Json;
using NodaTime.Serialization.SystemTextJson;
using ElectricFox.HomeAssistant.Model;
using ElectricFox.HomeAssistant.Model.Climate;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ElectricFox.HomeAssistant
{
    public class HomeAssistantClient : IHomeAssistantClient
    {
        private readonly IHomeAssistantClientOptions _options;
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeAssistantClient> _logger;
        private readonly JsonSerializerOptions _jsonOptions =
            new JsonSerializerOptions().ConfigureForNodaTime(new NodaJsonSettings());

        public HomeAssistantClient(
            IHomeAssistantClientOptions options,
            HttpClient client,
            ILogger<HomeAssistantClient> logger
        )
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Sensor?> GetSensorState(
            string sensorId,
            CancellationToken cancellationToken
        )
        {
            var builder = new UriBuilder(_options.BaseUrl) { Path = $"/api/states/{sensorId}", };

            return await Request<Sensor>(builder.Uri, cancellationToken);
        }

        public async Task<Climate?> GetClimate(
            string climateId,
            CancellationToken cancellationToken
        )
        {
            var builder = new UriBuilder(_options.BaseUrl) { Path = $"/api/states/{climateId}", };

            return await Request<Climate>(builder.Uri, cancellationToken);
        }

        public async Task<List<StateHistory>?> GetSensorStateHistory(
            string sensorId,
            DateTime from,
            CancellationToken cancellationToken
        )
        {
            var builder = new UriBuilder(_options.BaseUrl)
            {
                Path = $"/api/history/period/{from:yyyy-MM-ddTHH:mm:ssZ}",
                Query =
                    $"filter_entity_id={sensorId}&minimal_response&no_attributes&significant_changes_only",
            };

            var results = await Request<List<List<StateHistory>>>(builder.Uri, cancellationToken)
                .ConfigureAwait(false);

            return results?.FirstOrDefault();
        }

        public async Task<T?> Request<T>(Uri uri, CancellationToken cancellationToken)
            where T : class
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    _options.ApiToken
                );

                var response = await _httpClient
                    .SendAsync(requestMessage, cancellationToken)
                    .ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                try
                {
                    return await response.Content
                        .ReadFromJsonAsync<T?>(_jsonOptions, cancellationToken)
                        .ConfigureAwait(false);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Failed to deserialize response");
                    return null;
                }
            }
        }
    }
}
