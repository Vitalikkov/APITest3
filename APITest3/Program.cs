using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APITest3
{
        public partial class YouTubePlaylist
        {

            [JsonPropertyName("items")]
            public List<Item> Items { get; set; }
        }
        public partial class Item
        {
            [JsonPropertyName("snippet")]
            public Snippet Snippet { get; set; }

            [JsonPropertyName("contentDetails")]
            public ContentDetails ContentDetails { get; set; }

        }

        public partial class ContentDetails
        {
            [JsonPropertyName("videoPublishedAt")]
            public DateTimeOffset VideoPublishedAt { get; set; }
        }

        public partial class Snippet
        {
            [JsonPropertyName("title")]
            public string Title { get; set; }

            [JsonPropertyName("description")]
            public string Description { get; set; }

            [JsonPropertyName("channelTitle")]
            public string ChannelTitle { get; set; }

            [JsonPropertyName("resourceId")]
            public ResourceId ResourceId { get; set; }

            [JsonPropertyName("videoOwnerChannelId")]
            public string VideoOwnerChannelId { get; set; }
        }

        public partial class ResourceId
        {
            [JsonPropertyName("videoId")]
            public string VideoId { get; set; }
        }

        public partial class YouTubeVideo
        {
            [JsonPropertyName("items")]
            public List<ItemsVideo> ItemsVideo { get; set; }

        }

        public partial class ItemsVideo
        {
            [JsonPropertyName("snippet")]
            public SnippetVideo SnippetVideo { get; set; }

            [JsonPropertyName("statistics")]
            public Statistics Statistics { get; set; }
        }

        public partial class SnippetVideo
        {       
            [JsonPropertyName("publishedAt")]
            public DateTimeOffset PublishedAt { get; set; }

            [JsonPropertyName("title")]
            public string TitleVideo { get; set; }

            [JsonPropertyName("description")]
            public string DescriptionVideo { get; set; }
        }

        public partial class Statistics
        {
            [JsonPropertyName("viewCount")]
            public string ViewCount { get; set; }

            [JsonPropertyName("likeCount")]
            public string LikeCount { get; set; }
        }

        internal class Program
        {
            private static readonly HttpClient client = new HttpClient();
            private static string yotubeList = "https://youtube.googleapis.com/youtube/v3/playlistItems?part=contentDetails%2Cid%2Csnippet%2Cstatus&playlistId=PLSN6qXliOioz5lnckfofNcLJ3CnZJvEJO&key=AIzaSyCyXB4DG5SPAfdpwT9qXz4gMVYJI1_RHUs";
            
            private static List<string> videoIdsList = new List<string>();

        async static Task AddItemsToListAsync()
        {
            var responseString = await client.GetStringAsync(yotubeList);
            YouTubePlaylist youTubePlaylist =
                JsonSerializer.Deserialize<YouTubePlaylist>(responseString);
            foreach (var item in youTubePlaylist.Items)
            {
                
                string videoId = item.Snippet.ResourceId.VideoId;
                videoIdsList.Add(videoId);
            }
        }
        async static Task PrintYouTubePlaylistAsync()
            {
                var responseString = await client.GetStringAsync(yotubeList);
                YouTubePlaylist youTubePlaylist =
                    JsonSerializer.Deserialize<YouTubePlaylist>(responseString);
            Console.Clear();
            Console.WriteLine($"List of videos in Ekreative Internship Stories playlist:");
            Console.WriteLine();
            foreach (var title in youTubePlaylist.Items)
                {
                    string titleVideo = title.Snippet.Title;
                    Console.WriteLine($"{titleVideo}");
                
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("0. Enter to exit to the previous menu");
            Console.WriteLine();

            await Previous();


        }

        async static Task PrintYouTubeVideoLikeAsync()
        {
            int totalLikes = 0; 
            foreach (var id in videoIdsList)
            {
                string requestUrl = $"https://youtube.googleapis.com/youtube/v3/videos?part=snippet%2Cstatistics&id={id}&key=AIzaSyCyXB4DG5SPAfdpwT9qXz4gMVYJI1_RHUs";
                var responseString = await client.GetStringAsync(requestUrl);
                YouTubeVideo youTubeVideoLike =
                    JsonSerializer.Deserialize<YouTubeVideo>(responseString);
                int likeCount = 0;
                foreach (var like in youTubeVideoLike.ItemsVideo)
                {
                    likeCount = int.Parse(like.Statistics.LikeCount);
                
                }

                
                totalLikes += likeCount; 
            }
            Console.Clear();
            Console.WriteLine($"The total number of likes in the playlist: {totalLikes}");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("0. Enter to exit to the previous menu");
            Console.WriteLine();

            await Previous();
        }

        async static Task Start()
        {
            Console.Clear();
            Console.WriteLine("Please enter a number:");
            Console.WriteLine("1. Display a list of videos in the playlist.");
            Console.WriteLine("2. Display the total number of likes.");
            Console.WriteLine("0. Enter to exit to the program");
            int option = int.Parse(Console.ReadLine());

            switch (option)
            {
                case 1:
                    await PrintYouTubePlaylistAsync();
                    break;
                case 2:
                    await PrintYouTubeVideoLikeAsync();
                    break;
                case 0:
                    await Exit();
                    break;
                default:
                    Console.WriteLine("Invalid option selected.");
                    break;
            }
        }

        async static Task Previous()
        {
            int option = int.Parse(Console.ReadLine());

            switch (option)
            {
                case 0:
                    await Start();
                    break;
                default:
                    Console.WriteLine("Invalid option selected.");
                    break;
            }
        }

        async static Task Exit()
        {
            Console.Clear();
            Console.WriteLine("Thank you for using this program!");
            Console.WriteLine();
            Environment.Exit(0);
        }
        async static Task Main(string[] args)
        {
            await AddItemsToListAsync();
            await Start();
            
           

        }
    }
    
}
