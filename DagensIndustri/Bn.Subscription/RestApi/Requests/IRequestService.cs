using System.Net.Http;
using System.Threading.Tasks;

namespace Bn.Subscription.RestApi.Requests
{
    public interface IRequestService
    {
        Task<T> GetAsync<T>(string url, string token = null);

        Task<TOutput> PutAsJsonAsync<TInput, TOutput>(string url, TInput value, string token = null);

        Task<T> PutAsync<T>(string url, HttpContent content, string token = null);

        Task<T> PostAsync<T>(string url, HttpContent content, string token = null);

        Task<TOutput> PostAsJsonAsync<TInput, TOutput>(string url, TInput value, string token = null);

        Task<T> DeleteAsync<T>(string url, string token = null);
    }
}