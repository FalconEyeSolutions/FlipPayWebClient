namespace FlipPayApiLibrary
{
    public class FlipPayConfig
    {
        public required string Token { get; set; }
        public required bool IsDemo { get; set; }
        public string ProductionUrl { get; set; } = "https://app.flippay.com.au/api/v2/"; // Make sure to include the trailing slash at the end
        public string DemoUrl { get; set; } = "https://demo.flippay.com.au/api/v2/"; // Make sure to include the trailing slash at the end
    }
}
