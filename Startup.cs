using ABPCodeGenerator.Interceptors;
using ABPCodeGenerator.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;

namespace ABPCodeGenerator
{
    public class Startup
    {
        public ILifetimeScope AutofacContainer { get; set; }

        public Startup(
            IConfiguration configuration)
        {
            Console.WriteLine("StartUp");

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        #region Ĭ������ע��ʵ��
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("StartUp.ConfigureServices");

            services.AddControllersWithViews();

            #region ע�᲻ͬ�������ڵķ���

            //services.AddSingleton<ICPS8xCodeGeneratorService, CPS8xCodeGeneratorService>();

            //services.AddScoped<ICPS8xCodeGeneratorService, CPS8xCodeGeneratorService>();

            services.AddTransient<ICPS8xCodeGeneratorService, CPS8xCodeGeneratorService>();

            services.AddSingleton<IOrderService, OrderService>();

            //services.AddScoped<IOrderService, OrderService>();

            //services.AddTransient<IOrderService, OrderService>();

            #endregion

            #region ��ʽע��
            //var instance = new CPS8xCodeGeneratorService(this.razorViewEngine, this.tempDataProvider, this.serviceProvider);

            //services.AddSingleton<ICPS8xCodeGeneratorService>(instance);//ֱ��ע�����ʵ��

            //services.AddSingleton<ICPS8xCodeGeneratorService, CPS8xCodeGeneratorService>();

            //services.AddTransient<ICPS8xCodeGeneratorService>(factory => { return instance; });//����ģʽֱ��ע�����ʵ��
            #endregion

            #region ����ע��
            //services.TryAddTransient<ICPS8xCodeGeneratorService, CPS8xCodeGeneratorService>();//����ýӿ�����ע���������ע��

            services.TryAddEnumerable(ServiceDescriptor.Singleton<ICPS8xCodeGeneratorService, CPS8xCodeGeneratorService>());//���ע�����ǸýӿڵĲ�ͬʵ���࣬�����ע��

            services.TryAddEnumerable(ServiceDescriptor.Singleton<ICPS8xCodeGeneratorService, CPS8xCodeGeneratorService>());//���ע�����ǸýӿڵĲ�ͬʵ���࣬�����ע��

            //services.TryAddEnumerable(ServiceDescriptor.Singleton<ICPS8xCodeGeneratorService>(new CPS8xCodeGeneratorService()));//���ע�����ǸýӿڵĲ�ͬʵ���࣬�����ע��

            //services.TryAddEnumerable(ServiceDescriptor.Singleton<ICPS8xCodeGeneratorService>(p => { return new CPS8xCodeGeneratorService(); }));//���ע�����ǸýӿڵĲ�ͬʵ���࣬�����ע��
            #endregion

            #region �Ƴ����滻ע��
            //services.RemoveAll<ICPS8xCodeGeneratorService>();//���������Ƴ����иýӿڵķ���ʵ��

            //services.Replace(ServiceDescriptor.Singleton<ICPS8xCodeGeneratorService, CPS8xCodeGeneratorService>());//���������滻ԭ��ʵ���˸ýӿڵ�ʵ����ΪĿ����
            #endregion

            #region ע�᷺��ģ��
            services.AddSingleton(typeof(IGenerateService<>), typeof(GenerateService<>));
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostEnvironment env)
        //{

        //    Console.WriteLine("StartUp.Configure");

        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }
        //    else
        //    {
        //        app.UseExceptionHandler("/Home/Error");
        //        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        //        app.UseHsts();
        //    }

        //    app.UseHttpsRedirection();

        //    app.UseStaticFiles();

        //    //app.UseIdentity();

        //    app.UseRouting();

        //    app.UseCors(builder => builder.WithOrigins("https://localhost:5001/"));

        //    app.UseAuthorization();

        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapControllerRoute(
        //            name: "default",
        //            pattern: "{controller=Dashboard}/{action=Index}/{id?}");
        //    });
        //}
        #endregion

        #region Autofac����ע��ʵ��
        public void ConfigureContainer(ContainerBuilder builder)
        {
            ////Ĭ��ע��
            //builder.RegisterType<OrderService>().As<IOrderService>();
            ////����ע��
            //builder.RegisterType<OrderService>().Named<IOrderService>("orderservice");
            ////����ע��
            //builder.RegisterType<OrderNameService>();
            //builder.RegisterType<OrderService>().Named<IOrderService>("orderservice2").PropertiesAutowired();
            ////ʹ��������
            //builder.RegisterType<MyInterceptor>();
            //builder.RegisterType<OrderService>().Named<IOrderService>("orderservice3").PropertiesAutowired().InterceptedBy(typeof(MyInterceptor)).EnableClassInterceptors();

            #region ������
            builder.RegisterType<OrderNameService>().InstancePerMatchingLifetimeScope("myscope");//myscope����������ᱻmyscope2���������󸲸�
            builder.RegisterType<OrderNameService>().InstancePerMatchingLifetimeScope("myscope2");
            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            //var service1 = this.AutofacContainer.Resolve<IOrderService>();
            //service1.Show();

            //var orderservice = this.AutofacContainer.ResolveNamed<IOrderService>("orderservice");
            //orderservice.Show();

            //var orderservice2 = this.AutofacContainer.ResolveNamed<IOrderService>("orderservice2");
            //orderservice2.Show();

            //var orderservice3 = this.AutofacContainer.ResolveNamed<IOrderService>("orderservice3");
            //orderservice3.Show();


            using (var myscpoe = this.AutofacContainer.BeginLifetimeScope("myscope2"))
            {
                var s4 = myscpoe.Resolve<OrderNameService>();

                using (var myscpoe2 = myscpoe.BeginLifetimeScope("myscope"))
                {
                    var s5 = myscpoe2.Resolve<OrderNameService>();
                    var s6 = myscpoe2.Resolve<OrderNameService>();

                    Console.WriteLine($"s4==s5?{s4 == s5}");
                    Console.WriteLine($"s4==s6?{s4 == s6}");
                    Console.WriteLine($"s5==s6?{s5 == s6}");
                }
            }

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

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            //app.UseIdentity();

            app.UseRouting();

            app.UseCors(builder => builder.WithOrigins("https://localhost:5001/"));

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }
        #endregion
    }
}
