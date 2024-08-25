namespace MosadApiServer.Servises;

public class ServiceMatrix
{
    public static int[,] CreateMatrix(int rows, int columns)
    {
        var matrix = new int[rows, columns];
        return matrix;
    }

    public static void IntoMetrix(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int columns = matrix.GetLength(1);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                matrix[i, j] = i + j;
            }
        }
    }

    public static int[,] CreateExtendedMatrix(int[,] matrix)
    {

        //מביא את מספר השורות במימד הראשון שזה אינדקס 0
        int rowses = matrix.GetLength(0);
        //מביא את מספר השורות במימד הראשון שזה אינדקס 1
        int colomses = matrix.GetLength(1);
        //יצירת מטריצה גדולה בעוד 2 עמודות ובעוד 2 שורות כדי לתפעל טוב את התנאים על המיקומים והתזוזות
        int[,] extendedMatrix = new int[rowses + 2, colomses + 2];
        for (int i = 0; i < extendedMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < extendedMatrix.GetLength(1); j++)
            {
                extendedMatrix[i, j] = -1;
            }
        }

        for (int i = 0; i < rowses; i++)
        {
            for (int j = 0; j < colomses; j++)
            {
                //העתקת המטריצה המקורית לאמצע המטריצה החדשה כלומר מכל כיוון ישאר עמודה ריקה ושורה ריקה או שנכניס בהם ערכים מסויימים לצורך תנאי גבולות
                extendedMatrix[i + 1, j + 1] = matrix[i, j];
            }
        }
        return extendedMatrix;
    }





}