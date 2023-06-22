using System.Reflection;
using AMS.Model;
using AMS.Model.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor.Extensions.Components.ObjectEdit.Options;
using MudBlazor.Extensions.Components.ObjectEdit;
using MudBlazor;

namespace AMS_SCHEMA.Pages.Settings;

public class ProjectMetaConfiguration : IObjectMetaConfiguration<AmsNeo4JProject>
{

    public Task ConfigureAsync(ObjectEditMeta<AmsNeo4JProject> meta)
    {
        // Configure here

        meta.Property(x => x.Id)
            .Ignore();
        
        meta.Property(x => x.Description)
            .RenderWith(typeof(MudTextField<string>), new Dictionary<string, object>()
            {
                ["Lines"] = 4
            })
            .WrapInMudItem(x => x.md = 12);

        meta.WrapEachInMudItem(x =>
        {
            x.xs = 12;
            x.sm = 12;
            x.md = 6;
            //x.lg = 4;
        });
            
        return Task.CompletedTask;
    }
}


public class MicroserviceMetaConfiguration : IObjectMetaConfiguration<AmsNeo4JMicroservice>
{

    public Task ConfigureAsync(ObjectEditMeta<AmsNeo4JMicroservice> meta)
    {
        // Configure here

        meta.Property(x => x.Id)
            .Ignore();
        
        meta.Property(x => x.ProjectFk)
            .Ignore();

        meta.Property(x => x.Project).RenderWith<MudTextField<string>>( field =>
        {
            field.Value = meta.Value.Project.ToString();
            field.Disabled = true;
            field.ReadOnly = true;
        });

        meta.WrapEachInMudItem(x =>
        {
            x.xs = 12;
            x.sm = 12;
            x.md = 6;
            //x.lg = 4;
        });
            
        return Task.CompletedTask;
    }
}

public class MicroserviceModuleMetaConfiguration : IObjectMetaConfiguration<AmsNeo4JMicroserviceModule>
{

    public Task ConfigureAsync(ObjectEditMeta<AmsNeo4JMicroserviceModule> meta)
    {
        // Configure here

        meta.Property(x => x.Id)
            .Ignore();
        
        meta.Property(x => x.MicroserviceFk)
            .Ignore();

        meta.Property(x => x.Microservice)
            .RenderWith<MudTextField<string>>( field =>
        {
            field.Value = meta.Value.Microservice.Name;

            field.ReadOnly = true;
        })
            .WithOrder(0);
        
        
        meta.Property(x => x.ModuleType).WithOrder(1);

        meta.Property("Microservice.Project")
            .Ignore();


        meta.WrapEachInMudItem(x =>
        {
            x.xs = 12;
            x.sm = 12;
            x.md = 6;
            //x.lg = 4;
        });
            
        return Task.CompletedTask;
    }
}

