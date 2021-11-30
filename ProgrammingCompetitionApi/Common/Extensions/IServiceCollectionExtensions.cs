using Microsoft.Extensions.DependencyInjection;
using ProgrammingCompetitionApi.Tasks.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCompetitionTasks(this IServiceCollection services)
        {
            Type baseCompetitionTaskType = typeof(CompetitionTask);

            bool alreadyAdded = services.Any(x => baseCompetitionTaskType.IsAssignableFrom(x.ServiceType));
            if (alreadyAdded)
                return services;

            IEnumerable<Type> competitionTaskTypes = Assembly.GetAssembly(baseCompetitionTaskType).GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(baseCompetitionTaskType));

            List<CompetitionTask> competitionTasks = new List<CompetitionTask>();
            int id = 1;
            foreach (Type competitionTaskType in competitionTaskTypes)
            {
                CompetitionTask competitionTask = CreateCompetitionTask(competitionTaskType);
                competitionTask.Id = id;
                id++;

                services.AddSingleton<ICompetitionTask>(competitionTask);
                services.AddSingleton(competitionTaskType, competitionTask);
                competitionTasks.Add(competitionTask);
            }

            return services;
        }

        private static CompetitionTask CreateCompetitionTask(Type competitionTaskType)
        {
            var constructorInfo = competitionTaskType.GetConstructors()[0];
            var constructorParameterCount = constructorInfo.GetParameters().Length;
            var constructorParameters = Enumerable.Repeat<object>(null, constructorParameterCount).ToArray();
            CompetitionTask competitionTask = (CompetitionTask)constructorInfo.Invoke(constructorParameters);
            return competitionTask;
        }
    }
}
