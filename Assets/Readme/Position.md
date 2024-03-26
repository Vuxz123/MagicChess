# Postion
### Type of Position:
- Inner Position
- Outer Position

### Inner Position:
- Inner Position is the position of a `Piece` represented by the square it is on in the Inner `ChessBoard` object.
- Inner Position is a tuple of two integers, the first integer represents the row and the second integer represents the column.

### Outer Position:
- Outer Position is the position of an `ISquare` object in the `IChessBoardOuter` object.
- Outer Position is a tuple of two integers, the first integer represents the row and the second integer represents the column.

### Position Conversion:
- The conversion of Inner Position to Outer Position and vice versa is done by the `GameManagerOuter` object.
- The `GameManagerOuter` object has a method `ConvertOuterToInnerPos()` which takes Inner Position as input and returns the corresponding Outer Position.
- The position conversion can also be done by reversing the position.

#### Example:
- Inner Position: (1, 0)
- Outer Position: (0, 1)

#### Note:
- Remember to use the `GameManagerOuter` object to convert the position when passing position from Inner to Outer and vice versa.