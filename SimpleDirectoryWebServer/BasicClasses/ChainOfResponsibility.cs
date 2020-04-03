namespace SimpleDirectoryWebServer.BasicClasses {
    public abstract class ChainOfResponsibility<T, R> {
        public ChainOfResponsibility<T, R> Next { get; set; }

        public ChainOfResponsibility<T, R> SetNext(ChainOfResponsibility<T, R> next) {
            Next = next; 
            return Next;
        }

        public virtual object Handle(object request) {
            return this.Next != null ? this.Next.Handle(request) : null;
        }
    }
}