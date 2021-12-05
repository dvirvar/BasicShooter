
public abstract class BasicService
{
    protected ServerRequests serverRequests;

    public BasicService(int timeout = 10)
    {
        this.serverRequests = new ServerRequests(timeout);
    }
}
