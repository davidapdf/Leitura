using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }
        public void Configure(IApplicationBuilder app)
        {
            var builder = new RouteBuilder(app);
            builder.MapRoute("Livros/ParaLer", LivrosParaLer);
            builder.MapRoute("Livros/Lendo", LivrosLendo);
            builder.MapRoute("Livros/Lidos", LivrosLidos);
            builder.MapRoute("Cadastro/NovosLivros/{titulo}/{autor}", NovoLivroParaLer);
            builder.MapRoute("Licros/Detalhes/{id:int}", ExibeDetalhe);
            builder.MapRoute("Cadastro/NovoLivro", ExibeFormulario);
            builder.MapRoute("Cadastro/Incluir", ProcessaFromulario);
            var rotas = builder.Build(); 

            app.UseRouter(rotas);
         //   app.Run(Roteamento);
        }

        private Task ProcessaFromulario(HttpContext context)
        {
            var livro = new Livro()
            {
                Titulo = context.Request.Query["Titulo"].First(),
                Autor = context.Request.Query["Autor"].First(),
            };

            var repo = new LivroRepositorioCSV();
            repo.Incluir(livro);
            return context.Response.WriteAsync("Livro add com sucesso");
        }

        public Task ExibeFormulario(HttpContext context)
        {
            var html = @"
                        <html>
                             <form action = '/Cadastro/Incluir'>
                                 <label>Título:</label>
                                 <input name='Titulo'/>
                                 <br/>
                                 <label>Autor:</label>
                                 <input name='Autor'/>
                                 <button>Gravar</button>
                             </form>
                        </html>";
            return context.Response.WriteAsync(html);
        }

        public Task ExibeDetalhe(HttpContext context)
        {
            int id = Convert.ToInt32(context.GetRouteValue("id"));
            var repo = new LivroRepositorioCSV();
            var livro = repo.Todos.First(l => l.Id == id);
            return context.Response.WriteAsync(livro.Detalhes());

        }

        public Task NovoLivroParaLer(HttpContext context)
        {
            var livro = new Livro()
            {
                Titulo = Convert.ToString(context.GetRouteValue("titulo")),
                Autor = Convert.ToString(context.GetRouteValue("autor")),
            };

            var repo = new LivroRepositorioCSV();
            repo.Incluir(livro);
            return context.Response.WriteAsync("Livro add com sucesso");
        }

        public Task Roteamento(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            var caminhoAtendidos = new Dictionary<string, RequestDelegate>
            {
                {"/Livros/ParaLer",LivrosParaLer},
                {"/Livros/Lendo",LivrosLendo },
                {"/Livros/Lidos",LivrosLidos }
            };
            if (caminhoAtendidos.ContainsKey(context.Request.Path))
            {
                var metado = caminhoAtendidos[context.Request.Path];
                return metado.Invoke(context);
            }
            context.Response.StatusCode = 404;
            return context.Response.WriteAsync("Caminho Inexistente...");
        }
        public Task LivrosParaLer(HttpContext context)
        {
            
            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.ParaLer.ToString());
        }

        public Task LivrosLendo(HttpContext context)
        {

            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.Lendo.ToString());
        }

        public Task LivrosLidos(HttpContext context)
        {

            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.Lidos.ToString());
        }
    }
}