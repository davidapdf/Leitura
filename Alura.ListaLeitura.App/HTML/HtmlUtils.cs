using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Alura.ListaLeitura.App.HTML
{
 public   class HtmlUtils
    {
        public static string CarregaArquivoHTML(string Arquivo)
        {

            var nomeCompletoArquivo = $"../../../HTML/{Arquivo}.html";
            using (var arquivo = File.OpenText(nomeCompletoArquivo))
            {
                return arquivo.ReadToEnd();
            }
        }
    }
}
