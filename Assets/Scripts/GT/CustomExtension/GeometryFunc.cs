using UnityEngine;

namespace CustomExtension
{
    public static class GeometryFunc
    {
        public static Vector3 GetDirectionByEnum(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return Vector3.left;
                case Direction.Up:
                    return Vector3.up;
                case Direction.Right:
                    return Vector3.right;
                case Direction.Down:
                    return Vector3.down;
                case Direction.Forward:
                    return Vector3.forward;
                default:                
                    return Vector3.zero;
            }
        }
    }

    public enum XyzAxis
    {
        X,
        Y,
        Z
    }

    public enum Direction
    {
        Left,
        Up,
        Right,
        Down,
        Forward
    }
}