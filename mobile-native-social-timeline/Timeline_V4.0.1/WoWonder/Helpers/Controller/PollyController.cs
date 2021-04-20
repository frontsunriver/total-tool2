using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Polly;

namespace WoWonder.Helpers.Controller
{
    public class PollyController
     {
        public static void RunRetryPolicyFunction(List<Func<Task>> actionList, int retryCount = 4, int everySecond = 4)
        {
            var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(retryCount, i => TimeSpan.FromSeconds(everySecond));
            foreach (var action in actionList)
                retryPolicy.ExecuteAsync(action);
        } 
    }
}