using DnDProject.Backend.Contexts;
using DnDProject.Backend.Repository.Implementations;
using DnDProject.Backend.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDProject.UnitTests.Repository
{
    public static class RepositoryFactory
    {
        public static ICharacterRepository GetCharacterRepository(CharacterContext context)
        {
            return new CharacterRepository(context);
        }
        public static IIsProficientRepository GetIsProficientRepository(CharacterContext context)
        {
            return new IsProficientRepository(context);
        }
        public static IStatsRepository GetStatsRepository(CharacterContext context)
        {
            return new StatsRepository(context);
        }
        public static INotesRepository GetNotesRepository(CharacterContext context)
        {
            return new NotesRepository(context);
        }
        public static ICurrencyRepository GetCurrencyRepository(CharacterContext context)
        {
            return new CurrencyRepository(context);
        }
        public static IHealthRepository GetHealthRepository(CharacterContext context)
        {
            return new HealthRepository(context);
        }


        public static IPlayableClassRepository GetPlayableClassRepository(PlayableClassContext context)
        {
            return new PlayableClassRepository(context);
        }
        public static IClassAbilityRepository GetClassAbilityRepository(PlayableClassContext context)
        {
            return new ClassAbilityRepository(context);
        }
        public static ISubclassRepository GetSubclassRepository(PlayableClassContext context)
        {
            return new SubclassRepository(context);
        }
        public static ISubclassAbilityRepository GetSubclassAbilityRepository(PlayableClassContext context)
        {
            return new SubclassAbilityRepository(context);
        }

        public static IItemsRepository GetItemsRepository(ItemsContext context)
        {
            return new ItemsRepository(context);
        }


        public static IRaceRepository GetRaceRepository(RaceContext context)
        {
            return new RaceRepository(context);
        }
        public static ISpellsRepository GetSpellsRepository(SpellsContext context)
        {
            return new SpellsRepository(context);
        }
    }
}
