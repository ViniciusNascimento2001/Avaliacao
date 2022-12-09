namespace Avaliacao.Interfaces
{
    public interface ICalculadora
    {
        public int Soma(string[] parameters);
        public int Subtracao(int p1, int p2);
        public int Multiplicacao(string[] parameters);
        public decimal Divisao(int p1, int p2);
    }
}
