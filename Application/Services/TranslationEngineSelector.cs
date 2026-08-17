using Application.Interfaces;
//using Domain.Entities;

namespace Application.Services
{
    public class TranslationEngineSelector
    {
        private readonly IEnumerable<ITranslationEngine> _engines;
        public TranslationEngineSelector(IEnumerable<ITranslationEngine> engines)
        {
            _engines = engines;
        }

        /// <summary>
        /// This was the first implementation of the engine selector, which selected an engine based on the organization ID. It has been commented out in favor of a random selection method.
        /// Alpha (org 1) was always using the ReverseEngine, while Beta (org 2) was always using the UppercaseEngine.
        /// To restore it , uncomment the method below and comment out the random selection method and uncomment the using statement for Domain.Entities at the top of this file.
        /// </summary>
        /// <returns></returns>
        //public ITranslationEngine SelectEngine(int organizationId)
        //{
        //    return organizationId == 1
        //    ? _engines.OfType<ReverseEngine>().First()
        //    : _engines.OfType<UppercaseEngine>().First();
        //}


        public ITranslationEngine SelectEngine()
        {
            var engine = _engines.ToList();
            var randomEngine = new Random();
            return engine[randomEngine.Next(0, engine.Count)];
        }
    }
}
