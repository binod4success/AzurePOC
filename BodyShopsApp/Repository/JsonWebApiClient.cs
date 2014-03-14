using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace BodyShopsApp.Repository
{
    public class JsonWebApiClient : HttpClient
    {
        public JsonWebApiClient()
        {
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public JsonWebApiClient(string baseUri)
            : this()
        {
            BaseAddress = new Uri(baseUri);
        }

        public virtual TResponse Get<TResponse>(string uri)
        {
            var response = GetAsync(PrefixUri(uri)).Result;

            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<TResponse>().Result;
            else
                throw new HttpResponseException(response);
        }

        public virtual TResponse Post<TResponse>(string uri, object content)
        {
            var response = this.PostAsJsonAsync(PrefixUri(uri), content).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<TResponse>().Result;
            else
                throw new HttpResponseException(response);
        }

        public virtual TResponse Put<TResponse>(string uri, object content)
        {
            var response = this.PutAsJsonAsync(PrefixUri(uri), content).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<TResponse>().Result;
            else
                throw new HttpResponseException(response);
        }

        public virtual TResponse Delete<TResponse>(string uri)
        {
            var response = DeleteAsync(PrefixUri(uri)).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<TResponse>().Result;
            else
                throw new HttpResponseException(response);
        }

        private string PrefixUri(string uri)
        {
            var baseAddress = BaseAddress.ToString();
            if (!baseAddress.EndsWith("/"))
                baseAddress += "/";
            if (uri.Length > 0 && uri.StartsWith("/"))
                uri = uri.Substring(1);
            return baseAddress + uri;
        }
    }
}