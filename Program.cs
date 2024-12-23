using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeManager.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
   policy.RequireRole("Admin"));
});

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Recipes");
    options.Conventions.AllowAnonymousToPage("/Recipes/Index");
    options.Conventions.AllowAnonymousToPage("/Recipes/Details");
    options.Conventions.AuthorizeFolder("/Members", "AdminPolicy");

}

    );

// Configure the RecipeManagerContext for general database access.
builder.Services.AddDbContext<RecipeManagerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RecipeManagerContext")
        ?? throw new InvalidOperationException("Connection string 'RecipeManagerContext' not found.")));

// Configure Identity with RecipeIdentityContext for user authentication.
builder.Services.AddDbContext<RecipeIdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RecipeManagerContext")
        ?? throw new InvalidOperationException("Connection string 'RecipeManagerContext' not found.")));

// Register DefaultIdentity once and attach it to RecipeIdentityContext.
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<RecipeIdentityContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
