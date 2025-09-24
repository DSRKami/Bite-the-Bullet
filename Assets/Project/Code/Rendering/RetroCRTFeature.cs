using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RetroCRTFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public Material blitMaterial;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public Settings settings = new Settings();
    BlitPass _pass;

    class BlitPass : ScriptableRenderPass
    {
        static readonly string kTag = "RetroCRT";
        Material _mat;
        RTHandle _tempColor;

        public BlitPass(Material mat, RenderPassEvent evt)
        {
            _mat = mat;
            renderPassEvent = evt;
        }

        [System.Obsolete]
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var desc = renderingData.cameraData.cameraTargetDescriptor;
#pragma warning disable 0618
            RenderingUtils.ReAllocateIfNeeded(ref _tempColor, desc, name: "_RetroCRT_Temp");
            ConfigureTarget(renderingData.cameraData.renderer.cameraColorTargetHandle);
            ConfigureClear(ClearFlag.None, Color.black);
#pragma warning restore 0618
        }

        [System.Obsolete]
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_mat == null) return;

            var cmd = CommandBufferPool.Get(kTag);
#pragma warning disable 0618
            var src = renderingData.cameraData.renderer.cameraColorTargetHandle;
            Blitter.BlitCameraTexture(cmd, src, _tempColor, _mat, 0);
            Blitter.BlitCameraTexture(cmd, _tempColor, src);
#pragma warning restore 0618

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd) { }
    }

    public override void Create()
    {
        _pass = new BlitPass(settings.blitMaterial, settings.renderPassEvent);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.blitMaterial == null) return;
        renderer.EnqueuePass(_pass);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }
}