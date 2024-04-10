﻿namespace Utils;

public class RemainBytes
{
    private const int DEFAULT_SIZE = 8192;

    private int _currentMessageLength = -1;

    // 下一个写入字节的位置
    private int NextIndex => Count + StartIndex;
    // 剩余的可用大小
    private int AvaliableSize => Bytes.Length - Count - StartIndex;

    public byte[] Bytes { get; private set; } = new byte[DEFAULT_SIZE];

    /// <summary>
    /// Bytes的起始索引
    /// </summary>
    public int StartIndex { get; private set; }

    /// <summary>
    /// Bytes有效字节的数量
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Bytes的有效容量，该值可能大于Count
    /// </summary>
    public int Capacity => Bytes.Length - StartIndex;

    public void SetCurrentMessageLength(int length)
    {
        _currentMessageLength = length;
    }

    public int GetCurrentMessageLength()
    {
        return _currentMessageLength;
    }

    /// <summary>
    /// 当从Stream读取了新的数据后，调用该方法添加新数据
    /// </summary>
    /// <param name="newBytes"></param>
    /// <param name="startIndex"></param>
    /// <param name="size"></param>
    public void Append(byte[] newBytes, int startIndex, int size)
    {
        if (_currentMessageLength != -1)
        {
            // 当当前消息的大小确定时，直接调整容量到指定大小
            AddArrayCapacity(Math.Max(_currentMessageLength, Count + size));
        }
        else
        {
            // 检查并调整容量
            if (AvaliableSize < size)
            {
                AddArrayCapacity((int)((Capacity + size) * 1.5)); // 调整1.5倍
            }
        }
        // 添加到Bytes中
        Array.Copy(newBytes, startIndex, Bytes, NextIndex, size);
        Count += size;
    }

    public void RemoveHeader(int count)
    {
        StartIndex += count;
        Count -= count;
    }

    /// <summary>
    /// 增加Bytes容量. 如果newSize小于当前容量，do nothing.
    /// </summary>
    /// <param name="newSize"></param>
    private void AddArrayCapacity(int newSize)
    {
        if (newSize <= Capacity)
            return;
        ResizeArrayCapacity(newSize);
    }

    private void ResizeArrayCapacity(int newSize)
    {
        var tmp = new byte[newSize];
        Array.Copy(Bytes, StartIndex, tmp, 0, Count);
        Bytes = tmp;
        StartIndex = 0;
    }
}
