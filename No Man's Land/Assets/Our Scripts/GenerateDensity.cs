
public class GenerateDensity
{
     public GenerateDensity()
    {
        //Empty Constructor
    }

    public float FlatPlane(int y, float height)
    {
        return y - height - 10f;
    }

    public float CalculateDensity(int worldPosX, int worldPosY, int worldPosZ)
    {
        return FlatPlane(worldPosY, 10.0f );
    }

    
}
