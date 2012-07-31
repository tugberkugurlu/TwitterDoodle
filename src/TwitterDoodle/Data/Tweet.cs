using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace TwitterDoodle.Data {

    public class Tweet : ITwitterEntity {

        [JsonProperty(PropertyName = "id_str")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty(PropertyName = "retweeted")]
        public bool IsRetweeted { get; set; }

        [JsonProperty(PropertyName = "retweet_count")]
        public int RetweetCount { get; set; }

        [JsonProperty(PropertyName = "in_reply_to_user_id")]
        public string InReplyToUserId { get; set; }

        [JsonProperty(PropertyName = "in_reply_to_status_id")]
        public string InReplyToStatusId { get; set; }
    }
}