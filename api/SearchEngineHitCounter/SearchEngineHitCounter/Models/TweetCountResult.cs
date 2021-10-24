namespace SearchEngineHitCounter.Models
{
    public class TweetCountResult
    {
        public MetaData Meta { get; set; }

        public class MetaData
        {
            public string TotalTweetCount { get; set; }
        }
    }
}
