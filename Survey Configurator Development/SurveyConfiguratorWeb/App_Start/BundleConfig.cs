﻿using System;
using System.Web.Optimization;

namespace SurveyConfiguratorWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            try
            {
                bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-{version}.js"));

                bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                            "~/Scripts/jquery.validate*"));

                // Use the development version of Modernizr to develop with and learn from. Then, when you're
                // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
                bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));

                bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                          "~/Scripts/bootstrap.js"));

                bundles.Add(new StyleBundle("~/Content/css").Include(
                          "~/Content/bootstrap.css",
                          "~/Content/site.css"));
                bundles.Add(new StyleBundle("~/Content/right_to_left").Include(
                         "~/Content/right_to_left.css"));

                bundles.Add(new ScriptBundle("~/bundles/script").Include(
                "~/Scripts/script.js",
                "~/Scripts/question-table-operations.js"));

                bundles.Add(new ScriptBundle("~/bundles/hub").Include(
                    "~/Scripts/jquery.signalR-2.4.1.min.js",
                    "~/Scripts/question-list-hub.js"));
            }catch(Exception pError)
            {
                ErrorLogger.Log(pError);
            }
           
        }
    }
}
