public class BoundsCalculator
{
    public static void CalculateBounds(double centerX, double centerY, double delta, out double left, out double right, out double top, out double bottom)
    {
        // Przyjęte różnice pomiędzy środkowym punktem a krawędziami
        double deltaX = delta; // Różnica dla osi X
        double deltaY = delta; // Różnica dla osi Y

        // Oblicz krawędzie
        left = centerX - deltaX;
        right = centerX + deltaX;
        top = centerY + deltaY;
        bottom = centerY - deltaY;
    }
}