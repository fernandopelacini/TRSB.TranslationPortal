using Application.Interfaces;

namespace Application.Services
{
    public class UppercaseEngine : ITranslationEngine
    {
        public string Translate(string input)
        {
            return input.ToUpperInvariant();
        }
    }
}
