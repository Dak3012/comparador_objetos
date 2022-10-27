using System.Collections;
namespace Sistema_GrupoMister.Models.Classes;

/// <summary>
/// Classe para comparação entre objetos
/// </summary>
public static class ComparadorObjetos
{
	public static List<ListaDiferenças> ComparadorNotNull<T>(T ObjetoAlvo, T ObjetoEdit)
	{
		var Diferenças = new List<ListaDiferenças>();
		foreach (var properties in ObjetoAlvo.GetType().GetProperties())
		{
			var propertyEditValue = properties.GetValue(ObjetoEdit);
			var propertiAlvo = properties.GetValue(ObjetoAlvo);
			if (properties.PropertyType.GUID == typeof(DateTime).GUID)
			{
				var date = (DateTime)propertyEditValue;
				if (date.Ticks == 0)
				{
					propertyEditValue = null;
				}
			}
			if (propertyEditValue != null)
			{
				var tipo = propertiAlvo?.GetType() ?? propertyEditValue?.GetType();
				var objetoPrimitivoouString = tipo.IsValueType || tipo == typeof(string);
				if (propertyEditValue != null && !propertyEditValue.Equals(propertiAlvo) && !IsCollection(propertiAlvo) && objetoPrimitivoouString)
				{
					properties.SetValue(ObjetoAlvo, propertyEditValue);
					Diferenças.Add(new ListaDiferenças
					{
						Nome = properties.Name,
						Alvo = propertiAlvo,
						Editado = propertyEditValue,
					});
				}
			}
		}
		return Diferenças;
	}
	private static bool IsCollection(object objeto)
	{
		var teste = (objeto is IEnumerable) && !(objeto is string);
		return teste;
	}
	public class ListaDiferenças
	{
		public string Nome { get; set; }
		public object Alvo { get; set; }
		public object Editado { get; set; }
	}
}
