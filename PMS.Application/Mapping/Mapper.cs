
namespace Thunder.Application.Mapping
{
	public class Mapper : IMapper
	{
		public TDestination Map<TDestination, TSource>(TSource source)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source), "Source cannot be null");
			}

			var destination = Activator.CreateInstance<TDestination>();
			if (destination == null)
			{
				throw new InvalidOperationException($"Cannot create instance of {typeof(TDestination).Name}");
			}

			// Assuming a simple property mapping, you can use reflection or a library like AutoMapper for complex mappings
			foreach (var prop in typeof(TSource).GetProperties())
			{
				var destProp = typeof(TDestination).GetProperty(prop.Name);
				if (destProp != null && destProp.CanWrite)
				{
					destProp.SetValue(destination, prop.GetValue(source));
				}
			}

			return destination;
		}

		public List<TDestination> Map<TDestination, TSource>(List<TSource> source)
		{
			if (source == null || source.Count == 0)
			{
				return new List<TDestination>();
			}

			var list = new List<TDestination>();
			foreach (var item in source)
			{
				var mappedItem = Map<TDestination, TSource>(item);
				list.Add(mappedItem);
			}

			return list;
		}
	}
}
