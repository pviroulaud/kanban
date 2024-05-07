using System.Text;
using entidadesKanban.modelo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using serviciosKanban;
using serviciosKanban.SRVC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<AppDbContext>(opt=>opt.UseSqlServer(builder.Configuration.GetConnectionString("dbConnectionString1")??""));




builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt=>{
    opt.TokenValidationParameters = new TokenValidationParameters(){
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:ClaveSecreta"]??"")),
        SaveSigninToken =true,
        ValidateIssuer=true,
        ValidateAudience=true,
        ValidateLifetime=true,
        ValidateIssuerSigningKey=true,
        ValidIssuer=builder.Configuration["JWT:Issuer"],
        ValidAudience=builder.Configuration["JWT:Audience"]
    };
});

builder.Services.AddScoped<Ijwt, JwtSrvc>();
builder.Services.AddScoped<IusuarioSrvc, usuarioSrvc>();
builder.Services.AddScoped<IproyectoSrvc, proyectoSrvc>();
builder.Services.AddScoped<Ilogger, loggerSrvc>();
builder.Services.AddScoped<ItareaSrvc, tareaSrvc>();
builder.Services.AddScoped<IincidenciaSrvc, incidenciaSrvc>();
builder.Services.AddScoped<IreportesSrvc, reportesSrvc>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}




app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
