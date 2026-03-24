using System.IO;
using UBI.App.Components;
using UBI.App.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton(_ =>
    new UbiDataService(Path.Combine(builder.Environment.ContentRootPath, "App_Data", "ubi-state.json")));

var app = builder.Build();
var disableHttpsRedirect = string.Equals(
    Environment.GetEnvironmentVariable("UBI_DISABLE_HTTPS_REDIRECT"),
    "1",
    StringComparison.Ordinal);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (!disableHttpsRedirect)
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseAntiforgery();
app.MapGet("/api/company", (UbiDataService data) => Results.Ok(data.GetCompany()));
app.MapPut("/api/company/purpose", (UpdateCompanyPurposeRequest request, UbiDataService data) =>
{
    try
    {
        return Results.Ok(data.UpdatePurposeStatement(request));
    }
    catch (InvalidOperationException error)
    {
        return Results.BadRequest(new { error = error.Message });
    }
});
app.MapGet("/api/workspace", (UbiDataService data) => Results.Ok(data.Workspace));
app.MapGet("/api/employees", (UbiDataService data) => Results.Ok(data.GetEmployees()));
app.MapGet("/api/employees/{employeeId}", (string employeeId, UbiDataService data) =>
{
    var employee = data.GetEmployee(employeeId);
    return employee is null ? Results.NotFound() : Results.Ok(employee);
});
app.MapPost("/api/employees", (NewEmployeeRequest request, UbiDataService data) =>
{
    try
    {
        var employee = data.AddEmployee(request);
        return Results.Created($"/api/employees/{employee.EmployeeId}", employee);
    }
    catch (InvalidOperationException error)
    {
        return Results.BadRequest(new { error = error.Message });
    }
});
app.MapGet("/api/interventions", (UbiDataService data) => Results.Ok(data.GetInterventions()));
app.MapPost("/api/interventions", (NewInterventionRequest request, UbiDataService data) =>
{
    try
    {
        var intervention = data.AddIntervention(request);
        return Results.Created($"/api/interventions/{intervention.InterventionId}", intervention);
    }
    catch (InvalidOperationException error)
    {
        return Results.BadRequest(new { error = error.Message });
    }
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
