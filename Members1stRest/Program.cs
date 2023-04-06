using Members1stRest;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

#region Setup OpenApi properties    
var contact = new OpenApiContact()
{
    Name = "Geoff Gariepy",
    Email = "geoff@gariepy.dev",
    Url = new Uri("https://github.com/ggariepy")
};

var license = new OpenApiLicense()
{
    Name = "Creative Commons License",
    Url = new Uri("https://creativecommons.org/licenses/by-sa/4.0/")
};

var info = new OpenApiInfo()
{
    Version = "v1",
    Title = "Transaction DB Demo",
    Description = "Demonstrates a RESTful API for transaction data",
    TermsOfService = new Uri("https://creativecommons.org/licenses/by-sa/4.0/"),
    Contact = contact,
    License = license
};
#endregion

#region Setup ASP.NET Core 7 Minimal API application
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TransactionDataDB>(opt => opt.UseInMemoryDatabase("TransactionData"));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", info);
});

var app = builder.Build();

#endregion

async Task<IResult> GetByTransactionId(string transId, TransactionDataDB db)
{
    var transList = await db.Transactions.Where(t => t.Id == transId).ToListAsync<TransactionData>();
    if (transList.Any())
    {
        return Results.Ok(transList);
    }
    else
    {
        return Results.NotFound($"Sorry, transId {transId} doesn't exist.");
    }
};

async Task<IResult> GetByAny(string field, string value, string type, TransactionDataDB db)
{
    // Build an expression

    var PropertyName = field;
    var Value = value;
    var parameterExpression = Expression.Parameter(Type.GetType("Members1stRest.TransactionData"), PropertyName);
    var constant = Expression.Constant(Value);
    var property = Expression.Property(parameterExpression, PropertyName);
    var expression = type == "=" ? Expression.Equal(property, constant) : Expression.NotEqual(property, constant);
    var lambda = Expression.Lambda<Func<TransactionData, bool>>(expression, parameterExpression);
    var compiledLambda = lambda.Compile();
    var transList = db.Transactions.Where(compiledLambda).ToList<TransactionData>();

    if (transList.Any())
    {
        return Results.Ok(transList);
    }
    return Results.NotFound($"Found no records for query=[{PropertyName} {type} {Value}]");
}



async Task<IResult> GetByAccountNumber(string acctnum, TransactionDataDB db)
{
    var transList = await db.Transactions.Where(x => x.AccountNumber == acctnum).ToListAsync<TransactionData>();
    if (transList.Any())
    {
        return Results.Ok(transList);
    }
    else
    {
        return Results.NotFound($"Sorry, no transactions found for acctnum={acctnum}.");
    }
};

async Task<IResult> GetByDateRange(string StartDate, string EndDate, TransactionDataDB db)
{
    DateTime dateStart;
    DateTime dateEnd;

    if (!DateTime.TryParse(StartDate, out dateStart))
    {
        return Results.Problem("Please format start date as MM-DD-YYYY or YYYY-MM-DDTHH:MM:SS");
    }

    if (!DateTime.TryParse(EndDate, out dateEnd))
    {
        return Results.Problem("Please format end date as MM-DD-YYYY or YYYY-MM-DDTHH:MM:SS");
    }

    if (dateStart > dateEnd)
    {
        return Results.Problem("Please make the starting date earlier than the end date.");
    }

    var transList = await db.Transactions.Where(
            trans =>
            (DateTime.Parse(trans.ActivityDate) >= dateStart &&
            DateTime.Parse(trans.ActivityDate) <= dateEnd)
            ||
            (DateTime.Parse(trans.LastTranDate) >= dateStart &&
            DateTime.Parse(trans.LastTranDate) <= dateEnd)
            ||
            (DateTime.Parse(trans.PostDate) >= dateStart &&
            DateTime.Parse(trans.PostDate) <= dateEnd)
            ||
            (DateTime.Parse(trans.EffectiveDate) >= dateStart &&
            DateTime.Parse(trans.EffectiveDate) <= dateEnd)
            )
        .OrderBy(x => x.ActivityDate)
        .ToListAsync<TransactionData>();

    if (transList.Any())
    {
        return Results.Ok(transList);
    }
    else
    {
        return Results.NotFound($"Sorry, no transactions found between StartDate={StartDate} and EndDate={EndDate}.");
    }
};

IResult InitializeDB(TransactionDataDB db, IConfiguration configuration)
{
    var JSONFile = configuration.GetValue(typeof(string), "JSONFile").ToString();
    app.Logger.LogInformation($"Running InitializeDB, JSON file to be read: [{JSONFile}]");
    try
    {
        List<TransactionData> transactions = new List<TransactionData>();
        using StreamReader reader = new(JSONFile);
        var json = reader.ReadToEnd();
        transactions = JsonConvert.DeserializeObject<List<TransactionData>>((string)json);

        if (transactions.Any())
        {
            foreach (var transaction in transactions)
            {
                db.Transactions.Add(transaction);
            }
            db.SaveChanges();
            app.Logger.LogInformation($"Loaded {transactions.Count} transactions from JSON File.");
        }
        return Results.Redirect("swagger/index.html");
    }
    catch (Exception ex)
    {
        if (ex.Message != "An item with the same key has already been added. Key: System.Object[]")
        {
            app.Logger.LogCritical($"Failed to load transactions from JSON File [{JSONFile}]: {ex.Message}.");
            return Results.Problem("Couldn't load JSON Data.");
        }
        return Results.Ok(); // Ignore attempts to reload the same data.
    }
}

#region Minimal API route descriptions
// Initialize by reading JSON File to in-memory database.
app.MapGet("/", InitializeDB).ExcludeFromDescription();

// Retrieves a single transaction by ID.
app.MapGet("/transaction/{id}", GetByTransactionId);

// Retrieves all transactions for a given Account Number.
app.MapGet("/transaction/account/{acctnum}", GetByAccountNumber);

// Retrieves all transactions by DateRange based on DateFields
app.MapGet("/transaction/daterange/{startdate,enddate}", GetByDateRange);

// Find/Search for Transactions (any property is searchable)
// Multiple query parameters are possible
app.MapGet("/transaction/query/{field,value,type}", GetByAny);

#endregion


#region Setup Swagger UI

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Transaction REST API v1");
});

#endregion

app.Run();