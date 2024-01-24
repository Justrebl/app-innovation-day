using CafeReadConf;
using CafeReadConf.Frontend.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

//Conditional service injection
if( string.IsNullOrEmpty(builder.Configuration["BACKEND_API_URL"]) )
{
    builder.Services.AddSingleton<IUserService, TableStorageService>();
}
else
{
    builder.Services.AddSingleton<IUserService, TableStorageService>();
}
builder.Services.AddSingleton<IUserService, TableStorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
