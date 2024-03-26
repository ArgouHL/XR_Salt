#ifndef SHAPESRAYMARCHER_INCLUDE
#define SHAPESRAYMARCHER_INCLUDE

void RayMarch_float(float3 rayOrigion, float3 rayDirection, int MAX_STEPS, float SURF_DIST, float MAX_DIST, out float disFromOrigion)
{
    disFromOrigion = 0; 
    float disFromSphere; 
    for (int i = 0; i < MAX_STEPS; i++)
    {
        float3 position = rayOrigion + rayDirection * disFromOrigion;
        disFromSphere = length(position) - 0.5;
        disFromOrigion += disFromSphere; 
        if (disFromSphere < SURF_DIST || disFromOrigion > MAX_DIST)
            break; 
    }
    
}

void Frag_float(float disFromOrigion,float3 rayOrigion,float3 rayDirection, int MAX_STEPS, float SURF_DIST, float MAX_DIST, out float4 col) 
{
   
    float dis = disFromOrigion;
    col = 0;
    if (dis < MAX_DIST)
    {
        float3 position = rayOrigion + rayDirection * dis;
        col.rgb = normalize(position);
    }
    else
        discard;
   
}


#endif