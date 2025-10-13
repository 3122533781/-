using System.Collections.Generic;
using UnityEngine;

public class Randomizer<T>
{
	public List<T> Elements { get { return _elements; } }
	public List<float> Probabilities { get { return _probabilityBounds; } }

    public Randomizer()
    {
        _elements = new List<T>();
        _probabilityBounds = new List<float>();
    }

    public Randomizer(List<T> elements, List<float> probabilities)
	{
		_elements = new List<T>();
		_probabilityBounds = new List<float>();
		ResetElements(elements, probabilities);
	}
	
    public void ResetElements(List<T> elements)
	{
		_elements.Clear();
		_probabilityBounds.Clear();
		_isEven = true;
        _elements.AddRange(elements);
	}

    public void ResetElements(List<T> elements, List<int> probabilities)
    {
        List<float> floatProbabilities = new List<float>();
        for (int i = 0; i < probabilities.Count; i++)
        {
            floatProbabilities.Add((float)probabilities[i]);
        }
        ResetElements(elements, floatProbabilities);
    }

	public void ResetElements(List<T> elements, List<float> probabilities)
	{
		if (elements.Count != probabilities.Count)
		{
			Debug.LogError("Count of probabilities should be equal to count of elements");
			return;
		}

		float sum = 0;
        for (int i = 0; i < probabilities.Count; i++)
		{
            sum += probabilities[i];
		}

        _elements.Clear();
        _probabilityBounds.Clear();
		_isEven = false;
		
#if DEBUG_RANDOMIZER
		string debugStr = "{ ";
#endif

        _elements.AddRange(elements);
		for (int i = 0; i < elements.Count; i++)
		{
			float probability = probabilities[i] / sum;
			_probabilityBounds.Add(i == 0 ? probability : i == elements.Count - 1 ? 1 : _probabilityBounds[i - 1] + probability);
#if DEBUG_RANDOMIZER
			debugStr += (i == 0 ? ("[0 - " + _probabilityBounds[0] + "] ") : 
			             ("[" + _probabilityBounds[i - 1] + " - " + _probabilityBounds[i] + "] "));
#endif
		}
#if DEBUG_RANDOMIZER
		debugStr += "}";
	    Debug.Log(debugStr);
#endif
	}

	public T Get()
	{
		if (_elements.Count == 0)
		{
			return default(T);
		}

		if (_elements.Count == 1)
		{
			return _elements[0];
		}

		if (_isEven)
		{
			return _elements[Random.Range(0, _elements.Count)];
		}
		else
		{
			float number = Random.Range(0f, 1f);
			if (number < _probabilityBounds[0])
			{
#if DEBUG_RANDOMIZER
					Debug.Log("random number [" + number + "] is in range [0," +
					          _probabilityBounds[0] + "], return[" + _elements[0] + "]");
#endif
				return _elements[0];
			}
			else
			{
				for (int i = 1; i < _probabilityBounds.Count; i++)
				{
					if (number > _probabilityBounds[i - 1] &&
					    number <= _probabilityBounds[i])
					{
#if DEBUG_RANDOMIZER
							Debug.Log("random number [" + number + "] is in range [" +
							          _probabilityBounds[i - 1] + "," + _probabilityBounds[i] +
							          "], return[" + _elements[i] + "]");
#endif
						return _elements[i];
					}
				}
				Debug.LogError("Code should never run here");
				return default(T);
			}
		}
	}
	
	private List<float> _probabilityBounds;
	private List<T> _elements;
	private bool _isEven;
}