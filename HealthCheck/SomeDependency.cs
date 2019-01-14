//using Microsoft.Extensions.Diagnostics.HealthChecks;
//using System.Threading;
//using System.Threading.Tasks;

//namespace HealthCheck
//{
//    public class SomeDependency
//    {
//        public string GetMessage() => "Hello from SomeDependency";
//    }
//    public class SomeHealthCheck : IHealthCheck
//    {
//        public string Name => nameof(SomeHealthCheck);

//        private readonly SomeDependency someDependency;

//        public SomeHealthCheck(SomeDependency someDependency)
//        {
//            this.someDependency = someDependency;
//        }

//        public Task<HealthCheckResult> CheckHealthAsync(
//            CancellationToken cancellationToken = default(CancellationToken))
//        {
//            var message = this.someDependency.GetMessage();
//            var result = new HealthCheckResult(HealthCheckStatus.Failed, null, null, null);
//            return Task.FromResult(result);
//        }
//    }
//}
