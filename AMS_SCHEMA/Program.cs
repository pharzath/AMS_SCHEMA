using AMS.Model;
using AMS.Model.Models;
using AMS.Model.Services;
using AMS_SCHEMA.Application.Services;
using AMS_SCHEMA.Class;
using AMS_SCHEMA.Pages.Schema.TestData.Components;
using CodeGenerator.Gens;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using MudBlazor.Extensions;
using MudBlazor.Services;
using Neo4jClient;

using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<MyCachProvider>();

builder.Services.AddMudServices();
builder.Services.AddMudExtensions(c =>
{
    c.WithDefaultDialogOptions(ex =>
    {
        ex.Position = DialogPosition.Center;
    });
});



builder.Services.AddScoped<BrowserService>();

builder.Services.AddScoped<CodeGeneratorService>();

builder.Services.AddScoped<GenericRepository>();
builder.Services.AddScoped<MyCustomTreeService>();

builder.Services.AddScoped<IGraphClient>(s =>
{
    var uriString = builder.Configuration["NeO4jConnectionSettings:Server"];
    var username = builder.Configuration["NeO4jConnectionSettings:UserName"];
    var password = builder.Configuration["NeO4jConnectionSettings:Password"];

    var graphClient = new GraphClient(new Uri(uriString), username, password);

    return graphClient;
});

builder.Services.AddDbContextFactory<MyDbContext>(options =>
{
    options.EnableSensitiveDataLogging();
    options.ConfigureWarnings(configurationBuilder =>
    {
        configurationBuilder.Ignore(CoreEventId.NavigationBaseIncludeIgnored);
    });

    options.UseSqlServer(builder.Configuration.GetConnectionString("AMS12"), 
	    optionsBuilder =>
    {
        optionsBuilder.EnableRetryOnFailure(4);
    });
},ServiceLifetime.Singleton);

builder.Services.AddScoped<DataService>();

builder.Services.AddHotKeys2();




var app = builder.Build();

/*
using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<MyDbContext>();
    context.Database.EnsureCreated();
}
*/


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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
