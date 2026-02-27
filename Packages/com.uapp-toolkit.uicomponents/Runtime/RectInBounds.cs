using UnityEngine;


namespace UAppToolkit.UIComponents
{
    public class RectInBounds
    {
        public Vector3 RequiredOffset { get; }
        public bool TopOverflow => RequiredOffset.y < 0;
        public bool RightOverflow => RequiredOffset.x < 0;
        public bool BottomOverflow => RequiredOffset.y > 0;
        public bool LeftOverflow => RequiredOffset.x > 0;


        private RectInBounds(Vector3 offset)
        {
            RequiredOffset = offset;
        }

        public static RectInBounds CalculateOffsetToStayInBounds(Bounds innerBounds, Bounds outerBounds)
        {
            Vector2 offset = Vector2.zero;

            Vector3 rectMin = innerBounds.min;
            Vector3 rectMax = innerBounds.max;
            Vector3 boundsMin = outerBounds.min;
            Vector3 boundsMax = outerBounds.max;

            bool leftOverflow = rectMin.x < boundsMin.x;
            bool rightOverflow = rectMax.x > boundsMax.x;
            bool topOverflow = rectMax.y > boundsMax.y;
            bool bottomOverflow = rectMin.y < boundsMin.y;

            if (leftOverflow)
            {
                offset.x = boundsMin.x - rectMin.x;
            }
            else if (rightOverflow)
            {
                offset.x = boundsMax.x - rectMax.x;
            }
    
            if (topOverflow)
            {
                offset.y = boundsMax.y - rectMax.y;
            }
            else if (bottomOverflow)
            {
                offset.y = boundsMin.y - rectMin.y;
            }

            return new RectInBounds(offset);
        }
    }
}