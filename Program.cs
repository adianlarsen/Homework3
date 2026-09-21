using System;

// ============================================================
// Node class — kept OUTSIDE the DoublyLinkedList class.
// A DLL node carries TWO links: next (forward) and prev (backward).
// ============================================================
public class Node
{
    public int value;
    public Node next;
    public Node prev;

    public Node(int value)
    {
        this.value = value;
    }
}

// ============================================================
// DoublyLinkedList class — head/tail/length plus every method.
// ============================================================
public class DoublyLinkedList
{
    private Node head;
    private Node tail;
    private int length;

    public DoublyLinkedList(int value)
    {
        Node newNode = new Node(value);
        head = newNode;
        tail = newNode;
        length = 1;
    }

    public void PrintList()
    {
        Node temp = head;
        while (temp != null)
        {
            Console.Write(temp.value);
            if (temp.next != null) Console.Write(" <-> ");
            temp = temp.next;
        }
        Console.WriteLine();
    }

    public void GetHead()
    {
        Console.WriteLine(head == null ? "Head: null" : "Head: " + head.value);
    }

    public void GetTail()
    {
        Console.WriteLine(tail == null ? "Tail: null" : "Tail: " + tail.value);
    }

    public void GetLength()
    {
        Console.WriteLine("Length: " + length);
    }

    // ========================================================
    // Append — add to the end.
    // newNode.prev = old tail, then tail moves forward. O(1).
    // ========================================================
    public void Append(int value)
    {
        Node newNode = new Node(value);
        if (length == 0)
        {
            head = newNode;
            tail = newNode;
        }
        else
        {
            tail.next = newNode;
            newNode.prev = tail;
            tail = newNode;
        }
        length++;
    }

    // ========================================================
    // Prepend — add to the front.
    // newNode.next = old head, old head.prev = newNode. O(1).
    // ========================================================
    public void Prepend(int value)
    {
        Node newNode = new Node(value);
        if (length == 0)
        {
            head = newNode;
            tail = newNode;
        }
        else
        {
            newNode.next = head;
            head.prev = newNode;
            head = newNode;
        }
        length++;
    }

    // RemoveFirst — O(1), same idea as a singly linked list.
    public Node RemoveFirst()
    {
        if (length == 0) return null;

        Node temp = head;
        if (length == 1)
        {
            head = null;
            tail = null;
        }
        else
        {
            head = head.next;
            head.prev = null;
            temp.next = null;
        }
        length--;
        return temp;
    }

    // ========================================================
    // RemoveLast — O(1) here, unlike a singly linked list's O(n).
    // tail.prev gives direct access to the node before tail,
    // so there's no need to walk the list to find it.
    // ========================================================
    public Node RemoveLast()
    {
        if (length == 0) return null;

        Node temp = tail;
        if (length == 1)
        {
            head = null;
            tail = null;
        }
        else
        {
            tail = tail.prev;
            temp.prev = null;
            tail.next = null;
        }
        length--;
        return temp;
    }

    // ========================================================
    // Get — walk from whichever end is closer to index.
    // index <= length/2  -> start at head, walk forward with next.
    // index >  length/2  -> start at tail, walk backward with prev.
    // Still O(n) worst case, but roughly half the steps on average.
    // ========================================================
    public Node Get(int index)
    {
        if (index < 0 || index >= length) return null;

        Node temp;
        if (index <= length / 2)
        {
            temp = head;
            for (int i = 0; i < index; i++)
            {
                temp = temp.next;
            }
        }
        else
        {
            temp = tail;
            for (int i = length - 1; i > index; i--)
            {
                temp = temp.prev;
            }
        }
        return temp;
    }

    public bool Set(int index, int value)
    {
        Node temp = Get(index);
        if (temp == null) return false;
        temp.value = value;
        return true;
    }

    // ========================================================
    // Insert — splice a new node between "before" and "after".
    // Four links change instead of two, since each node has
    // both a next and a prev to keep consistent.
    // ========================================================
    public bool Insert(int index, int value)
    {
        if (index < 0 || index > length) return false;
        if (index == 0) { Prepend(value); return true; }
        if (index == length) { Append(value); return true; }

        Node before = Get(index - 1);
        Node after = before.next;
        Node newNode = new Node(value);

        before.next = newNode;
        newNode.prev = before;
        newNode.next = after;
        after.prev = newNode;

        length++;
        return true;
    }

    // ========================================================
    // Remove — the node's own prev/next tell you its neighbors
    // directly, no second Get() call needed to find "before".
    // ========================================================
    public Node Remove(int index)
    {
        if (index < 0 || index >= length) return null;
        if (index == 0) return RemoveFirst();
        if (index == length - 1) return RemoveLast();

        Node temp = Get(index);
        Node before = temp.prev;
        Node after = temp.next;

        before.next = after;
        after.prev = before;
        temp.next = null;
        temp.prev = null;

        length--;
        return temp;
    }

    // ========================================================
    // IsPalindrome — walk in from both ends at once.
    // A DLL can do this natively; a SLL cannot walk backward at all.
    // ========================================================
    public bool IsPalindrome()
    {
        if (length == 0 || length == 1) return true;

        Node before = head;
        Node after = tail;
        for (int i = 0; i < length / 2; i++)
        {
            if (before.value != after.value) return false;
            before = before.next;
            after = after.prev;
        }
        return true;
    }

    // ========================================================
    // Reverse — swap next/prev at every node, then swap head/tail.
    // current.prev temporarily holds the ORIGINAL next node, so
    // "current = current.prev" still walks forward through the list.
    // ========================================================
    public void Reverse()
    {
        if (length == 0 || length == 1) return;

        Node current = head;
        Node temp;
        while (current != null)
        {
            temp = current.prev;
            current.prev = current.next;
            current.next = temp;
            current = current.prev;
        }

        temp = head;
        head = tail;
        tail = temp;
    }
}

// ============================================================
// Demo
// ============================================================
public class Program
{
    private static DoublyLinkedList BuildList(int[] values)
    {
        DoublyLinkedList list = new DoublyLinkedList(values[0]);
        for (int i = 1; i < values.Length; i++)
        {
            list.Append(values[i]);
        }
        return list;
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("===== Build & PrintList =====");
        DoublyLinkedList dll = BuildList(new int[] { 10, 20, 30 });
        dll.PrintList();
        dll.GetHead();
        dll.GetTail();
        dll.GetLength();

        Console.WriteLine("\n===== Append(40) =====");
        dll.Append(40);
        dll.PrintList();

        Console.WriteLine("\n===== Prepend(5) =====");
        dll.Prepend(5);
        dll.PrintList();

        Console.WriteLine("\n===== RemoveFirst() =====");
        Node removedFirst = dll.RemoveFirst();
        Console.WriteLine("Removed: " + removedFirst.value);
        dll.PrintList();

        Console.WriteLine("\n===== RemoveLast() =====");
        Node removedLast = dll.RemoveLast();
        Console.WriteLine("Removed: " + removedLast.value);
        dll.PrintList();

        Console.WriteLine("\n===== Get(1) =====");
        Node found = dll.Get(1);
        Console.WriteLine("Get(1) = " + (found != null ? found.value.ToString() : "null"));

        Console.WriteLine("\n===== Set(1, 77) =====");
        dll.Set(1, 77);
        dll.PrintList();

        Console.WriteLine("\n===== Insert(1, 99) =====");
        dll.Insert(1, 99);
        dll.PrintList();

        Console.WriteLine("\n===== Remove(1) =====");
        Node removedMid = dll.Remove(1);
        Console.WriteLine("Removed: " + removedMid.value);
        dll.PrintList();

        Console.WriteLine("\n===== IsPalindrome() =====");
        DoublyLinkedList palindromeList = BuildList(new int[] { 1, 2, 3, 2, 1 });
        palindromeList.PrintList();
        Console.WriteLine("IsPalindrome: " + palindromeList.IsPalindrome());

        DoublyLinkedList notPalindrome = BuildList(new int[] { 1, 2, 3, 4 });
        notPalindrome.PrintList();
        Console.WriteLine("IsPalindrome: " + notPalindrome.IsPalindrome());

        Console.WriteLine("\n===== Reverse() =====");
        dll.PrintList();
        dll.Reverse();
        dll.PrintList();

        Console.WriteLine("\nDone. Press any key to exit.");
        Console.ReadKey();
    }
}
