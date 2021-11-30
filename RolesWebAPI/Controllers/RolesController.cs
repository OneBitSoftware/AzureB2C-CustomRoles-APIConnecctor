using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace RolesWebAPI;

[ApiController]
[Route("[controller]")]
public class RolesController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Index()
    {
        string? input = null;

        if (this.Request.Body == null)
            return StatusCode((int)HttpStatusCode.BadRequest, new B2CErrorResponseModel("Request content is null"));

        // Read the input claims from the request body
        using (StreamReader reader = new(Request.Body, Encoding.UTF8))
            input = await reader.ReadToEndAsync();

        if (string.IsNullOrEmpty(input))
            return StatusCode((int)HttpStatusCode.Conflict, new B2CErrorResponseModel("Request content is empty"));

        // Debug input
        Console.WriteLine(input);

        string? emailValue = GetEmailValue(input);

        // Assign SuperUser role
        if (!string.IsNullOrWhiteSpace(emailValue) && emailValue.ToLowerInvariant().Equals("radi.a@live.com.au"))
        {
            Console.WriteLine("Returning SuperUser role for " + emailValue);
            return Ok(new B2CRoleResponseModel() { extension_role = "SuperUser" });
        }

        return Ok();
    }

    /// <summary>
    /// Reads the "email" JSON element of the passed input value.
    /// </summary>
    /// <param name="input">The JSON string to examine.</param>
    /// <returns>A nullable string, possibly containing the email JSON element value.</returns>
    private static string? GetEmailValue(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        JsonSerializer.Deserialize<JsonElement>(input).TryGetProperty("email", out var email);
        
        return email.GetString();
    }
}

/// <summary>
/// A base class for an Azure B2C response.
/// </summary>
public class B2CResponseModel
{
    public string action { get; set; }
    public string version { get; set; }
    public int status { get; set; }

    public B2CResponseModel()
    {
        this.status = (int)status;
        this.version = "1.0";
        this.action = "Continue";
    }
}

public class B2CRoleResponseModel : B2CResponseModel
{
    public string? extension_role { get; set; }
}

public class B2CErrorResponseModel : B2CResponseModel
{
    public string? message { get; set; }

    public B2CErrorResponseModel(string message)
    {
        this.status = (int)HttpStatusCode.BadRequest;
    }
}