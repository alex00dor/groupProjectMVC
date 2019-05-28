using ChatProject.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChatProject.Infrastructure
{
    [HtmlTargetElement("ul", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext] [HtmlAttributeNotBound] public ViewContext ViewContext { get; set; }
        public PagingInfo PageModel { get; set; }
        public bool PagePreviousNext { get; set; } = false;
        public string PageAction { get; set; }
        public string PageClassLink { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }
        public string PageClassDisabled { get; set; }

        public override void Process(TagHelperContext context,
            TagHelperOutput output)
        {
            if (PageModel.TotalPages <= 1)
            {
                return;
            }
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new TagBuilder("ul");
            if (PagePreviousNext)
            {
                TagBuilder li = new TagBuilder("li");
                int prev = PageModel.CurrentPage - 1;
                li.AddCssClass(prev > 0 ? PageClassNormal : PageClassDisabled);
                TagBuilder tag = new TagBuilder("span");
                if (prev > 0)
                {
                    tag = new TagBuilder("a");
                    tag.Attributes["href"] = urlHelper.Action(PageAction, new {page = prev});
                }
                tag.AddCssClass(PageClassLink);
                tag.InnerHtml.Append("Previous");
                li.InnerHtml.AppendHtml(tag);
                result.InnerHtml.AppendHtml(li);

            }
            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder li = new TagBuilder("li");
                li.AddCssClass(i == PageModel.CurrentPage? PageClassSelected : PageClassNormal);
                TagBuilder tag = new TagBuilder("span");
                if (i != PageModel.CurrentPage)
                {
                    tag = new TagBuilder("a");
                    tag.Attributes["href"] =  urlHelper.Action(PageAction, new {page = i});
                }
                tag.AddCssClass(PageClassLink);
                tag.InnerHtml.Append(i.ToString());
                li.InnerHtml.AppendHtml(tag);
                result.InnerHtml.AppendHtml(li);
            }
            
            if (PagePreviousNext)
            {
                TagBuilder li = new TagBuilder("li");
                int next = PageModel.CurrentPage + 1;
                li.AddCssClass(next <= PageModel.TotalPages ? PageClassNormal : PageClassDisabled);
                TagBuilder tag = new TagBuilder("span");
                if (next <= PageModel.TotalPages)
                {
                    tag = new TagBuilder("a");
                    tag.Attributes["href"] = urlHelper.Action(PageAction, new {page = next});
                }
                tag.AddCssClass(PageClassLink);
                tag.InnerHtml.Append("Next");
                li.InnerHtml.AppendHtml(tag);
                result.InnerHtml.AppendHtml(li);
            }

            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}