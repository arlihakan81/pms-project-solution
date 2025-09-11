namespace Thunder.Application.Mapping
{
	public interface IMapper
	{
		TDestination Map<TDestination, TSource>(TSource source);
		List<TDestination> Map<TDestination, TSource>(List<TSource> source);



	}
}
