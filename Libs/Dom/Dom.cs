using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using Fort;

using RhoMicro.LogoSyn.Libs.Common.Strings;
using RhoMicro.LogoSyn.Libs.Dom.Dom;
using RhoMicro.LogoSyn.Libs.Dom.Dom.Abstractions;
using RhoMicro.Serialization.Attributes;

namespace RhoMicro.LogoSyn.Libs.Dom;

/// <summary>
/// Default implementation of <see cref="IDom{TDiscriminator}"/>.
/// </summary>
/// <typeparam name="TDiscriminator">
/// The discriminator by which to distinguish elements.
/// </typeparam>
[DataContract]
[JsonContract(SettingsMember = nameof(_contractSettings))]
public sealed partial class Dom<TDiscriminator> : IDom<TDiscriminator>
{
	/// <summary>
	/// Initializes a new instance with the chunk size provided.
	/// </summary>
	/// <param name="chunkSize">The new instances chunk size.</param>
	/// <param name="initialElements">The instances initial elements.</param>
	public Dom(Int32 chunkSize = 80, IEnumerable<IDomElement<TDiscriminator>>? initialElements = null)
	{
		ChunkSize = chunkSize;
		InitialElements = initialElements?.Select(DomElement<TDiscriminator>.Clone).ToArray();
		Initialize();
	}

	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	/// <remarks>
	/// The time complexity of <see cref="Add(IDomElement{TDiscriminator})"/> and <see cref="Remove(Int32, Int32)"/> is proportionally affected by <see cref="ChunkSize"/>.<br/>
	/// The storage complexity of the instance is inversely proportionally affected by <see cref="ChunkSize"/>.
	/// </remarks>
	[DataMember]
	public Int32 ChunkSize {
		get; private set;
	}
	[DataMember]
	private IDomElement<TDiscriminator>[]? InitialElements {
		get; set;
	}

	[OnDeserialized]
	private void OnDeserialized(StreamingContext context) => Initialize();

	private void Initialize()
	{
		if(InitialElements != null)
		{
			foreach(var element in InitialElements)
			{
				Add(element);
			}
		}
	}

	private IDictionary<Int32, Chunk<TDiscriminator>>? _chunks;
	private IDictionary<Int32, Chunk<TDiscriminator>> Chunks => _chunks ??= new Dictionary<Int32, Chunk<TDiscriminator>>();
	private Object? _syncRoot;
	private Object SyncRoot => _syncRoot ??= new Object();

	private static readonly DataContractJsonSerializerSettings _contractSettings
		= new()
		{
			KnownTypes = new[]
			{
				typeof(DomElement<TDiscriminator>),
				typeof(StringSlice)
			}
		};

	/// <summary>
	/// Serializes an instance of <see cref = "IDom{TDiscriminator}"/> into a stream.
	/// </summary>
	/// <param name = "target">The stream to serialize into.</param>
	/// <param name = "instance">The instance to serialize.</param>
	/// <exception cref = "System.ArgumentNullException">Thrown when <paramref name = "instance"/> or <paramref name = "target"/> is <see langword="null"/>.</exception>
	public static void WriteJson(Stream target, IDom<TDiscriminator> instance)
	{
		target.ThrowIfDefault(nameof(target));
		instance.ThrowIfDefault(nameof(instance));

		var clone = Clone(instance);

		WriteJson(target, clone);
	}
	private static Dom<TDiscriminator> Clone(IDom<TDiscriminator> instance)
	{
		instance.ThrowIfDefault(nameof(instance));

		var result = new Dom<TDiscriminator>(instance.ChunkSize, instance);

		return result;
	}

	/// <inheritdoc/>
	public Boolean TryGet(Int32 position, out IDomElement<TDiscriminator>? element) => TryGet(position, out var _, out element);

	private Boolean TryGet(Int32 position, out Chunk<TDiscriminator>? chunk, out IDomElement<TDiscriminator>? element)
	{
		var chunkLine = position - position % ChunkSize;

		lock(SyncRoot)
		{
			(chunk, element) = (null, null);

			var result = Chunks.TryGetValue(chunkLine, out chunk) && chunk.TryGet(position, out element);

			return result;
		}
	}

	/// <inheritdoc/>
	public void Add(IDomElement<TDiscriminator> element)
	{
		if(element == null)
		{
			throw new ArgumentNullException(nameof(element));
		}

		var subElements = Enumerable.Range(element.Position, element.Slice.Length)
			.GroupBy(i => i / ChunkSize)
			.Select(g => g.ToArray())
			.Select(a => (position: a[0], length: a.Length))
			.Select((t, i) => (t.position, start: t.position - element.Position + element.Slice.Start, t.length))
			.Select(t => (t.position, slice: element.Slice.ReSlice(t.start, t.length)))
			.Select(t => new DomElement<TDiscriminator>(element.Kind, t.slice, t.position));

		lock(SyncRoot)
		{
			foreach(var subElement in subElements)
			{
				var chunkLine = subElement.Position - subElement.Position % ChunkSize;

				if(Chunks.TryGetValue(chunkLine, out var chunk))
				{
					chunk.Add(subElement);
				} else
				{
					chunk = new Chunk<TDiscriminator>(chunkLine, ChunkSize)
					{
						subElement
					};
					Chunks.Add(chunkLine, chunk);
				}
			}
		}
	}

	/// <inheritdoc/>
	public void Clear()
	{
		lock(SyncRoot)
		{
			Chunks.Clear();
		}
	}

	/// <inheritdoc/>
	public Int32 Count()
	{
		lock(SyncRoot)
		{
			var count = Chunks.Sum(b => b.Value.AbsoluteLoad);

			return count;
		}
	}

	private IEnumerable<IDomElement<TDiscriminator>> Enumerate()
	{
		var elements = Chunks.Values.OrderBy(b => b.Position).SelectMany(b => b);

		var neighbours = new Queue<IDomElement<TDiscriminator>>();
		IDomElement<TDiscriminator>? concatenatedQueue = null;
		IDomElement<TDiscriminator>? last = null;

		foreach(var element in elements)
		{
			if(last != null && !last.IsKindNeighbour(element))
			{
				_ = tryConcatenateQueue();
				yield return concatenatedQueue!;
			}

			neighbours.Enqueue(element);
			last = element;
		}

		if(tryConcatenateQueue())
		{
			yield return concatenatedQueue!;
		}

		Boolean tryConcatenateQueue()
		{
			concatenatedQueue = null;

			if(neighbours!.TryDequeue(out var e))
			{
				while(neighbours.TryDequeue(out var next))
				{
					e = e.Concat(next);
				}

				concatenatedQueue = e;
			}

			return concatenatedQueue != null;
		}
	}

	/// <inheritdoc/>
	public IEnumerator<IDomElement<TDiscriminator>> GetEnumerator()
	{
		var enumerator = Enumerate().GetEnumerator();

		return enumerator;
	}

	/// <inheritdoc/>
	public IEnumerable<IDomElement<TDiscriminator>> Remove(Int32 position, Int32 length = 0)
	{
		var result = new List<IDomElement<TDiscriminator>>();

		lock(SyncRoot)
		{
			for(var i = position; i <= position + length; i++)
			{
				if(TryGet(i, out var chunk, out var element) && chunk!.TryRemove(element!))
				{
					i += element!.Slice.Length + element.Position - i;
					result.Add(element!);
				}
			}
		}

		return result;
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
