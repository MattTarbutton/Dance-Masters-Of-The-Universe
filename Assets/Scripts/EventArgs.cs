using System;
using System.Collections;
using System.Collections.Generic;

public class EventArgs<T> : EventArgs
{
    private T m_value;

    public EventArgs(T value)
    {
        m_value = value;
    }
    
    public T Value
    {
        get { return m_value; }
    }
}
