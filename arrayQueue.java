import java.util.NoSuchElementException;
import java.util.Random;
import java.util.Scanner;

/**
 *
 * @author arche
 * @param <E>
 */
public class ArrayQueue1<E> implements QueueADT1<E>
{

    public int head;
    public int tail = 0;

    private final int INITIAL_CAPACITY = 3;
    protected int numElements;
    protected E[] elements;
    public int arraySize;

    public ArrayQueue1()
    {
	super();
	elements = (E[]) (new Object[INITIAL_CAPACITY]);
	for (E e : elements)
	{
	    arraySize++;
	}
    }

    private void expandCapacity()
    {
	E[] largerArray = (E[]) (new Object[elements.length * 2]);
	int newIndex = 0;

	for (int i = head; i < arraySize; i++)
	{
	    largerArray[newIndex] = elements[i];
	    newIndex++;
	}

	for (int i = 0; i < tail; i++)
	{
	    largerArray[newIndex] = elements[i];
	    newIndex++;
	}
	head = 0;
	tail = numElements;
	elements = largerArray;
    }

    public static void main(String[] args)
    {
	ArrayQueue1 queue = new ArrayQueue1();
	boolean bool = true;
	Scanner scan = new Scanner(System.in);
	Random rand = new Random();

	do
	{
	    System.out.println("1.add 2.remove 3.stop 4.Print");
	    int input = scan.nextInt();

	    switch (input)
	    {
		case 1:
		    queue.enqueue(rand.nextInt(50));
		    break;
		case 2:
		    queue.dequeue();
		    break;
		case 3:
		    bool = false;
		    break;
		case 4:
		    System.out.println(queue.toString());
		    break;

		default:
		    break;
	    }

	    System.out.println(queue.toString());

	} while (bool);

    }

    @Override
    public void enqueue(E element)
    {
	if (this.size() == 0)
	{
	    head = 0;
	    tail = 0;
	}

	elements[tail] = element;
	tail++;

	numElements++;

	if (tail == head)
	{
	    this.expandCapacity();
	    this.arraySize *= 2;
	}

	if (tail == this.arraySize && head != 0)
	{
	    tail = 0;
	}
	else if (tail == this.arraySize && head == 0)
	{
	    tail = 0;
	    this.expandCapacity();
	    this.arraySize *= 2;
	}

    }

    @Override
    public E dequeue() throws NoSuchElementException
    {
	if (elements[head] == null)
	{
	    throw new NoSuchElementException();
	}

	E temp = elements[head];
	elements[head] = null;
	numElements--;
	head++;

	if (head >= this.arraySize)
	{
	    head = 0;
	}

	return temp;
    }

    @Override
    public E first() throws NoSuchElementException
    {
	return elements[head];
    }

    @Override
    public boolean isEmpty()
    {
	return (numElements == 0);
    }

    @Override
    public int size()
    {
	return numElements;
    }

    public String toString()
    {
	String temp = "";

	for (E e : elements)
	{
	    temp += e + " ";
	}

	return temp;
    }

}