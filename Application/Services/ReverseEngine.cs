using Application.Interfaces;

namespace Application.Services
{
    public class ReverseEngine : ITranslationEngine
    {
        public string Translate(string input)
        {
            return new string(input.Reverse().ToArray());
        }
    }
}
