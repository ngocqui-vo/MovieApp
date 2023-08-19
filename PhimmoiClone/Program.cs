﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PhimmoiClone.Areas.Cinema.Repository.ActorRepo;
using PhimmoiClone.Data;
using PhimmoiClone.Mail;

namespace PhimmoiClone
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            

            builder.Services.AddDbContext<MyDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("MyDB");
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddTransient<MyDbContext>();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                            .AddEntityFrameworkStores<MyDbContext>()
                            .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lần thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true; // Email là duy nhất

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = false; // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false; // Xác thực số điện thoại

            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/login/";
                options.LogoutPath = "/logout/";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            builder.Services.AddOptions();
            var mailSettings = builder.Configuration.GetSection("MailSettings");
            builder.Services.Configure<MailSettings>(mailSettings);
            builder.Services.AddTransient<IEmailSender, SendMailService>();
            builder.Services.AddScoped<IActorRepo, ActorRepo>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            

            app.Run();
        }
    }
}