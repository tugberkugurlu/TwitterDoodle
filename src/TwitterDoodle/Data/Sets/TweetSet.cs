using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using TwitterDoodle.Http;
using TwitterDoodle.OAuth;
using System.Net.Http.Formatting;

namespace TwitterDoodle.Data {

    public class TweetSet : ITwitterSet<Tweet> {

        public Task<Tweet> Tweet(TwitterQueryCollection collection, OAuthCredential creds, OAuthSignatureEntity signatureEntity) {

            using (TwitterHttpClient client = new TwitterHttpClient(creds, signatureEntity, collection)) {

                //TODO: don't use Result
                var response = client.PostAsTwitterQueryAsync(TwitterUriConstants.StatusUpdateUri).Result;
                return response.Content.ReadAsAsync<Tweet>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });
            }
        }
    }
}