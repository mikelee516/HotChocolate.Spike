using HotChocolate.Types;

namespace HotChocolate.Spike.GraphQL
{
    public class MyQueryType : ObjectType<MyQuery>
    {
        protected override void Configure(IObjectTypeDescriptor<MyQuery> queryDescriptor)
        {
            // TODO: Sort out authorization methods and policies
            queryDescriptor.Field(t => t.GetWeatherForecasts())
                .Name("PassWeatherForecasts")
                .Authorize("Pass")
                ;

            queryDescriptor.Field(t => t.GetWeatherForecasts())
                .Name("DenyWeatherForecasts")
                .Authorize("Deny")
                ;
        }
    }
}