

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 string base62Convert(int deci)
{
    string s = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    string hash_str = string.Empty;
    while (deci > 0)
    {
        hash_str = s[deci % 62] + hash_str;
        deci /= 62;
    }
    return hash_str;
}
app.UseHttpsRedirection();

app.MapPost("/shorten", async (DataContext context, Url url) =>
{
    Random rnd = new Random();
    int counter = rnd.Next(1000000, 2000000); 
    url.shortUrl = base62Convert(counter);
    url.counter = counter;
    DbSet<Url> db =context.Urls;
    IQueryable<bool> c = db.Select(x => x.url.Contains(url.url));
    if (c.All(x=>x.Equals(false)))
    {
        context.Urls.Add(url);
        await context.SaveChangesAsync();
        return Results.Ok(await GetAllUrls(context));
    }
    else
    {
        //return Results.BadRequest("url already exists");
        return Results.BadRequest(await GetExistingUrls(context,url));

    }
   
});
async Task<List<Url>> GetAllUrls(DataContext context) =>
    await context.Urls.ToListAsync();
async Task<List<Url>> GetExistingUrls(DataContext context, Url url) =>
    await context.Urls.Where(x => x.url.Contains(url.url)).ToListAsync();


//app.MapGet("/", () => "Test Data acccess!");
app.MapGet("/{shortUrl}", async (DataContext context, string shortUrl) =>
     await context.Urls.SingleAsync(s => s.shortUrl == shortUrl) is Url url ?
     Results.Ok(url) :
     Results.NotFound("Sorry, url not found. :/"));

app.Run();

