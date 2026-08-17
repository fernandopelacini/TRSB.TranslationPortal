using Application.Interfaces;

namespace Application.Services
{
    public class RotateEngine : ITranslationEngine
    {
        private const sbyte _rotateAmount = 7;
        private const string _plaintext = "abcdefghijklmnopqrstuvwxyz";
        public string Translate(string input)
        {
            string ciphertext = "";

            for (int i = 0; i < input.Length; i++)
            {
                //Just letters, numbers, spaces, commas, etc are left as they are.
                if (!char.IsLetter(input[i]))
                {
                    ciphertext += input[i];
                    continue;
                }

                var character = _plaintext.IndexOf(char.ToLower(input[i])) + _rotateAmount;

                //Exceeding the length of the alphabet, wrap around to the beginning.
                if (character >= _plaintext.Length)
                {
                    character = character - _plaintext.Length;
                }

                string temp = _plaintext.Substring(character, 1);

                ciphertext += char.IsUpper(input[i]) ? temp.ToUpper() : temp;
            }

            return ciphertext;
        }
    }
}
