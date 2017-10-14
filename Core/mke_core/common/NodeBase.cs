namespace mke_core
{
    public class NodeBase
    {
        //identification       
        public int id;
        //node type
        public MKENodeType Type;

        //coordinate for any coordinate system
        public double x1;
        public double x2;
        public double x3;

        public virtual int GetDof()
        {
            return 0;
        }
    }
}