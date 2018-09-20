using AutoMapper;

using FluentValidation.AspNetCore;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Probability.Core.Calculations;
using Probability.Models;
using Probability.ViewModels;

namespace Probability
{
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
            services.AddScoped<ICombineCalculator, CombineCalculator>();
            services.AddScoped<IEitherCalculator, EitherCalculator>();
            services.AddScoped<ICalculatorFactory, CalculatorFactory>();

            services.AddMediatR();
            services.AddAutoMapper();

            services.AddMvc().AddFluentValidation(
                fvc =>
                    fvc.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
