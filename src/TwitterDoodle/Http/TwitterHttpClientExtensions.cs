using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAPIDoodle.Http;

namespace TwitterDoodle.Http {

    public static class TwitterHttpClientExtensions {

        public static Task<HttpResponseMessage> PostAsTwitterQueryAsync(this TwitterHttpClient twitterHttpClient, string requestUri) {

            if (twitterHttpClient.TwitterQueryCollection == null) {
                throw new NullReferenceException("TwitterHttpClient.TwitterQueryCollection");
            }

            return twitterHttpClient.PostAsync(requestUri, new TwitterQueryContent(twitterHttpClient.TwitterQueryCollection));
        }

        public static Task<HttpResponseMessage> PostAsEmptyTwitterQueryAsync(this TwitterHttpClient twitterHttpClient, string requestUri) {

            return twitterHttpClient.PostAsync(requestUri, new EmptyContent());
        }
    }
}