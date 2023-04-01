namespace UI.Core
{
    public interface IPage
    {
        public void Open();
        public void Close();
        public void TriggerNextPage(IPage page);
    }
}
