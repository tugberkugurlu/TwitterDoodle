using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using TwitterDoodle.Data;
using TwitterDoodle.OAuth;

namespace TwitterDoodle.Http {
    
    public class TwitterHttpClient : HttpClient {

        private readonly OAuthCredential _oAuthCredential;
        private readonly OAuthSignatureEntity _oAuthSignatureEntity;
        private readonly TwitterQueryCollection _twitterQueryCollection;

        public TwitterHttpClient(OAuthCredential oAuthCredential, OAuthSignatureEntity oAuthSignatureEntity) 
            : this(oAuthCredential, oAuthSignatureEntity, null) { 
        }

        public TwitterHttpClient(OAuthCredential oAuthCredential, OAuthSignatureEntity oAuthSignatureEntity,
            TwitterQueryCollection queryCollection) : base(new TwitterOAuthMessageHandler(oAuthCredential, oAuthSignatureEntity, queryCollection, new HttpClientHandler())) {

            if (oAuthCredential == null) {
                throw new NullReferenceException("oAuthCredential");
            }

            if (oAuthSignatureEntity == null) {
                throw new NullReferenceException("signatureEntity");
            }

            _oAuthCredential = oAuthCredential;
            _twitterQueryCollection = queryCollection;
            _oAuthSignatureEntity = oAuthSignatureEntity;
        }

        public TwitterQueryCollection TwitterQueryCollection { 

            get {
                return _twitterQueryCollection;
            } 
        }
    }
}