
HttpClient client = new HttpClient();
var request = new HttpRequestMessage();
//Obviously you'd never have a key outside of secrets
request.Headers.Add("x-api-key", "6KPfnVYllW4vSvTikAeGW3du7Cqb84aS9DEDAAZ0");
request.Headers.Add("Content-Type", "application/json");
request.Method = HttpMethod.Post;
request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
    { "registrationNumber", "VU64 NDO" },
});

var response = await client.PostAsync("https://uat.driver-vehicle-licensing.api.gov.uk/vehicle-enquiry/v1/vehicles", );
