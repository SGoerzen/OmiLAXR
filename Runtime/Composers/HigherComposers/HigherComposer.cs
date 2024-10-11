using System.Collections.Generic;
using System.Linq;

namespace OmiLAXR.Composers.HigherComposers
{
    public abstract class HigherComposer<T> : PipelineComponent, IComposer
        where T : IStatement
    {
        protected abstract Dictionary<string, MatchCondition<T>> Conditions();
        protected static TP GetPipeline<TP>()
            where TP : Pipeline => FindObjectOfType<TP>(true);
        
        protected struct MatchCondition<T0>
            where T0 : IStatement
        {
            public delegate bool MatchConditionHandler(T0 statement);
            public readonly MatchConditionHandler Condition;
            public readonly int ExpectedMatches;
            public readonly List<T0> MatchingStatements;
            public int ActualMatches => MatchingStatements.Count;
            public bool HasEnoughMatches => ActualMatches >= ExpectedMatches;
            
            public MatchCondition(int expectedMatches, MatchConditionHandler condition)
            {
                Condition = condition;
                ExpectedMatches = expectedMatches;
                MatchingStatements = new List<T0>();
            }

            public void CheckCondition(T0 statement)
            {
                if (Condition(statement))
                    MatchingStatements.Add(statement);
            }
            
        }

        public event ComposerAction<IStatement> AfterComposed;
        public bool IsHigherComposer => true;
        public bool IsEnabled => enabled;
        public abstract Author GetAuthor();

        public void LookFor(IStatement statement)
        {
            if (statement.GetType() != typeof(T))
                return;
            
            var conditions = Conditions();
            var stmt = (T)statement;
            foreach (var condition in conditions.Values)
            {
                condition.CheckCondition(stmt);
            }

            var matchesAny = 
                conditions.Where(c => c.Value.HasEnoughMatches)
                    .ToDictionary<KeyValuePair<string, MatchCondition<T>>, string, IEnumerable<T>>(
                        c => c.Key, 
                        c => c.Value.MatchingStatements
                        );
            // check for any match
            if (matchesAny.Count > 0)
            {
                OnMatchAnyConditions(matchesAny);
            }
            
            var matchesAll = conditions.Values.All(c => c.HasEnoughMatches);
            if (!matchesAll) 
                return;
            
            var matchingStatements = conditions.ToDictionary(
                i => i.Key,
                i => (IEnumerable<T>)i.Value.MatchingStatements
            );
            OnMatchAllConditions(matchingStatements);
        }

        protected abstract void OnMatchAllConditions(Dictionary<string, IEnumerable<T>> matchingStatements);

        protected virtual void OnMatchAnyConditions(Dictionary<string, IEnumerable<T>> matchingStatements)
        {
            
        }
    }
}