using System;
using System.Linq;
using EPiServer.DataAbstraction;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace Pren.Web.Business.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class TabInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            RegisterTabs();
        }

        public void Uninitialize(InitializationEngine context) { }

        public void Preload(string[] parameters) { }

        private void RegisterTabs()
        {
            var sortIndex = 100;
            foreach (var field in typeof(CustomTabs).GetFields())
            {
                var tabDefinition = field.GetValue(typeof(CustomTabs)) as TabDefinition;
                if (tabDefinition == null)
                    continue;
                var tabDefinitionRepository = ServiceLocator.Current.GetInstance<ITabDefinitionRepository>();

                if (tabDefinition.SortIndex == 0)
                {
                    tabDefinition.SortIndex = sortIndex;
                    sortIndex = sortIndex + 100;
                }

                AddTabToList(tabDefinitionRepository, tabDefinition);
            }
        }

        private void AddTabToList(ITabDefinitionRepository tabDefinitionRepository, TabDefinition definition)
        {
            var existingTab = GetExistingTabDefinition(tabDefinitionRepository, definition);
            if (existingTab != null)
            {
                definition.ID = existingTab.ID;
            }
            tabDefinitionRepository.Save(definition);
        }

        private static TabDefinition GetExistingTabDefinition(ITabDefinitionRepository tabDefinitionRepository, TabDefinition definition)
        {
            return tabDefinitionRepository.List()
                                          .FirstOrDefault(
                                              t =>
                                              t.Name.Equals(definition.Name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
