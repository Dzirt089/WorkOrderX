using System.Net.Http.Json;
using System.Text.Json;

namespace WorkOrderX.ApiClients
{
	public abstract class BaseApiClient
	{
		protected readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _jsonOptions;

		protected BaseApiClient(HttpClient httpClient, JsonSerializerOptions jsonOptions)
		{
			_httpClient = httpClient;
			_jsonOptions = jsonOptions;
		}

		public async Task<T> PostTJsonTAsync<T>(string requestUri, object content, CancellationToken token = default)
		{
			var response = await _httpClient.PostAsJsonAsync(requestUri, content, _jsonOptions, token).ConfigureAwait(false);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadFromJsonAsync<T>(_jsonOptions, token).ConfigureAwait(false)
				   ?? throw new InvalidOperationException("Десериализация вернула null");
		}

		public async Task<T> GetTJsonTAsync<T>(string requestUri, CancellationToken token = default)
		{
			var result = await _httpClient.GetFromJsonAsync<T>(requestUri, _jsonOptions, token).ConfigureAwait(false)
			?? throw new InvalidOperationException("Response content is null.");

			return result;
		}

		public async Task<T> DeleteTJsonTAsync<T>(string requestUri, CancellationToken token = default)
		{
			var response = await _httpClient.DeleteAsync(requestUri, token).ConfigureAwait(false);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadFromJsonAsync<T>(_jsonOptions, token).ConfigureAwait(false)
				?? throw new InvalidOperationException("Response content is null.");
		}
	}
}
