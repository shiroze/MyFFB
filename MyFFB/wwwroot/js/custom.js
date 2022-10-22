////$("#side-menu").length &&
////(
////    (i = s("#side-menu li .collapse")).on({
////        "show.bs.collapse": function (t) {
////            var e = s(t.target).parents(".collapse.show");
////            s("#side-menu .collapse.show").not(e).collapse("hide");
////        }
////    }),
////    $("#side-menu a").each(function () {
////        var t,
////            e,
////            n,
////            o = window.location.href.split(/[?#]/)[0];
////        this.href == o &&
////            (s(this).addClass("active"),
////                s(this).parent().addClass("menuitem-active"),
////                s(this).parent().parent().parent().addClass("show"),
////                s(this).parent().parent().parent().parent().addClass("menuitem-active"),
////                "sidebar-menu" !== (t = s(this).parent().parent().parent().parent().parent().parent()).attr("id") && t.addClass("show"),
////                s(this).parent().parent().parent().parent().parent().parent().parent().addClass("menuitem-active"),
////                "wrapper" !== (e = s(this).parent().parent().parent().parent().parent().parent().parent().parent().parent()).attr("id") && e.addClass("show"),
////                (n = s(this).parent().parent().parent().parent().parent().parent().parent().parent().parent().parent()).is("body") || n.addClass("menuitem-active")),
////            setTimeout(function () {
////                var t,
////                    e,
////                    n = document.querySelector("li.menuitem-active .active");
////                null != n && ((t = document.querySelector(".left-side-menu .simplebar-content-wrapper")), (e = n.offsetTop - 300), t && 100 < e && scrollTo(t, e, 600));
////            }, 200);
////    })
////);