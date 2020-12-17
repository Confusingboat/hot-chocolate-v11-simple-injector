using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CyclicalDependency.Types;
using SimpleInjector;

namespace CyclicalDependency
{
    public class Startup
    {
        private Container Container { get; } = new Container();

        private static void InitializeContainer(Container container)
        {
            // No registrations needed for this example, just here for completeness
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services
                .AddGraphQLServer()
                .AddQueryType<QueryType>()
                .AddHttpRequestInterceptor((context, requestExecutor, requestBuilder, cancellationToken) =>
                {
                    // This is so that HotChocolate can resolve services registered with Simple Injector
                    context.RequestServices = new CustomServiceProvider(Container, context.RequestServices);
                    return new ValueTask();
                });

            services.AddSimpleInjector(Container, options =>
            {
                options
                    .AddAspNetCore()
                    .AddControllerActivation();
                options.AutoCrossWireFrameworkComponents = true;
            });

            InitializeContainer(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSimpleInjector(Container);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            Container.Verify();
        }
    }
}
