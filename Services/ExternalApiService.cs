

namespace Tourism_Api.Services;

using Microsoft.AspNetCore.Identity.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

public class EgyptGuideService
{
    private readonly HttpClient _httpClient;
    private string _currentToken;

    public EgyptGuideService(HttpClient httpClient )
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://egypt-guid26.runasp.net/");

        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<List<string>> GetAllPlacesAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<string>>("api/Place/All-PlacesName");
            return response ?? new List<string>();
        }
        catch (HttpRequestException ex)
        {
            // Handle API call errors
            Console.WriteLine($"Error calling Egypt Guide API: {ex.Message}");
            return new List<string>();
        }
    }

    public async Task<LoginResponse> LoginAsync(string email, string password)
    {
        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = password
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/authenticat/Login", loginRequest);

            if (!response.IsSuccessStatusCode)
            {
                // Handle non-success status codes
                throw new HttpRequestException($"Login failed with status code: {response.Content}");
            }

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return loginResponse;
        }
        catch (HttpRequestException ex)
        {
            // Handle API call errors
            Console.WriteLine($"Error calling authentication API: {ex.Message}");
            throw;
        }
    }

    public void SetAuthToken(string token)
    {
        _currentToken = token;
    }
    public async Task<PlaceDetailsResponse> GetPlaceDetailsAsync(string placeName, string token)
    {
        try
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/Place/PlacesDetails?name={Uri.EscapeDataString(placeName)}");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Invalid or expired token");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"API request failed: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<PlaceDetailsResponse>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetPlaceDetailsAsync: {ex.Message}");
            throw;
        }
    }
}
public class AuthHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = _httpContextAccessor.HttpContext?
            .Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
public class LoginResponse
{
    public string Name { get; set; }
    public string Id { get; set; }
    public string Role { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public long ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiretion { get; set; }
}

public class TourGuide
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsActive { get; set; }
    public bool IsBooked { get; set; }
    public string Gender { get; set; }
    public double Rate { get; set; }
    public string Photo { get; set; }
}

public class PlaceDetailsResponse
{
    public string Name { get; set; }
    public string Photo { get; set; }
    public string Location { get; set; }
    public string VisitingHours { get; set; }
    public double GoogleRate { get; set; }
    public string Description { get; set; }
    public bool IsFavorite { get; set; }
    public string GovernmentName { get; set; }
    public List<object> Comments { get; set; }
    public List<TourGuide> Tourguids { get; set; }
    public List<string> TypeOfTourism { get; set; }
    public List<object> UserRates { get; set; }
}