using System.Web.Mvc;

namespace MideaAscm.Areas.Ascm
{
    public class AscmAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Ascm";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Ascm_default",
                "Ascm/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
