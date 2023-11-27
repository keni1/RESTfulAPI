**RESTful API for Hacker News API**

Only one endpoint has been implemented because the instructions say that the API should return an array.

The controller doesn’t need to know about how the data (stories) are sourced, it should be independent from the data source (lean controller). 

Since the stories’ scores could change any instant, then to cache we should allow for some expiration offset. This is to reflect potential changes to the best stories id list or to the story objects. I’ve implemented in-memory caching, which reduces workload on the Hacker News API as requested.

The implementation should be thread-safe because the only shared state is the cache and that’s thread-safe.

An entity should have identity and as such an Id, but the exercise specifies no Id to be returned for the stories’ JSON. In addition, the API works correctly without an Id in the Story model. Therefore an Id has not been added.

If this small application was to scale in the future, properties such as ‘Time’ could be mapped to a DateTime property (to make it easier to be used internally). However the exercise doesn’t require it, therefore applying YAGNI this has not been done.

Returning thrown exceptions messages with detailed error description should be avoided for security purposes. Added filter for unhandled exceptions implemented in the class HttpResponseExceptionFilter.

At first glance the Firebase .NET SDK doesn’t support real-time listeners (no tick on real-time column in https://firebase.google.com/docs/admin/setup) so it might not be possible to use it to get notified of changes to the best stories Ids API or changes to story objects themselves.

We only have one endpoint with a parameterless get call so routing is straightforward. In addition no versioning considerations at this stage.

We could abstract the caching by wrapping the in-memory cache class in an application-specific cache class. This would allow us to separate the specific cache implementation from the service (and from the rest of the app if it was to scale).

