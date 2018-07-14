# TestApplicationInsightsApp
An example demonstrating using Azure Application Insights from a .NET application.

To use this example, first make an Application Insights resource as outlined here:
https://docs.microsoft.com/en-us/azure/application-insights/app-insights-create-new-resource

Then set the instrumetation key in App.xaml.cs, at line 29.

The applicaiton is now ready to be built and run.
When running it will begin to send log events, exception events, and telemetry data to the Application Insights resouce.

This data can then be viewed in the Azure portal.
