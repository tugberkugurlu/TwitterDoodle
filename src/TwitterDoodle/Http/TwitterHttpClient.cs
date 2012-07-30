using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using TwitterDoodle.Data;
using TwitterDoodle.OAuth;

namespace TwitterDoodle.Http {
    
    public class TwitterHttpClient : HttpClient {

        public TwitterHttpClient(OAuthCredential oAuthCredential, OAuthSignatureEntity signatureEntity) 
            : this(oAuthCredential, signatureEntity, null) { 
        }

        public TwitterHttpClient(OAuthCredential oAuthCredential, OAuthSignatureEntity signatureEntity,
            TwitterQueryCollection parameters) : base(new TwitterOAuthMessageHandler(oAuthCredential, signatureEntity, parameters, new HttpClientHandler())) {

            if (oAuthCredential == null) {
                throw new NullReferenceException("oAuthCredential");
            }

            if (signatureEntity == null) {
                throw new NullReferenceException("signatureEntity");
            }
        }
    }
}