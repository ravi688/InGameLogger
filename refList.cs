using System;
using UnityEngine; 


public class refList<Type>
{
    public delegate bool Comparer(Type type1, Type type2); 
    private Type[] elements;
    public int Count { get; private set; } 
    public int Capacity { get; private set; }  

    public refList(int size = 8)
    {
        elements = new Type[size] ; 
        Count = 0;
        Capacity = size; 
    }
    public void Add(Type new_value)
    {
        if((Count  + 1) > Capacity)
        {
            Capacity *= 2;
            Type[] new_elements = new Type[Capacity];
            for (int i = 0; i < Count; i++)
                new_elements[i] = elements[i];
            elements = new_elements;

            Add(new_value) ; 
        }
        else
        {
            Count++; 
            elements[Count - 1] = new_value ; 
        }
    }
    public Type GetValue(int index)
    {
        if (index > (Count - 1))
        {
            Debug.LogError("Index out of range exception in refList");
            return default(Type); 
        }

        return elements[index]; 
    }
    public void Remove(Type type , Comparer comparer)
    {
        for (int i = 0; i < Count; i++)
        {
            if (comparer(elements[i], type) == true)
            {
                for (int j = i; j < Count - 1; j++)
                {
                    elements[j] = elements[j + 1];
                }
                Count--;
                break;
            }
        }
    }
}
