using AMS.Model.Models;
using AMS.Model.Services;
using AMS_SCHEMA.Class;
using AMS_SCHEMA.CodeGenerator;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MudBlazor.Services;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddMudServices();

builder.Services.AddScoped<BrowserService>();

builder.Services.AddScoped<AMSCodeGenerator>();

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
