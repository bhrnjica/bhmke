using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mke_core
{
    public enum MKENodeType
    {
        uv = 1,//u,v - displacement 
        uvuf,//u,v - displacement and rotation related to u
        uvvf,//u,v - displacement and rotation related to v
        uvufvf,//u,v - displacement and rotation related to u , v 
        t//heat transfer node type
    }
    public enum MKEElementType
    {
        triangle = 1,//three node finite element
        quad,//four node finite element
        ltriangle,//four node triangle linear triangle
        quadquad// eight node quad
    }
    public enum MKEPlaneType
    {
        stress = 1,//plane stress type
        strain,//plane strain type
        heat,
    }
}
