namespace GeometricProgramming
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using GeometricProgramming.Services.Abstract;
    using GeometricProgramming.Services.Concrete;
    using Steeltoe.Messaging.RabbitMQ.Extensions;
    using Steeltoe.Messaging.RabbitMQ.Config;
    using GeometricProgramming.Common;
    using Steeltoe.Messaging.RabbitMQ.Connection;
    using Steeltoe.Messaging.RabbitMQ.Core;
    using RestSharp;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton<IRestClient>(new RestClient(Configuration.GetSection("ConsumerUrl").Value));

            // Add singleton that will process incoming messages
            services.AddSingleton<RabbitListener>();

            // Tell steeltoe about singleton so it can wire up queues with methods to process queues (i.e. RabbitListenerAttribute)
            services.AddRabbitListeners<RabbitListener>();

            var connectionFactory = new CachingConnectionFactory()
            {
                Host = Configuration.GetSection("Spring:RabbitMq").GetValue("Host", string.Empty),
                Port = Configuration.GetSection("Spring:RabbitMq").GetValue("Port", 5672),
                Password = Configuration.GetSection("Spring:RabbitMq").GetValue("Username", string.Empty),
                Username = Configuration.GetSection("Spring:RabbitMq").GetValue("Password", string.Empty)
            };
            var admin = new RabbitAdmin(connectionFactory);
            admin.DeclareQueue(new Queue("Geometry.Inputs"));

            services.AddControllers();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddSingleton<IPrintService>(new PrintService(new RabbitTemplate(connectionFactory)));
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeometricProgramming API V1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
