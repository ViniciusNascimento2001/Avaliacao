using Avaliacao.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Data;
using System.Text;

namespace Avaliacao.Controllers
{
    public class CalculadoraController : Controller
    {
        //Serviço das operações matematicas
        public Services services = Services.GetInstance();
        public IActionResult Index()
        {
            return View();
        }

        //Mensagens de erros
        public string MessageErrorLetter = "Por favor não inserir letras somente numeros.";
        public string MessageErrorSpecialCharacters = "Por favor não inserir caracteres especiais somente numeros.";
        public string MessageErrorNone = "Por favor não deixe espaços vazios.";
        public string MessageErrorZero = "Por favor não inserir somente zero.";

        public IActionResult Somar(string parameters)
        {
            //Dados inseridos divididos por virgula para que haja mais de um numero
            string[] arrayString = parameters.Split(',');

            //Validando os dados
            if (Validador(arrayString))
            {
                this.Result("Somar", services.Soma(arrayString));
            }
            return View("Index");
        }
        public IActionResult Subtrair(string parameter1, string parameter2)
        {
            string[] stringArray = { parameter1, parameter2 };

            if (this.Validador(stringArray))
            {
                this.Result("Subtrair", services.Subtracao(Convert.ToInt32(parameter1), Convert.ToInt32(parameter2)));
            }
            return View("Index");
        }
        public IActionResult Multiplicar(string parameters)
        {
            string[] arrayString = parameters.Split(',');

            if (Validador(arrayString))
            {
                this.Result("Multiplicar", services.Multiplicacao(arrayString));
            }
            return View("Index");
        }
        public IActionResult Dividir(string parameter1, string parameter2)
        {
            string[] stringArray = { parameter1, parameter2 };

            if (this.Validador(stringArray))
            {
                this.Result("Dividir", services.Divisao(Convert.ToInt32(parameter1), Convert.ToInt32(parameter2)));
            }
            return View("Index");
        }

        //Retorna os atributos dos resultados 
        public void Result(string operation, decimal result)
        {
            ViewBag.Operation = operation;
            ViewBag.Result = result;
        }

        //Valida os dados inseridos
        public bool Validador(string[] parameters)
        {
            //Lista de erros
            ViewBag.MessageErrorCalculadora = new List<String>();

            //Variavel que indica se os dados estao validados
            bool result = true;

            //Passando cada item que foi inserido
            for (int i = 0; i < parameters.Length; i++)
            {
                //Verifica se há letras
                if (parameters[i].Where(c => char.IsLetter(c)).Count() > 0)
                {

                    //Verifica se o erro já aconteceu antes para não repetir a mensagem
                    if (!ViewBag.MessageErrorCalculadora.Contains(MessageErrorLetter))
                    {
                        ViewBag.MessageErrorCalculadora.Add(MessageErrorLetter);
                    }
                    result = false;
                }

                //Verifica se há caracteres especiais
                if (Regex.IsMatch(parameters[i], (@"[!""#$%&'()*+-/:;?@[\\\]_`{|}~]")))
                {

                    if (!ViewBag.MessageErrorCalculadora.Contains(MessageErrorSpecialCharacters))
                    {
                        ViewBag.MessageErrorCalculadora.Add(MessageErrorSpecialCharacters);
                    }
                    result = false;
                }

                //Verifica se foi inserido um numero
                if (parameters[i].Length == 0)
                {

                    if (!ViewBag.MessageErrorCalculadora.Contains(MessageErrorNone))
                    {
                        ViewBag.MessageErrorCalculadora.Add(MessageErrorNone);
                    }
                    result = false;
                }

                //Verifica se foi colocado somente zero 
                if (parameters[i] == "0")
                {

                    if (!ViewBag.MessageErrorCalculadora.Contains(MessageErrorZero))
                    {
                        ViewBag.MessageErrorCalculadora.Add(MessageErrorZero);
                    }
                    result = false;
                }
            }
            return result;
        }

        public IActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {

                //Verifica se foi inserido um arquivo
                if (file != null)
                {

                    //Verifica se é um arquivo de texto
                    if (file.ContentType == "text/plain")
                    {

                        //Realizando a leitura do arquivo
                        var text = new StringBuilder();
                        using (var reader = new StreamReader(file.OpenReadStream()))
                        {
                            while (reader.Peek() >= 0)
                                text.AppendLine(await reader.ReadLineAsync());
                        }

                        //Dividindo os nomes linha por linha
                        string[] textArray = text.ToString().Split("\r\n");

                        List<string> list = new();

                        //Tirando os espaços vazios
                        textArray = textArray.Where(text => (text != String.Empty && text != null)).ToArray();

                        //Passando cada item que esta dentro do arquivo
                        for (int i = 0; i < textArray.Length; i++)
                        {

                            //Verifica se ha numeros em um dos nomes
                            if (textArray[i].Any(char.IsDigit))
                            {
                                ViewBag.MessageErrorUpload = "O arquivo contem numeros nos nomes, por favor insira os dados corretamente.";
                                return View("UploadFile");
                            }

                            //Verifica se ha caracteres especiais em um dos nomes
                            if (Regex.IsMatch(textArray[i], (@"[!""#$%&'()*+-/:;?@[\\\]_`{|}~]")))
                            {
                                ViewBag.MessageErrorUpload = "O arquivo contem caracteres especiais, por favor insira os dados corretamente.";
                                return View("UploadFile");
                            }
                            list.Add(textArray[i]);
                        }

                        //Ordenando a lista de nomes
                        list.Sort();

                        ViewBag.Names = list.ToArray();
                        return View("UploadFile");
                    }
                    else
                    {
                        ViewBag.MessageErrorUpload = "Por favor insira somente arquivo de texto.";
                        return View("UploadFile");
                    }
                }
                else
                {
                    ViewBag.MessageErrorUpload = "Nao foi inserido nenhum arquivo, por favor insira o arquivo corretamente.";
                    return View("UploadFile");
                }
            }
            catch (Exception e)
            {
                ViewBag.MessageErrorUpload = "Nao foi possivel realizar o upload por conta desse erro: " + e.Message;
                return View("UploadFile");
            }

        }
    }

}
