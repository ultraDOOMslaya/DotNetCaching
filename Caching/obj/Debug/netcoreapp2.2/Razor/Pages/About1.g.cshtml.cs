#pragma checksum "C:\Projects\DotNet\Caching\Caching\Pages\About1.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c0db45536ff2d3e9f161033f366e7579a3361aac"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(Caching.Pages.Pages_About1), @"mvc.1.0.razor-page", @"/Pages/About1.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/About1.cshtml", typeof(Caching.Pages.Pages_About1), null)]
namespace Caching.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Projects\DotNet\Caching\Caching\Pages\_ViewImports.cshtml"
using Caching;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c0db45536ff2d3e9f161033f366e7579a3361aac", @"/Pages/About1.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fdeefc58c4c2dde4121a770c1db4d1a346808ee7", @"/Pages/_ViewImports.cshtml")]
    public class Pages_About1 : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Projects\DotNet\Caching\Caching\Pages\About1.cshtml"
  
    ViewData["Title"] = "What is a Cache?";

#line default
#line hidden
            BeginContext(78, 4, true);
            WriteLiteral("<h1>");
            EndContext();
            BeginContext(83, 17, false);
#line 6 "C:\Projects\DotNet\Caching\Caching\Pages\About1.cshtml"
Write(ViewData["Title"]);

#line default
#line hidden
            EndContext();
            BeginContext(100, 706, true);
            WriteLiteral(@"</h1>

<br />
<br />
<h3>Caches are temporary memory stores that allow us to access temporary data quickly.</h3>
<br />
<br />
<h3>Data is stored similar to a dictionary in the form of key value pairings.</h3>
<br />
<br />
<h3>Keys are typically the request or query string used to retrieve the data originally.</h3>
<h4 style=""text-emphasis-color:gainsboro"">ex: {key: drug/13, value: ""Viagra""}</h4>
<br />
<br />
<h3>Cached key/value pairings typically have a set lifespan and are evicted once expired but several different eviction policies exist depending on the cache being used.</h3>
<br />
<br />
<a class=""btn"" href=""/About"">Back</a>&nbsp;<a class=""btn"" href=""/About2"">Next</a>

");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<AboutModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<AboutModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<AboutModel>)PageContext?.ViewData;
        public AboutModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
