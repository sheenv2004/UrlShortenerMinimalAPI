var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Add SqlServer connection string for DbContext
builder.Services.AddDbContext<DataContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Method to Implement a logic to encode the url using base62 encoder 
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

//HTTP POST method to encode and post url data if it is new else show the existing shorturl
app.MapPost("/shorten", async (DataContext context, string OriginalUrl) =>
{
    DbSet<Url> db = context.Urls;
    //Setting counter to to create the short url code. Incrementing by one for every subsequent request
    int counter;
    if(!db.Any())
    {
        counter = 1000000;
    }
    else
    {
        counter = db.Select(x => x.counter).Max()+1; 
    }
    //Random rnd = new Random();
    //int counter = cnt++;
    Url url = new Url();
    url.shortUrl = base62Convert(counter);
    url.url = OriginalUrl;
    url.counter = counter;
    //url.counter = counter;
    
    //Check if url exists
    IQueryable<bool> c = db.Select(x => x.url.Contains(url.url));
    if (c.All(x=>x.Equals(false)))
    {
        context.Urls.Add(url);
        await context.SaveChangesAsync();
        return Results.Ok(await GetAllUrls(context));
    }
    else
    {
        return Results.BadRequest(await GetExistingUrls(context,url));
    }
   
});
//Method to get all urls to list
async Task<List<Url>> GetAllUrls(DataContext context) =>
    await context.Urls.ToListAsync();
//Method to get existing url with url
async Task<List<Url>> GetExistingUrls(DataContext context, Url url) =>
    await context.Urls.Where(x => x.url.Contains(url.url)).ToListAsync();


//HTTP GET method for original url using short url
app.MapGet("/{shortUrl}", async (DataContext context, string shortUrl) =>
     await context.Urls.SingleAsync(s => s.shortUrl == shortUrl) is Url url ?
     Results.Ok(url) :
     Results.NotFound("Sorry, url not found. :/"));

app.Run();

