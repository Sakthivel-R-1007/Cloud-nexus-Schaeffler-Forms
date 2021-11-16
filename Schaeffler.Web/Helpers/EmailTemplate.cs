using System.IO;
using System.Web.Mvc;

namespace Schaeffler.Helpers
{
    public class EmailTemplate
    {
        public static string RenderViewToString<T>(ControllerBase controller, string viewName, T model)
        {
            using (var writer = new StringWriter())
            {
                ViewEngineResult result = ViewEngines
                          .Engines
                          .FindView(controller.ControllerContext,
                                    viewName, null);

                var viewPath = ((RazorView)result.View);
                var view = new RazorView(controller.ControllerContext, viewPath.ViewPath, null, false, null);
                var vdd = new ViewDataDictionary<T>(model);
                var viewCxt = new ViewContext(
                                    controller.ControllerContext,
                                    view,
                                    vdd,
                                    new TempDataDictionary(), writer);
                viewCxt.View.Render(viewCxt, writer);
                return writer.ToString();
            }
        }

    }
}
