using System.Web.Mvc;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Pren.Web.Business.Rendering;

namespace Pren.Web.Business.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class DisplayRegistryInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            if (context.HostType == HostType.WebApplication)
            {
                var options = ServiceLocator.Current.GetInstance<DisplayOptions>();
                options
                    .Add("full", "/displayoptions/full", RenderingConstants.ContentAreaTags.FullWidth, "", "epi-icon__layout--full")
                    .Add("fullclear", "/displayoptions/fullclear", RenderingConstants.ContentAreaTags.FullWidthClear, "", "epi-icon__layout--full")
                    .Add("wide", "/displayoptions/wide", RenderingConstants.ContentAreaTags.TwoThirdsWidth, "", "epi-icon__layout--two-thirds")
                    .Add("wideclear", "/displayoptions/wideclear", RenderingConstants.ContentAreaTags.TwoThirdsWidthClear, "", "epi-icon__layout--two-thirds")
                    .Add("half", "/displayoptions/half", RenderingConstants.ContentAreaTags.HalfWidth, "", "epi-icon__layout--half")
                    .Add("halfclear", "/displayoptions/halfclear", RenderingConstants.ContentAreaTags.HalfWidthClear, "", "epi-icon__layout--half")
                    .Add("narrow", "/displayoptions/narrow", RenderingConstants.ContentAreaTags.OneThirdWidth, "", "epi-icon__layout--one-third")
                    .Add("narrowclear", "/displayoptions/narrowclear", RenderingConstants.ContentAreaTags.OneThirdWidthClear, "", "epi-icon__layout--one-third")
                    .Add("quarter", "/displayoptions/quarter", RenderingConstants.ContentAreaTags.QuarterWidth, "", "epi-icon__layout--one-quarter")
                    .Add("quarterclear", "/displayoptions/quarterclear", RenderingConstants.ContentAreaTags.QuarterWidthClear, "", "epi-icon__layout--one-quarter")
                    .Add("sixth", "/displayoptions/sixth", RenderingConstants.ContentAreaTags.OneSixthWidth, "", "epi-icon__layout--half")
                    .Add("sixthclear", "/displayoptions/sixthclear", RenderingConstants.ContentAreaTags.OneSixthWidthClear, "", "epi-icon__layout--half");

                AreaRegistration.RegisterAllAreas();
            }
        }

        public void Uninitialize(InitializationEngine context){}

        public void Preload(string[] parameters){}
    }
}
