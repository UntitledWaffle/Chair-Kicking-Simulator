using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChunkyRenderFeature : ScriptableRendererFeature
{
    // Used chatgpt to update chunky script since old code is out of date and won't work and I am trying to focus on AI,
    class ChunkyPass : ScriptableRenderPass
    {
        private Material material;
        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTexture;
        public Texture2D SprTex;
        public Color Color = Color.white;

        public ChunkyPass(Material mat)
        {
            this.material = mat;
            tempTexture.Init("_TemporaryColorTexture");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (SprTex == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("ChunkyEffect");

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;

            float w = opaqueDesc.width;
            float h = opaqueDesc.height;
            Vector2 count = new Vector2(w / SprTex.height, h / SprTex.height);
            Vector2 size = new Vector2(1.0f / count.x, 1.0f / count.y);

            material.SetVector("BlockCount", count);
            material.SetVector("BlockSize", size);
            material.SetColor("_Color", Color);
            material.SetTexture("_SprTex", SprTex);

            // ✅ Get the camera color target here
            var source = renderingData.cameraData.renderer.cameraColorTarget;

            cmd.GetTemporaryRT(tempTexture.id, opaqueDesc, FilterMode.Bilinear);
            cmd.Blit(source, tempTexture.Identifier(), material);
            cmd.Blit(tempTexture.Identifier(), source);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    public Shader ChunkyShader;
    public Texture2D SprTex;
    public Color Color = Color.white;

    private Material material;
    private ChunkyPass renderPass;

    public override void Create()
    {
        if (ChunkyShader == null)
        {
            Debug.LogError("Chunky shader not assigned.");
            return;
        }

        material = CoreUtils.CreateEngineMaterial(ChunkyShader);
        renderPass = new ChunkyPass(material)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents,
            SprTex = SprTex,
            Color = Color
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (material != null)
        {
            renderPass.SprTex = SprTex;
            renderPass.Color = Color;
            renderer.EnqueuePass(renderPass);
        }
    }
}
