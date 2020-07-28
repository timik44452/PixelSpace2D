using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu]
public class GameRendererPipelineAsset : RenderPipelineAsset
{
    protected override RenderPipeline CreatePipeline()
    {
        return new GameRendererPipeline();
    }
}

public class GameRendererPipeline : RenderPipeline
{
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        
    }
}
