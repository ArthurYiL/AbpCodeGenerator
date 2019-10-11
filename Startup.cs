using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABPCodeGenerator.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ABPCodeGenerator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //��һЩ����ȫ�ֻ����������
            ConfigureGlobalVariables(Configuration);
        }

        /// <summary>
        ///  ��һЩ����ȫ�ֻ����������
        /// </summary>
        /// <param name="configuration"></param>
        private void ConfigureGlobalVariables(IConfiguration configuration)
        {
            //����Sql Server ���ݿ������ֶ�
            var sqlSeverConnectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(sqlSeverConnectionString))
            {
                throw new Exception("û������Sql Server���ݿ������ַ���");
            }

            AppConfig.SqlServerConnectionString = sqlSeverConnectionString;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder => builder.WithOrigins("https://localhost:5001/"));

            app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }
    }
}
