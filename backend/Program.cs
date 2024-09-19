using Microsoft.EntityFrameworkCore;
using backend.Data;
using Microsoft.Extensions.Configuration;

namespace backend
{
    public class Program
    {


        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            //Acceso y direccion de la base de datos
            builder.Services.AddDbContext<ApiContext>(opciones =>
                opciones.UseSqlServer(builder.Configuration.GetConnectionString("BackendDb")));



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
