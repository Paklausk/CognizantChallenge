using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProgrammingCompetitionApi.Compilers;
using ProgrammingCompetitionApi.Configure;
using ProgrammingCompetitionApi.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "t", Version = "v1" });
            });
            services.AddCompetitionTasks();
            services.AddDbContext<Db>(options => options.UseSqlServer(Configuration.GetConnectionString("MsSql")));
            services.Configure<JDoodleSettings>(Configuration.GetSection("JDoodle"));
            services.AddSingleton<CompilerFactory>();
            services.AddSingleton<ICompiler>(services => services.GetService<CompilerFactory>().Create());

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Db db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "t v1"));
            }
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
