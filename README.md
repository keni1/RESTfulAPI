**RESTful API for Hacker News API**

A. Implementation notes

Following the instructions, an endpoint has been implemented that returns an array with the best stories from the Hacker News API.

With regards to system design, the controller doesn’t need to know about how the data (stories) are sourced, it should be independent from the data source (lean controller). Based on this, a service layer handles the provision of story data to the controller.

To reduce the workload on the Hacker News API, as requested, I’ve implemented in-memory caching. Since the stories’ scores could change any instant, then to cache we should allow for some expiration offset. This is to reflect potential changes to the best stories id list or to the story objects. Other pressure reduction methods such as rate limiting cannot be implemented on the client side.

The provided implementation should be thread-safe because the only shared state is the cache and that’s thread-safe.

Returning thrown exceptions messages with detailed error description should be avoided for security purposes. For this reason, I've added a filter for unhandled exceptions implemented in the class HttpResponseExceptionFilter.

An entity should have identity and as such an Id, but the exercise specifies no Id to be returned for the stories’ JSON. In addition, the API works correctly without an Id in the Story model. Therefore an Id has not been added.

We only have one endpoint with a parameterless get call so routing is straightforward. In addition no versioning considerations at this stage.

B. Additional notes on potential further development

If this small application was to scale in the future, properties such as ‘Time’ could be mapped to a DateTime property (to make it easier to be used internally). However the exercise doesn’t require it, therefore applying YAGNI this has not been done.

We could abstract caching by wrapping the in-memory cache class in an application-specific cache class. This would allow us to separate the specific cache implementation from the service (and from the rest of the app if it was to scale).

Also a data layer or repository could have been added, as a third tier, to further segregate responsibilites and move the API data sourcing away from the service. However, due to the scale of the exercise and the general guidelines given of a two-hour implementation, this data access/repository could be added later on, if needed.

We could look into adding a way to getting notified of changes to stories and their scores in the Hacker News API database (to help with caching expiration policies). At first glance the Firebase .NET SDK doesn’t support real-time listeners (no tick on real-time for C# column in https://firebase.google.com/docs/admin/setup) so it might not be possible to use it to get notified of changes to the best stories Ids API or changes to story objects themselves.

C. How To Run

Created with VS2022, ideally run on same environment by downloading, opening and running the solution locally. Then execute the only end-point method call from the Swagger page, for example. The same could be done from Postman or similar.
