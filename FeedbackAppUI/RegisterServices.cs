using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace FeedbackAppUI;

public static class RegisterServices {
  public static void ConfigureService(this WebApplicationBuilder builder) {
    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
      .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));
    builder.Services.AddAuthorization(options => {
      options.AddPolicy("Admin", policy => {
        policy.RequireClaim("jobTitle", "Admin");
      });
    });
    builder.Services.AddMemoryCache();
    builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();
    //this is a web project,all web project built with cache,
    //blazor server, it's already include memory cache package,
    //library project need to install a memory package, it's not a web project.
    builder.Services.AddSingleton<IDbConection, DbConection>();
    builder.Services.AddSingleton<IVehicleData,MongoVehicleData>();
    builder.Services.AddSingleton<ISalespersonData,MongoSalespersonData>();
    builder.Services.AddSingleton<IFeedbackData, MongoFeedbackData>();
    builder.Services.AddSingleton<IUserData,MongoUserData>();
    //each of four,<IVehicleModelData,MongoVehicleModelData> , <IStatusData,MongoStatusData>
    //<IFeedbackData, MongoFeedbackData>,<IUserData,MongoUserData>
    //all rely on <IDbConection, DbConection>,
    //caller have to have all of dependency injection.
    //they also rely on cache, which is     builder.Services.AddMemoryCache();
    //it;s important to use DI , then can properly send all those data where it need to go in the right manner,
    //as in this case, singleton

  }
}
