namespace HotelsManager.Controllers;
using System.Data.SqlClient;
using System.Net;
using System.Text.Json;
using HotelsManager.Controllers.Base;
using HotelsManager.Models;
using Dapper;
using HotelsManager.Extensions;
using HotelsManager.Attributes;

public class HotelsController : ControllerBase
{
    private const string ConnectionString = @"Data Source=LAPTOP-8U7UGFTE; Initial Catalog = HotelsDb; Integrated Security = SSPI;TrustServerCertificate=True;";

    [HttpGet("GetAll")]
    public async Task GethotelsAsync()
    {
        using var writer = new StreamWriter(base.HttpContext.Response.OutputStream);

        using var connection = new SqlConnection(ConnectionString);
        var hotels = await connection.QueryAsync<Hotels>("select * from hotels");

        var hotelsHtml = hotels.GetHtml();
        await writer.WriteLineAsync(hotelsHtml);
        base.HttpContext.Response.ContentType = "text/html";

        base.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpGet("GetById")]
    public async Task GethotelsByIdAsync()
    {
        var hotelsIdToGetObj = base.HttpContext.Request.QueryString["id"];

        if (hotelsIdToGetObj == null || int.TryParse(hotelsIdToGetObj, out int hotelsIdToGet) == false)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var hotels = await connection.QueryFirstOrDefaultAsync<Hotels>(
            sql: "select top 1 * from hotels where Id = @Id",
            param: new { Id = hotelsIdToGet });

        if (hotels is null)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        using var writer = new StreamWriter(base.HttpContext.Response.OutputStream);
        await writer.WriteLineAsync(JsonSerializer.Serialize(hotels));

        base.HttpContext.Response.ContentType = "application/json";
        base.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
    }



    [HttpPost("Create")]
    public async Task PosthotelsAsync()
    {
        using var reader = new StreamReader(base.HttpContext.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        var newhotels = JsonSerializer.Deserialize<Hotels>(json);

        if (newhotels == null || newhotels.Price == null || string.IsNullOrWhiteSpace(newhotels.Name))
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var hotels = await connection.ExecuteAsync(
            @"insert into hotels (Name, Price) 
        values(@Name, @Price)",
            param: new
            {
                newhotels.Name,
                newhotels.Price,
            });

        base.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
    }

    [HttpDelete]
    public async Task DeletehotelsAsync()
    {
        var hotelsIdToDeleteObj = base.HttpContext.Request.QueryString["id"];

        if (hotelsIdToDeleteObj == null || int.TryParse(hotelsIdToDeleteObj, out int hotelsIdToDelete) == false)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var deletedRowsCount = await connection.ExecuteAsync(
            @"delete hotels
            where Id = @Id",
            param: new
            {
                Id = hotelsIdToDelete,
            });

        if (deletedRowsCount == 0)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        base.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpPut]
    public async Task PuthotelsAsync()
    {
        var hotelsIdToUpdateObj = base.HttpContext.Request.QueryString["id"];

        if (hotelsIdToUpdateObj == null || int.TryParse(hotelsIdToUpdateObj, out int hotelsIdToUpdate) == false)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var reader = new StreamReader(base.HttpContext.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        var hotelsToUpdate = JsonSerializer.Deserialize<Hotels>(json);

        if (hotelsToUpdate == null || hotelsToUpdate.Price == null || string.IsNullOrEmpty(hotelsToUpdate.Name))
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var affectedRowsCount = await connection.ExecuteAsync(
            @"update hotels
        set Name = @Name, Price = @Price
        where Id = @Id",
            param: new
            {
                hotelsToUpdate.Name,
                hotelsToUpdate.Price,
                Id = hotelsIdToUpdate
            });

        if (affectedRowsCount == 0)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        base.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
    }
}