using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using TwitterDoodle.Data;

namespace TwitterDoodle.Http {

    internal class TwitterQueryContent : StringContent {

        //TODO: Get rid of this! They already one
        //      OOB you smart-ass: FormUrlEncodedContent
        public TwitterQueryContent(TwitterQueryCollection collection) : base(collection.ToString()) {
            
            this.Headers.ContentType = MediaTypeConstants.ApplicationFormUrlEncodedMediaType;
        }
    }
}