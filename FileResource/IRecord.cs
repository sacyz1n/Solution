namespace FileResource
{

    /// <summary>
    /// 싱글 레코드 인터페이스입니다.
    /// </summary>
    public interface IRecordSingle { }

    /// <summary>
    /// Integer Key를 가지는 레코드 인터페이스입니다.
    /// </summary>
    public interface IRecordWithIntegerKey
    {
        int GetKey();
    }

    /// <summary>
    /// String Key를 가지는 레코드 인터페이스입니다.
    /// </summary>
    public interface  IRecordWithStringKey
    {
        string GetKey();    
    }

}
