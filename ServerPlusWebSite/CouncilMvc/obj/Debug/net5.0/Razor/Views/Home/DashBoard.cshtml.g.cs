#pragma checksum "C:\Users\a\OneDrive\Documents\GitHub\CityCouncilSystem\ServerPlusWebSite\CouncilMvc\Views\Home\DashBoard.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c150b3d79501e422cbad4b64e8ccba9df86a4623"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_DashBoard), @"mvc.1.0.view", @"/Views/Home/DashBoard.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\a\OneDrive\Documents\GitHub\CityCouncilSystem\ServerPlusWebSite\CouncilMvc\Views\_ViewImports.cshtml"
using CouncilMvc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\a\OneDrive\Documents\GitHub\CityCouncilSystem\ServerPlusWebSite\CouncilMvc\Views\_ViewImports.cshtml"
using CouncilMvc.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c150b3d79501e422cbad4b64e8ccba9df86a4623", @"/Views/Home/DashBoard.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a2c7c01d9c2c1b3110de451f8439d5d025d9b3c9", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Home_DashBoard : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<CouncilMvc.Models.HomeDashBoardViewModel>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Home", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "ChatDetail", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("list-group-item list-group-item-action"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\a\OneDrive\Documents\GitHub\CityCouncilSystem\ServerPlusWebSite\CouncilMvc\Views\Home\DashBoard.cshtml"
  
    ViewData["Title"] = "DashBoard";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div>\r\n    <H2>Chats</H2>\r\n    <ol class=\"list-group list-group-numbered\">\r\n");
#nullable restore
#line 9 "C:\Users\a\OneDrive\Documents\GitHub\CityCouncilSystem\ServerPlusWebSite\CouncilMvc\Views\Home\DashBoard.cshtml"
         foreach (var item in Model.Accounts)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c150b3d79501e422cbad4b64e8ccba9df86a46234851", async() => {
                WriteLiteral("\r\n                    <li class=\"list-group-item d-flex justify-content-between align-items-start\">\r\n\r\n                        <div class=\"ms-2 me-auto\">\r\n                            <div class=\"fw-bold\"><b>");
#nullable restore
#line 15 "C:\Users\a\OneDrive\Documents\GitHub\CityCouncilSystem\ServerPlusWebSite\CouncilMvc\Views\Home\DashBoard.cshtml"
                                               Write(item.AccountNumber);

#line default
#line hidden
#nullable disable
                WriteLiteral("</b></div>\r\n");
#nullable restore
#line 16 "C:\Users\a\OneDrive\Documents\GitHub\CityCouncilSystem\ServerPlusWebSite\CouncilMvc\Views\Home\DashBoard.cshtml"
                         if (item.Messages.Count > 0)
                        {
                            

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "C:\Users\a\OneDrive\Documents\GitHub\CityCouncilSystem\ServerPlusWebSite\CouncilMvc\Views\Home\DashBoard.cshtml"
                       Write(item.Messages.FirstOrDefault().Content);

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "C:\Users\a\OneDrive\Documents\GitHub\CityCouncilSystem\ServerPlusWebSite\CouncilMvc\Views\Home\DashBoard.cshtml"
                                                                   
                        }

#line default
#line hidden
#nullable disable
                WriteLiteral("                        </div>\r\n                        <span class=\"badge bg-primary rounded-pill\">");
#nullable restore
#line 21 "C:\Users\a\OneDrive\Documents\GitHub\CityCouncilSystem\ServerPlusWebSite\CouncilMvc\Views\Home\DashBoard.cshtml"
                                                               Write(item.Messages.Count);

#line default
#line hidden
#nullable disable
                WriteLiteral("</span>\r\n                    </li>\r\n                ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 11 "C:\Users\a\OneDrive\Documents\GitHub\CityCouncilSystem\ServerPlusWebSite\CouncilMvc\Views\Home\DashBoard.cshtml"
                                                                   WriteLiteral(item.AccountID);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 24 "C:\Users\a\OneDrive\Documents\GitHub\CityCouncilSystem\ServerPlusWebSite\CouncilMvc\Views\Home\DashBoard.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("        \r\n    </ol>\r\n\r\n</div>");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<CouncilMvc.Models.HomeDashBoardViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
