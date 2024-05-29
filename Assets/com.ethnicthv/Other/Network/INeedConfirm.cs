namespace com.ethnicthv.Other.Network
{
    public interface INeedConfirm
    {
        /// <summary>
        /// A method that is called when the object is not confirmed.
        /// </summary>
        void Rollback();
    }
}