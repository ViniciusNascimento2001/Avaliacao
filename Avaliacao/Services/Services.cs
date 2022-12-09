using Avaliacao.Interfaces;
using System.Text.RegularExpressions;

namespace Avaliacao.Service
{
    public class Services : ICalculadora
    {
        private static Services _instance;

        private static readonly object _lock = new object();

        //Aplicando o pattern Singleton
        public static Services GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Services();
                    }
                }
            }
            return _instance;
        }
        public int Soma(string[] parameters)
        {
            int result = 0;
            for (int i = 0; i < parameters.Length; i++)
            {
                result += Convert.ToInt32(parameters[i]);
            }
            return result;
        }
        public int Subtracao(int parameter1, int parameter2)
        {
            return parameter1 - parameter2;
        }
        public int Multiplicacao(string[] parameters)
        {
            int result = 1;

            for (int i = 0; i < parameters.Length; i++)
            {
                result *= Convert.ToInt32(parameters[i]);
            }
            return result;
        }
        public decimal Divisao(int parameter1, int parameter2)
        {
            //Passado para decimal por causa dos resultados das divisoes
            return Convert.ToDecimal(parameter1) / Convert.ToDecimal(parameter2);
        }

    }
}
