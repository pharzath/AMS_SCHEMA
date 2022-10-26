using AMS.Model.Models;
using AMS.Model.Services;
using AMS_SCHEMA.Application.Services;
using AMS_SCHEMA.Class;
using AMS_SCHEMA.CodeGenerator;
using AMS_SCHEMA.Pages.Schema.TestData.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using MudBlazor.Services;
using Neo4jClient;
using QOQNOS.Neo4j.TEST.Application.AMS.Domain.Repository.Generic;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<MyCachProvider>();

builder.Services.AddMudServices();

builder.Services.AddScoped<BrowserService>();

builder.Services.AddScoped<AMSCodeGenerator>();

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

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.ConfigureWarnings(configurationBuilder => configurationBuilder.Ignore(CoreEventId.NavigationBaseIncludeIgnored));

    //options.UseSqlServer(builder.Configuration["ConnectionString"]);
});

builder.Services.AddHotKeys();

builder.Services.AddScoped<DataService>();

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
